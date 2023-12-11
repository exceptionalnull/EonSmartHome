namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeDeviceInfoCommand : SmartHomeCommandBase
    {
        public override string CommandCategory => "system";

        public override string CommandName => "get_sysinfo";
    }
}
