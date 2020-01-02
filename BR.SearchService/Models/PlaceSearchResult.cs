using System.Collections.Generic;

namespace BR.SearchService.Models
{
    public class PlaceSearchResult
    {
        public IEnumerable<string> BeachIds { get; set; } = new List<string>();
    }
}
