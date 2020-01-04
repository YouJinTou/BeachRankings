using AutoMapper;
using BR.Core.Models;

namespace BR.Core.Profiles
{
    public class ModifyReviewModel_UserModifiedReviewModel : Profile
    {
        public ModifyReviewModel_UserModifiedReviewModel()
        {
            this.CreateMap<ModifyReviewModel, UserModifiedReviewModel>()
                .ForMember(d => d.ReviewId, o => o.MapFrom(s => s.Id));
        }
    }
}
