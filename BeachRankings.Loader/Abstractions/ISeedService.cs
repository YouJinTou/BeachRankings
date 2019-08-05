using System.Threading.Tasks;

namespace BeachRankings.Loader.Abstractions
{
    public interface ISeedService
    {
        Task SeedLocationsAsync();

        Task SeedBeachesAsync();
    }
}
