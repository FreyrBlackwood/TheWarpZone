using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Mappers
{
    public static class TagMapper
    {
        public static TagDto ToDto(Tag tag)
        {
            if (tag == null) return null;

            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public static Tag ToEntity(TagDto dto)
        {
            if (dto == null) return null;

            return new Tag
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
