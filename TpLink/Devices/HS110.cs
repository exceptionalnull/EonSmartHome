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
        /* system */
        public Task<SmartHomeDeviceInfoResponse> GetDeviceInfoAsync(CancellationToken cancellationToken) => client.GetDeviceInfoAsync(cancellationToken);
        public Task TurnPlugOffAsync(CancellationToken cancellationToken) => client.SetRelayStateAsync(false, cancellationToken);
        public Task TurnPlugOnAsyncAsync(CancellationToken cancellationToken) => client.SetRelayStateAsync(true, cancellationToken);
        public Task TurnLEDLightOnAsync(CancellationToken cancellationToken) => client.SetLEDStateAsync(true, cancellationToken);
        public Task TurnLEDLightOffAsync(CancellationToken cancellationToken) => client.SetLEDStateAsync(false, cancellationToken);
        public Task SetAliasAsync(string alias, CancellationToken cancellationToken) => client.SetAliasAsync(alias, cancellationToken);
        public Task RebootAsync(int delay, CancellationToken cancellationToken) => client.RebootAsync(delay, cancellationToken);
        /* wifi */

        /* time */
        public Task<SmartHomeTimeResponse> GetTimeAsync(CancellationToken cancellationToken) => client.GetTimeAsync(cancellationToken);
        public Task<SmartHomeTimeZoneResponse> GetZimeZoneAsync(CancellationToken cancellationToken) => client.GetTimeZoneAsync(cancellationToken);
        /* emeter */
        public Task<IEnumerable<SmartHomeEMeterDayStat>> GetDailyStatsAsync(int year, int month, CancellationToken cancellationToken) => client.GetDayStatsAsync(year, month, cancellationToken);
        public Task<IEnumerable<SmartHomeEMeterMonthStat>> GetMonthlyStatsAsync(int year, CancellationToken cancellationToken) => client.GetMonthStatsAsync(year, cancellationToken);
        public Task<SmartHomeEMeterRealtimeResponse> GetRealtimeStatsAsync(CancellationToken cancellationToken) => client.GetRealtimeMetricsAsync(cancellationToken);
    }
}
