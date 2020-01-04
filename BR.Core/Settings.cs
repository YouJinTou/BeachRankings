using BR.Core.Tools;

namespace BR.Core
{
    public class Settings
    {
        public string Stage => Env.Stage;

        public string MessagingTopic { get; set; } = Env.GetEnvironmentVariable("MessagingTopic");

        public string UsersTable { get; set; } = ToStage("Users");

        public string EventLogTable { get; set; } = ToStage("EventLog");

        public string PlacesTable { get; set; } = ToStage("Places");

        public string IndexTable { get; set; } = ToStage("Index");

        private static string ToStage(string property)
        {
            return $"{Env.Stage}_{property}";
        }
    }
}
