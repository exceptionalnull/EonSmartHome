using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeException : Exception
    {
        public string CommandType { get; set; }
        public string CommandName { get; set; }
        public IDictionary<string, object>? CommandParameters { get; set; }
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

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
