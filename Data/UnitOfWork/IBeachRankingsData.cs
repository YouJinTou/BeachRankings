namespace BeachRankings.Data.UnitOfWork
{
    using BeachRankings.Data.Repositories.Interfaces;
    using BeachRankings.Models;

    public interface IBeachRankingsData
    {
        IGenericRepository<User> Users { get; }

        IContinentRepository Continents { get; }

        ICountryRepository Countries { get; }

        IPrimaryDivisionRepository PrimaryDivisions { get; }

        ISecondaryDivisionRepository SecondaryDivisions { get; }

        ITertiaryDivisionRepository TertiaryDivisions { get; }

        IQuaternaryDivisionRepository QuaternaryDivisions { get; }

        IBeachRepository Beaches { get; }

        IWaterBodyRepository WaterBodies { get; }

        IGenericRepository<BeachImage> BeachImages { get; }

        IGenericRepository<Review> Reviews { get; }

        IGenericRepository<Blog> Blogs { get; }

        IGenericRepository<BlogArticle> BlogArticles { get; }

        void SaveChanges();
    }
}