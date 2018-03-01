using BeachRankings.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace App.Code.Web
{
    public class SitemapGenerator : ISitemapGenerator
    {
        private const string Url = "url";
        private const string Loc = "loc";
        private const string Home = "Home";
        private const string Contributors = "Contributors";
        private const string Account = "Account";
        private const string Criteria = "Criteria";
        private const string HowTo = "HowTo";
        private const string Top = "Top";
        private const string Contact = "Contact";
        private const string Bloggers = "Bloggers";
        private const string Register = "Register";
        private const string World = "World";
        private const string Continents = "Continents";
        private const string Countries = "Countries";
        private const string PrimaryDivisions = "PrimaryDivisions";
        private const string SecondaryDivisions = "SecondaryDivisions";
        private const string TertiaryDivisions = "TertiaryDivisions";
        private const string QuaternaryDivisions = "QuaternaryDivisions";
        private const string WaterBodies = "WaterBodies";
        private const string Statistics = "Statistics";
        private const string Beaches = "Beaches";
        private const string Details = "Details";
        private readonly string SavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sitemap.xml");
        private readonly XNamespace NS = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private readonly string Domain = ConfigurationManager.AppSettings["FullWebAddress"];

        private IBeachRankingsData data;

        public SitemapGenerator(IBeachRankingsData data)
        {
            this.data = data;
        }

        public void GenerateSitemap()
        {
            try
            {
                var sitemap = new XDocument(
                       new XDeclaration("1.0", "UTF-8", "no"),
                       new XElement(this.NS + "urlset", this.GetAllPages()));

                sitemap.Save(this.SavePath);
            }
            catch (Exception ex)
            {
            }
        }

        private XElement GetUrlNode(string controller, string action, int id = 0)
        {
            return new XElement(this.NS + Url,
                new XElement(this.NS + Loc, Domain + string.Format("{0}/{1}{2}", 
                    controller,
                    action,
                    id == 0 ? string.Empty : "/" + id)));
        }

        private IEnumerable<XElement> GetAllPages()
        {
            var allPages = new List<XElement>();

            allPages.AddRange(this.GetStaticPages());
            allPages.AddRange(this.GetDynamicPages());

            return allPages;
        }

        private IEnumerable<XElement> GetStaticPages()
        {
            var staticPages = new List<XElement>
            {
                this.GetUrlNode(Home, Criteria),
                this.GetUrlNode(Home, HowTo),
                this.GetUrlNode(Contributors, Top),
                this.GetUrlNode(World, Statistics),
                this.GetUrlNode(World, Beaches),
                this.GetUrlNode(Home, Contact),
                this.GetUrlNode(Home, Bloggers),
                this.GetUrlNode(Account, Register)
            };

            return staticPages;
        }

        private IEnumerable<XElement> GetDynamicPages()
        {
            var allDynamicPages = new List<XElement>();

            allDynamicPages.AddRange(this.GetBeaches());
            allDynamicPages.AddRange(this.GetContinents());
            allDynamicPages.AddRange(this.GetCountries());
            allDynamicPages.AddRange(this.GetPrimaryDivisions());
            allDynamicPages.AddRange(this.GetSecondaryDivisions());
            allDynamicPages.AddRange(this.GetTertiaryDivisions());
            allDynamicPages.AddRange(this.GetQuaternaryDivisions());
            allDynamicPages.AddRange(this.GetWaterBodies());

            return allDynamicPages;
        }

        private IEnumerable<XElement> GetBeaches()
        {
            foreach (var id in this.data.Beaches.All().Select(c => c.Id).ToList())
            {
                yield return this.GetUrlNode(Beaches, Details, id);
            }
        }

        private IEnumerable<XElement> GetContinents()
        {
            foreach (var id in this.data.Continents.All().Select(c => c.Id).ToList())
            {
                yield return this.GetUrlNode(Continents, Beaches, id);
                yield return this.GetUrlNode(Continents, Statistics, id);
            }
        }

        private IEnumerable<XElement> GetCountries()
        {
            foreach (var id in this.data.Countries.All().Select(c => c.Id).ToList())
            {
                yield return this.GetUrlNode(Countries, Beaches, id);
                yield return this.GetUrlNode(Countries, Statistics, id);
            }
        }

        private IEnumerable<XElement> GetPrimaryDivisions()
        {
            foreach (var id in this.data.PrimaryDivisions.All().Select(c => c.Id).ToList())
            {
                yield return this.GetUrlNode(PrimaryDivisions, Beaches, id);
                yield return this.GetUrlNode(PrimaryDivisions, Statistics, id);
            }
        }

        private IEnumerable<XElement> GetSecondaryDivisions()
        {
            foreach (var id in this.data.SecondaryDivisions.All().Select(c => c.Id).ToList())
            {
                yield return this.GetUrlNode(SecondaryDivisions, Beaches, id);
                yield return this.GetUrlNode(SecondaryDivisions, Statistics, id);
            }
        }

        private IEnumerable<XElement> GetTertiaryDivisions()
        {
            foreach (var id in this.data.TertiaryDivisions.All().Select(c => c.Id).ToList())
            {
                yield return this.GetUrlNode(TertiaryDivisions, Beaches, id);
                yield return this.GetUrlNode(TertiaryDivisions, Statistics, id);
            }
        }

        private IEnumerable<XElement> GetQuaternaryDivisions()
        {
            foreach (var id in this.data.QuaternaryDivisions.All().Select(c => c.Id).ToList())
            {
                yield return this.GetUrlNode(QuaternaryDivisions, Beaches, id);
                yield return this.GetUrlNode(QuaternaryDivisions, Statistics, id);
            }
        }

        private IEnumerable<XElement> GetWaterBodies()
        {
            foreach (var id in this.data.WaterBodies.All().Select(c => c.Id).ToList())
            {
                yield return this.GetUrlNode(WaterBodies, Beaches, id);
                yield return this.GetUrlNode(WaterBodies, Statistics, id);
            }
        }
    }
}