namespace BeachRankings.Data
{
    using BeachRankings.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration.Configuration;

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

            modelBuilder.Entity<Review>()
                .HasRequired(r => r.Author)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AuthorId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Review>()
                .HasRequired(r => r.Beach)
                .WithMany(b => b.Reviews)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Review>()
                .HasMany(r => r.BlogArticles)
                .WithRequired(ba => ba.Review)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<BeachImage>()
               .HasRequired(bi => bi.Beach)
               .WithMany(b => b.Images)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlogArticle>()
                .HasRequired(ba => ba.Blog)
                .WithMany(b => b.BlogArticles)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<BlogArticle>()
               .HasRequired(ba => ba.Beach)
               .WithMany(b => b.BlogArticles)
               .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }

    //internal static class TypeConfigurationExtensions
    //{
    //    public static PrimitivePropertyConfiguration HasUniqueIndexAnnotation(
    //        this PrimitivePropertyConfiguration property,
    //        string indexName,
    //        int columnOrder)
    //    {
    //        var indexAttribute = new IndexAttribute(indexName, columnOrder) { IsUnique = true };
    //        var indexAnnotation = new IndexAnnotation(indexAttribute);

    //        return property.HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
    //    }
    //}
}