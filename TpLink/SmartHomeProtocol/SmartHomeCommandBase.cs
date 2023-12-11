using System.Text.Json;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public abstract class SmartHomeCommandBase
    {
        public abstract string CommandCategory { get; }
        public abstract string CommandName { get; }
        protected abstract Dictionary<string, object> CommandParameters { get; }

        public string GetCommandJson() => JsonSerializer.Serialize(new Dictionary<string, object> {
                { CommandCategory, new Dictionary<string, object?>() {
                    { CommandName, (CommandParameters.Count == 0) ? null : CommandParameters } // if the parameter collection is empty then use null
                } }
            });
    }
}
