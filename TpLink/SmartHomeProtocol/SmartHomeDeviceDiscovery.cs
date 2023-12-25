using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeDeviceDiscovery : IDisposable
    {
        public TimeSpan DiscoveryTimeout { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan DiscoveryDelay { get; set; } = TimeSpan.FromMilliseconds(600);

        public int TimeoutRetries { get; set; } = 2;

        public IReadOnlyList<IPEndPoint> DeviceAddresses => deviceAddresses.AsReadOnly();

        /// <summary>
        /// Gets or sets the number of discovery packets to send when discovering devices. (Default is 3)
        /// </summary>
        public int DiscoveryPackets { get; set; } = 3;

        private readonly UdpClient udp = new UdpClient(SmartHomeProtocol.Port) { EnableBroadcast = true };
        private List<IPEndPoint> deviceAddresses = new List<IPEndPoint>();

        public void Dispose() => udp.Dispose();

        public Task DiscoverDevicesAsync(CancellationToken cancellationToken) => Task.WhenAll(SendDiscoveryPacketsAsync(cancellationToken), ListenForDevicesAsync(cancellationToken));

        private async Task SendDiscoveryPacketsAsync(CancellationToken cancellationToken)
        {
            // create discovery packet
            IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, SmartHomeProtocol.Port);
            var command = new SmartHomeCommand<SmartHomeResponse>("system", "get_sysinfo");
            var commandJson = command.GetCommandJson();
            var discoveryPacket = SmartHomeCypher.Encrypt(commandJson);

            for (int i = 0; i < DiscoveryPackets; i++)
            {
                await udp.SendAsync(discoveryPacket, discoveryPacket.Length, broadcastEndpoint);
                await Task.Delay(DiscoveryDelay); // stagger the sending of packets
            }
        }

        private async Task ListenForDevicesAsync(CancellationToken cancellationToken)
        {
            int timeouts = 0;
            while (timeouts < TimeoutRetries)
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
        }
    }
}
