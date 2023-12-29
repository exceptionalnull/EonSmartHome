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
//lat: -316702; lng: 1157080

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
    await client.SetRelayStateAsync(true, cts.Token);
    //var d = await client.SetLocationAsync(234333 / 10000 * -1, 1800000 / 10000, cts.Token);
    //var e = await client.GetMonthStatsAsync(2023, cts.Token);

    /* discovery */
    var discovery = new SmartHomeDeviceDiscovery();
    var r = await discovery.DiscoverDevicesAsync(cts.Token);
    int l = 0;
    foreach(var da in r)
    {
        try
        {
            var discoveryClient = new SmartHomeClient(da.Key);
            var i = await discoveryClient.GetDeviceInfoAsync(cts.Token);
            Console.WriteLine($"[{++l}] device at {da.Key} alias: {i.Alias} == {da.Value.Alias}");
        }
        catch(Exception ex) { Console.WriteLine($"error with device at {da}: {ex.Message}"); }
    }

    await client.SetRelayStateAsync(false, cts.Token);
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