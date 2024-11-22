using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Data.Mappers
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
                Episodes = season.Episodes?.Select(EpisodeMapper.ToDto).ToList() ?? new List<EpisodeDto>()
            };
        }

        public static Season ToEntity(SeasonDto dto)
        {
            if (dto == null) return null;

            return new Season
            {
                Id = dto.Id,
                SeasonNumber = dto.SeasonNumber
            };
        }
    }
}
