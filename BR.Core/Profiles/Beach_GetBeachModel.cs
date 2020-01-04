using AutoMapper;
using BR.Core.Models;

namespace BR.Core.Profiles
{
    public class Beach_GetBeachModel : Profile
    {
        public Beach_GetBeachModel()
        {
            this.CreateMap<Beach, GetBeachModel>();
        }
    }
}
