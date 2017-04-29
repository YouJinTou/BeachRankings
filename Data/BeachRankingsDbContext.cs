﻿namespace BeachRankings.Data
{
    using BeachRankings.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class BeachRankingsDbContext : IdentityDbContext<User>
    {
        public BeachRankingsDbContext()
            : base("BeachRankingsConnection", throwIfV1Schema: false)
        {
        }

        public IDbSet<Continent> Continents { get; set; }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<PrimaryDivision> PrimaryDivisions { get; set; }

        public IDbSet<SecondaryDivision> SecondaryDivisions { get; set; }

        public IDbSet<TertiaryDivision> TertiaryDivisions { get; set; }

        public IDbSet<QuaternaryDivision> QuaternaryDivisions { get; set; }

        public IDbSet<WaterBody> WaterBodies { get; set; }

        public IDbSet<Beach> Beaches { get; set; }

        public IDbSet<BeachImage> BeachImages { get; set; }

        public IDbSet<Review> Reviews { get; set; }

        public IDbSet<Blog> Blogs { get; set; }

        public IDbSet<BlogArticle> BlogArticles { get; set; }

        public static BeachRankingsDbContext Create()
        {
            return new BeachRankingsDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOptional(u => u.Blog)
                .WithRequired(b => b.User);

            modelBuilder.Entity<Continent>()
                .HasMany(c => c.Countries)
                .WithOptional(pd => pd.Continent)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<Review>()
                .HasRequired(r => r.Author)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AuthorId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Review>()
                .HasRequired(r => r.Beach)
                .WithMany(b => b.Reviews)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BeachImage>()
               .HasRequired(bi => bi.Beach)
               .WithMany(b => b.Images)
               .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}