using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    internal class SmartHomeProtocol
    {
        /// <summary>
        /// Gets the port number to use for network communications. (Default is 9999)
        /// </summary>
        public const int Port = 9999;

        /// <summary>
        /// Gets or sets the default buffer size to use for network communications. (Default is 2048)
        /// </summary>
        public int ReadBufferSize { get; set; } = 2048;

        /// <summary>
        /// Time to wait when listening for responses.
        /// </summary>
        public TimeSpan DiscoveryTimeout { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Delay between sending discovery packets.
        /// </summary>
        public TimeSpan DiscoveryDelay { get; set; } = TimeSpan.FromMilliseconds(600);

        /// <summary>
        /// Number of timeouts before giving up on discovery.
        /// </summary>
        public int DiscoveryRetries { get; set; } = 2;

        /// <summary>
        /// Gets or sets the number of discovery packets to send when discovering devices. (Default is 3)
        /// </summary>
        public int DiscoveryPackets { get; set; } = 3;

        /// <summary>
        /// Asynchronously sends a string of data to the smart home device and returns the response string.
        /// </summary>
        /// <param name="data">JSON data command string</param>
        /// <returns>JSON data response string</returns>
        public async Task<string> SendDataAsync(string address, string data, CancellationToken cancellationToken)
        {
            string response;

            using (var tcp = new TcpClient())
            {
                await tcp.ConnectAsync(address, Port, cancellationToken);
                await using var netStream = tcp.GetStream();
                var dataBytes = SmartHomeCypher.Encrypt(data);
                await netStream.WriteAsync(dataBytes, 0, dataBytes.Length, cancellationToken);
                var buffer = new byte[ReadBufferSize];
                await netStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                response = SmartHomeCypher.Decrypt(buffer, buffer.Length);
            }

            return response;
        }

        /// <summary>
        /// Discovers devices on the local network.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Device addresses.</returns>
        public async Task<IEnumerable<string>> DiscoverDevicesAsync(CancellationToken cancellationToken)
        {
            using UdpClient udp = new UdpClient(Port) { EnableBroadcast = true };
            var listenTask = ListenForDevicesAsync(udp, cancellationToken);
            await Task.WhenAll(SendDiscoveryPacketsAsync(udp, cancellationToken), listenTask);
            return (await listenTask).Select(x => x.Address.ToString());
        }

        /// <summary>
        /// Sends discovery packets to the broadcast address.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SendDiscoveryPacketsAsync(UdpClient udp, CancellationToken cancellationToken)
        {
            // create discovery packet
            IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, Port);
            var command = new SmartHomeCommand<SmartHomeResponse>("system", "get_sysinfo");
            var commandJson = command.GetCommandJson();
            var discoveryPacket = SmartHomeCypher.Encrypt(commandJson);

            for (int i = 0; i < DiscoveryPackets; i++)
            {
                await udp.SendAsync(discoveryPacket, discoveryPacket.Length, broadcastEndpoint);
                await Task.Delay(DiscoveryDelay); // stagger the sending of packets
            }
        }

        /// <summary>
        /// Listens for responses from devices.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<IEnumerable<IPEndPoint>> ListenForDevicesAsync(UdpClient udp, CancellationToken cancellationToken)
        {
            List<IPEndPoint> deviceAddresses = new();
            int timeouts = 0;
            while (timeouts < DiscoveryRetries)
            {
                if (udp.Client.Poll(DiscoveryTimeout, SelectMode.SelectRead))
                {
                    UdpReceiveResult response = await udp.ReceiveAsync(cancellationToken);
                    string responseString = SmartHomeCypher.Decrypt(response.Buffer, response.Buffer.Length);
                    if (!deviceAddresses.Contains(response.RemoteEndPoint))
                    {
                        deviceAddresses.Add(response.RemoteEndPoint);
                    }
                }
                else
                {
                    timeouts++;
                }
            }
            return deviceAddresses.AsReadOnly();
        }
    }
}