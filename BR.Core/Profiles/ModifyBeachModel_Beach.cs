using AutoMapper;
using BR.Core.Models;
using System;

namespace BR.Core.Profiles
{
    public class ModifyBeachModel_Beach : Profile
    {
        public ModifyBeachModel_Beach()
        {
            this.CreateMap<ModifyBeachModel, Beach>()
                .AfterMap((s, d) => d.AddedBy = s.ModifiedBy)
                .AfterMap((s, d) => d.LastUpdatedOn = DateTime.UtcNow);
        }
    }
}
