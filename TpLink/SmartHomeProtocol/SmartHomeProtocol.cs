using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    /// <summary>
    /// Allows for sending data to a TP-Link smart home device.
    /// </summary>
    public static class SmartHomeProtocol
    {
        /// <summary>
        /// Port number to use for network communications.
        /// </summary>
        public static int Port = 9999;

        /// <summary>
        /// Initial cypher key value to use when encrypting and decrypting data transmissions.
        /// </summary>
        private const int INITIAL_CYPHERKEY = 171;

        /// <summary>
        /// Encrypts data before being transmitted to the smart home device
        /// </summary>
        /// <param name="data">Data to be encrypted</param>
        /// <returns>Encrypted byte array</returns>
        public static byte[] Encrypt(string data)
        {

            List<byte> result = new();
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
        public static string Decrypt(byte[] data)
        {
            var key = INITIAL_CYPHERKEY;

            var result = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
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

        /// <summary>
        /// Asynchronously sends a string of data to the smart home device and returns the response string.
        /// </summary>
        /// <param name="data">JSON data command string</param>
        /// <returns>JSON data response string</returns>
        public static async Task<string> SendDataAsync(string address, string data, CancellationToken cancellationToken)
        {
            // connect to device
            using var tcp = new TcpClient();
            await tcp.ConnectAsync(address, Port, cancellationToken);
            await using var netStream = tcp.GetStream();

            // encrypt command data
            var commandBytes = Encrypt(data);
            // padding contains the length of the data
            byte[] commandPadding = { 0, 0, (byte)(commandBytes.Length / 256), (byte)(commandBytes.Length % 256) };
            // combine padding and command data
            byte[] commandPacket = new byte[commandBytes.Length + 4];
            commandPadding.CopyTo(commandPacket, 0);
            commandBytes.CopyTo(commandPacket, 4);
            // send command packet
            await netStream.WriteAsync(commandPacket, 0, commandPacket.Length, cancellationToken);

            // read response padding
            byte[] padding = await ReadBytesAsync(netStream, 4, cancellationToken);
            int length = padding[2] * 256 + padding[3];

            // read response data
            byte[] responseBytes = await ReadBytesAsync(netStream, length, cancellationToken);
            return Decrypt(responseBytes);
        }

        /// <summary>
        /// Reads the specified number of bytes from the network stream.
        /// </summary>
        /// <param name="netStream">Network stream to read from</param>
        /// <param name="length">Number of bytes to read</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks><see cref="NetworkStream.ReadAsync(byte[], int, int, CancellationToken)"/> does not guarantee that the entire data packet will be read. For instance, fragmentation may cause only partial data retrieval in the first read even if the data is not large. This method ensures that all of the data is read.</remarks>
        private static async Task<byte[]> ReadBytesAsync(NetworkStream netStream, int length, CancellationToken cancellationToken)
        {
            byte[] responseBytes = new byte[length];
            do
            {
                byte[] buffer = new byte[length];
                var readBytes = await netStream.ReadAsync(buffer, 0, length, cancellationToken);
                buffer.CopyTo(responseBytes, responseBytes.Length - length);
                length -= readBytes;
            } while (length > 0);
            return responseBytes;
        }
    }
}