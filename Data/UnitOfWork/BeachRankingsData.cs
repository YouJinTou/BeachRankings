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
        private IDivisionRepository divisions;
        private IRegionRepository regions;
        private IAreaRepository areas;
        private IWaterBodyRepository waterBodies;
        private IBeachRepository beaches;
        private IGenericRepository<BeachImage> beachImages;
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

        public IDivisionRepository Divisions
        {
            get
            {
                if (this.divisions == null)
                {
                    this.divisions = new DivisionRepository(this.dbContext);
                }

                return this.divisions;
            }
        }

        public IRegionRepository Regions
        {
            get
            {
                if (this.regions == null)
                {
                    this.regions = new RegionRepository(this.dbContext);
                }

                return this.regions;
            }
        }

        public IAreaRepository Areas
        {
            get
            {
                if (this.areas == null)
                {
                    this.areas = new AreaRepository(this.dbContext);
                }

                return this.areas;
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

        public IGenericRepository<BeachImage> BeachImages
        {
            get
            {
                if (this.beachImages == null)
                {
                    this.beachImages = new GenericRepository<BeachImage>(this.dbContext);
                }

                return this.beachImages;
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

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }
    }
}