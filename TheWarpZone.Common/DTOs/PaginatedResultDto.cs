namespace TheWarpZone.Common.DTOs
{
    public class PaginatedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; } 
        public int CurrentPage { get; set; }     
        public int TotalPages { get; set; }     
    }
}
