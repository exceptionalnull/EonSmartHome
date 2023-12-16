namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeCommandFactory
    {
        public SmartHomeCommand<T> CreateCommand<T>(string target, string operation) where T : SmartHomeResponse =>
            new SmartHomeCommand<T>(target, operation);

        public SmartHomeCommand<T> CreateCommand<T>(string target, string operation, Dictionary<string, object> parameters) where T : SmartHomeResponse =>
            new SmartHomeCommand<T>(target, operation, parameters);
    }
}