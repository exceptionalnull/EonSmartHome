using EonData.SmartHome.TpLink.Devices;
using EonData.SmartHome.TpLink.SmartHomeProtocol;

using System.Runtime.CompilerServices;
Dictionary<string, string> devices = new() {
    { "dryer", "192.168.86.32" },
    { "toytv", "192.168.86.26" },
    { "fsun", "192.168.86.36" },
    { "xmas", "192.168.86.56" }
};
string deviceAddress = devices["xmas"];


using CancellationTokenSource cts = new();
Console.CancelKeyPress += (s, e) =>
{
    cts.Cancel();
    e.Cancel = true;
};

var client = new SmartHomeClient(deviceAddress);
var a = await client.GetDeviceInfoAsync(cts.Token);
Console.WriteLine($"got info for: {a.Alias}");
Console.ReadLine();
//var b = await client.SetRelayStateAsync(true, cts.Token);


//var splug = new HS110(deviceAddress);
//var a = await splug.GetDeviceInfoAsync(cts.Token);
//Console.WriteLine($"got info for: {a.Alias}");
//var b = await splug.TurnLEDLightOff(cts.Token);
//var c = await splug.TurnLEDLightOn(cts.Token);