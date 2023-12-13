using EonData.SmartHome.TpLink.SmartHomeProtocol;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.Devices
{
    public abstract class SmartHomeDeviceBase
    {
        protected SmartHomeClient client;
        public SmartHomeDeviceBase(SmartHomeClient protocolClient) => client = protocolClient;
        public SmartHomeDeviceBase(string address) : this(new SmartHomeClient(address)) { }
    }
}
