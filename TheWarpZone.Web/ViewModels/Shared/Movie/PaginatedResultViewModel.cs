using System.Collections.Generic;

namespace TheWarpZone.Web.ViewModels.Shared.Movie
{
    public class PaginatedResultViewModel<T>
    {
        public List<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
