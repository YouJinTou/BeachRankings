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
        private static readonly string dolorSitAmet =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus egestas ante a neque congue, " +
            "id eleifend felis laoreet. Pellentesque eget dui id libero rhoncus vestibulum. Nam bibendum rutrum sem...";
        private BeachRankingsDbContext data;
        private static int currentCountryId;
        private static int currentPrimaryDivisionId;
        private static int currentSecondaryDivisionId;
        private static int currentTertiaryDivisionId;
        private static int currentQuaternaryDivisionId;
        private static int? currentCountryWaterBodyId;
        private static int? currentPrimaryDivisionWaterBodyId;
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
                var countryName = this.GetPlaceName(country.Key);

                if (this.data.Countries.Any(c => c.Name == countryName))
                {
                    continue;
                }

                currentCountryWaterBodyId = this.GetWaterBodyId(country.Key);
                var countryEntity = new Country() { Name = countryName, WaterBodyId = currentCountryWaterBodyId };

                countriesIndexList.Add(countryEntity);
                this.data.Countries.Add(countryEntity);
                this.data.SaveChanges();

                currentCountryId = countryEntity.Id;

                this.TraverseDivisions(country, 1);
            }

            LuceneSearch.Index = Index.CountryIndex;

            LuceneSearch.AddUpdateIndexEntries(countriesIndexList);
        }

        private string GetPlaceName(string place)
        {
            var tokens = place.Split(new char[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            var noWaterBody = (tokens.Length == 1);

            if (noWaterBody)
            {
                return place;
            }

            var placeName = tokens[1];

            return placeName;
        }

        private int? GetWaterBodyId(string place)
        {
            var tokens = place.Split(new char[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            var noWaterBody = (tokens.Length == 1);

            if (noWaterBody)
            {
                return null;
            }

            var waterBodyName = tokens[0];
            var waterBodyId = this.data.WaterBodies.FirstOrDefault(wb => wb.Name == waterBodyName).Id;

            return waterBodyId;
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
            var countriesCount = this.data.Countries.Count();
            var waterBodiesCount = this.data.WaterBodies.Count();
            var randomCreatorId = new Random();
            var randomCountryId = new Random();
            var randomWaterBodyId = new Random();
            var randomPrimaryDivisionId = new Random();
            var randomSecondaryDivisionId = new Random();
            var bulgariaCountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Bulgaria").Id;
            var blackSeaWaterBodyId = this.data.WaterBodies.FirstOrDefault(l => l.Name == "Black Sea").Id;
            var beaches = new List<Beach>();

            for (int i = 0; i < 50; i++)
            {
                var countryId = randomCountryId.Next(1, countriesCount + 1);
                var waterBodyId = randomWaterBodyId.Next(1, waterBodiesCount + 1);
                var primaryId = randomPrimaryDivisionId.Next(7, 10);
                var secondaryId = randomSecondaryDivisionId.Next(30, 45);

                beaches.Add(new Beach()
                {
                    Name = "Beach " + i,
                    CreatorId = creatorIds[randomCreatorId.Next(0, 3)],
                    CountryId = countryId,
                    PrimaryDivisionId = primaryId,
                    SecondaryDivisionId = secondaryId,
                    WaterBodyId = waterBodyId,
                    Description = dolorSitAmet
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
                    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Durrës").Id,
                    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "Krujë").Id,
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

            var creatorIds = new string[]
            {
                this.data.Users.FirstOrDefault(u => u.UserName == "admin").Id,
                this.data.Users.FirstOrDefault(u => u.UserName == "user").Id,
                this.data.Users.FirstOrDefault(u => u.UserName == "user2").Id
            };
            var imagePaths = new string[]
            {
                "/Content/Images/kamchia_river.jpg",
                "/Content/Images/bolata.jpg",
                "/Content/Images/sunny_day.jpg",
                "/Content/Images/mamaia_beach.jpg"
            };
            var randomCreatorId = new Random();
            var randomImagePath = new Random();
            var beaches = this.data.Beaches.ToList();

            for (int i = 0; i < beaches.Count; i++)
            {
                var imagePath = imagePaths[randomImagePath.Next(0, imagePaths.Length)];
                var imageName = imagePath.Substring(imagePath.LastIndexOf('/') + 1);

                beaches[i].Images.Add(new BeachImage()
                {
                    AuthorId = creatorIds[randomCreatorId.Next(0, creatorIds.Length)],
                    BeachId = beaches[i].Id,
                    Path = imagePath,
                    Name = imageName
                });
            }
            
            this.data.SaveChanges();
        }

        private void SeedReviews()
        {
            if (this.data.Reviews.Any())
            {
                return;
            }

            var c = dolorSitAmet;
            var reviews = new List<Review>();
            var authorIds = new string[]
            {
                this.data.Users.FirstOrDefault(u => u.UserName == "admin").Id,
                this.data.Users.FirstOrDefault(u => u.UserName == "user").Id,
                this.data.Users.FirstOrDefault(u => u.UserName == "user2").Id
            };
            var randomAuthorId = new Random();
            randomBeach = new Random();
            randomScore = new Random();
            beachesCount = this.data.Beaches.Count();

            for (int i = 0; i < 40; i++)
            {
                reviews.Add(new Review(b(), c, s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s(), s()));
            }

            for (int i = 0; i < reviews.Count; i++)
            {
                reviews[i].AuthorId = authorIds[randomAuthorId.Next(0, 3)];

                reviews[i].UpdateTotalScore();

                this.data.Reviews.Add(reviews[i]);
                this.data.SaveChanges();

                var reviewedBeach = this.data.Beaches.Find(reviews[i].BeachId);

                reviewedBeach.UpdateScores();
            }

            var authors = new User[]
            {
                this.data.Users.Find(authorIds[0]),
                this.data.Users.Find(authorIds[1]),
                this.data.Users.Find(authorIds[2])
            };

            for (int i = 0; i < authors.Length; i++)
            {
                authors[i].RecalculateLevel();
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
                        Name = this.GetPlaceName(value),
                        CountryId = currentCountryId,
                        WaterBodyId = currentCountryWaterBodyId ?? this.GetWaterBodyId(value)
                    };
                    currentPrimaryDivisionWaterBodyId = primaryDivision.WaterBodyId;

                    this.data.PrimaryDivisions.Add(primaryDivision);
                    this.data.SaveChanges();

                    currentPrimaryDivisionId = primaryDivision.Id;
                    searchableDivision = primaryDivision;
                    LuceneSearch.Index = Index.PrimaryDivisionIndex;

                    break;
                case 2:
                    var secondaryDivision = new SecondaryDivision()
                    {
                        Name = this.GetPlaceName(value),
                        CountryId = currentCountryId,
                        PrimaryDivisionId = currentPrimaryDivisionId,
                        WaterBodyId = currentPrimaryDivisionWaterBodyId ?? this.GetWaterBodyId(value)
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
            return randomBeach.Next(1, beachesCount);
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