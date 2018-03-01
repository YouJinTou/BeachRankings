[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(App.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(App.App_Start.NinjectWebCommon), "Stop")]

namespace App.App_Start
{
    using App.Code.Beaches;
    using App.Code.Blogs;
    using App.Code.Images;
    using App.Code.Users;
    using App.Code.WaterBodies;
    using App.Code.Web;
    using BeachRankings.Data;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Services.Aggregation;
    using BeachRankings.Services.Crawlers;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Services.Notifications;
    using System;
    using System.Web;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IBeachRankingsData>().To<BeachRankingsData>().WithConstructorArgument("dbContext", new BeachRankingsDbContext());
            kernel.Bind<IBeachUpdater>().To<BeachUpdater>();
            kernel.Bind<IBeachQueryManager>().To<BeachQueryManager>();
            kernel.Bind<IBeachDisplayManager>().To<BeachDisplayManager>();
            kernel.Bind<IBlogValidator>().To<BlogValidator>();
            kernel.Bind<IBlogArticleUpdater>().To<BlogArticleUpdater>();
            kernel.Bind<IBlogQueryManager>().To<BlogQueryManager>();
            kernel.Bind<IImageManager>().To<ImageManager>();
            kernel.Bind<IUserLevelCalculator>().To<UserLevelCalculator>();
            kernel.Bind<IWebNameParser>().To<WebNameParser>();
            kernel.Bind<IArticleCrawler>().To<ArticleCrawler>();
            kernel.Bind<IWaterBodyAllocator>().To<WaterBodyAllocator>();
            kernel.Bind<IWaterBodyPermissionChecker>().To<WaterBodyPermissionChecker>();
            kernel.Bind<ISearchService>().To<LuceneSearch>().WithConstructorArgument("index", Index.BeachIndex);
            kernel.Bind<IDataAggregationService>().To<DataAggregationService>();
            kernel.Bind<IMailService>().To<MailService>();
            kernel.Bind<ISitemapGenerator>().To<SitemapGenerator>();
        }        
    }
}