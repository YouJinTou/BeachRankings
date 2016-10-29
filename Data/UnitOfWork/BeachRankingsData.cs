namespace BeachRankings.Data.UnitOfWork
{
    using BeachRankings.Data.Repositories.Interfaces;
    using BeachRankings.Models;
    using BeachRankings.Data.Repositories;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class BeachRankingsData : IBeachRankingsData
    {
        private readonly DbContext dbContext;

        private IGenericRepository<User> users;
        private IUserStore<User> userStore;
        private ICountryRepository countries;
        private IPrimaryDivisionRepository primaryDivisions;
        private ISecondaryDivisionRepository secondaryDivisions;
        private ITertiaryDivisionRepository tertiaryDivisions;
        private IQuaternaryDivisionRepository quaternaryDivisions;
        private IWaterBodyRepository waterBodies;
        private IBeachRepository beaches;
        private IGenericRepository<BeachImage> beachImages;
        private IGenericRepository<Review> reviews;

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

        public ICountryRepository Countries
        {
            get
            {
                if (this.countries == null)
                {
                    this.countries = new CountryRepository(this.dbContext);
                }

                return this.countries;
            }
        }

        public IPrimaryDivisionRepository PrimaryDivisions
        {
            get
            {
                if (this.primaryDivisions == null)
                {
                    this.primaryDivisions = new PrimaryDivisionRepository(this.dbContext);
                }

                return this.primaryDivisions;
            }
        }

        public ISecondaryDivisionRepository SecondaryDivisions
        {
            get
            {
                if (this.secondaryDivisions == null)
                {
                    this.secondaryDivisions = new SecondaryDivisionRepository(this.dbContext);
                }

                return this.secondaryDivisions;
            }
        }

        public ITertiaryDivisionRepository TertiaryDivisions
        {
            get
            {
                if (this.tertiaryDivisions == null)
                {
                    this.tertiaryDivisions = new TertiaryDivisionRepository(this.dbContext);
                }

                return this.tertiaryDivisions;
            }
        }

        public IQuaternaryDivisionRepository QuaternaryDivisions
        {
            get
            {
                if (this.quaternaryDivisions == null)
                {
                    this.quaternaryDivisions = new QuaternaryDivisionRepository(this.dbContext);
                }

                return this.quaternaryDivisions;
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