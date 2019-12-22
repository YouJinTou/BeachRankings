using AutoMapper;
using BR.BeachesService.Models;
using System;

namespace BR.BeachesService.Profiles
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
