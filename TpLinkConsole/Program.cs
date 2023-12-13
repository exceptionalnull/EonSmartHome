using EonData.SmartHome.TpLink.Devices;
using EonData.SmartHome.TpLink.SmartHomeProtocol;

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

using CancellationTokenSource cts = new();
Console.CancelKeyPress += (s, e) =>
{
    cts.Cancel();
    e.Cancel = true;
};

var client = new SmartHomeClient(deviceAddress);
var a = await client.GetDeviceInfoAsync(cts.Token);
Console.WriteLine($"got info for: {a.Alias}");
//Console.ReadLine();
//var b = await client.SetRelayStateAsync(false, cts.Token);
var c = await client.SetMACAsync("AA:BB:CC:DD:EE:FF", cts.Token);


//var splug = new HS110(deviceAddress);
//var a = await splug.GetDeviceInfoAsync(cts.Token);
//Console.WriteLine($"got info for: {a.Alias}");
//var b = await splug.TurnLEDLightOff(cts.Token);
//var c = await splug.TurnLEDLightOn(cts.Token);