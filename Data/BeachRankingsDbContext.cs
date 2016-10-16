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

        public IDbSet<Location> Locations { get; set; }

        public IDbSet<Beach> Beaches { get; set; }

        public IDbSet<BeachPhoto> BeachPhotos { get; set; }

        public IDbSet<Review> Reviews { get; set; }

        public static BeachRankingsDbContext Create()
        {
            return new BeachRankingsDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beach>()
                .HasMany(b => b.Photos);

            base.OnModelCreating(modelBuilder);
        }
    }
}