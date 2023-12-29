using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeDeviceDiscovery : IDisposable
    {
        private readonly UdpClient udp = new(SmartHomeProtocol.Port);
        private readonly SemaphoreSlim discoverySemaphore = new(0, 1);
        private readonly SmartHomeCommand<SmartHomeDeviceInfoResponse> command = new("system", "get_sysinfo");
        private bool disposedValue = false;

        /// <summary>
        /// Time to wait when listening for responses.
        /// </summary>
        public TimeSpan ListenerTimeout { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Delay between sending discovery packets.
        /// </summary>
        public TimeSpan AnnounceDelay { get; set; } = TimeSpan.FromMilliseconds(225);


        /// <summary>
        /// Number of discovery packets to send.
        /// </summary>
        public int DiscoveryPackets { get; set; } = 3;

        /// <summary>
        /// Discovers devices on the local network.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Device addresses.</returns>
        public async Task<IReadOnlyDictionary<string, SmartHomeDeviceInfoResponse>> DiscoverDevicesAsync(CancellationToken cancellationToken)
        {
            var sendingPackets = SendDiscoveryPacketsAsync(cancellationToken);
            var listeningForDevices = ListenForDevicesAsync(cancellationToken);
            await Task.WhenAll(sendingPackets, listeningForDevices);
            return await listeningForDevices;
        }

        /// <summary>
        /// Sends discovery packets to the broadcast address.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SendDiscoveryPacketsAsync(CancellationToken cancellationToken)
        {
            // create discovery packet
            IPEndPoint broadcastEndpoint = new(IPAddress.Broadcast, SmartHomeProtocol.Port);
            var discoveryPacket = SmartHomeCypher.Encrypt(command.GetCommandJson(), false);

            for (int i = 0; i < DiscoveryPackets; i++)
            {
                await udp.SendAsync(discoveryPacket, discoveryPacket.Length, broadcastEndpoint);

                // release the semaphore after the first packet is sent
                if (i == 0)
                {
                    discoverySemaphore.Release();
                }

                // stagger the sending of packets
                await Task.Delay(AnnounceDelay, cancellationToken);
            }
        }

        /// <summary>
        /// Listens for responses from devices.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<IReadOnlyDictionary<string, SmartHomeDeviceInfoResponse>> ListenForDevicesAsync(CancellationToken cancellationToken)
        {
            // wait for discovery packets to start being sent
            await discoverySemaphore.WaitAsync(cancellationToken);

            // listen for device responses
            Dictionary<string, SmartHomeDeviceInfoResponse> result = new();
            while (udp.Client.Poll(ListenerTimeout, SelectMode.SelectRead))
            {
                // receive and process response
                UdpReceiveResult response = await udp.ReceiveAsync(cancellationToken);
                var deviceInfo = command.GetResponseValue(SmartHomeCypher.Decrypt(response.Buffer, false));

                // this receiver will also catch discovery packets which should be ignored, but they will have an empty device info object.
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

            // release the semaphore and return the results
            discoverySemaphore.Release();
            return result.AsReadOnly();
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    udp.Dispose();
                    discoverySemaphore.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
