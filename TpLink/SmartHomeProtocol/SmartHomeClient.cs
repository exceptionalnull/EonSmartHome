/*
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
    public class SmartHomeClient
    {
        public string Address { get; set; }
        private readonly SmartHomeProtocol protocol = new SmartHomeProtocol();

        public SmartHomeClient(string deviceAddress) => Address = deviceAddress;

        /// <summary>
        /// Sends a SmartHomeProtocol command to the device.
        /// </summary>
        /// <typeparam name="T">Response type for this command.</typeparam>
        /// <param name="commandType">Command category</param>
        /// <param name="commandName">Command name</param>
        /// <param name="commandParameters">A collection of parameter names and object values.</param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns>Specified response type.</returns>
        public Task<T> SendCommandAsync<T>(string commandType, string commandName, IDictionary<string, object>? commandParameters, CancellationToken cancellationToken) where T : SmartHomeResponse =>
            SendCommandAsync<T>(new SmartHomeCommand<T>(commandType, commandName, commandParameters), cancellationToken);

        /// <summary>
        /// Sends a SmartHomeProtocol command with no parameters to the device.
        /// </summary>
        /// <typeparam name="T">Response type for this command.</typeparam>
        /// <param name="commandType">Command category</param>
        /// <param name="commandName">Command name</param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns>Specified response type.</returns>
        /// <remarks>This is equivalent to passing null for commandParameters in the above method.</remarks>
        public Task<T> SendCommandAsync<T>(string commandType, string commandName, CancellationToken cancellationToken) where T : SmartHomeResponse =>
            SendCommandAsync<T>(commandType, commandName, null, cancellationToken);

        public Task<T> SendCommandAsync<T>(SmartHomeCommand<T> command, CancellationToken cancellationToken) where T : SmartHomeResponse =>
            command.ExecuteAsync(protocol, Address, cancellationToken);

        public Task DiscoverDevicesAsync(CancellationToken cancellationToken) => protocol.DiscoverDevicesAsync(cancellationToken);
    }
}