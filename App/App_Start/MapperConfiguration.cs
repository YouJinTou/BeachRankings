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
                cfg.CreateMap<Beach, ConciseBeachViewModel>()
                    .ForMember(vm => vm.ImagePath, model => model.MapFrom(m => m.Photos.FirstOrDefault().Path));
                cfg.CreateMap<Beach, DetailedBeachViewModel>();
                cfg.CreateMap<AddBeachBindingModel, Beach>();
                cfg.CreateMap<Review, ConciseReviewViewModel>();
                cfg.CreateMap<PostReviewBindingModel, Review>();                
                cfg.CreateMap<Review, EditReviewViewModel>();
            });
        }
    }
}