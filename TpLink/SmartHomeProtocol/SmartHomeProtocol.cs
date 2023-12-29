using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeProtocol
    {
        /// <summary>
        /// Gets the port number to use for network communications.
        /// </summary>
        public const int Port = 9999;

        /// <summary>
        /// Gets or sets the default buffer size to use for network communications.
        /// </summary>
        public int ReadBufferSize { get; set; } = 2048;

        /// <summary>
        /// Time to wait when listening for responses.
        /// </summary>
        public TimeSpan DiscoveryTimeout { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Delay between sending discovery packets.
        /// </summary>
        public TimeSpan DiscoveryDelay { get; set; } = TimeSpan.FromMilliseconds(125);

        /// <summary>
        /// Gets or sets the number of discovery packets to send when discovering devices.
        /// </summary>
        public int DiscoveryPackets { get; set; } = 3;

        /// <summary>
        /// Asynchronously sends a string of data to the smart home device and returns the response string.
        /// </summary>
        /// <param name="data">JSON data command string</param>
        /// <returns>JSON data response string</returns>
        public async Task<string> SendDataAsync(string address, string data, CancellationToken cancellationToken)
        {
            using var tcp = new TcpClient();
            await tcp.ConnectAsync(address, Port, cancellationToken);
            await using var netStream = tcp.GetStream();
            var dataBytes = SmartHomeCypher.Encrypt(data, true);
            await netStream.WriteAsync(dataBytes, 0, dataBytes.Length, cancellationToken);
            var buffer = new byte[ReadBufferSize];
            await netStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
            return SmartHomeCypher.Decrypt(buffer, true);
        }

        /// <summary>
        /// Discovers devices on the local network.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Device addresses.</returns>
        public async Task<IReadOnlyDictionary<string, SmartHomeDeviceInfoResponse>> DiscoverDevicesAsync(CancellationToken cancellationToken)
        {
            using UdpClient udp = new(Port) { EnableBroadcast = true };
            using SemaphoreSlim packetSemaphore = new(0, 1);
            var sendingPackets = SendDiscoveryPacketsAsync(udp, packetSemaphore, cancellationToken);
            var listeningForDevices = ListenForDevicesAsync(udp, packetSemaphore, cancellationToken);
            await Task.WhenAll(sendingPackets, listeningForDevices);
            return await listeningForDevices;
        }

        /// <summary>
        /// Sends discovery packets to the broadcast address.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SendDiscoveryPacketsAsync(UdpClient udp, SemaphoreSlim discoverySemaphore, CancellationToken cancellationToken)
        {
            // create discovery packet
            IPEndPoint broadcastEndpoint = new(IPAddress.Broadcast, Port);
            SmartHomeCommand<SmartHomeDeviceInfoResponse> command = new("system", "get_sysinfo");
            var discoveryPacket = SmartHomeCypher.Encrypt(command.GetCommandJson(), false);
            for (int i = 0; i < DiscoveryPackets; i++)
            {
                await udp.SendAsync(discoveryPacket, discoveryPacket.Length, broadcastEndpoint);
                discoverySemaphore.Release();
                await Task.Delay(DiscoveryDelay, cancellationToken); // stagger the sending of packets
            }
        }

        /// <summary>
        /// Listens for responses from devices.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<IReadOnlyDictionary<string, SmartHomeDeviceInfoResponse>> ListenForDevicesAsync(UdpClient udp, SemaphoreSlim discoverySemaphore, CancellationToken cancellationToken)
        {
            // wait for discovery packets to start being sent
            await discoverySemaphore.WaitAsync(cancellationToken);

            SmartHomeCommand<SmartHomeDeviceInfoResponse> command = new("system", "get_sysinfo");
            List<IPEndPoint> deviceAddresses = new();
            Dictionary<string, SmartHomeDeviceInfoResponse> result = new();
            // listen for device responses
            while (true)
            {
                if (udp.Client.Poll(DiscoveryTimeout, SelectMode.SelectRead))
                {
                    // receive and process response
                    UdpReceiveResult response = await udp.ReceiveAsync(cancellationToken);
                    var deviceInfo = command.GetResponseValue(SmartHomeCypher.Decrypt(response.Buffer, false));

                    // this receiver will also catch discovery packets but they will have an empty device info object
                    if (deviceInfo.HardwareId != null)
                    {
                        // sending multiple packets can cause multiple replies. only add the first one.
                        string deviceAddress = response.RemoteEndPoint.Address.ToString();
                        if (!result.ContainsKey(deviceAddress))
                        {
                            result.Add(deviceAddress, deviceInfo);
                        }
                    }
                }
                else
                {
                    // no response
                    break;
                }
            }
            return result.AsReadOnly();
        }
    }
}