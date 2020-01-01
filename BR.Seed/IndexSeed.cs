using BR.Core.Cloud.Aws;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.IndexService.Processors;
using BR.Seed.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BR.Seed
{
    public static class IndexSeed
    {
        public static async Task SeedIndexAsync()
        {
            var db = new DynamoRepository<IndexEntry>("Index");
            var indices = new List<IndexEntry>(GetIndices());

            await db.AddManyAsync(indices);
        }

        private static IEnumerable<IndexEntry> GetIndices()
        {
            using var reader = new StreamReader("seed.xml");
            var serializer = new XmlSerializer(typeof(Seed));
            var seed = (Seed)serializer.Deserialize(reader);
            var tokens = new HashSet<IndexToken>();

            foreach (var continent in seed.Continent)
            {
                tokens.Add(new IndexToken(continent.name, PlaceType.Continent));

                foreach (var country in continent.Country.NewIfNull())
                {
                    tokens.Add(new IndexToken(country.name, PlaceType.Country));

                    tokens.Add(new IndexToken(country.waterBody, PlaceType.WaterBody));

                    foreach (var l1 in country.L1.NewIfNull())
                    {
                        tokens.Add(new IndexToken(l1.name, PlaceType.L1));

                        tokens.Add(new IndexToken(l1.waterBody, PlaceType.WaterBody));

                        foreach (var l2 in l1.L2.NewIfNull())
                        {
                            tokens.Add(new IndexToken(l2.name, PlaceType.L2));

                            tokens.Add(new IndexToken(l2.waterBody, PlaceType.WaterBody));

                            foreach (var l3 in l2.L3.NewIfNull())
                            {
                                tokens.Add(new IndexToken(l3.name, PlaceType.L3));

                                tokens.Add(new IndexToken(l3.waterBody, PlaceType.WaterBody));

                                foreach (var l4 in l3.L4.NewIfNull())
                                {
                                    tokens.Add(new IndexToken(l4.name, PlaceType.L4));

                                    tokens.Add(new IndexToken(l4.waterBody, PlaceType.WaterBody));
                                }
                            }
                        }
                    }
                }
            }

            var indices = new IndexEntryPreprocessor().PreprocessTokens(tokens);
            var groupedIndices = indices.Group();

            return groupedIndices;
        }
    }
}
