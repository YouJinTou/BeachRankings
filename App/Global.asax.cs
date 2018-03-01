namespace App
{
    using App.App_Start;
    using BeachRankings.Data;
    using BeachRankings.Data.Migrations;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new RequireHttpsAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MapperConfig.RegisterMappings();

            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"]))
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<BeachRankingsDbContext, Configuration>());

                var configuration = new Configuration();
                var migrator = new DbMigrator(configuration);

                migrator.Update();
            }                
        }
    }
}