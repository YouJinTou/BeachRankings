using System.Linq;

namespace BR.Seed
{
    class Program
    {
        static void Main(string[] args)
        {
            //PlacesSeed.SeedPlacesAsync().Wait();

            //IndexSeed.SeedIndexAsync().Wait();

            BeachesSeed.SeedBeachesAsync().Wait();
        }
    }
}
