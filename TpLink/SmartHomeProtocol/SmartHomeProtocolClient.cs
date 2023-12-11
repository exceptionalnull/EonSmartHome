﻿/*
 * A huge thanks to Lubomir Stroetmann for reverse engineering the protocol: https://www.softscheck.com/en/reverse-engineering-tp-link-hs110/
 * This code is based on their work in python: https://github.com/softScheck/tplink-smartplug
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeProtocolClient
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
        public SmartHomeProtocolClient(string Address) => this.Address = Address;

        /// <summary>
        /// Sends a SmartHomeProtocol command to the device.
        /// </summary>
        /// <typeparam name="T">Response type for this command.</typeparam>
        /// <param name="commandType">Command category</param>
        /// <param name="commandName">Command name</param>
        /// <param name="commandParameters">A collection of parameter names and object values.</param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns>Specified response type.</returns>
        public async Task<T?> SendCommandAsync<T>(string commandType, string commandName, IDictionary<string, object>? commandParameters, CancellationToken cancellationToken) where T : SmartHomeResponse
        {
            string commandJson = JsonSerializer.Serialize(new Dictionary<string, object> {
                { commandType, new Dictionary<string, object?>() {
                    { commandName, commandParameters }
                } }
            });

            var responseJson = await SendDataAsync(commandJson, cancellationToken);
            var responseJsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            responseJsonOptions.Converters.Add(new JsonBoolConverter());
            var response = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, T>>>(responseJson, responseJsonOptions);
            return response?[commandType][commandName];
        }

        /// <summary>
        /// Sends a SmartHomeProtocol command with no parameters to the device.
        /// </summary>
        /// <typeparam name="T">Response type for this command.</typeparam>
        /// <param name="commandType">Command category</param>
        /// <param name="commandName">Command name</param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns>Specified response type.</returns>
        /// <remarks>This is equivalent to passing null for commandParameters in the above method.</remarks>
        public Task<T?> SendCommandAsync<T>(string commandType, string commandName, CancellationToken cancellationToken) where T : SmartHomeResponse => SendCommandAsync<T>(commandType, commandName, null, cancellationToken);

        /// <summary>
        /// Asynchronously sends a string of data to the smart home device and returns the response string.
        /// </summary>
        /// <param name="data">JSON data command string</param>
        /// <returns>JSON data response string</returns>
        private async Task<string> SendDataAsync(string data, CancellationToken cancellationToken)
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