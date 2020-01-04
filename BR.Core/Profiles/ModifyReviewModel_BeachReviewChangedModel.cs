using AutoMapper;
using BR.Core.Models;

namespace BR.Core.Profiles
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
