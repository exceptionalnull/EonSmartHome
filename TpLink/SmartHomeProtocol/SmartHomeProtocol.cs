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

            // send command
            var dataBytes = SmartHomeCypher.Encrypt(data);
            byte[] commandPadding = { 0, 0, (byte)(dataBytes.Length / 256), (byte)(dataBytes.Length % 256) };
            byte[] commandPacket = new byte[dataBytes.Length + 4];
            commandPadding.CopyTo(commandPacket, 0);
            dataBytes.CopyTo(commandPacket, 4);
            await netStream.WriteAsync(commandPacket, 0, dataBytes.Length, cancellationToken);

            // read response padding
            byte[] padding = new byte[4];
            await netStream.ReadAsync(padding, 0, 4, cancellationToken);
            int length = padding[2] * 256 + padding[3];

            // read response data
            byte[] responseBytes = new byte[length];
            do
            {
                byte[] buffer = new byte[length];
                var readBytes = await netStream.ReadAsync(buffer, 0, length, cancellationToken);
                buffer.CopyTo(responseBytes, responseBytes.Length - length);
                length -= readBytes;
            } while (length > 0);
            
            return SmartHomeCypher.Decrypt(responseBytes);
        }
    }
}