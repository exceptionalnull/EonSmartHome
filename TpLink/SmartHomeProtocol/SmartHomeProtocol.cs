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
        /// Gets or sets the port number to use for network communications. (Default is 9999)
        /// </summary>
        public int Port { get; set; } = 9999;

        /// <summary>
        /// Gets or sets the default buffer size to use for network communications. (Default is 2048)
        /// </summary>
        public int ReadBufferSize { get; set; } = 2048;

        public int DiscoveryTimeout { get; set; } = 5000;

        /// <summary>
        /// Gets or sets the number of discovery packets to send when discovering devices. (Default is 3)
        /// </summary>
        public int DiscoveryPackets { get; set; } = 1;

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

        public async Task DiscoverDevicesAsync(CancellationToken cancellationToken)
        {
            // create and configure udp client
            using var udp = new UdpClient(Port);
            udp.EnableBroadcast = true;
            udp.Client.ReceiveTimeout = DiscoveryTimeout;
            IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, Port);

            // create discovery packet
            var command = new SmartHomeCommand<SmartHomeResponse>("system", "get_sysinfo");
            var commandJson = command.GetCommandJson();
            var discoveryPacket = SmartHomeCypher.Encrypt(commandJson);

            // send packets
            for (int i = 0; i < DiscoveryPackets; i++)
            {
                await udp.SendAsync(discoveryPacket, discoveryPacket.Length, broadcastEndpoint);
            }

            // listen for response
            var localIP = ((IPEndPoint)udp.Client.LocalEndPoint).Address;
            List<IPEndPoint> deviceAddresses = new List<IPEndPoint>();
            while (true)
            {
                if (udp.Client.Poll(TimeSpan.FromSeconds(5), SelectMode.SelectRead))
                {
                    UdpReceiveResult response = await udp.ReceiveAsync(cancellationToken);
                    // exclude self-connection
                    if (!response.RemoteEndPoint.Address.Equals(localIP))
                    {
                        deviceAddresses.Add(response.RemoteEndPoint);
                        string responseString = SmartHomeCypher.Decrypt(response.Buffer, response.Buffer.Length);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}