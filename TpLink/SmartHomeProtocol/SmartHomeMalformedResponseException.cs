﻿namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeMalformedResponseException : Exception
    {
        public string CommandType { get; set; }
        public string CommandName { get; set; }
        public IDictionary<string, object>? CommandParameters { get; set; }
        public string Response { get; set; }

        public SmartHomeMalformedResponseException(string commandType, string commandName, string response) :
            base($"malformed response to {commandType}/{commandName}: {response}")
        {
            CommandType = commandType;
            CommandName = commandName;
            Response = response;
        }
    }
}
