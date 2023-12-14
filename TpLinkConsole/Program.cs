using EonData.SmartHome.TpLink.Devices;
using EonData.SmartHome.TpLink.SmartHomeProtocol;

using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

Dictionary<string, string> devices = new() {
    { "xmas", "192.168.86.56" }, //hs100
    { "leds", "192.168.86.57" },


    { "dryer", "192.168.86.32" },
    { "toytv", "192.168.86.26" }, //hs100
    { "fsun", "192.168.86.36" }, //hs100
    { "bdtbl", "192.168.86.37" },

    { "fish", "192.168.86.30" },
};
string deviceAddress = devices["leds"];

const string oldmac = "50:C7:BF:4A:41:D9";
const string oldhwid = "8006E965DE6F4384A1258DC65108EA9D183E774B";
//lat: -316702
//lng: 1157080

using CancellationTokenSource cts = new();
Console.CancelKeyPress += (s, e) =>
{
    cts.Cancel();
    e.Cancel = true;
};


try
{
    var client = new SmartHomeClient(deviceAddress);
    var a = await client.GetDeviceInfoAsync(cts.Token);
    Console.WriteLine($"got info for: {a.Alias}");
    //Console.ReadLine();
    //var b = await client.SetRelayStateAsync(true, cts.Token);
    //var c = await client.SetLocationAsync(234333 / 10000 * -1, 1800000 / 10000, cts.Token);
    var d = await client.CheckBootloaderAsync(cts.Token);
}
catch (SmartHomeException ex)
{
    Console.WriteLine($"smarthome protocol error: {ex.Message}");
}
catch (SmartHomeMalformedResponseException ex)
{
    Console.WriteLine($"smarthome response invalid: {ex.Message}");
}
//catch (SocketException ex)
//{
//    Console.WriteLine($"error: {ex.Message}");
//}
//catch (JsonException ex)
//{
//    Console.WriteLine($"error: {ex.Message}");
//}
//catch (CryptographicException ex)
//{
//    Console.WriteLine($"error: {ex.Message}");
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"error: {ex.Message}");
//}   


//var splug = new HS110(deviceAddress);
//var a = await splug.GetDeviceInfoAsync(cts.Token);
//Console.WriteLine($"got info for: {a.Alias}");
//var b = await splug.TurnLEDLightOff(cts.Token);
//0var c = await splug.TurnLEDLightOn(cts.Token);


