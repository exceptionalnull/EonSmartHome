/*
 * A huge thanks to Lubomir Stroetmann for reverse engineering the protocol: https://www.softscheck.com/en/reverse-engineering-tp-link-hs110/
 * This code is based on their work in python: https://github.com/softScheck/tplink-smartplug
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink
{
    internal class SmartHomeProtocol
    {
        /// <summary>
        /// Initial cypher key value to use when encrypting and decrypting data transmissions.
        /// </summary>
        private const int INITIAL_CYPHERKEY = 171;

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
        public async Task<string> SendDataAsync(string data)
        {
            string response = null;

            using (var tcp = new TcpClient())
            {
                await tcp.ConnectAsync(Address, Port);
                using (var netStream = tcp.GetStream())
                {
                    var dataBytes = Encrypt(data);
                    await netStream.WriteAsync(dataBytes, 0, dataBytes.Length);
                    var buffer = new byte[ReadBufferSize];
                    await netStream.ReadAsync(buffer, 0, buffer.Length);
                    response = Decrypt(buffer, buffer.Length);
                }
            }

            return response;
        }

        /// <summary>
        /// Encrypts data before being transmitted to the smart home device
        /// </summary>
        /// <param name="data">Data to be encrypted</param>
        /// <returns>Encrypted byte array</returns>
        public byte[] Encrypt(string data)
        {
            var result = new List<byte>() { 0, 0, 0, (byte)data.Length };

            var key = INITIAL_CYPHERKEY;

            foreach (int dataChar in data)
            {
                var a = key ^ dataChar;
                key = a;
                result.Add((byte)a);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Decrypts data sent back from the smart home device
        /// </summary>
        /// <param name="data">Encrypted byte array</param>
        /// <param name="bufferLength">Length of buffer array</param>
        /// <returns>Decrypted string</returns>
        public string Decrypt(byte[] data, int bufferLength)
        {
            var key = INITIAL_CYPHERKEY;

            var result = new StringBuilder();
            for (int i = 4; i < bufferLength; i++)
            {
                if (data[i] == 0)
                {
                    break;
                }

                var a = key ^ data[i];
                key = data[i];

                result.Append((char)a);
            }
            return result.ToString();
        }
    }
}
