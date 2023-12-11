using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public static class SmartHomeClientSystemExtensions
    {
        public static Task<SmartHomeDeviceInfoResponse?> GetDeviceInfoAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeDeviceInfoResponse>("system", "get_sysinfo", cancellationToken);

        public static Task<SmartHomeResponse?> SetRelayState(this SmartHomeClient client, bool setRelayOn, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_relay_state", new Dictionary<string, object>() { { "state", 0 } }, cancellationToken);

        public static Task<SmartHomeResponse?> FactoryReset(this SmartHomeClient client, int delay, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "reset", new Dictionary<string, object>() { { "delay", delay } }, cancellationToken);

        public static Task<SmartHomeResponse?> SetDeviceLED(this SmartHomeClient client, bool lightOn, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("", "", new Dictionary<string, object>() { { "", lightOn } }, cancellationToken);

        /*
         Set Device Alias
{"system":{"set_dev_alias":{"alias":"supercool plug"}}}

Set MAC Address
{"system":{"set_mac_addr":{"mac":"50-C7-BF-01-02-03"}}}

Set Device ID
{"system":{"set_device_id":{"deviceId":"0123456789ABCDEF0123456789ABCDEF01234567"}}}

Set Hardware ID
{"system":{"set_hw_id":{"hwId":"0123456789ABCDEF0123456789ABCDEF"}}}

Set Location
{"system":{"set_dev_location":{"longitude":6.9582814,"latitude":50.9412784}}}

Perform uBoot Bootloader Check
{"system":{"test_check_uboot":null}}

Get Device Icon
{"system":{"get_dev_icon":null}}

Set Device Icon
{"system":{"set_dev_icon":{"icon":"xxxx","hash":"ABCD"}}}

Set Test Mode (command only accepted coming from IP 192.168.1.100)
{"system":{"set_test_mode":{"enable":1}}}

Download Firmware from URL
{"system":{"download_firmware":{"url":"http://...."}}}

Get Download State
{"system":{"get_download_state":{}}}

Flash Downloaded Firmware
{"system":{"flash_firmware":{}}}

Check Config
{"system":{"check_new_config":null}}
        */
    }
}
