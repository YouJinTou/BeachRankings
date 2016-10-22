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

        public IDbSet<WaterBody> WaterBodies { get; set; }

        public IDbSet<Location> Locations { get; set; }

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
                .HasMany(c => c.Locations)
                .WithOptional(l => l.Country)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}