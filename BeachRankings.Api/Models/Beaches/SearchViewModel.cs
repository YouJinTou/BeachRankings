using AutoMapper;
using BeachRankings.Core.DAL;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Api.Models.Beaches
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            this.Continents = new List<ContinentSearchViewModel>();
            this.Countries = new List<CountrySearchViewModel>();
            this.L1s = new List<L1SearchViewModel>();
            this.L2s = new List<L2SearchViewModel>();
            this.L3s = new List<L3SearchViewModel>();
            this.L4s = new List<L4SearchViewModel>();
            this.WaterBodies = new List<WaterBodySearchViewModel>();
            this.Beaches = new List<BeachSearchViewModel>();
        }

        public SearchViewModel(IEnumerable<Beach> beaches, string prefix, IMapper mapper)
        {
            prefix = new QueryPrefix(prefix);
            this.Continents = mapper.Map<IEnumerable<ContinentSearchViewModel>>(
                beaches.Where(b => b.Id == BeachPartitionKey.Continent.ToString()), 
                o => o.Items[nameof(prefix)] = prefix);
            this.Countries = mapper.Map<IEnumerable<CountrySearchViewModel>>(
                beaches.Where(b => b.Id == BeachPartitionKey.Country.ToString()), 
                o => o.Items[nameof(prefix)] = prefix);
            this.L1s = mapper.Map<IEnumerable<L1SearchViewModel>>(
                beaches.Where(b => b.Id == BeachPartitionKey.L1.ToString()), 
                o => o.Items[nameof(prefix)] = prefix);
            this.L2s = mapper.Map<IEnumerable<L2SearchViewModel>>(
                beaches.Where(b => b.Id == BeachPartitionKey.L2.ToString()),
                o => o.Items[nameof(prefix)] = prefix);
            this.L3s = mapper.Map<IEnumerable<L3SearchViewModel>>(
                beaches.Where(b => b.Id == BeachPartitionKey.L3.ToString()),
                o => o.Items[nameof(prefix)] = prefix);
            this.L4s = mapper.Map<IEnumerable<L4SearchViewModel>>(
                beaches.Where(b => b.Id == BeachPartitionKey.L4.ToString()),
                o => o.Items[nameof(prefix)] = prefix);
            this.WaterBodies = mapper.Map<IEnumerable<WaterBodySearchViewModel>>(
                beaches.Where(b => b.Id == BeachPartitionKey.WaterBody.ToString()),
                o => o.Items[nameof(prefix)] = prefix);
            this.Beaches = mapper.Map<IEnumerable<BeachSearchViewModel>>(
                beaches.Where(b => b.Id == BeachPartitionKey.Beach.ToString()),
                o => o.Items[nameof(prefix)] = prefix);
        }

        public IEnumerable<ContinentSearchViewModel> Continents { get; }

        public IEnumerable<CountrySearchViewModel> Countries { get; }

        public IEnumerable<L1SearchViewModel> L1s { get; }

        public IEnumerable<L2SearchViewModel> L2s { get; }

        public IEnumerable<L3SearchViewModel> L3s { get; }

        public IEnumerable<L4SearchViewModel> L4s { get; }

        public IEnumerable<WaterBodySearchViewModel> WaterBodies { get; }

        public IEnumerable<BeachSearchViewModel> Beaches { get; }
    }
}
