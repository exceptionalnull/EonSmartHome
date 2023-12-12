namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public static partial class SmartHomeClientExtensions
    {
        /* countdown */

        public class SmartHomeCountDownRule
        {
            public int Enable { get; set; }
            public int Delay { get; set; }
            public int Act { get; set; }
            public string Name { get; set; }
        }

    }
}



/*
public static Task<SmartHomeResponse?> SetThing(this SmartHomeClient client, string thing, CancellationToken cancellationToken) =>
    client.SendCommandAsync<SmartHomeResponse>("system", "", new Dictionary<string, object>() { { "", thing } }, cancellationToken);

*/