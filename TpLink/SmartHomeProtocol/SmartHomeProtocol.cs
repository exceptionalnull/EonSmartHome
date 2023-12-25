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
            using SmartHomeDeviceDiscovery discovery = new SmartHomeDeviceDiscovery();
            await discovery.DiscoverDevicesAsync(cancellationToken);
            return discovery.DeviceAddresses.Select(x => x.Address.ToString());
        }
    }
}