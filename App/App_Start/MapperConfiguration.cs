namespace App.App_Start
{
    using AutoMapper;
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
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
                cfg.CreateMap<PostReviewViewModel, Review>();
                cfg.CreateMap<Review, DetailedReviewViewModel>();
                cfg.CreateMap<BlogArticle, BlogArticleViewModel>();
                cfg.CreateMap<EditReviewViewModel, Review>().AfterMap((vm, model) => model.UpdateTotalScore());
                cfg.CreateMap<AddBeachViewModel, Beach>().ForMember(vm => vm.Images, model => model.Ignore());
                cfg.CreateMap<User, TableUserReviewsViewModel>().ForMember(vm => vm.AuthorName, model => model.MapFrom(m => m.UserName));
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
                cfg.CreateMap<Beach, PostReviewViewModel>()
                    .ForMember(vm => vm.BeachId, model => model.MapFrom(m => m.Id))
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Name))
                    .ForMember(vm => vm.BeachCountry, model => model.MapFrom(m => m.Country.Name))
                    .ForMember(vm => vm.CountryId, model => model.MapFrom(m => m.CountryId))
                    .ForMember(vm => vm.BeachPrimaryDivision, model => model.MapFrom(m => m.PrimaryDivision.Name))
                    .ForMember(vm => vm.PrimaryDivisionId, model => model.MapFrom(m => m.PrimaryDivisionId))
                    .ForMember(vm => vm.BeachSecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivisionId, model => model.MapFrom(m => m.SecondaryDivisionId))
                    .ForMember(vm => vm.BeachTotalScore, model => model.MapFrom(m => m.TotalScore))
                    .ForMember(vm => vm.BeachReviewsCount, model => model.MapFrom(m => m.Reviews.Count(r => r.TotalScore != null)))
                    .ForMember(vm => vm.BeachImagePaths, model => model.MapFrom(m => m.Images))
                    .ForMember(vm => vm.SandQuality, model => model.Ignore())
                    .ForMember(vm => vm.BeachCleanliness, model => model.Ignore())
                    .ForMember(vm => vm.BeautifulScenery, model => model.Ignore())
                    .ForMember(vm => vm.CrowdFree, model => model.Ignore())
                    .ForMember(vm => vm.WaterPurity, model => model.Ignore())
                    .ForMember(vm => vm.WasteFreeSeabed, model => model.Ignore())
                    .ForMember(vm => vm.FeetFriendlyBottom, model => model.Ignore())
                    .ForMember(vm => vm.SeaLifeDiversity, model => model.Ignore())
                    .ForMember(vm => vm.CoralReef, model => model.Ignore())
                    .ForMember(vm => vm.Walking, model => model.Ignore())
                    .ForMember(vm => vm.Snorkeling, model => model.Ignore())
                    .ForMember(vm => vm.Kayaking, model => model.Ignore())
                    .ForMember(vm => vm.Camping, model => model.Ignore())
                    .ForMember(vm => vm.Infrastructure, model => model.Ignore())
                    .ForMember(vm => vm.LongTermStay, model => model.Ignore());
                cfg.CreateMap<Review, EditReviewViewModel>()
                    .ForMember(vm => vm.BeachId, model => model.MapFrom(m => m.Beach.Id))
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Beach.Name))
                    .ForMember(vm => vm.BeachCountry, model => model.MapFrom(m => m.Beach.Country.Name))
                    .ForMember(vm => vm.CountryId, model => model.MapFrom(m => m.Beach.CountryId))
                    .ForMember(vm => vm.BeachPrimaryDivision, model => model.MapFrom(m => m.Beach.PrimaryDivision.Name))
                    .ForMember(vm => vm.PrimaryDivisionId, model => model.MapFrom(m => m.Beach.PrimaryDivisionId))
                    .ForMember(vm => vm.BeachSecondaryDivision, model => model.MapFrom(m => m.Beach.SecondaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivisionId, model => model.MapFrom(m => m.Beach.SecondaryDivisionId))
                    .ForMember(vm => vm.BeachTotalScore, model => model.MapFrom(m => m.Beach.TotalScore))
                    .ForMember(vm => vm.BeachReviewsCount, model => model.MapFrom(m => m.Beach.Reviews.Count(r => r.TotalScore != null)))
                    .ForMember(vm => vm.BeachImagePaths, model => model.MapFrom(m => m.Beach.Images))
                    .ForMember(vm => vm.ArticleLinks, model => model.MapFrom(m => string.Join("@", m.BlogArticles.Where(ba => ba.ReviewId == m.Id).Select(ba => ba.Url))));
                cfg.CreateMap<Beach, TableRowViewModel>()
                    .ForMember(vm => vm.BeachId, model => model.MapFrom(m => m.Id))
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Name))
                    .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Country.Name))
                    .ForMember(vm => vm.PrimaryDivision, model => model.MapFrom(m => m.PrimaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name))
                    .ForMember(vm => vm.TertiaryDivision, model => model.MapFrom(m => m.TertiaryDivision.Name))
                    .ForMember(vm => vm.QuaternaryDivision, model => model.MapFrom(m => m.QuaternaryDivision.Name))
                    .ForMember(vm => vm.WaterBody, model => model.MapFrom(m => m.WaterBody.Name));
                cfg.CreateMap<Review, TableRowViewModel>()
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Beach.Name))
                    .ForMember(vm => vm.CountryId, model => model.MapFrom(m => m.Beach.Country.Id))
                    .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Beach.Country.Name))
                    .ForMember(vm => vm.PrimaryDivisionId, model => model.MapFrom(m => m.Beach.PrimaryDivision.Id))
                    .ForMember(vm => vm.PrimaryDivision, model => model.MapFrom(m => m.Beach.PrimaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivisionId, model => model.MapFrom(m => m.Beach.SecondaryDivision.Id))
                    .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.Beach.SecondaryDivision.Name))
                    .ForMember(vm => vm.TertiaryDivisionId, model => model.MapFrom(m => m.Beach.TertiaryDivision.Id))
                    .ForMember(vm => vm.TertiaryDivision, model => model.MapFrom(m => m.Beach.TertiaryDivision.Name))
                    .ForMember(vm => vm.QuaternaryDivisionId, model => model.MapFrom(m => m.Beach.QuaternaryDivision.Id))
                    .ForMember(vm => vm.WaterBodyId, model => model.MapFrom(m => m.Beach.WaterBody.Id))
                    .ForMember(vm => vm.WaterBody, model => model.MapFrom(m => m.Beach.WaterBody.Name));
                cfg.CreateMap<Review, ConciseReviewViewModel>()
                    .ForMember(vm => vm.UserName, model => model.MapFrom(m => m.Author.UserName))
                    .ForMember(vm => vm.AvatarPath, model => model.MapFrom(m => m.Author.AvatarPath))
                    .ForMember(vm => vm.ReviewsCount, model => model.MapFrom(m => m.Author.Reviews.Count))
                    .ForMember(vm => vm.CountriesVisited, model => model.MapFrom(m => m.Author.GetVisitedCountriesCount()))
                    .ForMember(vm => vm.Level, model => model.MapFrom(m => m.Author.Level))
                    .ForMember(vm => vm.IsBlogger, model => model.MapFrom(m => m.Author.IsBlogger))
                    .ForMember(vm => vm.BlogUrl, model => model.MapFrom(m => m.Author.Blog.Url))
                    .ForMember(vm => vm.BlogArticles, model => model.MapFrom(m => m.BlogArticles));
            });
        }                
    }
}