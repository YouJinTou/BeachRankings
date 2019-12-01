using AutoMapper;
using BR.BeachesService.Models;

namespace BR.BeachesService.Profiles
{
    public class Beach_GetBeachModel : Profile
    {
        public Beach_GetBeachModel()
        {
            this.CreateMap<Beach, GetBeachModel>();
        }
    }
}
