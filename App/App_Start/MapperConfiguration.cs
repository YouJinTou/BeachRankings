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
                cfg.CreateMap<Location, WaterBodyViewModel>();
                
                cfg.CreateMap<Beach, ConciseBeachViewModel>()
                    .ForMember(vm => vm.ImagePath, model => model.MapFrom(m => m.Photos.FirstOrDefault().Path))
                    .ForMember(vm => vm.Location, model => model.MapFrom(m => m.Location.Name));
                cfg.CreateMap<Beach, DetailedBeachViewModel>()
                    .ForMember(vm => vm.Location, model => model.MapFrom(m => m.Location.Name));
                cfg.CreateMap<AddBeachBindingModel, Beach>();
                cfg.CreateMap<Beach, AutocompleteViewModel>();

                cfg.CreateMap<Review, ConciseReviewViewModel>()
                    .ForMember(vm => vm.UserName, model => model.MapFrom(m => m.Author.UserName))
                    .ForMember(vm => vm.AvatarPath, model => model.MapFrom(m => m.Author.AvatarPath));
                cfg.CreateMap<PostReviewBindingModel, Review>();
                cfg.CreateMap<Review, EditReviewViewModel>();
                cfg.CreateMap<EditReviewBindingModel, Review>().AfterMap((vm, model) => model.UpdateTotalScore());
                cfg.CreateMap<Review, DetailedReviewViewModel>()
                    .ForMember(vm => vm.UserName, model => model.MapFrom(m => m.Author.UserName))
                    .ForMember(vm => vm.AvatarPath, model => model.MapFrom(m => m.Author.AvatarPath));
            });
        }
    }
}