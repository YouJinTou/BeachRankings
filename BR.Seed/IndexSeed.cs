using BR.Core.Cloud.Aws;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.IndexService.Processors;
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
            var indices = new HashSet<IndexEntry>(GetIndices(), new IndexEntryEqualityComparer());

            indices.Add(new IndexEntry
            {
                Bucket = "b",
                Token = "beach"
            });

            await db.AddManyAsync(indices);
        }

        private static IEnumerable<IndexEntry> GetIndices()
        {
            using var reader = new StreamReader("seed.xml");
            var serializer = new XmlSerializer(typeof(Seed));
            var seed = (Seed)serializer.Deserialize(reader);
            var tokens = new HashSet<string>();

            foreach (var continent in seed.Continent)
            {
                tokens.Add(continent.name);

                foreach (var country in continent.Country.NewIfNull())
                {
                    tokens.Add(country.name);

                    tokens.Add(country.waterBody);

                    foreach (var l1 in country.L1.NewIfNull())
                    {
                        tokens.Add(l1.name);

                        tokens.Add(l1.waterBody);

                        foreach (var l2 in l1.L2.NewIfNull())
                        {
                            tokens.Add(l2.name);

                            tokens.Add(l2.waterBody);

                            foreach (var l3 in l2.L3.NewIfNull())
                            {
                                tokens.Add(l3.name);

                                tokens.Add(l3.waterBody);

                                foreach (var l4 in l3.L4.NewIfNull())
                                {
                                    tokens.Add(l4.name);

                                    tokens.Add(l4.waterBody);
                                }
                            }
                        }
                    }
                }
            }

            return new IndexEntryPreprocessor().PreprocessTokens(tokens);
        }
    }
}
