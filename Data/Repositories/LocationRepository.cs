namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Services.Search;
    using System.Data.Entity;

    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        private DbContext dbContext;
        private IDbSet<Location> entitySet;

        public LocationRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<Location>();
        }

        public void AddLocationToIndex(Location location)
        {
            LuceneSearch.Index = Indices.LocationIndex;

            LuceneSearch.AddUpdateIndexEntry(location);
        }
    }
}