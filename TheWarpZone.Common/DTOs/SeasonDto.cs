namespace TheWarpZone.Common.DTOs
{
    public class SeasonDto
    {
        public int Id { get; set; }
        public int SeasonNumber { get; set; }
        public List<EpisodeDto> Episodes { get; set; }
    }
}
