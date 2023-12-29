namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    /// <summary>
    /// Thrown when the response from the device is not formatted as expected.
    /// </summary>
    public class SmartHomeMalformedResponseException : Exception
    {
        /// <summary>
        /// Primary portion of the command object.
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// Secondary portion of the command object.
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// Parameter values sent with the command.
        /// </summary>
        public IDictionary<string, object>? CommandParameters { get; set; }

        /// <summary>
        /// Response JSON from the device.
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType">Primary portion of the command object.</param>
        /// <param name="commandName">Secondary portion of the command object.</param>
        /// <param name="response">Response JSON from the device.</param>
        public SmartHomeMalformedResponseException(string commandType, string commandName, string response) :
            base($"malformed response to {commandType}/{commandName}: {response}")
        {
            CommandType = commandType;
            CommandName = commandName;
            Response = response;
        }
    }
}
