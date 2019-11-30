using AutoMapper;
using BR.Iam.Models;

namespace BR.Iam.Profiles
{
    public class CreateUserModel_User : Profile
    {
        public CreateUserModel_User()
        {
            this.CreateMap<CreateUserModel, User>();
        }
    }
}
