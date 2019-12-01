using AutoMapper;
using BR.BeachesService.Models;

namespace BR.BeachesService.Profiles
{
    public class CreateBeachModel_Beach : Profile
    {
        public CreateBeachModel_Beach()
        {
            this.CreateMap<Beach, CreateBeachModel>();
        }
    }
}
