using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;

public class TVShowSeeder
{
    private readonly ApplicationDbContext _context;

    public TVShowSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var tvShows = new List<TVShow>
        {
            new TVShow
            {
                Title = "The Expanse",
                Description = "In a colonized galaxy, political tensions rise as the discovery of an ancient artifact threatens humanity.",
                ReleaseDate = new DateTime(2015, 12, 14),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BYzUyYmI3MjctY2Q2MC00NmFjLTgwZGUtNWQzZWNlYmVjNzE2XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Space Opera" },
                    new Tag { Name = "Political Intrigue" },
                    new Tag { Name = "Adventure" }
                },
                Seasons = GenerateSeasonsAndEpisodes(6, 10)
            },
            new TVShow
            {
                Title = "Stranger Things",
                Description = "A group of kids uncovers a government lab experiment gone wrong, leading to encounters with supernatural forces.",
                ReleaseDate = new DateTime(2016, 7, 15),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BMjQ5ODIyODg5Ml5BMl5BanBnXkFtZTgwOTI4Njk5MzI@._V1_QL75_UX820_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Supernatural" },
                    new Tag { Name = "Mystery" },
                    new Tag { Name = "Adventure" }
                },
                Seasons = GenerateSeasonsAndEpisodes(4, 8)
            },
            new TVShow
            {
                Title = "Black Mirror",
                Description = "An anthology series exploring the dark, twisted aspects of modern and futuristic society.",
                ReleaseDate = new DateTime(2011, 12, 4),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BMzMzOWVkNWQtODZjOS00ZjJlLTkwMDYtNmNmNWVjNTQ4NWRlXkEyXkFqcGc@._V1_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Anthology" },
                    new Tag { Name = "Dystopia" },
                    new Tag { Name = "Psychological" }
                },
                Seasons = GenerateSeasonsAndEpisodes(5, 5)
            },
            new TVShow
            {
                Title = "The Mandalorian",
                Description = "A lone bounty hunter navigates the outer reaches of the galaxy, far from the authority of the New Republic.",
                ReleaseDate = new DateTime(2019, 11, 12),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BNjgxZGM0OWUtZGY1MS00MWRmLTk2N2ItYjQyZTI1OThlZDliXkEyXkFqcGc@._V1_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Space Western" },
                    new Tag { Name = "Adventure" },
                    new Tag { Name = "Action" }
                },
                Seasons = GenerateSeasonsAndEpisodes(3, 8)
            },
            new TVShow
            {
                Title = "Doctor Who",
                Description = "A time-traveling alien known as the Doctor embarks on adventures across space and time.",
                ReleaseDate = new DateTime(1963, 11, 23),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BNmI5ZjhkYWYtY2I4ZS00MDgxLWE0ZTUtMDExYzM0MzQ5YjIzXkEyXkFqcGc@._V1_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Time Travel" },
                    new Tag { Name = "Adventure" },
                    new Tag { Name = "Sci-Fi Icon" }
                },
                Seasons = GenerateSeasonsAndEpisodes(12, 13)
            },
            new TVShow
            {
                Title = "Battlestar Galactica",
                Description = "A fleet of ships carrying the last of humanity flee the Cylons while searching for Earth.",
                ReleaseDate = new DateTime(2004, 10, 18),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BNmMzYzdmNWMtM2ZmMi00Y2E2LWI5ZTMtYWJlNmJiMDgyYWRiXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Space Opera" },
                    new Tag { Name = "Drama" },
                    new Tag { Name = "Survival" }
                },
                Seasons = GenerateSeasonsAndEpisodes(4, 20)
            },
            new TVShow
            {
                Title = "Firefly",
                Description = "A ragtag crew aboard the Serenity spaceship navigate the fringes of society while evading authority.",
                ReleaseDate = new DateTime(2002, 9, 20),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BYzYyZWYzNzUtOWQ4Yi00Y2Q4LWJjZjgtZTllNjg2ZGM0MTcyXkEyXkFqcGc@._V1_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Space Western" },
                    new Tag { Name = "Adventure" },
                    new Tag { Name = "Comedy" }
                },
                Seasons = GenerateSeasonsAndEpisodes(1, 14)
            },
            new TVShow
            {
                Title = "Westworld",
                Description = "A futuristic theme park populated by androids hides dark secrets about humanity and artificial intelligence.",
                ReleaseDate = new DateTime(2016, 10, 2),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BMjM2MTA5NjIwNV5BMl5BanBnXkFtZTgwNjI2OTMxNTM@._V1_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Artificial Intelligence" },
                    new Tag { Name = "Dystopia" },
                    new Tag { Name = "Thriller" }
                },
                Seasons = GenerateSeasonsAndEpisodes(4, 10)
            },
            new TVShow
            {
                Title = "Fringe",
                Description = "An FBI team investigates strange occurrences that defy explanation, often involving alternate dimensions.",
                ReleaseDate = new DateTime(2008, 9, 9),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BMWVlMmE1MmEtNjhjMC00MDdmLWIzZGMtNjc1YTZmNDc2MWExXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Alternate Realities" },
                    new Tag { Name = "Mystery" },
                    new Tag { Name = "Thriller" }
                },
                Seasons = GenerateSeasonsAndEpisodes(5, 22)
            },
            new TVShow
            {
                Title = "Orphan Black",
                Description = "A woman discovers she's a clone and delves into a conspiracy involving the science of human cloning.",
                ReleaseDate = new DateTime(2013, 3, 30),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BNzdmMzI1NjktOTE5MS00MWM1LWI1YTAtODE4YjkzZmRlNzI4XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Cloning" },
                    new Tag { Name = "Drama" },
                    new Tag { Name = "Thriller" }
                },
                Seasons = GenerateSeasonsAndEpisodes(5, 10)
            },
            new TVShow
            {
                Title = "The Twilight Zone",
                Description = "An anthology series exploring the strange and unexplained, with moral and philosophical twists.",
                ReleaseDate = new DateTime(1959, 10, 2),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BZjdmOTE3NjItY2JkMi00YjlhLWI0M2UtZjEwZTQ4ZDBkM2I0XkEyXkFqcGc@._V1_QL75_UX190_CR0,7,190,281_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Anthology" },
                    new Tag { Name = "Psychological" },
                    new Tag { Name = "Mystery" }
                },
                Seasons = GenerateSeasonsAndEpisodes(5, 36)
            },
            new TVShow
            {
                Title = "Lost",
                Description = "Survivors of a plane crash must navigate a mysterious island full of secrets and dangers.",
                ReleaseDate = new DateTime(2004, 9, 22),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BYjZlMmJmOTQtNDc4YS00MmM4LWExMDMtZWQ0MjBiMWI4OWIzXkEyXkFqcGc@._V1_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Mystery" },
                    new Tag { Name = "Drama" },
                    new Tag { Name = "Survival" }
                },
                Seasons = GenerateSeasonsAndEpisodes(6, 24)
            },
            new TVShow
            {
                Title = "Star Trek: The Next Generation",
                Description = "The crew of the USS Enterprise explores the galaxy, encountering new species and challenges.",
                ReleaseDate = new DateTime(1987, 9, 28),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BMmNiNTQ2YzYtODBjYy00ZWMwLTlmNWMtYWE1NTQ2ZTYyZmMwXkEyXkFqcGc@._V1_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Space Exploration" },
                    new Tag { Name = "Diplomacy" },
                    new Tag { Name = "Adventure" }
                },
                Seasons = GenerateSeasonsAndEpisodes(7, 26)
            },
            new TVShow
            {
                Title = "The X-Files",
                Description = "Two FBI agents investigate paranormal phenomena and government conspiracies.",
                ReleaseDate = new DateTime(1993, 9, 10),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BNzg3ODVhZWEtYmI0MC00NDQ0LWEyNGYtM2M0MTRhZTZmMjU3XkEyXkFqcGc@._V1_QL75_UX190_CR0,0,190,281_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Paranormal" },
                    new Tag { Name = "Conspiracy" },
                    new Tag { Name = "Mystery" }
                },
                Seasons = GenerateSeasonsAndEpisodes(11, 24)
            },
            new TVShow
            {
                Title = "The 100",
                Description = "A group of juvenile delinquents return to Earth from a space station to see if it's habitable after a nuclear apocalypse.",
                ReleaseDate = new DateTime(2014, 3, 19),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BNDdmZGYwOWEtN2FkZC00Y2ExLWJkY2UtNzFlODVlNzc3MGIzXkEyXkFqcGc@._V1_QL75_UY281_CR6,0,190,281_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Post-Apocalyptic" },
                    new Tag { Name = "Survival" },
                    new Tag { Name = "Drama" }
                },
                Seasons = GenerateSeasonsAndEpisodes(7, 13)
            },
            new TVShow
            {
                Title = "Foundation",
                Description = "A mathematician's predictive model foretells the fall of the Galactic Empire, sparking a plan to preserve knowledge.",
                ReleaseDate = new DateTime(2021, 9, 24),
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BOTRiNGMxOGMtMTQ5Ni00OGVjLWE3YWEtZDNhYzlmMjc2ZWUwXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Space Opera" },
                    new Tag { Name = "Political Intrigue" },
                    new Tag { Name = "Philosophical" }
                },
                Seasons = GenerateSeasonsAndEpisodes(2, 10)
            },

        };

        foreach (var tvShow in tvShows)
        {
            var existingTVShow = await _context.TVShows
                .Include(t => t.Tags)
                .Include(t => t.Seasons)
                .ThenInclude(s => s.Episodes)
                .FirstOrDefaultAsync(t => t.Title == tvShow.Title);

            if (existingTVShow == null)
            {
                var resolvedTags = tvShow.Tags.Select(tag =>
                    _context.Tags.FirstOrDefault(t => t.Name == tag.Name) ?? new Tag { Name = tag.Name }).ToList();

                tvShow.Tags = resolvedTags;

                await _context.TVShows.AddAsync(tvShow);
            }
            else
            {
                foreach (var tag in tvShow.Tags)
                {
                    if (!existingTVShow.Tags.Any(existingTag => existingTag.Name == tag.Name))
                    {
                        var resolvedTag = _context.Tags.FirstOrDefault(t => t.Name == tag.Name) ?? new Tag { Name = tag.Name };
                        existingTVShow.Tags.Add(resolvedTag);
                    }
                }
            }
        }

        await _context.SaveChangesAsync();
    }

    private ICollection<Season> GenerateSeasonsAndEpisodes(int seasonCount, int episodeCount)
    {
        var seasons = new List<Season>();
        for (int i = 1; i <= seasonCount; i++)
        {
            var season = new Season
            {
                SeasonNumber = i,
                Title = $"Season {i}",
                Episodes = GenerateEpisodes(i, episodeCount)
            };
            seasons.Add(season);
        }
        return seasons;
    }

    private ICollection<Episode> GenerateEpisodes(int seasonNumber, int episodeCount)
    {
        var episodes = new List<Episode>();
        for (int i = 1; i <= episodeCount; i++)
        {
            episodes.Add(new Episode
            {
                EpisodeNumber = i,
                Title = $"Episode {i}",
                EpisodeDescription = $"This is a description of Episode {i} from Season {seasonNumber}."
            });
        }
        return episodes;
    }
}
