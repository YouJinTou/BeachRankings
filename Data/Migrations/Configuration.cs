namespace BeachRankings.Data.Migrations
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    public static class DivisionCollectionFactory
    {
        public static dynamic Collection(object division)
        {
            if (division is Dictionary<string, object>)
            {
                return (Dictionary<string, object>)division;
            }
            else if (division is KeyValuePair<string, object>)
            {
                var dictDivision = (KeyValuePair<string, object>)division;

                return dictDivision.Value;
            }
            else if (division is List<object>)
            {
                return (List<object>)division;
            }

            return division.ToString();
        }
    }

    public sealed class Configuration : DbMigrationsConfiguration<BeachRankingsDbContext>
    {
        private BeachRankingsDbContext data;
        private int waterBodyId = 31;
        private static int currentCountryId;
        private static int currentPrimaryDivisionId;
        private static int currentSecondaryDivisionId;
        private static int currentTertiaryDivisionId;
        private static int currentQuaternaryDivisionId;
        private static int nextLevel;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BeachRankingsDbContext data)
        {
            this.data = data;

            //this.SeedRoles();
            //this.SeedUsers();
            this.SeedWaterBodies();
            this.SeedAll();
            //this.SeedCountries();
            //this.SeedPrimaryDivisions();
            //this.SeedSecondaryDivisions();
            //this.SeedBeaches();
            //this.SeedBeachImages();
            //this.SeedReviews();
        }

        private void SeedAll()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Seed.txt");
            var json = File.ReadAllText(path);
            var countries = JsonHelper.Deserialize(json);

            foreach (var country in (Dictionary<string, object>)countries)
            {
                var countryEntity = new Country() { Name = country.Key };

                this.data.Countries.Add(countryEntity);
                this.data.SaveChanges();

                currentCountryId = countryEntity.Id;
                nextLevel = 1;

                this.TraverseDivisions(country);
            }
        }


        private void TraverseDivisions(object parent)
        {
            if (parent is string)
            {
                nextLevel--;

                return;
            }

            foreach (var child in DivisionCollectionFactory.Collection(parent))
            {
                this.SaveDivision(child);

                this.TraverseDivisions(child);
            }

            nextLevel = 1;
        }

        private void SaveDivision(object division)
        {
            if (!(division is KeyValuePair<string, object> || division is string))
            {
                return;
            }

            var value = string.Empty;

            if (division is KeyValuePair<string, object>)
            {
                var kvpDivision = (KeyValuePair<string, object>)division;
                value = kvpDivision.Key;
            }
            else
            {
                value = division.ToString();
            }

            switch (nextLevel)
            {
                case 1:
                    var primaryDivision = new PrimaryDivision()
                    {
                        Name = value,
                        CountryId = currentCountryId,
                        WaterBodyId = waterBodyId
                    };

                    this.data.PrimaryDivisions.Add(primaryDivision);
                    this.data.SaveChanges();

                    currentPrimaryDivisionId = primaryDivision.Id;

                    break;
                case 2:
                    var secondaryDivision = new SecondaryDivision()
                    {
                        Name = value,
                        CountryId = currentCountryId,
                        PrimaryDivisionId = currentPrimaryDivisionId
                    };

                    this.data.SecondaryDivisions.Add(secondaryDivision);
                    this.data.SaveChanges();

                    currentSecondaryDivisionId = secondaryDivision.Id;

                    break;
                case 3:
                    var tertiaryDivision = new TertiaryDivision()
                    {
                        Name = value,
                        CountryId = currentCountryId,
                        PrimaryDivisionId = currentPrimaryDivisionId,
                        SecondaryDivisionId = currentSecondaryDivisionId
                    };

                    this.data.TertiaryDivisions.Add(tertiaryDivision);
                    this.data.SaveChanges();

                    currentTertiaryDivisionId = tertiaryDivision.Id;

                    break;
                case 4:
                    var quaternaryDivision = new QuaternaryDivision()
                    {
                        Name = value,
                        CountryId = currentCountryId,
                        PrimaryDivisionId = currentPrimaryDivisionId,
                        SecondaryDivisionId = currentSecondaryDivisionId,
                        TertiaryDivisionId = currentTertiaryDivisionId
                    };

                    this.data.QuaternaryDivisions.Add(quaternaryDivision);
                    this.data.SaveChanges();

                    currentQuaternaryDivisionId = quaternaryDivision.Id;

                    break;
                default:
                    throw new ArgumentException("Invalid division.");
            }

            nextLevel += 1;
        }

        //private void TraverseDivisions(object parent)
        //{
        //    if (parent is string)
        //    {
        //        nextLevel--;

        //        return;
        //    }

        //    if (parent is Dictionary<string, object>)
        //    {
        //        var dictParent = (Dictionary<string, object>)parent;

        //        foreach (var child in dictParent)
        //        {
        //            this.SaveDivision(child);

        //            this.TraverseDivisions(child);
        //        }
        //    }
        //    else
        //    {
        //        var kvpParent = (KeyValuePair<string, object>)parent;

        //        if (kvpParent.Value is Dictionary<string, object>)
        //        {
        //            foreach (var child in (Dictionary<string, object>)kvpParent.Value)
        //            {
        //                this.SaveDivision(child);

        //                this.TraverseDivisions(child);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var child in (List<object>)kvpParent.Value)
        //            {
        //                this.SaveDivision(child);

        //                this.TraverseDivisions(child);
        //            }
        //        }
        //    }

        //    nextLevel = 1;
        //}

        //private void SaveDivision(object division)
        //{
        //    if (!(division is KeyValuePair<string, object> || division is string))
        //    {
        //        return;
        //    }

        //    var value = string.Empty;

        //    if (division is KeyValuePair<string, object>)
        //    {
        //        var kvpDivision = (KeyValuePair<string, object>)division;
        //        value = kvpDivision.Key;
        //    }
        //    else
        //    {
        //        value = division.ToString();
        //    }

        //    switch (nextLevel)
        //    {
        //        case 1:
        //            var primaryDivision = new PrimaryDivision()
        //            {
        //                Name = value,
        //                CountryId = currentCountryId,
        //                WaterBodyId = waterBodyId
        //            };

        //            this.data.PrimaryDivisions.Add(primaryDivision);

        //            currentPrimaryDivisionId = primaryDivision.Id;

        //            break;
        //        case 2:
        //            var secondaryDivision = new SecondaryDivision()
        //            {
        //                Name = value,
        //                CountryId = currentCountryId,
        //                PrimaryDivisionId = currentPrimaryDivisionId
        //            };

        //            this.data.SecondaryDivisions.Add(secondaryDivision);

        //            currentSecondaryDivisionId = secondaryDivision.Id;

        //            break;
        //        case 3:
        //            var tertiaryDivision = new TertiaryDivision()
        //            {
        //                Name = value,
        //                CountryId = currentCountryId,
        //                PrimaryDivisionId = currentPrimaryDivisionId,
        //                SecondaryDivisionId = currentSecondaryDivisionId
        //            };

        //            this.data.TertiaryDivisions.Add(tertiaryDivision);

        //            currentTertiaryDivisionId = tertiaryDivision.Id;

        //            break;
        //        case 4:
        //            var quaternaryDivision = new QuaternaryDivision()
        //            {
        //                Name = value,
        //                CountryId = currentCountryId,
        //                PrimaryDivisionId = currentPrimaryDivisionId,
        //                SecondaryDivisionId = currentSecondaryDivisionId,
        //                TertiaryDivisionId = currentTertiaryDivisionId
        //            };

        //            this.data.QuaternaryDivisions.Add(quaternaryDivision);

        //            currentQuaternaryDivisionId = quaternaryDivision.Id;

        //            break;
        //        default:
        //            throw new ArgumentException("Invalid division.");
        //    }

        //    this.data.SaveChanges();

        //    nextLevel += 1;
        //}

        public static class JsonHelper
        {
            public static object Deserialize(string json)
            {
                return ToObject(JToken.Parse(json));
            }

            private static object ToObject(JToken token)
            {
                switch (token.Type)
                {
                    case JTokenType.Object:
                        return token.Children<JProperty>()
                                    .ToDictionary(prop => prop.Name,
                                                  prop => ToObject(prop.Value));

                    case JTokenType.Array:
                        return token.Select(ToObject).ToList();

                    default:
                        return ((JValue)token).Value;
                }
            }
        }


        private void GetJTokenName(JToken token) { }

        private void SeedRoles()
        {
            if (!this.data.Roles.Any(r => r.Name == "Admin"))
            {
                var roleStore = new RoleStore<IdentityRole>(this.data);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole { Name = "Admin" };

                roleManager.Create(role);
            }

            if (!this.data.Roles.Any(r => r.Name == "User"))
            {
                var roleStore = new RoleStore<IdentityRole>(this.data);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole { Name = "User" };

                roleManager.Create(role);
            }
        }

        private void SeedUsers()
        {
            if (!this.data.Users.Any(u => u.UserName == "admin"))
            {
                var userStore = new UserStore<User>(this.data);
                var userManager = new UserManager<User>(userStore);
                var user = new User { UserName = "admin", Email = "dandreevd@gmail.com", AvatarPath = "/Content/Images/unknown_profile.jpg" };

                userManager.Create(user, "123456");
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!this.data.Users.Any(u => u.UserName == "user"))
            {
                var userStore = new UserStore<User>(this.data);
                var userManager = new UserManager<User>(userStore);
                var user = new User { UserName = "user", Email = "some@email.com" };

                userManager.Create(user, "123456");
                userManager.AddToRole(user.Id, "User");
            }
        }

        private void SeedWaterBodies()
        {
            if (this.data.WaterBodies.Any())
            {
                return;
            }

            var waterBodiesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "WaterBodies.txt");
            var waterBodies = new List<WaterBody>();

            using (var sr = new StreamReader(waterBodiesPath))
            {
                string waterBodyName;

                while ((waterBodyName = sr.ReadLine()) != null)
                {
                    var waterBody = new WaterBody() { Name = waterBodyName };

                    waterBodies.Add(waterBody);
                    this.data.WaterBodies.Add(waterBody);
                }
            }

            this.data.SaveChanges();

            LuceneSearch.Index = Index.WaterBodyIndex;

            LuceneSearch.AddUpdateIndexEntries(waterBodies);
        }

        private void SeedCountries()
        {
            if (this.data.Countries.Any())
            {
                return;
            }

            var countriesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Countries.txt");
            var countries = new List<Country>();

            using (var sr = new StreamReader(countriesPath))
            {
                string countryName;

                while ((countryName = sr.ReadLine()) != null)
                {
                    var country = new Country() { Name = countryName };

                    countries.Add(country);
                    this.data.Countries.Add(country);
                }
            }

            this.data.SaveChanges();

            LuceneSearch.Index = Index.CountryIndex;

            LuceneSearch.AddUpdateIndexEntries(countries);
        }

        private void SeedPrimaryDivisions()
        {
            if (this.data.PrimaryDivisions.Any())
            {
                return;
            }

            var bulgariaId = this.data.Countries.FirstOrDefault(c => c.Name == "Bulgaria").Id;
            var romaniaId = this.data.Countries.FirstOrDefault(c => c.Name == "Romania").Id;
            var blackSeaWaterBodyId = this.data.WaterBodies.FirstOrDefault(l => l.Name == "Black Sea").Id;
            var primaryDivisions = new List<PrimaryDivision>()
            {
                new PrimaryDivision() { Name = "Dobrich", CountryId = bulgariaId, WaterBodyId = blackSeaWaterBodyId },
                new PrimaryDivision() { Name= "Varna", CountryId = bulgariaId, WaterBodyId = blackSeaWaterBodyId },
                new PrimaryDivision() { Name= "Bourgas", CountryId = bulgariaId, WaterBodyId = blackSeaWaterBodyId },
                new PrimaryDivision() { Name = "Dobrogea", CountryId = romaniaId, WaterBodyId = blackSeaWaterBodyId }
            };

            foreach (var primaryDivision in primaryDivisions)
            {
                this.data.PrimaryDivisions.Add(primaryDivision);
            }

            this.data.SaveChanges();

            //LuceneSearch.Index = Index.RegionIndex;

            //LuceneSearch.AddUpdateIndexEntries(regions);
        }

        private void SeedSecondaryDivisions()
        {
            if (this.data.SecondaryDivisions.Any())
            {
                return;
            }

            var bulgariaCountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Bulgaria").Id;
            var romaniaCountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Romania").Id;
            var dobrichRegionId = this.data.PrimaryDivisions.FirstOrDefault(c => c.Name == "Dobrich").Id;
            var varnaRegionId = this.data.PrimaryDivisions.FirstOrDefault(c => c.Name == "Varna").Id;
            var bourgasRegionId = this.data.PrimaryDivisions.FirstOrDefault(c => c.Name == "Bourgas").Id;
            var dobrogeaRegionId = this.data.PrimaryDivisions.FirstOrDefault(c => c.Name == "Dobrogea").Id;
            var secondaryDivisions = new List<SecondaryDivision>()
            {
                new SecondaryDivision() { Name = "Shabla", CountryId = bulgariaCountryId, PrimaryDivisionId = dobrichRegionId },
                new SecondaryDivision() { Name = "Kavarna", CountryId = bulgariaCountryId, PrimaryDivisionId = dobrichRegionId },
                new SecondaryDivision() { Name = "Balchik", CountryId = bulgariaCountryId, PrimaryDivisionId = dobrichRegionId },
                new SecondaryDivision() { Name = "Aksakovo", CountryId = bulgariaCountryId, PrimaryDivisionId = varnaRegionId },
                new SecondaryDivision() { Name = "Varna", CountryId = bulgariaCountryId, PrimaryDivisionId = varnaRegionId },
                new SecondaryDivision() { Name = "Avren", CountryId = bulgariaCountryId, PrimaryDivisionId = varnaRegionId },
                new SecondaryDivision() { Name= "Dolni Chiflik", CountryId = bulgariaCountryId, PrimaryDivisionId = varnaRegionId },
                new SecondaryDivision() { Name = "Nesebar", CountryId = bulgariaCountryId, PrimaryDivisionId = bourgasRegionId },
                new SecondaryDivision() { Name = "Pomorie", CountryId = bulgariaCountryId, PrimaryDivisionId = bourgasRegionId },
                new SecondaryDivision() { Name = "Bourgas", CountryId = bulgariaCountryId, PrimaryDivisionId = bourgasRegionId },
                new SecondaryDivision() { Name = "Sozopol", CountryId = bulgariaCountryId, PrimaryDivisionId = bourgasRegionId },
                new SecondaryDivision() { Name = "Primorsko", CountryId = bulgariaCountryId, PrimaryDivisionId = bourgasRegionId },
                new SecondaryDivision() { Name = "Tsarevo", CountryId = bulgariaCountryId, PrimaryDivisionId = bourgasRegionId },
                new SecondaryDivision() { Name = "Tulcea", CountryId = romaniaCountryId, PrimaryDivisionId = dobrogeaRegionId },
                new SecondaryDivision() { Name = "Constanta", CountryId = romaniaCountryId, PrimaryDivisionId = dobrogeaRegionId }
            };

            foreach (var secondaryDivision in secondaryDivisions)
            {
                this.data.SecondaryDivisions.Add(secondaryDivision);
            }

            this.data.SaveChanges();

            //LuceneSearch.Index = Index.AreaIndex;

            //LuceneSearch.AddUpdateIndexEntries(areas);
        }

        private void SeedBeaches()
        {
            if (this.data.Beaches.Any())
            {
                return;
            }

            var bulgariaCountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Bulgaria").Id;
            var blackSeaWaterBodyId = this.data.WaterBodies.FirstOrDefault(l => l.Name == "Black Sea").Id;
            var beaches = new List<Beach>()
            {
                new Beach()
                {
                    Name = "Kamchia Beach",
                    CountryId = bulgariaCountryId,
                    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Varna").Id,
                    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "Varna").Id,
                    WaterBodyId = blackSeaWaterBodyId,
                    Description = "Kamchia beach is situated where the muddy Kamchia flows into the Black Sea.",
                    Coordinates = "43.204666,27.910543"
                },
                new Beach()
                {
                    Name = "Bolata Beach",
                    CountryId = bulgariaCountryId,
                    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Dobrich").Id,
                    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "Kavarna").Id,
                    WaterBodyId = blackSeaWaterBodyId,
                    Description = "Situated north of Albena, Bolata is an ungainly sight.",
                    Coordinates = "43.204666,27.910543"
                },
                new Beach()
                {
                    Name = "Sunny Day Beach",
                    CountryId = bulgariaCountryId,
                    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Varna").Id,
                    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "Varna").Id,
                    WaterBodyId = blackSeaWaterBodyId,
                    Description = "Gracefully surrounded by concrete buildings, this is where you don't want to be.",
                    Coordinates = "43.204666,27.910543",
                },
                new Beach()
                {
                    Name = "Mamaia Beach",
                    CountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Romania").Id,
                    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Dobrogea").Id,
                    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "Constanta").Id,
                    WaterBodyId = blackSeaWaterBodyId,
                    Description = "Gracefully surrounded by concrete buildings, this is where you don't want to be.",
                    Coordinates = "43.204666,27.910543",
                }
            };

            foreach (var beach in beaches)
            {
                this.data.Beaches.Add(beach);
                beach.SetBeachData();
            }

            this.data.SaveChanges();

            LuceneSearch.Index = Index.BeachIndex;

            LuceneSearch.AddUpdateIndexEntries(beaches);
        }

        private void SeedBeachImages()
        {
            if (this.data.BeachImages.Any())
            {
                return;
            }

            var adminId = this.data.Users.FirstOrDefault(u => u.UserName == "admin").Id;
            var userId = this.data.Users.FirstOrDefault(u => u.UserName == "user").Id;

            this.data.Beaches.FirstOrDefault(b => b.Name == "Kamchia Beach").Images.Add(new BeachImage()
            {
                AuthorId = userId,
                BeachId = this.data.Beaches.FirstOrDefault(b => b.Name == "Kamchia Beach").Id,
                Path = "/Content/Images/kamchia_river.jpg",
                Name = "kamchia_river.jpg"
            });
            this.data.Beaches.FirstOrDefault(b => b.Name == "Bolata Beach").Images.Add(new BeachImage()
            {
                AuthorId = adminId,
                BeachId = this.data.Beaches.FirstOrDefault(b => b.Name == "Bolata Beach").Id,
                Path = "/Content/Images/bolata.jpg",
                Name = "bolata.jpg"
            });
            this.data.Beaches.FirstOrDefault(b => b.Name == "Sunny Day Beach").Images.Add(new BeachImage()
            {
                AuthorId = userId,
                BeachId = this.data.Beaches.FirstOrDefault(b => b.Name == "Sunny Day Beach").Id,
                Path = "/Content/Images/sunny_day.jpg",
                Name = "sunny_day.jpg"
            });
            this.data.Beaches.FirstOrDefault(b => b.Name == "Mamaia Beach").Images.Add(new BeachImage()
            {
                AuthorId = adminId,
                BeachId = this.data.Beaches.FirstOrDefault(b => b.Name == "Mamaia Beach").Id,
                Path = "/Content/Images/mamaia_beach.jpg",
                Name = "mamaia_beach.jpg"
            });

            this.data.SaveChanges();
        }

        //private void SeedReviews()
        //{
        //    if (this.data.Reviews.Any())
        //    {
        //        return;
        //    }

        //    Random rand = new Random();
        //    var adminId = this.data.Users.FirstOrDefault(u => u.UserName == "admin").Id;
        //    var userId = this.data.Users.FirstOrDefault(u => u.UserName == "user").Id;
        //    var beachIds = new List<int>()
        //    {
        //        this.data.Beaches.FirstOrDefault(b => b.Name == "Kamchia Beach").Id,
        //        this.data.Beaches.FirstOrDefault(b => b.Name == "Bolata").Id,
        //        this.data.Beaches.FirstOrDefault(b => b.Name == "Sunny Day Beach").Id,

        //    };

        //    for (int i = 0; i < 20; i++)
        //    {
        //        bool even = ((i % 2) == 0);

        //        this.data.Reviews.Add(new Review()
        //        {
        //            AuthorId = (even ? adminId : userId),
        //            BeachId = rand.Next(1, beachIds.Count + 1),
        //            PostedOn = DateTime.Now.AddDays(-i),
        //            TotalScore = Math.Round(rand.NextDouble() * 10, 1),
        //            Content = "This is the content for the " + i + ". review, whether you like it or not." +
        //                    "There's more to come as well, so you'd better watch out. We have pancakes, too, so shut it.",
        //            WaterQuality = Math.Round((rand.NextDouble() * 10), 1),
        //            SeafloorCleanliness = Math.Round((rand.NextDouble() * 10), 1),
        //            CoralReefFactor = Math.Round((rand.NextDouble() * 10), 1),
        //            SeaLifeDiversity = Math.Round((rand.NextDouble() * 10), 1),
        //            SnorkelingSuitability = Math.Round((rand.NextDouble() * 10), 1),
        //            BeachCleanliness = Math.Round((rand.NextDouble() * 10), 1),
        //            CrowdFreeFactor = Math.Round((rand.NextDouble() * 10), 1),
        //            SandQuality = Math.Round((rand.NextDouble() * 10), 1),
        //            BreathtakingEnvironment = Math.Round((rand.NextDouble() * 10), 1),
        //            TentSuitability = Math.Round((rand.NextDouble() * 10), 1),
        //            KayakSuitability = Math.Round((rand.NextDouble() * 10), 1),
        //            LongStaySuitability = Math.Round((rand.NextDouble() * 10), 1)
        //        });
        //    }

        //    this.data.Reviews.Add(new Review()
        //    {
        //        AuthorId = adminId,
        //        BeachId = rand.Next(1, beachIds.Count + 1),
        //        PostedOn = DateTime.Now.AddDays(-3),
        //        Content = "This is a review without any scores. This is a review without any scores. " +
        //                "This is a review without any scores. This is a review without any scores. This is a review without any scores. "
        //    });

        //    this.data.SaveChanges();
        //}
    }
}