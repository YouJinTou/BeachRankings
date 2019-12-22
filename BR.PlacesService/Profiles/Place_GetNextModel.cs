using AutoMapper;
using BR.Core.Models;
using BR.PlacesService.Models;

namespace BR.PlacesService.Profiles
{
    public class Place_GetNextModel : Profile
    {
        public Place_GetNextModel()
        {
            this.CreateMap<Place, GetNextModel>();
        }
    }
}
