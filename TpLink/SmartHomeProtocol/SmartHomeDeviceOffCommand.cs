namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeDeviceOffCommand : SmartHomeCommandBase
    {
        public override string CommandCategory => "system";

        public override string CommandName => "set_relay_state";

        public SmartHomeDeviceOffCommand()
        {
            CommandParameters.Add("state", 0);
        }
    }
}
