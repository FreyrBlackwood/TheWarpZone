using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheWarpZone.Common.Constraints;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Web.Areas.Admin.ViewModels.Movie
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(MovieConstraints.TitleMaxLength, MinimumLength = MovieConstraints.TitleMinLength, ErrorMessage = "Title must be between {2} and {1} characters.")]
        public string Title { get; set; }

        [StringLength(MovieConstraints.DescriptionMaxLength, ErrorMessage = "Description cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Director is required.")]
        [StringLength(MovieConstraints.DirectorMaxLength, MinimumLength = MovieConstraints.DirectorMinLength, ErrorMessage = "Director name must be between {2} and {1} characters.")]
        public string Director { get; set; }

        [Required(ErrorMessage = "Release date is required.")]
        public DateTime ReleaseDate { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters.")]
        public string? ImageUrl { get; set; }

        public string? TagsString { get; set; }

        public MovieDto ToDto()
        {
            return new MovieDto
            {
                Id = Id,
                Title = Title,
                Description = Description,
                Director = Director,
                ReleaseDate = ReleaseDate,
                ImageUrl = ImageUrl,
                Tags = string.IsNullOrWhiteSpace(TagsString)
                    ? new List<string>()
                    : TagsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(tag => tag.Trim())
                        .ToList()
            };
        }

        public static MovieFormViewModel FromDto(MovieDto dto)
        {
            return new MovieFormViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Director = dto.Director,
                ReleaseDate = dto.ReleaseDate,
                ImageUrl = dto.ImageUrl,
                TagsString = string.Join(", ", dto.Tags ?? new List<string>())
            };
        }
    }
}
