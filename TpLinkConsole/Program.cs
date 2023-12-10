// See https://aka.ms/new-console-template for more information
using EonData.SmartHome.TpLink;

Console.WriteLine("Hello, World!");

var shp = new TpLinkSmartHomeClient("192.168.86.32");
var response = await shp.SendDataAsync(@"{""system"":{""get_sysinfo"":null}}");
Console.WriteLine(response);