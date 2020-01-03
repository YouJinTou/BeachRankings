using System;
using System.Collections.Generic;

namespace BR.Seed
{
    internal class Program
    {
        static void Main()
        {
            var options = new List<string>
            {
                "1. Places",
                "2. Index",
                "3. Beaches",
                "4. Reviews"
            };
            var formattedOptions = string.Join(Environment.NewLine, options);

            Console.WriteLine($"Select a seed option:{Environment.NewLine}{formattedOptions}");

            switch (Console.ReadLine())
            {
                case "1":
                    PlacesSeed.SeedPlacesAsync().Wait();
                    break;
                case "2":
                    IndexSeed.SeedIndexAsync().Wait();
                    break;
                case "3":
                    BeachesSeed.SeedBeachesAsync().Wait();
                    break;
                case "4":
                    ReviewsSeed.SeedReviewsAsync().Wait();
                    break;
                default:
                    throw new InvalidOperationException("Invalid option.");
            }
        }
    }
}
