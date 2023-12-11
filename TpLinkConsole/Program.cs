using EonData.SmartHome.TpLink.Devices;
using EonData.SmartHome.TpLink.SmartHomeProtocol;

using System.Runtime.CompilerServices;

const string deviceAddress = "192.168.86.32";
//var shp = new SmartHomeProtocolClient(deviceAddress);
//var cmd = new SmartHomeDeviceInfoCommand();
//var rsp = await shp.SendCommandAsync<SmartHomeDeviceInfoResponse>(cmd, new CancellationToken());
//Console.WriteLine(rsp);

using CancellationTokenSource cts = new();
Console.CancelKeyPress += (s, e) =>
{
    cts.Cancel();
    e.Cancel = true;
};

var splug = new HS110(deviceAddress);
var r = await splug.GetDeviceInfoAsync(cts.Token);
Console.WriteLine($"got info for: {r.Alias}");