namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeDeviceRebootCommand : SmartHomeCommandBase
    {

        public override string CommandCategory => "system";

        public override string CommandName => "reboot";

        protected override Dictionary<string, object> CommandParameters => new() { { "delay", DelaySeconds } };

        public int DelaySeconds { get; set; } = 1;
    }
}
