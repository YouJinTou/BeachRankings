using Amazon;
using System;

namespace BeachRankings.Core.Extensions
{
    public static class StringExtensions
    {
        public static RegionEndpoint ToRegionEndpoint(this string s)
        {
            switch (s)
            {
                case "eu-central-1":
                    return RegionEndpoint.EUCentral1;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static string ToSortDirection(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return Constants.View.Descending;
            }

            return (s == Constants.View.Ascending) ? s : Constants.View.Descending;
        }
    }
}
