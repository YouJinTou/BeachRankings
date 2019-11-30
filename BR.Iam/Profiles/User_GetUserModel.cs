using AutoMapper;
using BR.Iam.Models;

namespace BR.Iam.Profiles
{
    public class User_GetUserModel : Profile
    {
        public User_GetUserModel()
        {
            this.CreateMap<User, GetUserModel>();
        }
    }
}
