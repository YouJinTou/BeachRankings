using BeachRankings.Core.Tools;
using System;

namespace BeachRankings.Core.Models
{
    public class Settings
    {
        public string AwsRegion
        {
            get
            {
                var region = Environment.GetEnvironmentVariable(Constants.Env.AwsRegion);

                return InputValidator.ReturnOrThrowIfNullOrWhiteSpace(
                    region, Constants.Env.AwsRegion);
            }
        }
            
        public string AwsAccessToken
        {
            get
            {
                var accessKey = Environment.GetEnvironmentVariable(Constants.Env.AwsAccessToken);

                return InputValidator.ReturnOrThrowIfNullOrWhiteSpace(
                    accessKey, Constants.Env.AwsAccessToken);
            }
        }

        public string AwsSecret
        {
            get
            {
                var secret = Environment.GetEnvironmentVariable(Constants.Env.AwsSecret);

                return InputValidator.ReturnOrThrowIfNullOrWhiteSpace(
                    secret, Constants.Env.AwsSecret);
            }
        }
    }
}
