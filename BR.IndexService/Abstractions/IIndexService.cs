using System.Threading.Tasks;

namespace BR.IndexService.Abstractions
{
    public interface IIndexService
    {
        Task AddToIndexAsync(params string[] tokens);
    }
}
