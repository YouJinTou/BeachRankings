using AutoMapper;
using BR.Core.Models;

namespace BR.Core.Profiles
{
    public class User_CreateUserResult : Profile
    {
        public User_CreateUserResult()
        {
            this.CreateMap<User, CreateUserResult>()
                .ForMember(d => d.IsSuccess, o => o.MapFrom(s => true));
        }
    }
}
