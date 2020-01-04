﻿using BR.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IBeachesService
    {
        Task<Beach> GetBeachAsync(string id);

        Task<IEnumerable<Beach>> GetBeachesAsync(IEnumerable<string> ids);

        Task<Beach> CreateBeachAsync(CreateBeachModel model);

        Task<Beach> ModifyBeachAsync(ModifyBeachModel model);
    }
}