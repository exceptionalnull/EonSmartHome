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

        protected virtual string GetCommandJson()
        {
            var commandObject = new Dictionary<string, object> {
                { CommandType, new Dictionary<string, object?>() {
                    { CommandName, CommandParameters }
                } }
            };
            return JsonSerializer.Serialize(commandObject, GetDefaultSerializerOptions());
        }

        protected JsonSerializerOptions GetDefaultSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            jsonOptions.Converters.Add(new JsonBoolConverter());
            return jsonOptions;
        }

        internal virtual async Task<T> ExecuteAsync(SmartHomeProtocol protocol, CancellationToken cancellationToken)
        {
            string responseJson = await protocol.SendDataAsync(GetCommandJson(), cancellationToken);
            var response = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, T>>>(responseJson, GetDefaultSerializerOptions());
            if (response == null || !response.ContainsKey(CommandType) || !response[CommandType].ContainsKey(CommandName))
            {
                throw new SmartHomeMalformedResponseException(CommandType, CommandName, protocol.Address, responseJson);
            }

            T responseObject = response[CommandType][CommandName];
            if ((responseObject?.ErrorCode ?? 0) != 0)
            {
                throw new SmartHomeException(CommandType, CommandName, protocol.Address, responseObject!.ErrorCode, responseObject?.ErrorMessage);
            }
            return responseObject!;
        }
    }
}