/*
SendCommandAsync<SmartHomeResponse>("system", "set_mac_addr", new Dictionary<string, object>() { { "mac", MAC } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "set_device_id", new Dictionary<string, object>() { { "deviceId", deviceId } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "set_dev_location", new Dictionary<string, object>() { { "longitude", coordLong }, { "latitude", coordLat } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "set_hw_id", new Dictionary<string, object>() { { "hwId", hardwareId } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "test_check_uboot", cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "reboot", new Dictionary<string, object>() { { "delay", delay } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "get_dev_icon", cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "set_dev_icon", new Dictionary<string, object>() { { "icon", icon }, { "hash", hash } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "set_test_mode", new Dictionary<string, object>() { { "enable", enable } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "download_firmware", new Dictionary<string, object>() { { "url", firmwareUrl } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "get_download_state", cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "flash_firmware", cts.Token);
SendCommandAsync<SmartHomeResponse>("system", "check_new_config", cts.Token);
SendCommandAsync<SmartHomeResponse>("netif", "get_scaninfo", new Dictionary<string, object>() { { "refresh", refresh } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("netif", "set_stainfo", new Dictionary<string, object>() { { "ssid", ssid }, { "password", password }, { "key_type", keyType } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("cnCloud", "get_info", cts.Token); SendCommandAsync<SmartHomeResponse>("cnCloud", "get_intl_fw_list", cts.Token);
SendCommandAsync<SmartHomeResponse>("cnCloud", "set_server_url", new Dictionary<string, object>() { { "server", serverUrl } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("cnCloud", "bind", new Dictionary<string, object>() { { "username", username }, { "password", password } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("cnCloud", "unbind", cts.Token);
SendCommandAsync<SmartHomeResponse>("time", "get_time", cts.Token);
SendCommandAsync<SmartHomeResponse>("time", "get_timezone", cts.Token);
SendCommandAsync<SmartHomeResponse>("time", "set_timezone", new Dictionary<string, object>() {
                { "year", timeZoneSettings.DateTime.Year },
                { "month", timeZoneSettings.DateTime.Month },
                { "mday", timeZoneSettings.DateTime.Day },
                { "hour", timeZoneSettings.DateTime.Hour },
                { "min", timeZoneSettings.DateTime.Minute },
                { "sec", timeZoneSettings.DateTime.Second },
                { "index", timeZoneSettings.Index }
            }, cts.Token);
SendCommandAsync<SmartHomeResponse>("emeter", "get_realtime", cts.Token);
SendCommandAsync<SmartHomeResponse>("emeter", "get_vgain_igain", cts.Token);
SendCommandAsync<SmartHomeResponse>("emeter", "set_vgain_igain", new Dictionary<string, object>() { { "vgain", voltageGain }, { "igain", currentGain } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("emeter", "start_calibration", new Dictionary<string, object>() { { "vtarget", targetVoltage }, { "itarget", targetCurrent } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("emeter", "get_daystat", new Dictionary<string, object>() { { "month", month }, { "year", year } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("emeter", "get_monthstat", new Dictionary<string, object>() { { "year", year } }, cts.Token);
SendCommandAsync<SmartHomeResponse>("emeter", "erase_emeter_stat", cts.Token);
SendCommandAsync<SmartHomeResponse>("schedule", "get_next_action", cts.Token);
SendCommandAsync<SmartHomeResponse>("schedule", "get_rules", cts.Token);
SendCommandAsync<SmartHomeResponse>("count_down", "get_rules", cts.Token);
SendCommandAsync<SmartHomeResponse>("count_down", "delete_all_rules", cts.Token);
SendCommandAsync<SmartHomeResponse>("anti_theft", "get_rules", cts.Token);
SendCommandAsync<SmartHomeResponse>("anti_theft", "delete_all_rules", cts.Token);















public Task<T?> SendCommandAsync<T>(string commandType, string commandName, CancellationToken cancellationToken) where T : SmartHomeResponse => SendCommandAsync<T>(commandType, commandName, null, cancellationToken);
async Task<T?> SendCommandAsync<T>(string commandType, string commandName, IDictionary<string, object>? commandParameters, CancellationToken cancellationToken) where T : SmartHomeResponse
{
    // json processing options
    var jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    jsonOptions.Converters.Add(new JsonBoolConverter());

    // construct command
    var commandObject = new Dictionary<string, object> {
                { commandType, new Dictionary<string, object?>() {
                    { commandName, commandParameters }
                } }
            };
    string commandJson = JsonSerializer.Serialize(commandObject, jsonOptions);

    // send command
    string responseJson = await XSendDataAsync(commandJson, cancellationToken);

    // process response
    var response = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, T>>>(responseJson, jsonOptions);
    if (response == null)
    {
        return null;
    }
    if (!response.ContainsKey(commandType) || !response[commandType].ContainsKey(commandName))
    {
        throw new SmartHomeMalformedResponseException(commandType, commandName, deviceAddress, responseJson);
    }

    T? responseObject = response?[commandType][commandName];
    if ((responseObject?.ErrorCode ?? 0) != 0)
    {
        throw new SmartHomeException(commandType, commandName, deviceAddress, responseObject!.ErrorCode, responseObject?.ErrorMessage);
    }

    return responseObject;
}


async Task<string> XSendDataAsync(string data, CancellationToken cancellationToken)
{
    string response;

    using (var tcp = new TcpClient())
    {
        await tcp.ConnectAsync(deviceAddress, 9999, cancellationToken);
        await using var netStream = tcp.GetStream();
        var dataBytes = SmartHomeCypher.Encrypt(data);
        await netStream.WriteAsync(dataBytes, 0, dataBytes.Length, cancellationToken);
        var buffer = new byte[2048];
        await netStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
        response = SmartHomeCypher.Decrypt(buffer, buffer.Length);
    }

    return response;
}

internal class JsonBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32() == 1;
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            return bool.Parse(reader.GetString());
        }
        else
        {
            throw new JsonException();
        }
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
*/