using AutoMapper;
using BR.ReviewsService.Models;

namespace BR.ReviewsService.Profiles
{
    public class ModifyReviewModel_BeachReviewChangedModel : Profile
    {
        public ModifyReviewModel_BeachReviewChangedModel()
        {
            this.CreateMap<ModifyReviewModel, BeachReviewChangedModel>()
                .ForMember(d => d.ReviewId, o => o.MapFrom(s => s.Id));
        }
    }
}
