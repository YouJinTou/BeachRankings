namespace BeachRankings.Data.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IGenericRepository<T>
    {
        IQueryable<T> All();

        T Find(object id);

        T Add(T entity);

        void AddMany(IEnumerable<T> entities);

        T Update(T entity);

        void Remove(T entity);

        void RemoveMany(IEnumerable<T> entities);

        T Remove(object id);

        void Detach(T entity);

        void SaveChanges();
    }
}