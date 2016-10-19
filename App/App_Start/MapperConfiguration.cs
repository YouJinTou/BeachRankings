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
                cfg.CreateMap<WaterBody, AddBeachWaterBodyViewModel>();
                cfg.CreateMap<WaterBody, AutocompleteWaterBodyViewModel>();

                cfg.CreateMap<Location, AutocompleteLocationViewModel>()
                    .ForMember(vm => vm.BeachCount, model => model.MapFrom(m => m.Beaches.Count));

                cfg.CreateMap<Beach, AutocompleteBeachViewModel>()
                    .ForMember(vm => vm.Location, model => model.MapFrom(m => m.Location.Name));
                cfg.CreateMap<Beach, ConciseBeachViewModel>()
                    .ForMember(vm => vm.ImagePath, model => model.MapFrom(m => m.Photos.FirstOrDefault().Path))
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
                    .ForMember(vm => vm.BeachImagePaths, model => model.MapFrom(m => m.Photos))
                    .ForAllOtherMembers(members => members.Ignore());

                cfg.CreateMap<BeachPhoto, string>().ConvertUsing(bp => bp.Path);

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