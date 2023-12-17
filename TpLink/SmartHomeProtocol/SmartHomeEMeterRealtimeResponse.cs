using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeEMeterRealtimeResponse : SmartHomeResponse
    {
        /// <summary>
        /// Voltage in millivolts
        /// </summary>
        [JsonPropertyName("voltage_mv")]
        public int Voltage { get; set; }

        /// <summary>
        /// Current in milliamps
        /// </summary>
        [JsonPropertyName("current_ma")]
        public int Current { get; set; }

        /// <summary>
        /// Power in milliwatts
        /// </summary>
        [JsonPropertyName("power_mw")]
        public int Power { get; set; }

        /// <summary>
        /// Total watt hours
        /// </summary>
        [JsonPropertyName("total_wh")]
        public int TotalWattHours { get; set; }
    }
}
