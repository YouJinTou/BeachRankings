using BR.Core.Helpers;

namespace BR.Iam.Configuration
{
    internal class Settings
    {
        public string UsersTable { get; set; } = $"Users_{EnvHelper.Stage}";
    }
}
