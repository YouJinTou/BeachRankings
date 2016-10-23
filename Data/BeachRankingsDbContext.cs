namespace BeachRankings.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using BeachRankings.Models;
    using System.Data.Entity;

    public class BeachRankingsDbContext : IdentityDbContext<User>
    {
        public BeachRankingsDbContext()
            : base("BeachRankingsConnection", throwIfV1Schema: false)
        {
        }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Region> Regions { get; set; }

        public IDbSet<Area> Areas { get; set; }

        public IDbSet<WaterBody> WaterBodies { get; set; }

        public IDbSet<Beach> Beaches { get; set; }

        public IDbSet<BeachImage> BeachImages { get; set; }

        public IDbSet<Review> Reviews { get; set; }

        public static BeachRankingsDbContext Create()
        {
            return new BeachRankingsDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasMany(c => c.Regions)
                .WithRequired(r => r.Country)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Country>()
               .HasMany(c => c.Beaches)
               .WithRequired(b => b.Country)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<Region>()
               .HasMany(c => c.Beaches)
               .WithRequired(b => b.Region)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<WaterBody>()
               .HasMany(c => c.Beaches)
               .WithRequired(b => b.WaterBody)
               .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}