using System;

namespace BR.Core.Tools
{
    public static class Env
    {
        public static string GetEnvironmentVariable(string name, string fallback = null)
        {
            var value = Environment.GetEnvironmentVariable(name);

            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        public static string Stage => 
            GetEnvironmentVariable(Constants.Env.Stage, Constants.Env.QA);
    }
}
