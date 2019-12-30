using BR.Core;
using BR.Core.Helpers;

namespace BR.Iam.Configuration
{
    internal class IamSettings : Settings
    {
        public string UsersTable { get; set; } = $"Users_{EnvHelper.Stage}";
    }
}
