using EonData.SmartHome.TpLink.SmartHomeProtocol;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.Devices
{
    internal class HS110
    {
        private TpLinkSmartHomeClient client;
        public HS110(TpLinkSmartHomeClient protocolClient) => client = protocolClient;
        public HS110(string address) : this(new TpLinkSmartHomeClient(address)) { }

        public Task<SmartHomeDeviceInfoResponse?> GetDeviceInfoAsync(CancellationToken cancellationToken) => client.SendCommandAsync<SmartHomeDeviceInfoResponse>(new SmartHomeDeviceInfoCommand(), cancellationToken);
    }
}
