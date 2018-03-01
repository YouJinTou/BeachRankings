namespace App.App_Start
{
    using AutoMapper;
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Services.Search.Models;
    using System;
    using System.Linq;
    using BeachRankings.Services.Aggregation;
    using System.Configuration;

    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<BeachSearchResultModel, AutocompleteBeachViewModel>();
                cfg.CreateMap<ContinentSearchResultModel, AutocompleteContinentViewModel>();
                cfg.CreateMap<CountrySearchResultModel, AutocompleteCountryViewModel>();
                cfg.CreateMap<WaterBodySearchResultModel, AutocompleteWaterBodyViewModel>();
                cfg.CreateMap<PrimaryDivisionSearchResultModel, AutocompletePrimaryViewModel>();
                cfg.CreateMap<SecondaryDivisionSearchResultModel, AutocompleteSecondaryViewModel>();
                cfg.CreateMap<TertiaryDivisionSearchResultModel, AutocompleteTertiaryViewModel>();
                cfg.CreateMap<QuaternaryDivisionSearchResultModel, AutocompleteQuaternaryViewModel>();
                cfg.CreateMap<Beach, Beach>();
                cfg.CreateMap<BeachImage, BeachImageThumbnailViewModel>();
                cfg.CreateMap<BeachImage, DashboardBeachImageThumbnailViewModel>();
                cfg.CreateMap<BlogArticle, BlogArticleViewModel>();
                cfg.CreateMap<Beach, CriteriaViewModel>();
                cfg.CreateMap<Review, CriteriaViewModel>();
                cfg.CreateMap<ScoreWeight, ScoreWeight>();
                cfg.CreateMap<ScoreWeight, WeightViewModel>();
                cfg.CreateMap<WeightViewModel, ScoreWeight>();
                cfg.CreateMap<PlaceBeachesViewModel, StatsHeadViewModel>()
                    .ForMember(vm => vm.TotalBeachesCount, model => model.MapFrom(m => m.Beaches.Count()));
                cfg.CreateMap<StatisticsViewModel, StatsHeadViewModel>()
                    .ForMember(vm => vm.TotalBeachesCount, model => model.MapFrom(m => m.Rows.Count()));
                cfg.CreateMap<IPlaceSearchable, StatisticsViewModel>()
                    .ForMember(vm => vm.TotalBeachesCount, model => model.MapFrom(m => m.Beaches.Count));
                cfg.CreateMap<WatchlistStatisticsViewModel, StatsHeadViewModel>()
                    .ForMember(vm => vm.TotalBeachesCount, model => model.MapFrom(m => m.Rows.Count()));
                cfg.CreateMap<IPlaceSearchable, PlaceBeachesViewModel>()
                    .ForMember(vm => vm.TotalBeachesCount, model => model.MapFrom(m => m.Beaches.Count));
                cfg.CreateMap<Beach, HorizontalCriteriaViewModel>()
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Name))
                    .ForMember(vm => vm.PrimaryDivisionName, model => model.MapFrom(m => m.PrimaryDivision.Name));
                cfg.CreateMap<Review, HorizontalCriteriaViewModel>()
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Beach.Name))
                    .ForMember(vm => vm.PrimaryDivisionName, model => model.MapFrom(m => m.Beach.PrimaryDivision.Name));
                cfg.CreateMap<EditReviewViewModel, Review>()
                    .AfterMap((vm, model) => Mapper.Map(vm.Criteria, model))
                    .AfterMap((vm, model) => model.UpdateScores());
                cfg.CreateMap<AddBeachViewModel, Beach>().ForMember(vm => vm.Images, model => model.Ignore());
                cfg.CreateMap<EditBeachViewModel, Beach>().ForMember(vm => vm.Images, model => model.Ignore());
                cfg.CreateMap<User, TableUserReviewsViewModel>().ForMember(vm => vm.Contributor, model => model.MapFrom(m => m));
                cfg.CreateMap<User, ContributorRowViewModel>()
                    .ForMember(vm => vm.ReviewsCount, model => model.MapFrom(m => m.Reviews.Count))
                    .ForMember(vm => vm.BlogPostsCount, model => model.MapFrom(m => m.Blog == null ? 0 : m.Blog.BlogArticles.Count))
                    .ForMember(vm => vm.BeachesByCountry, model => model.MapFrom(m => m.GetTopBeachesByCountry(1)))
                    .ForMember(vm => vm.CountriesVisited, model => model.MapFrom(m => m.GetVisitedCountriesCount()))
                    .ForMember(vm => vm.BlogUrl, model => model.MapFrom(m => m.Blog.Url));
                cfg.CreateMap<User, ContributorVerticalViewModel>()
                    .ForMember(vm => vm.ReviewsCount, model => model.MapFrom(m => m.Reviews.Count))
                    .ForMember(vm => vm.CountriesVisited, model => model.MapFrom(m => m.GetVisitedCountriesCount()))
                    .ForMember(vm => vm.BlogUrl, model => model.MapFrom(m => m.Blog.Url));
                cfg.CreateMap<Beach, ConciseBeachViewModel>()
                    .ForMember(vm => vm.ImagePath, model => model.MapFrom(m => m.Images.Count > 0 ? m.Images.First().Path :
                        ConfigurationManager.AppSettings["DefaultBeachImagePath"]))
                    .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Country.Name))
                    .ForMember(vm => vm.PrimaryDivision, model => model.MapFrom(m => m.PrimaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name))
                    .ForMember(vm => vm.WaterBody, model => model.MapFrom(m => m.WaterBody.Name))
                    .ForMember(vm => vm.ReviewsCount, model => model.MapFrom(m => m.Reviews.Count(r => r.TotalScore != null)));
                cfg.CreateMap<Beach, CrossTableViewModel>();
                cfg.CreateMap<Beach, DetailedBeachViewModel>()
                    .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Country.Name))
                    .ForMember(vm => vm.PrimaryDivision, model => model.MapFrom(m => m.PrimaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name))
                    .ForMember(vm => vm.TertiaryDivision, model => model.MapFrom(m => m.TertiaryDivision.Name))
                    .ForMember(vm => vm.QuaternaryDivision, model => model.MapFrom(m => m.QuaternaryDivision.Name))
                    .ForMember(vm => vm.WaterBody, model => model.MapFrom(m => m.WaterBody.Name))
                    .ForMember(vm => vm.ImageCount, model => model.MapFrom(m => m.Images.Count))
                    .ForMember(vm => vm.Criteria, model => model.MapFrom(m => m))
                    .AfterMap((m, vm) => vm.CrossTable = new CrossTableViewModel())
                    .AfterMap((m, vm) => vm.CrossTable.Country = m.Country.Name)
                    .AfterMap((m, vm) => vm.CrossTable.Area = m.PrimaryDivision.Name)
                    .AfterMap((m, vm) => vm.CrossTable.WaterBody = m.WaterBody.Name)
                    .AfterMap((m, vm) => vm.CrossTable.Continent = m.Continent.Name)
                    .AfterMap((m, vm) => 
                    {
                        if (vm.Images.Count() == 0)
                        {
                            vm.Images.Add(
                                new BeachImageThumbnailViewModel { Path = ConfigurationManager.AppSettings["DefaultBeachImagePath"] });
                        }
                    });
                cfg.CreateMap<RankContainer, CrossTableRowViewModel>();
                cfg.CreateMap<Beach, PostReviewViewModel>()
                    .BeforeMap((m, vm) => vm.Criteria = new CriteriaViewModel())
                    .ForMember(vm => vm.BeachHead, model => model.MapFrom(m => m))
                    .ForMember(vm => vm.BeachReviewsCount, model => model.MapFrom(m => m.Reviews.Count(r => r.TotalScore != null)))
                    .ForMember(vm => vm.Images, model => model.Ignore())
                    .ForMember(vm => vm.SandQuality, model => model.Ignore())
                    .ForMember(vm => vm.BeachCleanliness, model => model.Ignore())
                    .ForMember(vm => vm.BeautifulScenery, model => model.Ignore())
                    .ForMember(vm => vm.CrowdFree, model => model.Ignore())
                    .ForMember(vm => vm.WaterVisibility, model => model.Ignore())
                    .ForMember(vm => vm.LitterFree, model => model.Ignore())
                    .ForMember(vm => vm.FeetFriendlyBottom, model => model.Ignore())
                    .ForMember(vm => vm.SeaLifeDiversity, model => model.Ignore())
                    .ForMember(vm => vm.CoralReef, model => model.Ignore())
                    .ForMember(vm => vm.Walking, model => model.Ignore())
                    .ForMember(vm => vm.Snorkeling, model => model.Ignore())
                    .ForMember(vm => vm.Kayaking, model => model.Ignore())
                    .ForMember(vm => vm.Camping, model => model.Ignore())
                    .ForMember(vm => vm.Infrastructure, model => model.Ignore())
                    .ForMember(vm => vm.LongTermStay, model => model.Ignore());
                cfg.CreateMap<Beach, EditBeachViewModel>()
                    .ForMember(vm => vm.ArticleLinks, model => model.MapFrom(m => string.Join("@", m.BlogArticles.Select(ba => ba.Url))))
                    .ForMember(vm => vm.Images, model => model.Ignore());
                cfg.CreateMap<Beach, BeachRowViewModel>()
                   .ForMember(vm => vm.BeachId, model => model.MapFrom(m => m.Id))
                   .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Name))
                   .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Country.Name))
                   .ForMember(vm => vm.PrimaryDivision, model => model.MapFrom(m => m.PrimaryDivision.Name))
                   .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name))
                   .ForMember(vm => vm.TertiaryDivision, model => model.MapFrom(m => m.TertiaryDivision.Name))
                   .ForMember(vm => vm.QuaternaryDivision, model => model.MapFrom(m => m.QuaternaryDivision.Name))
                   .ForMember(vm => vm.WaterBody, model => model.MapFrom(m => m.WaterBody.Name));
                cfg.CreateMap<Beach, ExportScoresAsHtmlViewModel>()
                   .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Country.Name))
                   .ForMember(vm => vm.PrimaryDivision, model => model.MapFrom(m => m.PrimaryDivision.Name))
                   .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.SecondaryDivision.Name));
                cfg.CreateMap<Review, DetailedReviewViewModel>()
                   .ForMember(vm => vm.ReviewHead, model => model.MapFrom(m => m))
                   .ForMember(vm => vm.ReviewUser, model => model.MapFrom(m => m))
                   .ForMember(vm => vm.BlogArticles, model => model.MapFrom(m => m.BlogArticles))
                   .ForMember(vm => vm.BeachHead, model => model.MapFrom(m => m.Beach));
                cfg.CreateMap<CriteriaViewModel, Review>();
                cfg.CreateMap<PostReviewViewModel, Review>()
                    .BeforeMap((vm, m) => m.PostedOn = DateTime.Now)
                    .ForMember(model => model.BeachId, vm => vm.MapFrom(m => m.BeachHead.Id))
                    .AfterMap((vm, m) => m.UpdateScores())
                    .AfterMap((vm, m) => Mapper.Map(vm.Criteria, m));
                cfg.CreateMap<Review, EditReviewViewModel>()
                    .BeforeMap((m, vm) => vm.Criteria = new CriteriaViewModel())
                    .ForMember(vm => vm.BeachHead, model => model.MapFrom(m => m.Beach))
                    .ForMember(vm => vm.BeachReviewsCount, model => model.MapFrom(m => m.Beach.Reviews.Count(r => r.TotalScore != null)))
                    .ForMember(vm => vm.ArticleLinks, model => model.MapFrom(m => string.Join("@", m.BlogArticles.Where(ba => ba.ReviewId == m.Id).Select(ba => ba.Url))))
                    .AfterMap((m, vm) => Mapper.Map(m, vm.Criteria));
                cfg.CreateMap<Review, BeachRowViewModel>()
                    .ForMember(vm => vm.ReviewId, model => model.MapFrom(m => m.Id))
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
                    .ForMember(vm => vm.ReviewHead, model => model.MapFrom(m => m))
                    .ForMember(vm => vm.ReviewUser, model => model.MapFrom(m => m))
                    .ForMember(vm => vm.IsBlogger, model => model.MapFrom(m => m.Author.IsBlogger))
                    .ForMember(vm => vm.BlogArticles, model => model.MapFrom(m => m.BlogArticles))
                    .ForMember(vm => vm.Criteria, model => model.MapFrom(m => m));
                cfg.CreateMap<Review, ReviewHeadViewModel>()
                    .ForMember(vm => vm.BeachName, model => model.MapFrom(m => m.Beach.Name))
                    .ForMember(vm => vm.PrimaryDivisionName, model => model.MapFrom(m => m.Beach.PrimaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivisionName, model => model.MapFrom(m => m.Beach.SecondaryDivision.Name));
                cfg.CreateMap<Review, ReviewUserViewModel>()
                    .ForMember(vm => vm.UserName, model => model.MapFrom(m => m.Author.UserName))
                    .ForMember(vm => vm.Badge, model => model.MapFrom(m => m.Author.Badge))
                    .ForMember(vm => vm.AvatarPath, model => model.MapFrom(m => m.Author.AvatarPath))
                    .ForMember(vm => vm.ReviewsCount, model => model.MapFrom(m => m.Author.Reviews.Count))
                    .ForMember(vm => vm.CountriesVisited, model => model.MapFrom(m => m.Author.GetVisitedCountriesCount()))
                    .ForMember(vm => vm.IsBlogger, model => model.MapFrom(m => m.Author.IsBlogger))
                    .ForMember(vm => vm.HasWatchlists, model => model.MapFrom(m => m.Author.Watchlists.Count > 0))
                    .ForMember(vm => vm.BlogUrl, model => model.MapFrom(m => m.Author.Blog.Url))
                    .ForMember(vm => vm.ThanksReceived, model => model.MapFrom(m => m.Author.ThanksReceived))
                    .ForMember(vm => vm.Level, model => model.MapFrom(m => m.Author.Level));
                cfg.CreateMap<Review, ExportScoresAsHtmlViewModel>()
                    .ForMember(vm => vm.Name, model => model.MapFrom(m => m.Beach.Name))
                    .ForMember(vm => vm.CountryId, model => model.MapFrom(m => m.Beach.Country.Id))
                    .ForMember(vm => vm.Country, model => model.MapFrom(m => m.Beach.Country.Name))
                    .ForMember(vm => vm.PrimaryDivisionId, model => model.MapFrom(m => m.Beach.PrimaryDivision.Id))
                    .ForMember(vm => vm.PrimaryDivision, model => model.MapFrom(m => m.Beach.PrimaryDivision.Name))
                    .ForMember(vm => vm.SecondaryDivisionId, model => model.MapFrom(m => m.Beach.SecondaryDivision.Id))
                    .ForMember(vm => vm.SecondaryDivision, model => model.MapFrom(m => m.Beach.SecondaryDivision.Name));
                cfg.CreateMap<User, WatchlistsTableViewModel>()
                    .ForMember(vm => vm.AuthorId, model => model.MapFrom(m => m.Id))
                    .ForMember(vm => vm.Contributor, model => model.MapFrom(m => m))
                    .AfterMap((m, vm) => vm.Watchlists = vm.Watchlists.OrderByDescending(w => w.BeachesCount).ToList());
                cfg.CreateMap<Watchlist, BeachWatchlistViewModel>();
                cfg.CreateMap<AddEditWatchlistViewModel, Watchlist>();
                cfg.CreateMap<Watchlist, UserWatchlistViewModel>()
                    .ForMember(vm => vm.BeachesCount, model => model.MapFrom(m => m.Beaches.Count));
            });
        }    
    }
}