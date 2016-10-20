namespace BeachRankings.Data.UnitOfWork
{
    using BeachRankings.Models;
    using BeachRankings.Data.Repositories;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;    
    using System.Data.Entity;

    public class BeachRankingsData : IBeachRankingsData
    {
        private readonly DbContext dbContext;

        private IGenericRepository<User> users;
        private IGenericRepository<Country> countries;
        private IWaterBodyRepository waterBodies;
        private ILocationRepository locations;
        private IBeachRepository beaches;
        private IGenericRepository<BeachPhoto> beachPhotos;
        private IGenericRepository<Review> reviews;
        private IUserStore<User> userStore;

        public BeachRankingsData()
            : this(new BeachRankingsDbContext())
        {
        }

        public BeachRankingsData(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IGenericRepository<User> Users
        {
            get
            {
                if (this.users == null)
                {
                    this.users = new GenericRepository<User>(this.dbContext);
                }

                return this.users;
            }
        }

        public IGenericRepository<Country> Countries
        {
            get
            {
                if (this.countries == null)
                {
                    this.countries = new GenericRepository<Country>(this.dbContext);
                }

                return this.countries;
            }
        }

        public IWaterBodyRepository WaterBodies
        {
            get
            {
                if (this.waterBodies == null)
                {
                    this.waterBodies = new WaterBodyRepository(this.dbContext);
                }

                return this.waterBodies;
            }
        }

        public ILocationRepository Locations
        {
            get
            {
                if (this.locations == null)
                {
                    this.locations = new LocationRepository(this.dbContext);
                }

                return this.locations;
            }
        }

        public IBeachRepository Beaches
        {
            get
            {
                if (this.beaches == null)
                {
                    this.beaches = new BeachRepository(this.dbContext);
                }

                return this.beaches;
            }
        }

        public IGenericRepository<BeachPhoto> BeachPhotos
        {
            get
            {
                if (this.beachPhotos == null)
                {
                    this.beachPhotos = new GenericRepository<BeachPhoto>(this.dbContext);
                }

                return this.beachPhotos;
            }
        }

        public IGenericRepository<Review> Reviews
        {
            get
            {
                if (this.reviews == null)
                {
                    this.reviews = new GenericRepository<Review>(this.dbContext);
                }

                return this.reviews;
            }
        }    

        public IUserStore<User> UserStore
        {
            get
            {
                if (this.userStore == null)
                {
                    this.userStore = new UserStore<User>(this.dbContext);
                }

                return this.userStore;
            }
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }
    }
}