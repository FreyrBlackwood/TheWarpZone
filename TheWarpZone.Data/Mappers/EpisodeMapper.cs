using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Data.Mappers
{
    public static class EpisodeMapper
    {
        public static EpisodeDto ToDto(Episode episode)
        {
            if (episode == null) return null;

            return new EpisodeDto
            {
                Id = episode.Id,
                EpisodeNumber = episode.EpisodeNumber,
                Title = episode.Title,
                Description = episode.EpisodeDescription
            };
        }

        public static Episode ToEntity(EpisodeDto dto)
        {
            if (dto == null) return null;

            return new Episode
            {
                Id = dto.Id,
                EpisodeNumber = dto.EpisodeNumber,
                Title = dto.Title,
                EpisodeDescription = dto.Description
            };
        }
    }
}
