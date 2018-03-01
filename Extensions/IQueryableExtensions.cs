using BeachRankings.Models;
using BeachRankings.Models.Enums;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<Beach> OrderByCriterion<Beach>(this IQueryable<Beach> beaches, int criterion)
        {
            var noCriterionSelected = (criterion == 0);
            var criterionExist = (criterion > 0 && criterion <= 15);

            if (noCriterionSelected || !criterionExist)
            {
                return beaches;
            }

            var criterionName = Enum.GetName(typeof(Criterion), criterion);
            var parameter = Expression.Parameter(typeof(Beach), "p");
            var property = Expression.Property(parameter, criterionName);
            var expression = Expression.Lambda(property, parameter);
            var method = "OrderByDescending";
            var types = new Type[] { beaches.ElementType, expression.Body.Type };
            var call = Expression.Call(typeof(Queryable), method, types, beaches.Expression, expression);

            return beaches.Provider.CreateQuery<Beach>(call);
        }

        public static IQueryable<Beach> FilterByContinent(this IQueryable<Beach> beaches, int id)
        {
            var noContinentSelected = (id == 0);

            if (noContinentSelected)
            {
                return beaches;
            }

            return beaches.Where(b => b.ContinentId == id);
        }

        public static IQueryable<Beach> FilterByCountry(this IQueryable<Beach> beaches, int id)
        {
            var noCountrySelected = (id == 0);

            if (noCountrySelected)
            {
                return beaches;
            }

            return beaches.Where(b => b.CountryId == id);
        }

        public static IQueryable<Beach> FilterByWaterBody(this IQueryable<Beach> beaches, int id)
        {
            var noWaterBodySelected = (id == 0);

            if (noWaterBodySelected)
            {
                return beaches;
            }

            return beaches.Where(b => b.WaterBodyId == id);
        }
    }
}
