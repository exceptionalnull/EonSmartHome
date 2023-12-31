using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeEMeterDayStatsResponse : SmartHomeResponse
    {
        [JsonPropertyName("day_list")]
        public IEnumerable<SmartHomeEMeterDayStat> Days { get; set; }
    }
}
