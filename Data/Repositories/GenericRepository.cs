﻿namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Data.Repositories.Interfaces;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private DbContext dbContext;
        private IDbSet<TEntity> entitySet;

        public GenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<TEntity>();
        }

        public IDbSet<TEntity> Set
        {
            get { return this.entitySet; }
        }

        public IQueryable<TEntity> All()
        {
            return this.entitySet;
        }

        public TEntity Find(object id)
        {
            return this.entitySet.Find(id);
        }

        public TEntity Add(TEntity entity)
        {
            return this.ChangeState(entity, EntityState.Added);
        }

        public void AddMany(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Add(entity);
            }
        }

        public TEntity Update(TEntity entity)
        {
            return this.ChangeState(entity, EntityState.Modified);
        }

        public void Remove(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public void RemoveMany(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Remove(entity);
            }
        }

        public TEntity Remove(object id)
        {
            var entity = this.Find(id);

            this.Remove(entity);

            return entity;
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        private TEntity ChangeState(TEntity entity, EntityState state)
        {
            var entry = this.dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.entitySet.Attach(entity);
            }

            entry.State = state;

            return entity;
        }
    }
}