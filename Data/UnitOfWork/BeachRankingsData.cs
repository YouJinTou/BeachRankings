namespace BeachRankings.Data.UnitOfWork
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using BeachRankings.Models;
    using BeachRankings.Data.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    public class BeachRankingsData : IBeachRankingsData
    {
        private readonly DbContext dbContext;

        private readonly IDictionary<Type, object> repositories;

        private IUserStore<User> userStore;

        public BeachRankingsData()
            : this(new BeachRankingsDbContext())
        {
        }

        public BeachRankingsData(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IRepository<Beach> Beaches
        {
            get
            {
                return this.GetRepository<Beach>();
            }
        }

        public IRepository<BeachPhoto> BeachPhotos
        {
            get
            {
                return this.GetRepository<BeachPhoto>();
            }
        }

        public IRepository<Review> Reviews
        {
            get
            {
                return this.GetRepository<Review>();
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

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.dbContext));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}