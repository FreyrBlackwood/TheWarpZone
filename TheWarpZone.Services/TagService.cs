﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;
using TheWarpZone.Data.Mappers;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _context.Tags.ToListAsync();
            return tags.Select(TagMapper.ToDto).ToList();
        }

        public async Task<TagDto> GetTagByIdAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found.");
            }

            return TagMapper.ToDto(tag);
        }

        public async Task AddTagAsync(TagDto tagDto)
        {
            if (tagDto == null)
            {
                throw new ArgumentNullException(nameof(tagDto), "Tag cannot be null.");
            }

            var tag = TagMapper.ToEntity(tagDto);

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found.");
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesByTagAsync(int tagId)
        {
            var movies = await _context.Movies
                .Where(m => m.Tags.Any(t => t.Id == tagId))
                .ToListAsync();

            return movies.Select(MovieMapper.ToDto).ToList();
        }

        public async Task<IEnumerable<TVShowDto>> GetTVShowsByTagAsync(int tagId)
        {
            var tvShows = await _context.TVShows
                .Where(tv => tv.Tags.Any(t => t.Id == tagId))
                .ToListAsync();

            return tvShows.Select(TVShowMapper.ToDto).ToList();
        }
    }
}