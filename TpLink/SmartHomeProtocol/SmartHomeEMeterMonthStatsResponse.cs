using System.Text.Json.Serialization;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeEMeterMonthStatsResponse : SmartHomeResponse
    {
        [JsonPropertyName("month_list")]
        public IEnumerable<SmartHomeEMeterMonthStat> Months { get; set; }
    }
}
