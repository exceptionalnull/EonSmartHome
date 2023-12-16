using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class WifiNetwork
    {
        public string SSID { get; set; }

        [JsonPropertyName("key_type")]
        //public int EncryptionType { get; set; }
        public WifiEncryption EncryptionType { get; set; }
    }
}
