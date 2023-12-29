using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    /// <summary>
    /// Thrown when the response from the device indicates an error occured.
    /// </summary>
    public class SmartHomeException : Exception
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
        /// Error code from the device response.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Error message from the device response.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartHomeException"/> class.
        /// </summary>
        /// <param name="commandType">Primary portion of the command object.</param>
        /// <param name="commandName">Secondary portion of the command object.</param>
        /// <param name="errCode">Error code from the device response.</param>
        /// <param name="errMessage">Error message from the device response.</param>
        public SmartHomeException(string commandType, string commandName, int errCode, string? errMessage) :
            base($"error in response to {commandType}/{commandName}: ({errCode}) {errMessage}")
        {
            CommandType = commandType;
            CommandName = commandName;
            ErrorCode = errCode;
            ErrorMessage = errMessage;
        }
    }
}
