namespace BR.Seed
{
    class Program
    {
        static void Main(string[] args)
        {
            //PlacesSeed.SeedPlacesAsync().Wait();

            IndexSeed.SeedIndexAsync().Wait();
        }
    }
}
