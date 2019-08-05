using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Models;
using BeachRankings.Loader.Abstractions;
using BeachRankings.Loader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BeachRankings.Loader.Services
{
    public class SeedService : ISeedService
    {
        private class PermutedBeachEqualityComparer : IEqualityComparer<Beach>
        {
            public bool Equals(Beach x, Beach y)
            {
                return ($"{x.Id}%{x.Location}" == $"{y.Id}%{y.Location}");
            }

            public int GetHashCode(Beach obj)
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    // Suitable nullity checks etc, of course :)
                    hash = hash * 23 + obj.Id.GetHashCode();
                    hash = hash * 23 + obj.Location.GetHashCode();
                    return hash;
                }
            }
        }

        private readonly IRepository<Location> locationsRepo;
        private readonly IRepository<Beach> beachesRepo;

        public SeedService(IRepository<Location> locationsRepo, IRepository<Beach> beachesRepo)
        {
            this.locationsRepo = locationsRepo;
            this.beachesRepo = beachesRepo;
        }

        public async Task SeedLocationsAsync()
        {
            var seed = this.GetSeed();
            var locations = seed.GetLocations();

            await this.locationsRepo.AddManyAsync(
                locations, 60, (int)TimeSpan.FromSeconds(60).TotalMilliseconds);
        }

        public async Task SeedBeachesAsync()
        {
            var beaches = this.GetBeaches().ToList();

            await this.beachesRepo.AddManyAsync(
                beaches, 60, (int)TimeSpan.FromSeconds(60).TotalMilliseconds);
        }

        private Seed GetSeed()
        {
            using (var sr = new StreamReader(@"C:\Users\dandr\Desktop\seed.xml"))
            {
                var serializer = new XmlSerializer(typeof(Seed));
                var seed = (Seed)serializer.Deserialize(sr);

                return seed;
            }
        }

        private IEnumerable<Beach> GetBeaches()
        {
            using (var sr = new StreamReader(@"C:\Users\dandr\Desktop\beaches.csv"))
            {
                string line;
                var beaches = new HashSet<Beach>(new PermutedBeachEqualityComparer());

                while ((line = sr.ReadLine()) != null)
                {
                    var tokens = line.Split('\t');
                    var beach = new Beach(
                        this.GetToken(tokens[0]),
                        this.GetToken(tokens[1]),
                        this.GetToken(tokens[2]),
                        this.GetToken(tokens[3]),
                        this.GetToken(tokens[4]),
                        this.GetToken(tokens[5]),
                        this.GetToken(tokens[6]),
                        this.GetToken(tokens[7]),
                        this.GetToken(tokens[8]),
                        double.TryParse(tokens[9], out double a) ? a : (double?)null,
                        double.TryParse(tokens[10], out double b) ? b : (double?)null,
                        double.TryParse(tokens[11], out double c) ? c : (double?)null,
                        double.TryParse(tokens[12], out double d) ? d : (double?)null,
                        double.TryParse(tokens[13], out double e) ? e : (double?)null,
                        double.TryParse(tokens[14], out double f) ? f : (double?)null,
                        double.TryParse(tokens[15], out double g) ? g : (double?)null,
                        double.TryParse(tokens[16], out double h) ? h : (double?)null,
                        double.TryParse(tokens[17], out double i) ? i : (double?)null,
                        double.TryParse(tokens[18], out double j) ? j : (double?)null,
                        double.TryParse(tokens[19], out double k) ? k : (double?)null,
                        double.TryParse(tokens[20], out double l) ? l : (double?)null,
                        double.TryParse(tokens[21], out double m) ? m : (double?)null,
                        double.TryParse(tokens[22], out double n) ? n : (double?)null,
                        double.TryParse(tokens[23], out double o) ? o : (double?)null,
                        double.TryParse(tokens[24], out double p) ? p : (double?)null);
                    var permutedBeaches = this.PermuteBeach(beach);

                    foreach (var permutedBeach in permutedBeaches)
                    {
                        beaches.Add(permutedBeach);
                    }
                }

                return beaches;
            }
        }

        private string GetToken(string token)
        {
            return (token == "NULL") ? null : token.Trim(new char[] { ' ', '"' });
        }

        private IEnumerable<Beach> PermuteBeach(Beach beach)
        {
            var locationTokens =
                    beach.Location.Split('_')
                    .Where(lt => lt != beach.Continent)
                    .ToArray();
            var beaches = new List<Beach> { beach };

            foreach (var permutation in locationTokens)
            {
                var permutedBeach = new Beach(
                    beach.Name,
                    beach.Continent,
                    null,
                    null,
                    null,
                    null,
                    null,
                    beach.WaterBody,
                    null);
                permutedBeach.Id = beach.Id;
                permutedBeach.Location = permutation;

                beaches.Add(permutedBeach);
            }

            return beaches;
        }
    }
}
