using BR.Core.Models;
using System.Collections.Generic;

namespace BR.SearchService.Models
{
    public class PlaceSearchResult
    {
        public IEnumerable<IndexBeach> Beaches { get; set; } = new List<IndexBeach>();
    }
}
