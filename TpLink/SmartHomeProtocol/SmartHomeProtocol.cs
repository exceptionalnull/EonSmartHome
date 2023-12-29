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
    }
}