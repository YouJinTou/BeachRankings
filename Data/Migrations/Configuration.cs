namespace BeachRankings.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<BeachRankingsDbContext>
    {
        private BeachRankingsDbContext data;

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
            this.SeedBeaches();
            this.SeedBeachPhotos();
            //this.SeedReviews();
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
        }

        private void SeedBeaches()
        {
            if (this.data.Beaches.Any())
            {
                return;
            }

            this.data.Beaches.Add(new Beach()
            {
                Name = "Kamchia Beach",
                Location = "Kamchia",
                Description = "Kamchia beach is situated where the muddy Kamchia flows into the Black Sea.",
                Photos = new HashSet<BeachPhoto>()
            });
            this.data.Beaches.Add(new Beach()
            {
                Name = "Bolata",
                Location = "Kaliakra",
                Description = "Situated north of Albena, Bolata is an ungainly sight.",
                Photos = new HashSet<BeachPhoto>()
            });
            this.data.Beaches.Add(new Beach()
            {
                Name = "Sunny Day Beach",
                Location = "Varna",
                Description = "Gracefully surrounded by concrete buildings, this is where you don't want to be.",
                Photos = new HashSet<BeachPhoto>()
            });

            this.data.SaveChanges();
        }

        private void SeedBeachPhotos()
        {
            if (this.data.BeachPhotos.Any())
            {
                return;
            }

            var adminId = this.data.Users.FirstOrDefault(u => u.UserName == "admin").Id;
            var userId = this.data.Users.FirstOrDefault(u => u.UserName == "user").Id;

            this.data.Beaches.FirstOrDefault(b => b.Name == "Kamchia Beach").Photos.Add(new BeachPhoto()
            {
                AuthorId = userId,
                BeachId = this.data.Beaches.FirstOrDefault(b => b.Name == "Kamchia Beach").Id,
                Description = "We were pretty happy taking this shot.",
                Path = "/Content/Images/kamchia_river.jpg"
            });
            this.data.Beaches.FirstOrDefault(b => b.Name == "Bolata").Photos.Add(new BeachPhoto()
            {
                AuthorId = adminId,
                BeachId = this.data.Beaches.FirstOrDefault(b => b.Name == "Bolata").Id,
                Description = "We took this beautiful shot from the chopper.",
                Path = "/Content/Images/bolata.jpg"
            });
            this.data.Beaches.FirstOrDefault(b => b.Name == "Sunny Day Beach").Photos.Add(new BeachPhoto()
            {
                AuthorId = userId,
                BeachId = this.data.Beaches.FirstOrDefault(b => b.Name == "Sunny Day Beach").Id,
                Description = "The weather was nice.",
                Path = "/Content/Images/sunny_day.jpg"
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