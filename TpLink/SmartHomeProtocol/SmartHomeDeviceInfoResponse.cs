using System.Text.Json.Serialization;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeDeviceInfoResponse : SmartHomeResponse
    {
        [JsonPropertyName("sw_ver")]
        public string SoftwareVersion { get; set; }

        [JsonPropertyName("hw_ver")]
        public string HardwareVersion { get; set; }

        public string Type { get; set; }

        public string Model { get; set; }

        public string MAC { get; set; }

        [JsonPropertyName("dev_name")]
        public string DeviceName { get; set; }

        public string Alias { get; set; }

        [JsonPropertyName("relay_state")]
        public bool IsRelay { get; set; }

        [JsonPropertyName("on_time")]
        public int OnTime { get; set; }

        [JsonPropertyName("active_mode")]
        public string ActiveMode { get; set; }

        public string Feature { get; set; }

        [JsonPropertyName("updating")]
        public bool IsUpdating { get; set; }

        public int RSSI { get; set; }

        [JsonPropertyName("led_off")]
        public bool IsLedOff { get; set; }

        [JsonPropertyName("longitude_i")]
        public int Longitude { get; set; }

        [JsonPropertyName("latitude_i")]
        public int Latitude { get; set; }

        [JsonPropertyName("hwId")]
        public string HardwareId { get; set; }

        public string OemId { get; set; }

        [JsonPropertyName("fwId")]
        public string FirmwareId { get; set; }

        public string DeviceId { get; set; }
    }
}
