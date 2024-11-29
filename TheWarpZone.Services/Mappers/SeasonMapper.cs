using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Mappers
{
    public static class SeasonMapper
    {
        public static SeasonDto ToDto(Season season)
        {
            if (season == null) return null;

            return new SeasonDto
            {
                Id = season.Id,
                SeasonNumber = season.SeasonNumber,
                Title = season.Title,
                Episodes = season.Episodes?.Select(EpisodeMapper.ToDto).ToList() ?? new List<EpisodeDto>(),
                TVShowId = season.TVShowId
            };
        }

        public static Season ToEntity(SeasonDto dto)
        {
            if (dto == null) return null;

            return new Season
            {
                Id = dto.Id,
                SeasonNumber = dto.SeasonNumber,
                Title = dto.Title,
                Episodes = dto.Episodes?.Select(EpisodeMapper.ToEntity).ToList(),
                TVShowId = dto.TVShowId
            };
        }
    }
}
