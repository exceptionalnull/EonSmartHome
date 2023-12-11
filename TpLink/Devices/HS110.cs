using EonData.SmartHome.TpLink.SmartHomeProtocol;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.Devices
{
    public class HS110
    {
        private SmartHomeProtocolClient client;
        public HS110(SmartHomeProtocolClient protocolClient) => client = protocolClient;
        public HS110(string address) : this(new SmartHomeProtocolClient(address)) { }
        public Task<SmartHomeDeviceInfoResponse?> GetDeviceInfoAsync(CancellationToken cancellationToken) => client.SendCommandAsync<SmartHomeDeviceInfoResponse>("system", "get_sysinfo", cancellationToken);
    }
}
