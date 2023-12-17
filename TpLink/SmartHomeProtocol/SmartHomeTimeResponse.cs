using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeTimeResponse : SmartHomeResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        [JsonPropertyName("mday")]
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public int Sec { get; set; }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day, Hour, Min, Sec);
        }
    }
}
