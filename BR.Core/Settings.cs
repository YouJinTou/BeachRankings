using BR.Core.Helpers;

namespace BR.Core
{
    public class Settings
    {
        public string Stage => EnvHelper.Stage;

        public string EventBusTopic { get; set; }
    }
}
