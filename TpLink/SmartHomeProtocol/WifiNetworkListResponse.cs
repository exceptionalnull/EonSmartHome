using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class WifiNetworkListResponse : SmartHomeResponse
    {
        [JsonPropertyName("ap_list")]
        public IEnumerable<WifiNetwork> WifiNetworks { get; set; }
    }
}
