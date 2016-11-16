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

        public IDbSet<PrimaryDivision> PrimaryDivisions { get; set; }

        public IDbSet<SecondaryDivision> SecondaryDivisions { get; set; }

        public IDbSet<TertiaryDivision> TertiaryDivisions { get; set; }

        public IDbSet<QuaternaryDivision> QuaternaryDivisions { get; set; }

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
                .HasMany(c => c.PrimaryDivisions)
                .WithRequired(pd => pd.Country)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Country>()
                .HasMany(c => c.SecondaryDivisions)
                .WithRequired(pd => pd.Country)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Country>()
                .HasMany(c => c.TertiaryDivisions)
                .WithRequired(pd => pd.Country)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Country>()
                .HasMany(c => c.QuaternaryDivisions)
                .WithRequired(pd => pd.Country)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PrimaryDivision>()
                .HasMany(pd => pd.Beaches)
                .WithRequired(b => b.PrimaryDivision)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PrimaryDivision>()
                .HasMany(pd => pd.SecondaryDivisions)
                .WithRequired(b => b.PrimaryDivision)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PrimaryDivision>()
                .HasMany(pd => pd.TertiaryDivisions)
                .WithRequired(b => b.PrimaryDivision)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PrimaryDivision>()
                .HasMany(pd => pd.QuaternaryDivisions)
                .WithRequired(b => b.PrimaryDivision)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<SecondaryDivision>()
               .HasMany(pd => pd.TertiaryDivisions)
               .WithRequired(b => b.SecondaryDivision)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<SecondaryDivision>()
               .HasMany(pd => pd.QuaternaryDivisions)
               .WithRequired(b => b.SecondaryDivision)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<TertiaryDivision>()
               .HasMany(pd => pd.QuaternaryDivisions)
               .WithRequired(b => b.TertiaryDivision)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Beach>()
                .HasMany(b => b.Reviews)
                .WithRequired(r => r.Beach)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Beach>()
               .HasMany(b => b.Images)
               .WithRequired(i => i.Beach)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithRequired(r => r.Author)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}