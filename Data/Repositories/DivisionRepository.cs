namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Models.Interfaces;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class DivisionRepository : IDivisionRepository
    {
        private DbContext dbContext;
        private IDbSet<PrimaryDivision> primaryDivisionsEntitySet;
        private IDbSet<SecondaryDivision> secondaryDivisionsEntitySet;
        private IDbSet<TertiaryDivision> tertiaryDivisionsEntitySet;
        private IDbSet<QuaternaryDivision> quaternaryDivisionsEntitySet;

        public DivisionRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.primaryDivisionsEntitySet = dbContext.Set<PrimaryDivision>();
            this.secondaryDivisionsEntitySet = dbContext.Set<SecondaryDivision>();
            this.tertiaryDivisionsEntitySet = dbContext.Set<TertiaryDivision>();
            this.quaternaryDivisionsEntitySet = dbContext.Set<QuaternaryDivision>();
        }

        public IDivision Find(Type type, object id)
        {
            if (type == typeof(PrimaryDivision))
            {
                return this.primaryDivisionsEntitySet.Find(id);
            } 
            else if (type == typeof(SecondaryDivision))
            {
                return this.secondaryDivisionsEntitySet.Find(id);
            }
            else if (type == typeof(TertiaryDivision))
            {
                return this.tertiaryDivisionsEntitySet.Find(id);
            }
            else if (type == typeof(QuaternaryDivision))
            {
                return this.quaternaryDivisionsEntitySet.Find(id);
            }

            throw new ArgumentException("The type provided was incorrect.");
        }

        public IDivision Add(IDivision division)
        {
            return this.ChangeState(division, EntityState.Added);
        }

        public void Remove(IDivision division)
        {
            this.ChangeState(division, EntityState.Deleted);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public IQueryable<PrimaryDivision> PrimaryDivisions()
        {
            return this.primaryDivisionsEntitySet;
        }

        public IQueryable<SecondaryDivision> SecondaryDivisions()
        {
            return this.secondaryDivisionsEntitySet;
        }

        public IQueryable<TertiaryDivision> TertiaryDivisions()
        {
            return this.tertiaryDivisionsEntitySet;
        }

        public IQueryable<QuaternaryDivision> QuaternaryDivisions()
        {
            return this.quaternaryDivisionsEntitySet;
        }

        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            LuceneSearch.Index = Index.RegionIndex;
            var results = LuceneSearch.SearchByPrefix(prefix, 10);

            return results;
        }

        public void AddToIndex(ISearchable searchable)
        {
            LuceneSearch.Index = Index.RegionIndex;

            LuceneSearch.AddUpdateIndexEntry(searchable);
        }

        private IDivision ChangeState(IDivision division, EntityState state)
        {
            var entry = this.dbContext.Entry(division);

            if (entry.State == EntityState.Detached)
            {
                if (division is PrimaryDivision)
                {
                    this.primaryDivisionsEntitySet.Attach((PrimaryDivision)division);
                }
                else if (division is SecondaryDivision)
                {
                    this.secondaryDivisionsEntitySet.Attach((SecondaryDivision)division);
                }
                else if (division is TertiaryDivision)
                {
                    this.tertiaryDivisionsEntitySet.Attach((TertiaryDivision)division);
                }
                else if (division is QuaternaryDivision)
                {
                    this.quaternaryDivisionsEntitySet.Attach((QuaternaryDivision)division);
                }
            }

            entry.State = state;

            return division;
        }
    }
}