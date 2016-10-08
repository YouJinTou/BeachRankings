namespace App.App_Start
{
    using AutoMapper;
    using BeachRankings.Models;
    using Models.BindingModels;
    using Models.ViewModels;
    using System.Linq;

    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PostReviewBindingModel, Review>();
                cfg.CreateMap<Beach, BeachSummaryViewModel>()
                    .ForMember(viewModel => viewModel.ImagePath, model => model.MapFrom(m => m.Photos.FirstOrDefault().Path));
            });
        }
    }
}