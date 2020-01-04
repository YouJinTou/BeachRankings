using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IIndexService
    {
        Task UpdateIndexAsync(string eventString);
    }
}
