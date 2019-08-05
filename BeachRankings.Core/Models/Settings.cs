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
                var region = Environment.GetEnvironmentVariable("AWS_REGION");

                return InputValidator.ReturnOrThrowIfNullOrWhiteSpace(region);
            }
        }
            
        public string AwsAccessToken
        {
            get
            {
                var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_TOKEN");

                return InputValidator.ReturnOrThrowIfNullOrWhiteSpace(accessKey);
            }
        }

        public string AwsSecret
        {
            get
            {
                var secret = Environment.GetEnvironmentVariable("AWS_SECRET");

                return InputValidator.ReturnOrThrowIfNullOrWhiteSpace(secret);
            }
        }
    }
}
