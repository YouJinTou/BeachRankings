﻿namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;

    public interface IWaterBodyRepository : IGenericRepository<WaterBody>
    {
        IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix);

        void AddWaterBodyToIndex(WaterBody waterBody);
    }
}