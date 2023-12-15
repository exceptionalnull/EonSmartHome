using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    internal class SmartHomeProtocol
    {
        /// <summary>
        /// Gets or sets the IP address or hostname of the smart home device
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the port number to use for network communications. (Default is 9999)
        /// </summary>
        public int Port { get; set; } = 9999;

        /// <summary>
        /// Gets or sets the default buffer size to use for network communications. (Default is 2048)
        /// </summary>
        public int ReadBufferSize { get; set; } = 2048;

        /// <summary>
        /// Creates a new SmartHomeProtocol object
        /// </summary>
        /// <param name="Address">IP address or hostname of the smart home device</param>
        public SmartHomeProtocol(string Address) => this.Address = Address;

        /// <summary>
        /// Asynchronously sends a string of data to the smart home device and returns the response string.
        /// </summary>
        /// <param name="data">JSON data command string</param>
        /// <returns>JSON data response string</returns>
        public async Task<string> SendDataAsync(string data, CancellationToken cancellationToken)
        {
            string response;

            using (var tcp = new TcpClient())
            {
                await tcp.ConnectAsync(Address, Port, cancellationToken);
                await using var netStream = tcp.GetStream();
                var dataBytes = SmartHomeCypher.Encrypt(data);
                await netStream.WriteAsync(dataBytes, 0, dataBytes.Length, cancellationToken);
                var buffer = new byte[ReadBufferSize];
                await netStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                response = SmartHomeCypher.Decrypt(buffer, buffer.Length);
            }

            return response;
        }
    }
}
