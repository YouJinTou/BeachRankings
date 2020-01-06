using AutoMapper;
using BR.Core.Models;

namespace BR.Core.Profiles
{
    public class Beach_GetBeachModel : Profile
    {
        public Beach_GetBeachModel()
        {
            this.CreateMap<Beach, GetBeachModel>()
                .ForMember(d => d.CountryId, o => o.MapFrom(s => s.Country))
                .ForMember(d => d.L1Id, o => o.MapFrom(s => $"{s.Continent}_{s.Country}_{s.L1}"))
                .ForMember(d => d.L2Id, o => o.MapFrom(s => $"{s.Continent}_{s.Country}_{s.L1}_{s.L2}"))
                .ForMember(d => d.L3Id, o => o.MapFrom(s => $"{s.Continent}_{s.Country}_{s.L1}_{s.L2}_{s.L3}"))
                .ForMember(d => d.L4Id, o => o.MapFrom(s => $"{s.Continent}_{s.Country}_{s.L1}_{s.L2}_{s.L3}_{s.L4}"));
        }
    }
}
