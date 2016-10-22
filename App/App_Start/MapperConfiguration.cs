namespace App.App_Start
{
    using AutoMapper;
    using BeachRankings.Models;
    using BeachRankings.App.Models.BindingModels;
    using BeachRankings.App.Models.ViewModels;
    using System.Linq;

    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Country, AutocompleteCountryViewModel>()
                    .ForMember(vm => vm.BeachCount, model => model.MapFrom(m => m.Beaches.Count));
                cfg.CreateMap<Country, CountryBeachesViewModel>();

                cfg.CreateMap<WaterBody, AddBeachWaterBodyViewModel>();
                cfg.CreateMap<WaterBody, AutocompleteWaterBodyViewModel>()
                    .ForMember(vm => vm.BeachCount, model => model.MapFrom(m => m.Beaches.Count));
                cfg.CreateMap<WaterBody, WaterBodyBeachesViewModel>();

                cfg.CreateMap<Location, AutocompleteLocationViewModel>()
                    .ForMember(vm => vm.BeachCount, model => model.MapFrom(m => m.Beaches.Count));
                cfg.CreateMap<Location, LocationBeachesViewModel>();

                cfg.CreateMap<Beach, AutocompleteBeachViewModel>()
                    .ForMember(vm => vm.WaterBody, model => model.MapFrom(m => m.WaterBodyName))
                    .ForMember(vm => vm.Location, model => model.MapFrom(m => m.Location.Name));
                cfg.CreateMap<Beach, ConciseBeachViewModel>()
                    .ForMember(vm => vm.ImagePath, model => model.MapFrom(m => m.Images.FirstOrDefault().Path))
                    .ForMember(vm => vm.Location, model => model.MapFrom(m => m.Location.Name));
                cfg.CreateMap<Beach, DetailedBeachViewModel>()
                    .ForMember(vm => vm.Location, model => model.MapFrom(m => m.Location.Name));
                cfg.CreateMap<AddBeachBindingModel, Beach>();
                cfg.CreateMap<Beach, AutocompleteMainViewModel>();
                cfg.CreateMap<Beach, PostReviewViewModel>()
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Name))
                    .ForMember(vm => vm.BeachLocation, model => model.MapFrom(m => m.Location.Name))
                    .ForMember(vm => vm.BeachTotalScore, model => model.MapFrom(m => m.TotalScore))
                    .ForMember(vm => vm.BeachReviewsCount, model => model.MapFrom(m => m.Reviews.Count(r => r.TotalScore != null)))
                    .ForMember(vm => vm.BeachImagePaths, model => model.MapFrom(m => m.Images));

                cfg.CreateMap<BeachImage, string>().ConvertUsing(bp => bp.Path);

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