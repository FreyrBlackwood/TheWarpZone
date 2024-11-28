using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheWarpZone.Common.Constraints;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Web.Areas.Admin.ViewModels.TVShow
{
    public class TVShowFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(TVShowConstraints.TitleMaxLength, MinimumLength = TVShowConstraints.TitleMinLength, ErrorMessage = "Title must be between {2} and {1} characters.")]
        public string Title { get; set; }

        [StringLength(TVShowConstraints.DescriptionMaxLength, ErrorMessage = "Description cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Release date is required.")]
        public DateTime ReleaseDate { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters.")]
        public string? ImageUrl { get; set; }

        public string? TagsString { get; set; }

        public TVShowDto ToDto()
        {
            return new TVShowDto
            {
                Id = Id,
                Title = Title,
                Description = Description,
                ReleaseDate = ReleaseDate,
                ImageUrl = ImageUrl,
                Tags = string.IsNullOrWhiteSpace(TagsString)
                    ? new List<string>()
                    : TagsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(tag => tag.Trim())
                        .ToList()
            };
        }

        public static TVShowFormViewModel FromDto(TVShowDto dto)
        {
            return new TVShowFormViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                ReleaseDate = dto.ReleaseDate,
                ImageUrl = dto.ImageUrl,
                TagsString = string.Join(", ", dto.Tags ?? new List<string>())
            };
        }
    }
}
