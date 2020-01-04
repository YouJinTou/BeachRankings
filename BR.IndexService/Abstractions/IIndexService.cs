using System.Threading.Tasks;

namespace BR.IndexService.Abstractions
{
    public interface IIndexService
    {
        Task UpdateIndexAsync(string eventString);
    }
}
