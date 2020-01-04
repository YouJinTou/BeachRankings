using AutoMapper;
using BR.Core.Models;

namespace BR.Core.Profiles
{
    public class User_GetUserModel : Profile
    {
        public User_GetUserModel()
        {
            this.CreateMap<User, GetUserModel>();
        }
    }
}
