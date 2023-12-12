using EonData.SmartHome.TpLink.SmartHomeProtocol;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.Devices
{
    public class HS110
    {
        private SmartHomeClient client;
        public HS110(SmartHomeClient protocolClient) => client = protocolClient;
        public HS110(string address) : this(new SmartHomeClient(address)) { }
        public Task<SmartHomeDeviceInfoResponse?> GetDeviceInfoAsync(CancellationToken cancellationToken) => client.GetDeviceInfoAsync(cancellationToken);
        public Task<SmartHomeResponse?> TurnPlugOffAsync(CancellationToken cancellationToken) => client.SetRelayState(false, cancellationToken);
        public Task<SmartHomeResponse?> TurnPlugOnAsync(CancellationToken cancellationToken) => client.SetRelayState(true, cancellationToken);
        public Task<SmartHomeResponse?> TurnLEDLightOn(CancellationToken cancellationToken) => client.SetLEDState(true, cancellationToken);
        public Task<SmartHomeResponse?> TurnLEDLightOff(CancellationToken cancellationToken) => client.SetLEDState(false, cancellationToken);
    }
}
