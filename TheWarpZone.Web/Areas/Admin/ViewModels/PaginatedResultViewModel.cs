using System.Collections.Generic;

namespace TheWarpZone.Web.Areas.Admin.ViewModels
{
    public class PaginatedResultViewModel<T>
    {
        public List<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
