using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeTimeZoneResponse : SmartHomeResponse
    {
        [JsonPropertyName("index")]
        public int TimeZoneIndex { get; set; }
    }
}
