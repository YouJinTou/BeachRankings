namespace BeachRankings.Data.Migrations
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<BeachRankingsDbContext>
    {
        private BeachRankingsDbContext data;
        private int waterBodyId = 31;
        private static int currentCountryId;
        private static int currentPrimaryDivisionId;
        private static int currentSecondaryDivisionId;
        private static int currentTertiaryDivisionId;
        private static int currentQuaternaryDivisionId;
        private static int beachesCount;
        private static Random randomBeach;
        private static Random randomScore;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BeachRankingsDbContext data)
        {
            this.data = data;

            this.SeedRoles();
            this.SeedUsers();
            this.SeedWaterBodies();
            this.SeedAdministrativeUnits();
            this.SeedBeaches();
            this.SeedBeachImages();
            this.SeedReviews();
        }

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

            if (!this.data.Users.Any(u => u.UserName == "user2"))
            {
                var userStore = new UserStore<User>(this.data);
                var userManager = new UserManager<User>(userStore);
                var user = new User { UserName = "user2", Email = "some2@email.com" };

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

        private void SeedAdministrativeUnits()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Seed.txt");
            var json = File.ReadAllText(path);
            var countries = JsonHelper.Deserialize(json);
            var countriesIndexList = new List<Country>();

            foreach (var country in (Dictionary<string, object>)countries)
            {
                if (this.data.Countries.Any(c => c.Name == country.Key))
                {
                    continue;
                }

                var countryEntity = new Country() { Name = country.Key };

                countriesIndexList.Add(countryEntity);
                this.data.Countries.Add(countryEntity);
                this.data.SaveChanges();

                currentCountryId = countryEntity.Id;

                this.TraverseDivisions(country, 1);
            }

            LuceneSearch.Index = Index.CountryIndex;

            LuceneSearch.AddUpdateIndexEntries(countriesIndexList);
        }

        private void SeedBeaches()
        {
            if (this.data.Beaches.Any())
            {
                return;
            }

            var creatorIds = new string[]
            {
                this.data.Users.FirstOrDefault(u => u.UserName == "admin").Id,
                this.data.Users.FirstOrDefault(u => u.UserName == "user").Id,
                this.data.Users.FirstOrDefault(u => u.UserName == "user2").Id
            };
            var randomCreatorId = new Random();
            var randomPrimaryDivisionId = new Random();
            var randomSecondaryDivisionId = new Random();
            var description = "dddddddddddddddddddddddddddddddddddddddddddddd";
            var bulgariaCountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Bulgaria").Id;
            var blackSeaWaterBodyId = this.data.WaterBodies.FirstOrDefault(l => l.Name == "Black Sea").Id;
            var beaches = new List<Beach>();

            for (int i = 0; i < 120; i++)
            {
                var randomPrimaryId = randomPrimaryDivisionId.Next(7, 10);
                var randomSecondaryId = randomSecondaryDivisionId.Next(30, 45);

                beaches.Add(new Beach()
                {
                    Name = "Beach " + i,
                    CreatorId = creatorIds[randomCreatorId.Next(0, 3)],
                    CountryId = bulgariaCountryId,
                    PrimaryDivisionId = randomPrimaryId,
                    SecondaryDivisionId = randomSecondaryId,
                    WaterBodyId = blackSeaWaterBodyId,
                    Description = description
                });
            }

            beaches.AddRange(new List<Beach>()
            {
                new Beach()
                {
                    Name = "Kamchia Beach",
                    CreatorId = creatorIds[0],
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
                    CreatorId = creatorIds[1],
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
                    CreatorId = creatorIds[1],
                    CountryId = bulgariaCountryId,
                    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Varna").Id,
                    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "Varna").Id,
                    WaterBodyId = blackSeaWaterBodyId,
                    Description = "Gracefully surrounded by concrete buildings, this is where you don't want to be.",
                    Coordinates = "43.204666,27.910543",
                },
                new Beach()
                {
                    Name = "Albanian Beach",
                    CreatorId = creatorIds[1],
                    CountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Albania").Id,
                    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Azores Islands").Id,
                    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "S�o Miguel").Id,
                    WaterBodyId = this.data.WaterBodies.FirstOrDefault(wb => wb.Name == "Ionian Sea").Id,
                    Description = "Gracefully surrounded by concrete buildings, this is where you don't want to be.",
                    Coordinates = "43.204666,27.910543",
                },
                new Beach()
                {
                    Name = "Skiathos First Beach",
                    CreatorId = creatorIds[2],
                    CountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Greece").Id,
                    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Thessaly").Id,
                    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "Sporades").Id,
                    TertiaryDivisionId = this.data.TertiaryDivisions.FirstOrDefault(r => r.Name == "Skiathos").Id,
                    WaterBodyId = this.data.WaterBodies.FirstOrDefault(wb => wb.Name == "Black Sea").Id,
                    Description = "Gracefully surrounded by concrete buildings, this is where you don't want to be.",
                    Coordinates = "43.204666,27.910543",
                }
            });

            foreach (var beach in beaches)
            {
                this.data.Beaches.Add(beach);
                beach.SetBeachData();

                this.data.SaveChanges();

                LuceneSearch.Index = Index.BeachIndex;
                LuceneSearch.AddUpdateIndexEntry(beach);

                var country = this.data.Countries.Find(beach.CountryId);
                var primaryDivision = this.data.PrimaryDivisions.Find(beach.PrimaryDivisionId);
                var secondaryDivision = this.data.SecondaryDivisions.Find(beach.SecondaryDivisionId);
                var tertiaryDivision = this.data.TertiaryDivisions.Find(beach.TertiaryDivisionId);
                var quaternaryDivision = this.data.QuaternaryDivisions.Find(beach.QuaternaryDivisionId);
                var waterBody = this.data.WaterBodies.Find(beach.WaterBodyId);

                LuceneSearch.Index = Index.CountryIndex;
                LuceneSearch.AddUpdateIndexEntry(country);
                LuceneSearch.Index = Index.PrimaryDivisionIndex;
                LuceneSearch.AddUpdateIndexEntry(primaryDivision);
                LuceneSearch.Index = Index.SecondaryDivisionIndex;
                LuceneSearch.AddUpdateIndexEntry(secondaryDivision);
                LuceneSearch.Index = Index.TertiaryDivisionIndex;
                LuceneSearch.AddUpdateIndexEntry(tertiaryDivision);
                LuceneSearch.Index = Index.QuaternaryDivisionIndex;
                LuceneSearch.AddUpdateIndexEntry(quaternaryDivision);
                LuceneSearch.Index = Index.WaterBodyIndex;
                LuceneSearch.AddUpdateIndexEntry(waterBody);
            }
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
            this.data.Beaches.FirstOrDefault(b => b.Name == "Albanian Beach").Images.Add(new BeachImage()
            {
                AuthorId = adminId,
                BeachId = this.data.Beaches.FirstOrDefault(b => b.Name == "Albanian Beach").Id,
                Path = "/Content/Images/mamaia_beach.jpg",
                Name = "mamaia_beach.jpg"
            });

            this.data.SaveChanges();
        }

        private void SeedReviews()
        {
            if (this.data.Reviews.Any())
            {
                return;
            }

            var c = "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii";
            var reviews = new List<Review>();
            var authors = new string[]
            {
                this.data.Users.FirstOrDefault(u => u.UserName == "admin").Id,
                this.data.Users.FirstOrDefault(u => u.UserName == "user").Id,
                this.data.Users.FirstOrDefault(u => u.UserName == "user2").Id
            };
            var randomAuthorId = new Random();
            randomBeach = new Random();
            randomScore = new Random();
            beachesCount = this.data.Beaches.Count();

            for (int i = 0; i < 150; i++)
            {
                reviews.Add(new Review(b(), c, s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s()));
            }

            for (int i = 0; i < reviews.Count; i++)
            {
                reviews[i].AuthorId = authors[randomAuthorId.Next(0, 3)];

                reviews[i].UpdateTotalScore();

                this.data.Reviews.Add(reviews[i]);
                this.data.SaveChanges();

                var reviewedBeach = this.data.Beaches.Find(reviews[i].BeachId);

                reviewedBeach.UpdateScores();
            }

            this.data.SaveChanges();
        }

        private void TraverseDivisions(object parent, int depth)
        {
            if (parent is string)
            {
                return;
            }

            foreach (var child in GetObjectCollection(parent))
            {
                var savedSuccessfully = this.SaveDivision(child, depth);
                depth = savedSuccessfully ? depth : depth - 1;

                this.TraverseDivisions(child, depth + 1);

                depth = savedSuccessfully ? depth : depth + 1;
            }
        }

        private bool SaveDivision(object division, int depth)
        {
            if (!(division is KeyValuePair<string, object> || division is string))
            {
                return false;
            }

            var value = string.Empty;
            ISearchable searchableDivision = null;

            if (division is KeyValuePair<string, object>)
            {
                var kvpDivision = (KeyValuePair<string, object>)division;
                value = kvpDivision.Key;
            }
            else
            {
                value = division.ToString();
            }

            switch (depth)
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
                    searchableDivision = primaryDivision;
                    LuceneSearch.Index = Index.PrimaryDivisionIndex;

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
                    searchableDivision = secondaryDivision;
                    LuceneSearch.Index = Index.SecondaryDivisionIndex;

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
                    searchableDivision = tertiaryDivision;
                    LuceneSearch.Index = Index.TertiaryDivisionIndex;

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
                    searchableDivision = quaternaryDivision;
                    LuceneSearch.Index = Index.QuaternaryDivisionIndex;

                    break;
                default:
                    throw new ArgumentException("Invalid division.");
            }

            LuceneSearch.AddUpdateIndexEntry(searchableDivision);

            return true;
        }

        private static dynamic GetObjectCollection(object division)
        {
            if (division is Dictionary<string, object>)
            {
                return (Dictionary<string, object>)division;
            }
            else if (division is KeyValuePair<string, object>)
            {
                var kvpDivision = (KeyValuePair<string, object>)division;

                return kvpDivision.Value ?? new List<object>();
            }
            else if (division is List<object>)
            {
                return (List<object>)division;
            }

            throw new ArgumentException("Received an unexpected type.");
        }

        private int b()
        {
            return randomBeach.Next(1, beachesCount + 1);
        }

        private double? s()
        {
            var isNull = (randomScore.Next(1, 11) > 9) ? true : false;

            if (isNull)
            {
                return null;
            }

            return Math.Round((randomScore.NextDouble() * 10), 1);
        }
    }
}