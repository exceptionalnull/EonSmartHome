namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public static partial class SmartHomeClientExtensions
    {
        public class SmartHomeAntiTheftRule
        {
            public int StimeOpt { get; set; }
            public int[] Wday { get; set; }
            public int Smin { get; set; }
            public int Enable { get; set; }
            public int Frequency { get; set; }
            public int Repeat { get; set; }
            public int EtimeOpt { get; set; }
            public int Duration { get; set; }
            public string Name { get; set; }
            public int LastFor { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            public int Longitude { get; set; }
            public int Day { get; set; }
            public int Latitude { get; set; }
            public int Force { get; set; }
            public int Emin { get; set; }
        }


    }
}



/*
public static Task<SmartHomeResponse?> SetThing(this SmartHomeClient client, string thing, CancellationToken cancellationToken) =>
    client.SendCommandAsync<SmartHomeResponse>("system", "", new Dictionary<string, object>() { { "", thing } }, cancellationToken);

*/