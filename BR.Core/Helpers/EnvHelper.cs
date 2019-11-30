using System;

namespace BR.Core.Helpers
{
    public static class EnvHelper
    {
        public static string GetEnvironmentVariable(string name, string fallback = null)
        {
            var value = Environment.GetEnvironmentVariable(name);

            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        public static string Stage => 
            GetEnvironmentVariable(Constants.Env.Stage, Constants.Env.Dev);
    }
}
