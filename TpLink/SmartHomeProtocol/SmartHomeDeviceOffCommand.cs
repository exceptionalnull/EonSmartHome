
namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeDeviceOffCommand : SmartHomeCommandBase
    {
        public override string CommandCategory => "system";

        public override string CommandName => "set_relay_state";

        protected override Dictionary<string, object> CommandParameters => new() { { "state", 0 } };
    }
}
