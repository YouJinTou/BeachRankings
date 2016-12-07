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
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<BeachRankingsDbContext>
    {
        private static readonly string dolorSitAmet =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus egestas ante a neque congue, " +
            "id eleifend felis laoreet. Pellentesque eget dui id libero rhoncus vestibulum. Nam bibendum rutrum sem...";
        private BeachRankingsDbContext data;
        private static int? currentContinentId;
        private static int currentCountryId = 1;
        private static int currentPrimaryDivisionId = 1;
        private static int currentSecondaryDivisionId = 1;
        private static int currentTertiaryDivisionId = 1;
        private static int currentQuaternaryDivisionId = 1;
        private static int? currentCountryWaterBodyId;
        private static int? currentPrimaryDivisionWaterBodyId;
        private static int beachesCount;
        private static Random randomBeach;
        private static Random randomScore;
        private static int entryCounter;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BeachRankingsDbContext data)
        {
            Type type = typeof(LuceneSearch);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            this.data = data;

            this.SeedRoles();
            this.SeedUsers();
            this.SeedWaterBodies();
            this.SeedContinents();
            this.SeedAdministrativeUnits();
            this.SeedBeaches();
            this.SeedIndexEntries();
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
                var user = new User { UserName = "user", Email = "some@email.com", AvatarPath = "/Content/Images/unknown_profile.jpg" };

                userManager.Create(user, "123456");
                userManager.AddToRole(user.Id, "User");
            }

            if (!this.data.Users.Any(u => u.UserName == "user2"))
            {
                var userStore = new UserStore<User>(this.data);
                var userManager = new UserManager<User>(userStore);
                var user = new User { UserName = "user2", Email = "some2@email.com", AvatarPath = "/Content/Images/unknown_profile.jpg" };

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

            var searchService = new LuceneSearch(Index.WaterBodyIndex);
            searchService.AddUpdateIndexEntries(waterBodies);
        }

        private void SeedContinents()
        {
            if (this.data.Continents.Any())
            {
                return;
            }

            var continentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Continents.txt");
            var continents = new List<Continent>();

            using (var sr = new StreamReader(continentsPath))
            {
                string continentName;

                while ((continentName = sr.ReadLine()) != null)
                {
                    var continent = new Continent() { Name = continentName };

                    continents.Add(continent);
                    this.data.Continents.Add(continent);
                }
            }

            this.data.SaveChanges();

            var searchService = new LuceneSearch(Index.ContinentIndex);
            searchService.AddUpdateIndexEntries(continents);
        }

        private void SeedAdministrativeUnits()
        {
            this.data.Configuration.AutoDetectChangesEnabled = false;
            this.data.Configuration.ValidateOnSaveEnabled = false;

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

                currentContinentId = this.GetContinentId(country.Key);
                currentCountryWaterBodyId = this.GetWaterBodyId(country.Key);
                var countryEntity = new Country() { Id = currentCountryId, Name = countryName, ContinentId = currentContinentId, WaterBodyId = currentCountryWaterBodyId };

                countriesIndexList.Add(countryEntity);
                this.data.Countries.Add(countryEntity);

                this.TraverseDivisions(country, 1);

                if (entryCounter >= 300)
                {
                    this.data.SaveChanges();

                    entryCounter = 0;
                }

                currentCountryId++;
                entryCounter++;
            }

            this.data.SaveChanges();

            var searchService = new LuceneSearch(Index.CountryIndex);
            searchService.AddUpdateIndexEntries(countriesIndexList);

            this.data.Configuration.AutoDetectChangesEnabled = true;
            this.data.Configuration.ValidateOnSaveEnabled = true;
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
            var continentsCount = this.data.Continents.Count();
            var countriesCount = this.data.Countries.Count();
            var waterBodiesCount = this.data.WaterBodies.Count();
            var randomCreatorId = new Random();
            var randomContinentId = new Random();
            var randomCountryId = new Random();
            var randomWaterBodyId = new Random();
            var randomPrimaryDivisionId = new Random();
            var randomSecondaryDivisionId = new Random();
            var randomTertiaryDivisionId = new Random();
            var randomQuaternaryDivisionId = new Random();
            var europeContinentId = this.data.Continents.FirstOrDefault(c => c.Name == "Europe").Id;
            var bulgariaCountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Bulgaria").Id;
            var blackSeaWaterBodyId = this.data.WaterBodies.FirstOrDefault(l => l.Name == "Black Sea").Id;
            var beaches = new List<Beach>();

            for (int i = 0; i < 50; i++)
            {
                var continentId = randomContinentId.Next(1, continentsCount + 1);
                var firstCountry = this.data.Countries.FirstOrDefault(c => c.ContinentId == continentId);
                var firstCountryId = (firstCountry == null) ? randomCountryId.Next(1, countriesCount + 1) : firstCountry.Id;
                var lastCountry = this.data.Countries.OrderByDescending(c => c.Id).FirstOrDefault(c => c.ContinentId == continentId);
                var lastCountryId = (lastCountry == null) ? firstCountryId : lastCountry.Id;
                var countryId = randomCountryId.Next(firstCountryId, lastCountryId + 1);
                var firstPrimaryDivision = this.data.PrimaryDivisions.FirstOrDefault(pd => pd.CountryId == countryId);
                var firstPrimaryDivisionId = (firstPrimaryDivision == null) ? 0 : firstPrimaryDivision.Id;
                var lastPrimaryDivision = this.data.PrimaryDivisions.OrderByDescending(d => d.Id).FirstOrDefault(pd => pd.CountryId == countryId);
                var lastPrimaryDivisionId = (lastPrimaryDivision == null) ? 0 : lastPrimaryDivision.Id;
                var primaryId = (lastPrimaryDivisionId > 0) ? (int?)randomPrimaryDivisionId.Next(firstPrimaryDivisionId, lastPrimaryDivisionId + 1) : null;
                var firstSecondaryDivision = this.data.SecondaryDivisions.FirstOrDefault(sd => sd.PrimaryDivisionId == primaryId);
                var firstSecondaryDivisionId = (firstSecondaryDivision == null) ? 0 : firstSecondaryDivision.Id;
                var lastSecondaryDivision = this.data.SecondaryDivisions.OrderByDescending(d => d.Id).FirstOrDefault(sd => sd.PrimaryDivisionId == primaryId);
                var lastSecondaryDivisionId = (lastSecondaryDivision == null) ? 0 : lastSecondaryDivision.Id;               
                var secondaryId = (lastSecondaryDivisionId > 0) ? (int?)randomSecondaryDivisionId.Next(firstSecondaryDivisionId, lastSecondaryDivisionId + 1) : null;
                var firstTertiaryDivision = this.data.TertiaryDivisions.FirstOrDefault(td => td.SecondaryDivisionId == secondaryId);
                var firstTertiaryDivisionId = (firstTertiaryDivision == null) ? 0 : firstTertiaryDivision.Id;
                var lastTertiaryDivision = this.data.TertiaryDivisions.OrderByDescending(d => d.Id).FirstOrDefault(td => td.SecondaryDivisionId == secondaryId);
                var lastTertiaryDivisionId = (lastTertiaryDivision == null) ? 0 : lastTertiaryDivision.Id;                
                var tertiaryId = (lastTertiaryDivisionId > 0) ? (int?)randomTertiaryDivisionId.Next(firstTertiaryDivisionId, lastTertiaryDivisionId + 1) : null;               
                var firstQuaternaryDivision = this.data.QuaternaryDivisions.FirstOrDefault(qd => qd.TertiaryDivisionId == tertiaryId);
                var firstQuaternaryDivisionId = (firstQuaternaryDivision == null) ? 0 : firstQuaternaryDivision.Id;
                var lastQuaternaryDivision = this.data.QuaternaryDivisions.OrderByDescending(d => d.Id).FirstOrDefault(qd => qd.TertiaryDivisionId == tertiaryId);
                var lastQuaternaryDivisionId = (lastQuaternaryDivision == null) ? 0 : lastQuaternaryDivision.Id;
                var quaternaryId = (lastQuaternaryDivisionId > 0) ? (int?)randomQuaternaryDivisionId.Next(firstQuaternaryDivisionId, lastQuaternaryDivisionId + 1) : null;
                var waterBodyId = this.GetWaterBodyId(countryId, primaryId, secondaryId);

                beaches.Add(new Beach()
                {
                    Name = "Beach " + i,
                    CreatorId = creatorIds[randomCreatorId.Next(0, 3)],
                    ContinentId = continentId,
                    CountryId = countryId,
                    PrimaryDivisionId = primaryId,
                    SecondaryDivisionId = secondaryId,
                    TertiaryDivisionId = tertiaryId,
                    QuaternaryDivisionId = quaternaryId,
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
                    ContinentId = europeContinentId,
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
                    ContinentId = europeContinentId,
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
                    ContinentId = europeContinentId,
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
                    ContinentId = europeContinentId,
                    CountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Albania").Id,
                    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Durrës").Id,
                    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "Krujë").Id,
                    WaterBodyId = this.data.WaterBodies.FirstOrDefault(wb => wb.Name == "Ionian Sea").Id,
                    Description = "Gracefully surrounded by concrete buildings, this is where you don't want to be.",
                    Coordinates = "43.204666,27.910543",
                }
                //new Beach()
                //{
                //    Name = "Skiathos First Beach",
                //    CreatorId = creatorIds[2],
                //    CountryId = this.data.Countries.FirstOrDefault(c => c.Name == "Greece").Id,
                //    PrimaryDivisionId = this.data.PrimaryDivisions.FirstOrDefault(r => r.Name == "Thessaly").Id,
                //    SecondaryDivisionId = this.data.SecondaryDivisions.FirstOrDefault(r => r.Name == "Sporades").Id,
                //    TertiaryDivisionId = this.data.TertiaryDivisions.FirstOrDefault(r => r.Name == "Skiathos").Id,
                //    WaterBodyId = this.data.WaterBodies.FirstOrDefault(wb => wb.Name == "Mediterranean Sea").Id,
                //    Description = "Gracefully surrounded by concrete buildings, this is where you don't want to be.",
                //    Coordinates = "43.204666,27.910543",
                //}
            });

            foreach (var beach in beaches)
            {
                this.data.Beaches.Add(beach);
                beach.SetBeachData();

                this.data.SaveChanges();

                var searchService = new LuceneSearch(Index.BeachIndex);
                searchService.AddUpdateIndexEntry(beach);

                var continent = this.data.Continents.Find(beach.ContinentId);
                var country = this.data.Countries.Find(beach.CountryId);
                var primaryDivision = this.data.PrimaryDivisions.Find(beach.PrimaryDivisionId);
                var secondaryDivision = this.data.SecondaryDivisions.Find(beach.SecondaryDivisionId);
                var tertiaryDivision = this.data.TertiaryDivisions.Find(beach.TertiaryDivisionId);
                var quaternaryDivision = this.data.QuaternaryDivisions.Find(beach.QuaternaryDivisionId);
                var waterBody = this.data.WaterBodies.Find(beach.WaterBodyId);

                searchService.Index = Index.ContinentIndex;
                searchService.AddUpdateIndexEntry(continent);
                searchService.Index = Index.CountryIndex;
                searchService.AddUpdateIndexEntry(country);
                searchService.Index = Index.PrimaryDivisionIndex;
                searchService.AddUpdateIndexEntry(primaryDivision);
                searchService.Index = Index.SecondaryDivisionIndex;
                searchService.AddUpdateIndexEntry(secondaryDivision);
                searchService.Index = Index.TertiaryDivisionIndex;
                searchService.AddUpdateIndexEntry(tertiaryDivision);
                searchService.Index = Index.QuaternaryDivisionIndex;
                searchService.AddUpdateIndexEntry(quaternaryDivision);
                searchService.Index = Index.WaterBodyIndex;
                searchService.AddUpdateIndexEntry(waterBody);
            }
        }

        private void SeedIndexEntries()
        {
            var continents = this.data.Continents.Include(c => c.Beaches);
            var countries = this.data.Countries.Include(c => c.Beaches).Include(c => c.Continent);
            var primaryDivisions = this.data.PrimaryDivisions.Include(pd => pd.Beaches).Include(pd => pd.Country);
            var secondaryDivisions = this.data.SecondaryDivisions.Include(sd => sd.Beaches).Include(sd => sd.Country).Include(sd => sd.PrimaryDivision);
            var tertiaryDivisions = this.data.TertiaryDivisions.Include(td => td.Beaches).Include(td => td.Country).Include(td => td.PrimaryDivision).Include(td => td.SecondaryDivision);
            var quaternaryDivisions = this.data.QuaternaryDivisions.Include(qd => qd.Beaches).Include(qd => qd.Country).Include(qd => qd.PrimaryDivision).Include(qd => qd.SecondaryDivision).Include(qd => qd.TertiaryDivision);
            var waterBodies = this.data.WaterBodies.Include(wb => wb.Beaches);

            var searchService = new LuceneSearch(Index.ContinentIndex);
            searchService.AddUpdateIndexEntries(continents);
            searchService.Index = Index.CountryIndex;
            searchService.AddUpdateIndexEntries(countries);
            searchService.Index = Index.PrimaryDivisionIndex;
            searchService.AddUpdateIndexEntries(primaryDivisions);
            searchService.Index = Index.SecondaryDivisionIndex;
            searchService.AddUpdateIndexEntries(secondaryDivisions);
            searchService.Index = Index.TertiaryDivisionIndex;
            searchService.AddUpdateIndexEntries(tertiaryDivisions);
            searchService.Index = Index.QuaternaryDivisionIndex;
            searchService.AddUpdateIndexEntries(quaternaryDivisions);
            searchService.Index = Index.WaterBodyIndex;
            searchService.AddUpdateIndexEntries(waterBodies);
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

                reviews[i].UpdateScores();

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

        private int? GetContinentId(string place)
        {
            var tokens = place.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            var continent = tokens[0];

            switch (continent)
            {
                case "Africa":
                    return 1;
                case "Antarctica":
                    return 2;
                case "Asia":
                    return 3;
                case "Europe":
                    return 4;
                case "North America":
                    return 5;
                case "Oceania":
                    return 6;
                case "South America":
                    return 7;
                default:
                    return null;
            }
        }

        private string GetPlaceName(string place)
        {
            var tokens = place.Split(new char[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            var noWaterBody = (tokens.Length == 1);

            if (noWaterBody)
            {
                var continentIndex = place.IndexOf('%');

                if (continentIndex > -1)
                {
                    place = place.Substring(continentIndex + 1);
                }

                return place;
            }

            var placeName = tokens[1];

            return placeName;
        }

        private int? GetWaterBodyId(string place)
        {
            var continentIndex = place.IndexOf('%');
            place = place.Substring(continentIndex + 1);
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

        private int GetWaterBodyId(int countryId, int? primaryDivisionid, int? secondaryDivisionId)
        {
            var secondaryDivision = this.data.SecondaryDivisions.Include(sd => sd.WaterBody).FirstOrDefault(sd => sd.Id == secondaryDivisionId);
            var secondaryDivisionHasWaterBody = (secondaryDivision != null && secondaryDivision.WaterBodyId != null);

            if (secondaryDivisionHasWaterBody)
            {
                return (int)secondaryDivision.WaterBodyId;
            }

            var primaryDivision = this.data.PrimaryDivisions.Include(pd => pd.WaterBody).FirstOrDefault(pd => pd.Id == primaryDivisionid);
            var primaryDivisionHasWaterBody = (primaryDivision != null && primaryDivision.WaterBodyId != null);

            if (primaryDivisionHasWaterBody)
            {
                return (int)primaryDivision.WaterBodyId;
            }

            var waterBodyId = this.data.Countries.Include(c => c.WaterBody).FirstOrDefault(c => c.Id == countryId).WaterBodyId;

            return (int)waterBodyId;
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
                        Id = currentPrimaryDivisionId,
                        Name = this.GetPlaceName(value),
                        ContinentId = currentContinentId,
                        CountryId = currentCountryId,
                        WaterBodyId = currentCountryWaterBodyId ?? this.GetWaterBodyId(value)
                    };
                    currentPrimaryDivisionWaterBodyId = primaryDivision.WaterBodyId;

                    this.data.PrimaryDivisions.Add(primaryDivision);

                    currentPrimaryDivisionId++;
                    entryCounter++;

                    searchableDivision = primaryDivision;

                    break;
                case 2:
                    var secondaryDivision = new SecondaryDivision()
                    {
                        Id = currentSecondaryDivisionId,
                        Name = this.GetPlaceName(value),
                        ContinentId = currentContinentId,
                        CountryId = currentCountryId,
                        PrimaryDivisionId = currentPrimaryDivisionId - 1,
                        WaterBodyId = currentPrimaryDivisionWaterBodyId ?? this.GetWaterBodyId(value)
                    };

                    this.data.SecondaryDivisions.Add(secondaryDivision);

                    currentSecondaryDivisionId++;
                    entryCounter++;

                    searchableDivision = secondaryDivision;

                    break;
                case 3:
                    var tertiaryDivision = new TertiaryDivision()
                    {
                        Id = currentTertiaryDivisionId,
                        Name = value,
                        ContinentId = currentContinentId,
                        CountryId = currentCountryId,
                        PrimaryDivisionId = currentPrimaryDivisionId - 1,
                        SecondaryDivisionId = currentSecondaryDivisionId - 1
                    };

                    this.data.TertiaryDivisions.Add(tertiaryDivision);

                    currentTertiaryDivisionId++;
                    entryCounter++;

                    searchableDivision = tertiaryDivision;

                    break;
                case 4:
                    var quaternaryDivision = new QuaternaryDivision()
                    {
                        Id = currentQuaternaryDivisionId,
                        Name = value,
                        ContinentId = currentContinentId,
                        CountryId = currentCountryId,
                        PrimaryDivisionId = currentPrimaryDivisionId - 1,
                        SecondaryDivisionId = currentSecondaryDivisionId - 1,
                        TertiaryDivisionId = currentTertiaryDivisionId - 1
                    };

                    this.data.QuaternaryDivisions.Add(quaternaryDivision);

                    currentQuaternaryDivisionId++;
                    entryCounter++;

                    searchableDivision = quaternaryDivision;

                    break;
                default:
                    throw new ArgumentException("Invalid division.");
            }

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