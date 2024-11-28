namespace TheWarpZone.Common.DTOs
{
    public class SeasonDto
    {
        public int Id { get; set; }
        public int SeasonNumber { get; set; }

        public string? Title { get; set; }
        public List<EpisodeDto> Episodes { get; set; }

        public int TVShowId { get; set; }
    }
}
