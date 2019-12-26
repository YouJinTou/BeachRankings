using BR.Core.Models;
using System.Threading.Tasks;

namespace BR.IndexService.Abstractions
{
    public interface IIndexService
    {
        Task AddToIndexAsync(IndexData data);
    }
}
