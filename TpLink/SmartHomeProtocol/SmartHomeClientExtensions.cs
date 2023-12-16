using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public static partial class SmartHomeClientExtensions
    {
        /* system */
        
        /// <summary>
        /// Gets the device info.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<SmartHomeDeviceInfoResponse> GetDeviceInfoAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeDeviceInfoResponse>("system", "get_sysinfo", cancellationToken);

        /// <summary>
        /// Sets the smart plug switch on or off.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="relayOn"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<SmartHomeResponse> SetRelayStateAsync(this SmartHomeClient client, bool relayOn, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_relay_state", new Dictionary<string, object>() { { "state", relayOn } }, cancellationToken);

        /// <summary>
        /// Factory resets the device.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="delay">Delay to wait before applying the reset.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<SmartHomeResponse> FactoryResetAsync(this SmartHomeClient client, int delay, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "reset", new Dictionary<string, object>() { { "delay", delay } }, cancellationToken);

        /// <summary>
        /// Turns the device's LED on or off.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="lightOn">Set to true to turn the LED on.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<SmartHomeResponse> SetLEDStateAsync(this SmartHomeClient client, bool lightOn, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_led_off", new Dictionary<string, object>() { { "off", !lightOn } }, cancellationToken);

        /// <summary>
        /// Sets the device's name.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="alias">Name for the device.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<SmartHomeResponse> SetAliasAsync(this SmartHomeClient client, string alias, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_dev_alias", new Dictionary<string, object>() { { "alias", alias } }, cancellationToken);

        /// <summary>
        /// Sets the device's MAC.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="MAC">MAC address</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>This only seems to return error -2. Needs further investigation.</remarks>
        public static Task<SmartHomeResponse> SetMACAsync(this SmartHomeClient client, string MAC, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_mac_addr", new Dictionary<string, object>() { { "mac", MAC } }, cancellationToken);

        /// <summary>
        /// Sets the device ID.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="deviceId">Device ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>This only seems to return error -2. Needs further investigation.</remarks>
        public static Task<SmartHomeResponse> SetDeviceIdAsync(this SmartHomeClient client, string deviceId, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_device_id", new Dictionary<string, object>() { { "deviceId", deviceId } }, cancellationToken);


        /// <summary>
        /// Sets the device's location.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="coordLat">Latitude</param>
        /// <param name="coordLong">Longitude</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>Expected values are still returning 'invalid argument'</remarks>
        public static Task<SmartHomeResponse> SetLocationAsync(this SmartHomeClient client, decimal coordLat, decimal coordLong, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_dev_location", new Dictionary<string, object>() { { "latitude", coordLat }, { "longitude", coordLong } }, cancellationToken);

        /// <summary>
        /// Set the device's hardware ID.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="hardwareId">Hardware ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>This only seems to return error -2. Needs further investigation.</remarks>
        public static Task<SmartHomeResponse> SetHardwareIdAsync(this SmartHomeClient client, string hardwareId, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_hw_id", new Dictionary<string, object>() { { "hwId", hardwareId } }, cancellationToken);

        /// <summary>
        /// Checks the device's bootloader.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>This only seems to return error -2. Needs further investigation.</remarks>
        public static Task<SmartHomeResponse> CheckBootloaderAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "test_check_uboot", cancellationToken);

        /// <summary>
        /// Reboots the device.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="delay">Delay to wait before rebooting.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<SmartHomeResponse> RebootAsync(this SmartHomeClient client, int delay, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "reboot", new Dictionary<string, object>() { { "delay", delay } }, cancellationToken);

        /// <summary>
        /// Gets the icon for the device.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>This only seems to return error -2. Needs further investigation.</remarks>
        public static Task<SmartHomeResponse> GetDeviceIconAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "get_dev_icon", cancellationToken);

        /// <summary>
        /// Sets the icon for the device.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="icon"></param>
        /// <param name="hash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>need to investigate valid inputs</remarks>
        public static Task<SmartHomeResponse> SetDeviceIconAsync(this SmartHomeClient client, string icon, string hash, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_dev_icon", new Dictionary<string, object>() { { "icon", icon }, { "hash", hash } }, cancellationToken);

        /// <summary>
        /// untested.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="enable"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>command only accepted coming from IP 192.168.1.100; untested.</remarks>
        public static Task<SmartHomeResponse> SetTestModeAsync(this SmartHomeClient client, bool enable, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "set_test_mode", new Dictionary<string, object>() { { "enable", enable } }, cancellationToken);

        /// <summary>
        /// untested.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="firmwareUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>untested.</remarks>
        public static Task<SmartHomeResponse> DownloadFirmwareAsync(this SmartHomeClient client, string firmwareUrl, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "download_firmware", new Dictionary<string, object>() { { "url", firmwareUrl } }, cancellationToken);

        /// <summary>
        /// untested.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>untested.</remarks>
        public static Task<SmartHomeResponse> GetDownloadStateAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "get_download_state", cancellationToken);

        /// <summary>
        /// untested.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>untested.</remarks>
        public static Task<SmartHomeResponse> FlashFirmwareAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "flash_firmware", cancellationToken);

        /// <summary>
        /// untested.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>untested.</remarks>
        public static Task<SmartHomeResponse> CheckNewConfigAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("system", "check_new_config", cancellationToken);



        /* network */

        public static Task<WifiNetworkListResponse> WifiNetworkScanAsync(this SmartHomeClient client, int refresh, CancellationToken cancellationToken) =>
            client.SendCommandAsync<WifiNetworkListResponse>("netif", "get_scaninfo", new Dictionary<string, object>() { { "refresh", refresh } }, cancellationToken);

        public static Task<SmartHomeResponse> WifiNetworConnectAsync(this SmartHomeClient client, string ssid, string password, WifiEncryption keyType, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("netif", "set_stainfo", new Dictionary<string, object>() { { "ssid", ssid }, { "password", password }, { "key_type", keyType } }, cancellationToken);


        /* cloud config */

        public static Task<SmartHomeResponse> GetCloudInfoAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("cnCloud", "get_info", cancellationToken);

        public static Task<SmartHomeResponse> GetFirmwareListAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("cnCloud", "get_intl_fw_list", cancellationToken);

        public static Task<SmartHomeResponse> SetServerUrlAsync(this SmartHomeClient client, string serverUrl, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("cnCloud", "set_server_url", new Dictionary<string, object>() { { "server", serverUrl } }, cancellationToken);

        public static Task<SmartHomeResponse> RegisterCloudAsync(this SmartHomeClient client, string username, string password, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("cnCloud", "bind", new Dictionary<string, object>() { { "username", username }, { "password", password } }, cancellationToken);

        public static Task<SmartHomeResponse> UnregisterCloudAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("cnCloud", "unbind", cancellationToken);
        
        
        /* time */

        public static Task<SmartHomeResponse> GetTimeAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("time", "get_time", cancellationToken);

        public static Task<SmartHomeResponse> GetTimeZoneAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("time", "get_timezone", cancellationToken);

        // TODO: this one needs further investigation.
        // { "time":{ "set_timezone":{ "year":2016,"month":1,"mday":1,"hour":10,"min":10,"sec":10,"index":42} } }
        public static Task<SmartHomeResponse> SetTimeZoneAsync(this SmartHomeClient client, SmartHomeTimezoneSettings timeZoneSettings, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("time", "set_timezone", new Dictionary<string, object>() {
                { "year", timeZoneSettings.DateTime.Year },
                { "month", timeZoneSettings.DateTime.Month },
                { "mday", timeZoneSettings.DateTime.Day },
                { "hour", timeZoneSettings.DateTime.Hour },
                { "min", timeZoneSettings.DateTime.Minute },
                { "sec", timeZoneSettings.DateTime.Second },
                { "index", timeZoneSettings.Index }
            }, cancellationToken);

        
        /* emeter */

        public static Task<SmartHomeResponse> GetRealtimeMetricsAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("emeter", "get_realtime", cancellationToken);

        // { "emeter":{ "get_vgain_igain":{ } } }
        public static Task<SmartHomeResponse> GetEMeterGainAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("emeter", "get_vgain_igain", cancellationToken);

        // { "emeter":{ "set_vgain_igain":{ "vgain":13462,"igain":16835} } }
        public static Task<SmartHomeResponse> SetEMeterGainAsync(this SmartHomeClient client, int voltageGain, int currentGain, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("emeter", "set_vgain_igain", new Dictionary<string, object>() { { "vgain", voltageGain }, { "igain", currentGain } }, cancellationToken);

        // { "emeter":{ "start_calibration":{ "vtarget":13462,"itarget":16835} } }
        public static Task<SmartHomeResponse> StartCalibrationAsync(this SmartHomeClient client, int targetVoltage, int targetCurrent, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("emeter", "start_calibration", new Dictionary<string, object>() { { "vtarget", targetVoltage }, { "itarget", targetCurrent } }, cancellationToken);

        // { "emeter":{ "get_daystat":{ "month":1,"year":2016} } }
        public static Task<SmartHomeResponse> GetDayStatsAsync(this SmartHomeClient client, int month, int year, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("emeter", "get_daystat", new Dictionary<string, object>() { { "month", month }, { "year", year } }, cancellationToken);

        // { "emeter":{ "get_monthstat":{ "year":2016} } }
        public static Task<SmartHomeResponse> GetMonthStatsAsync(this SmartHomeClient client, int year, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("emeter", "get_monthstat", new Dictionary<string, object>() { { "year", year } }, cancellationToken);

        // { "emeter":{ "erase_emeter_stat":null} }
        public static Task<SmartHomeResponse> EraseEMeterStatsAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("emeter", "erase_emeter_stat", cancellationToken);


        /* schedule */

        // { "schedule":{ "get_next_action":null} }
        public static Task<SmartHomeResponse> GetNextActionAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("schedule", "get_next_action", cancellationToken);

        // { "schedule":{ "get_rules":null} }
        public static Task<SmartHomeResponse> GetRulesAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("schedule", "get_rules", cancellationToken);

        // { "schedule":{ "add_rule": {...}, "set_overall_enable":{ "enable":1} } }
        public static Task<SmartHomeResponse?> AddScheduleRuleAsync(this SmartHomeClient client, SmartHomeScheduleRule rule, bool enabled, CancellationToken cancellationToken) => throw new NotImplementedException();
        //    client.SendCommandAsync<SmartHomeResponse>("schedule", "add_rule", new { rule, set_overall_enable = enabled }, cancellationToken);

        // { "schedule":{ "edit_rule": {...} } }
        public static Task<SmartHomeResponse> EditScheduleRuleAsync(this SmartHomeClient client, SmartHomeScheduleRule rule, CancellationToken cancellationToken) => throw new NotImplementedException();
        //    client.SendCommandAsync<SmartHomeResponse>("schedule", "edit_rule", rule, cancellationToken);

        // { "schedule":{ "delete_rule": { "id":"4B44932DFC09780B554A740BC1798CBC"} } }
        public static Task<SmartHomeResponse> DeleteScheduleRuleAsync(this SmartHomeClient client, string ruleId, CancellationToken cancellationToken) => throw new NotImplementedException();
        //    client.SendCommandAsync<SmartHomeResponse>("schedule", "delete_rule", new { id = ruleId }, cancellationToken);

        // { "schedule":{ "delete_all_rules":null,"erase_runtime_stat":null} }
        public static Task<SmartHomeResponse> DeleteAllScheduleRulesAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("schedule", "delete_all_rules", cancellationToken);

        // { "count_down":{ "get_rules":null} }
        public static Task<SmartHomeResponse> GetCountDownRulesAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("count_down", "get_rules", cancellationToken);

        // { "count_down":{ "add_rule":{ "enable":1,"delay":1800,"act":1,"name":"turn on"} } }
        public static Task<SmartHomeResponse> AddCountDownRuleAsync(this SmartHomeClient client, SmartHomeCountDownRule rule, CancellationToken cancellationToken) => throw new NotImplementedException();
        //    client.SendCommandAsync<SmartHomeResponse>("count_down", "add_rule", rule, cancellationToken);

        // { "count_down":{ "edit_rule":{ "enable":1,"id":"7C90311A1CD3227F25C6001D88F7FC13","delay":1800,"act":1,"name":"turn on"} } }
        public static Task<SmartHomeResponse> EditCountDownRuleAsync(this SmartHomeClient client, SmartHomeCountDownRule rule, CancellationToken cancellationToken) => throw new NotImplementedException();
        //    client.SendCommandAsync<SmartHomeResponse>("count_down", "edit_rule", rule, cancellationToken);

        // { "count_down":{ "delete_rule":{ "id":"7C90311A1CD3227F25C6001D88F7FC13"} } }
        public static Task<SmartHomeResponse> DeleteCountDownRuleAsync(this SmartHomeClient client, string ruleId, CancellationToken cancellationToken) => throw new NotImplementedException();
        //    client.SendCommandAsync<SmartHomeResponse>("count_down", "delete_rule", new { id = ruleId }, cancellationToken);

        // { "count_down":{ "delete_all_rules":null} }
        public static Task<SmartHomeResponse> DeleteAllCountDownRulesAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("count_down", "delete_all_rules", cancellationToken);

        // { "anti_theft":{ "get_rules":null} }
        public static Task<SmartHomeResponse> GetAntiTheftRulesAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("anti_theft", "get_rules", cancellationToken);

        // { "anti_theft":{ "add_rule": {...}, "set_overall_enable":1} }
        public static Task<SmartHomeResponse?> AddAntiTheftRuleAsync(this SmartHomeClient client, SmartHomeAntiTheftRule rule, bool enable, CancellationToken cancellationToken) => throw new NotImplementedException();
        //    client.SendCommandAsync<SmartHomeResponse>("anti_theft", "add_rule", new { rule, set_overall_enable = enable }, cancellationToken);

        // { "anti_theft":{ "edit_rule": {...}, "set_overall_enable":1} }
        public static Task<SmartHomeResponse> EditAntiTheftRuleAsync(this SmartHomeClient client, SmartHomeAntiTheftRule rule, bool enable, CancellationToken cancellationToken) => throw new NotImplementedException();
        //    client.SendCommandAsync<SmartHomeResponse>("anti_theft", "edit_rule", new { rule, set_overall_enable = enable }, cancellationToken);

        // { "anti_theft":{ "delete_rule":{ "id":"E36B1F4466B135C1FD481F0B4BFC9C30"} } }
        public static Task<SmartHomeResponse> DeleteAntiTheftRuleAsync(this SmartHomeClient client, string ruleId, CancellationToken cancellationToken) => throw new NotImplementedException();
        //    client.SendCommandAsync<SmartHomeResponse>("anti_theft", "delete_rule", new { id = ruleId }, cancellationToken);

        // { "anti_theft":{ "delete_all_rules":null} }
        public static Task<SmartHomeResponse> DeleteAllAntiTheftRulesAsync(this SmartHomeClient client, CancellationToken cancellationToken) =>
            client.SendCommandAsync<SmartHomeResponse>("anti_theft", "delete_all_rules", cancellationToken);

    }
}