using BeachRankings.Api.Models.Beaches;
using System.Threading.Tasks;

namespace BeachRankings.Api.Abstractions
{
    public interface IBeachService
    {
        Task<BeachViewModel> GetAsync(string id);

        Task AddAsync(AddBeachModel addBeachModel);
    }
}
