using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    internal interface ISmartHomeCommand
    {
        string CommandType { get; }
        IDictionary<string, object> Parameters { get; }
    }
}
