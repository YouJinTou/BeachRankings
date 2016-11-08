namespace App.App_Start
{
    using AutoMapper;
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.App.Models.BindingModels;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Services.Search.Models;
    using System.Linq;

    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<BeachSearchResultModel, AutocompleteBeachViewModel>();
                cfg.CreateMap<PlaceSearchResultModel, AutocompleteCountryViewModel>();
                cfg.CreateMap<PlaceSearchResultModel, AutocompleteWaterBodyViewModel>();
                cfg.CreateMap<PlaceSearchResultModel, AutocompletePrimaryViewModel>();
                cfg.CreateMap<PlaceSearchResultModel, AutocompleteSecondaryViewModel>();
                cfg.CreateMap<PlaceSearchResultModel, AutocompleteTertiaryViewModel>();
                cfg.CreateMap<PlaceSearchResultModel, AutocompleteQuaternaryViewModel>();
                cfg.CreateMap<IPlaceSearchable, LocationBeachesViewModel>();
                cfg.CreateMap<Beach, Beach>();
                cfg.CreateMap<Beach, EditBeachViewModel>();
                cfg.CreateMap<EditBeachViewModel, Beach>();
                cfg.CreateMap<BeachImage, BeachImageThumbnailViewModel>();
                cfg.CreateMap<Review, EditReviewViewModel>();
                cfg.CreateMap<PostReviewBindingModel, Review>();
                cfg.CreateMap<Beach, ConciseBeachViewModel>()
                    .ForMember(vm => vm.ImagePath, model => model.MapFrom(m => m.Images.FirstOrDefault().Path))
                    .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Country.Name))
                    .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name))
                    .ForMember(vm => vm.ReviewsCount, model => model.MapFrom(m => m.Reviews.Count(r => r.TotalScore != null)));
                cfg.CreateMap<Beach, DetailedBeachViewModel>()
                    .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Country.Name))
                    .ForMember(vm => vm.PrimaryDivision, model => model.MapFrom(m => m.PrimaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name))
                    .ForMember(vm => vm.TertiaryDivision, model => model.MapFrom(m => m.TertiaryDivision.Name))
                    .ForMember(vm => vm.QuaternaryDivision, model => model.MapFrom(m => m.QuaternaryDivision.Name))
                    .ForMember(vm => vm.WaterBody, model => model.MapFrom(m => m.WaterBody.Name));
                cfg.CreateMap<AddBeachViewModel, Beach>().ForMember(vm => vm.Images, model => model.Ignore());
                cfg.CreateMap<Beach, PostReviewViewModel>()
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Name))
                    .ForMember(vm => vm.BeachCountry, model => model.MapFrom(m => m.Country.Name))
                    .ForMember(vm => vm.BeachSecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name))
                    .ForMember(vm => vm.BeachWaterBody, model => model.MapFrom(m => m.WaterBody.Name))
                    .ForMember(vm => vm.BeachTotalScore, model => model.MapFrom(m => m.TotalScore))
                    .ForMember(vm => vm.BeachReviewsCount, model => model.MapFrom(m => m.Reviews.Count(r => r.TotalScore != null)))
                    .ForMember(vm => vm.BeachImagePaths, model => model.MapFrom(m => m.Images));
                cfg.CreateMap<Beach, BeachTableRowViewModel>()
                    .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Country.Name))
                    .ForMember(vm => vm.PrimaryDivision, model => model.MapFrom(m => m.PrimaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name))
                    .ForMember(vm => vm.TertiaryDivision, model => model.MapFrom(m => m.TertiaryDivision.Name))
                    .ForMember(vm => vm.QuaternaryDivision, model => model.MapFrom(m => m.QuaternaryDivision.Name))
                    .ForMember(vm => vm.WaterBody, model => model.MapFrom(m => m.WaterBody.Name));
                cfg.CreateMap<Review, ConciseReviewViewModel>()
                    .ForMember(vm => vm.UserName, model => model.MapFrom(m => m.Author.UserName))
                    .ForMember(vm => vm.AvatarPath, model => model.MapFrom(m => m.Author.AvatarPath));                
                cfg.CreateMap<EditReviewBindingModel, Review>().AfterMap((vm, model) => model.UpdateTotalScore());
                cfg.CreateMap<Review, DetailedReviewViewModel>()
                    .ForMember(vm => vm.UserName, model => model.MapFrom(m => m.Author.UserName))
                    .ForMember(vm => vm.AvatarPath, model => model.MapFrom(m => m.Author.AvatarPath));
                cfg.CreateMap<Review, DashboardReviewViewModel>()
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Beach.Name));
            });
        }
    }
}