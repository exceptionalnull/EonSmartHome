using EonData.SmartHome.TpLink.SmartHomeProtocol;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.Devices
{
    public class HS110 : SmartHomeDeviceBase
    {
        public HS110(SmartHomeClient protocolClient) : base(protocolClient) { }
        public HS110(string address) : this(new SmartHomeClient(address)) { }
        public Task<SmartHomeDeviceInfoResponse?> GetDeviceInfoAsync(CancellationToken cancellationToken) => client.GetDeviceInfoAsync(cancellationToken);
        public Task<SmartHomeResponse?> TurnPlugOffAsync(CancellationToken cancellationToken) => client.SetRelayStateAsync(false, cancellationToken);
        public Task<SmartHomeResponse?> TurnPlugOnAsync(CancellationToken cancellationToken) => client.SetRelayStateAsync(true, cancellationToken);
        public Task<SmartHomeResponse?> TurnLEDLightOn(CancellationToken cancellationToken) => client.SetLEDStateAsync(true, cancellationToken);
        public Task<SmartHomeResponse?> TurnLEDLightOff(CancellationToken cancellationToken) => client.SetLEDStateAsync(false, cancellationToken);
    }
}
