using BR.Core.Models;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IBeachesService
    {
        Task<Beach> GetBeachAsync(string id);

        Task<Beach> CreateBeachAsync(CreateBeachModel model);

        Task<Beach> ModifyBeachAsync(ModifyBeachModel model);
    }
}
