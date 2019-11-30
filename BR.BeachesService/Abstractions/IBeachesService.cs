using BR.BeachesService.Models;
using System.Threading.Tasks;

namespace BR.BeachesService.Abstractions
{
    public interface IBeachesService
    {
        Task<Beach> CreateBeachAsync(CreateBeachModel model);
    }
}
