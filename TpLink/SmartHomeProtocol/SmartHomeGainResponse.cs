using System.Text.Json.Serialization;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeEMeterGainResponse : SmartHomeResponse
    {
        [JsonPropertyName("vgain")]
        public int VoltageGain { get; set; }

        [JsonPropertyName("igain")]
        public int CurrentGain { get; set; }
    }
}
