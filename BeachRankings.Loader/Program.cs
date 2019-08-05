using BeachRankings.Loader.Abstractions;
using BeachRankings.Loader.Services;

namespace BeachRankings.Loader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var seedService = IoC.GetService<ISeedService>();

            //seedService.SeedLocationsAsync().Wait();
            seedService.SeedBeachesAsync().Wait();
        }
    }
}
