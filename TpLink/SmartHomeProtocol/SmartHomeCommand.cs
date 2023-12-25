using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeCommand<T> where T : SmartHomeResponse
    {
        protected string CommandType { get; set; }
        protected string CommandName { get; set; }
        protected IDictionary<string, object>? CommandParameters { get; set; }

        public SmartHomeCommand(string commandType, string commandName, IDictionary<string, object>? commandParameters = null)
        {
            CommandType = commandType;
            CommandName = commandName;
            CommandParameters = commandParameters;
        }

        internal virtual string GetCommandJson()
        {
            var commandObject = new Dictionary<string, object> {
                { CommandType, new Dictionary<string, object?>() {
                    { CommandName, CommandParameters }
                } }
            };
            return JsonSerializer.Serialize(commandObject, GetCommandSerializerOptions());
        }

        internal virtual async Task<T> ExecuteAsync(SmartHomeProtocol protocol, string address, CancellationToken cancellationToken)
        {
            string responseJson = await protocol.SendDataAsync(address, GetCommandJson(), cancellationToken);
            var response = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, T>>>(responseJson, GetCommandSerializerOptions());
            if (response == null || !response.ContainsKey(CommandType) || !response[CommandType].ContainsKey(CommandName))
            {
                throw new SmartHomeMalformedResponseException(CommandType, CommandName, address, responseJson);
            }

            T responseObject = response[CommandType][CommandName];
            if ((responseObject?.ErrorCode ?? 0) != 0)
            {
                throw new SmartHomeException(CommandType, CommandName, address, responseObject!.ErrorCode, responseObject?.ErrorMessage);
            }
            return responseObject!;
        }

        protected JsonSerializerOptions GetCommandSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            jsonOptions.Converters.Add(new JsonBoolConverter());
            return jsonOptions;
        }
    }
}
