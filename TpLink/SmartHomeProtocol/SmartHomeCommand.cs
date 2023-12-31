using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    /// <summary>
    /// Represents a command to be sent to a TP-Link smart home device.
    /// </summary>
    /// <typeparam name="T">The type of response expected from the device.</typeparam>
    public class SmartHomeCommand<T> where T : SmartHomeResponse
    {
        /// <summary>
        /// Primary portion of the command object.
        /// </summary>
        protected string CommandType { get; set; }

        /// <summary>
        /// Secondary portion of the command object.
        /// </summary>
        protected string CommandName { get; set; }

        /// <summary>
        /// Parameter values to be sent with the command.
        /// </summary>
        protected IDictionary<string, object>? CommandParameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartHomeCommand{T}"/> class.
        /// </summary>
        /// <param name="commandType">Primary portion of the command object.</param>
        /// <param name="commandName">Secondary portion of the command object.</param>
        /// <param name="commandParameters">Parameter values to be sent with the command.</param>
        public SmartHomeCommand(string commandType, string commandName, IDictionary<string, object>? commandParameters = null)
        {
            CommandType = commandType;
            CommandName = commandName;
            CommandParameters = commandParameters;
        }

        /// <summary>
        /// Generates the JSON string for this command.
        /// </summary>
        /// <returns></returns>
        internal virtual string GetCommandJson()
        {
            var commandObject = new Dictionary<string, object> {
                { CommandType, new Dictionary<string, object?>() {
                    { CommandName, CommandParameters ?? new Dictionary<string, object>() }
                } }
            };
            return JsonSerializer.Serialize(commandObject, GetCommandSerializerOptions());
        }

        /// <summary>
        /// Deserializes the expected response from the given JSON string.
        /// </summary>
        /// <param name="responseJson">Response JSON string</param>
        /// <returns>The expected response type for this command.</returns>
        /// <exception cref="SmartHomeMalformedResponseException">The response from the device was not formatted as expected.</exception>
        /// <exception cref="SmartHomeException">The resposne from the device indicated an error occured.</exception>
        internal virtual T GetResponseValue(string responseJson)
        {
            if (string.IsNullOrEmpty(responseJson))
            {
                throw new SmartHomeMalformedResponseException(CommandType, CommandName, responseJson);
            };

            Dictionary<string, Dictionary<string, T>>? response = null;
            try
            {
                response = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, T>>>(responseJson, GetCommandSerializerOptions());
            }
            catch (JsonException)
            {
                throw new SmartHomeMalformedResponseException(CommandType, CommandName, responseJson);
            }
            

            if (response == null || !response.ContainsKey(CommandType) || !response[CommandType].ContainsKey(CommandName))
            {
                throw new SmartHomeMalformedResponseException(CommandType, CommandName, responseJson);
            }

            T responseObject = response[CommandType][CommandName];
            if ((responseObject?.ErrorCode ?? 0) != 0)
            {
                throw new SmartHomeException(CommandType, CommandName, responseObject!.ErrorCode, responseObject?.ErrorMessage);
            }
            return responseObject!;
        }

        /// <summary>
        /// Sends the command to the device.
        /// </summary>
        /// <param name="address">Address of the device.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The expected response type for this command.</returns>
        internal virtual async Task<T> ExecuteAsync(string address, CancellationToken cancellationToken)
        {
            string responseJson = await SmartHomeProtocol.SendDataAsync(address, GetCommandJson(), cancellationToken);
            return GetResponseValue(responseJson);
        }

        /// <summary>
        /// Returns the JsonSerializerOptions to use for command and response JSON.
        /// </summary>
        /// <returns>JsonSerializerOptions to use for command and response JSON.</returns>
        protected JsonSerializerOptions GetCommandSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            jsonOptions.Converters.Add(new JsonBoolConverter());
            return jsonOptions;
        }
    }
}
