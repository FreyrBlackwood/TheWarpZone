using System.Collections.Generic;

namespace TheWarpZone.Web.ViewModels.Shared
{
    public class PaginatedResultViewModel<T>
    {
        public List<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
