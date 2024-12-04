using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
public class MovieSeeder
{
    private readonly ApplicationDbContext _context;

    public MovieSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var movies = new List<Movie>
        {
            new Movie
{
                Title = "Inception",
                Director = "Christopher Nolan",
                ReleaseDate = new DateTime(2010, 7, 16),
                Description = "A skilled thief is offered a chance to have his past crimes forgiven if he can successfully perform inception: planting an idea into the mind of a target.",
                Tags = new List<Tag>
                {
                    new Tag { Name = "Mind-Bending" },
                    new Tag { Name = "Thriller" },
                    new Tag { Name = "Heist" }
                },
                ImageUrl = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "Blade Runner",
            Director = "Ridley Scott",
            ReleaseDate = new DateTime(1982, 6, 25),
            Description = "In a dystopian future, a blade runner must track down and terminate four replicants who have stolen a spaceship and returned to Earth to find their creator.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Cyberpunk" },
                new Tag { Name = "Dystopia" },
                new Tag { Name = "Noir" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BOWQ4YTBmNTQtMDYxMC00NGFjLTkwOGQtNzdhNmY1Nzc1MzUxXkEyXkFqcGc@._V1_.jpg"
        },
        new Movie
        {
            Title = "The Matrix",
            Director = "Lana Wachowski, Lilly Wachowski",
            ReleaseDate = new DateTime(1999, 3, 31),
            Description = "A computer hacker learns about the true nature of his reality and his role in the war against its controllers.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Cyberpunk" },
                new Tag { Name = "Action" },
                new Tag { Name = "Philosophical" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BN2NmN2VhMTQtMDNiOS00NDlhLTliMjgtODE2ZTY0ODQyNDRhXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "Interstellar",
            Director = "Christopher Nolan",
            ReleaseDate = new DateTime(2014, 11, 7),
            Description = "A team of astronauts travels through a wormhole in search of a new home for humanity.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Space Exploration" },
                new Tag { Name = "Time Dilation" },
                new Tag { Name = "Drama" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BYzdjMDAxZGItMjI2My00ODA1LTlkNzItOWFjMDU5ZDJlYWY3XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "Jurassic Park",
            Director = "Steven Spielberg",
            ReleaseDate = new DateTime(1993, 6, 11),
            Description = "A billionaire creates a dinosaur theme park, but chaos ensues when the dinosaurs escape.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Genetic Engineering" },
                new Tag { Name = "Adventure" },
                new Tag { Name = "Thriller" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BMjM2MDgxMDg0Nl5BMl5BanBnXkFtZTgwNTM2OTM5NDE@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "The Terminator",
            Director = "James Cameron",
            ReleaseDate = new DateTime(1984, 10, 26),
            Description = "A cyborg assassin is sent back in time to kill the mother of humanity's future resistance leader.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Time Travel" },
                new Tag { Name = "Artificial Intelligence" },
                new Tag { Name = "Action" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BZmE0YzIxM2QtMGNlMi00MjRmLWE3MWMtOWQzMGVjMmU0YTFmXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "Back to the Future",
            Director = "Robert Zemeckis",
            ReleaseDate = new DateTime(1985, 7, 3),
            Description = "A teenager accidentally travels back in time and must ensure his parents fall in love to save his future.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Time Travel" },
                new Tag { Name = "Adventure" },
                new Tag { Name = "Comedy" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BZmM3ZjE0NzctNjBiOC00MDZmLTgzMTUtNGVlOWFlOTNiZDJiXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "The Fifth Element",
            Director = "Luc Besson",
            ReleaseDate = new DateTime(1997, 5, 7),
            Description = "A cab driver becomes the reluctant hero in a quest to recover a mystical artifact and save humanity.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Space Opera" },
                new Tag { Name = "Action" },
                new Tag { Name = "Adventure" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BODU4ZTE5MWYtNmY2MC00NDkyLTk0NDgtNTk5YjgzMzc4NmQwXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "E.T. the Extra-Terrestrial",
            Director = "Steven Spielberg",
            ReleaseDate = new DateTime(1982, 6, 11),
            Description = "A young boy befriends a stranded alien and helps him return home while evading government agents.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Alien Encounter" },
                new Tag { Name = "Family" },
                new Tag { Name = "Friendship" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BYTNhNmY0YWMtMTczYi00MTA0LThhMmUtMTIxYzE0Y2QwMzRlXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "Arrival",
            Director = "Denis Villeneuve",
            ReleaseDate = new DateTime(2016, 11, 11),
            Description = "A linguist is tasked with deciphering an alien language to prevent global conflict.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Alien Encounter" },
                new Tag { Name = "Linguistics" },
                new Tag { Name = "Drama" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BMTExMzU0ODcxNDheQTJeQWpwZ15BbWU4MDE1OTI4MzAy._V1_.jpg"
        },
        new Movie
        {
            Title = "Dune",
            Director = "Denis Villeneuve",
            ReleaseDate = new DateTime(2021, 10, 22),
            Description = "A noble family becomes embroiled in a galactic war for control of the desert planet Arrakis and its valuable spice.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Space Opera" },
                new Tag { Name = "Political Intrigue" },
                new Tag { Name = "Adventure" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BNWIyNmU5MGYtZDZmNi00ZjAwLWJlYjgtZTc0ZGIxMDE4ZGYwXkEyXkFqcGc@._V1_.jpg"
        },
        new Movie
        {
            Title = "Gravity",
            Director = "Alfonso Cuarón",
            ReleaseDate = new DateTime(2013, 10, 4),
            Description = "Two astronauts work together to survive after an accident leaves them stranded in space.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Space Survival" },
                new Tag { Name = "Drama" },
                new Tag { Name = "Thriller" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BNjE5MzYwMzYxMF5BMl5BanBnXkFtZTcwOTk4MTk0OQ@@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "Ex Machina",
            Director = "Alex Garland",
            ReleaseDate = new DateTime(2015, 4, 24),
            Description = "A young programmer is selected to test an intelligent humanoid robot, leading to complex moral dilemmas.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Artificial Intelligence" },
                new Tag { Name = "Psychological" },
                new Tag { Name = "Drama" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BMTUxNzc0OTIxMV5BMl5BanBnXkFtZTgwNDI3NzU2NDE@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "Children of Men",
            Director = "Alfonso Cuarón",
            ReleaseDate = new DateTime(2006, 12, 25),
            Description = "In a dystopian future where humans can no longer reproduce, a disillusioned bureaucrat becomes an unlikely hero.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Dystopia" },
                new Tag { Name = "Adventure" },
                new Tag { Name = "Thriller" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BMDNkNmNiYzYtYWY0YS00NWEwLTgwMWUtYjM0M2E4Nzk3MzhmXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "Star Trek",
            Director = "J.J. Abrams",
            ReleaseDate = new DateTime(2009, 5, 8),
            Description = "A young James T. Kirk and Spock join the crew of the USS Enterprise to stop a Romulan from destroying planets.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Space Exploration" },
                new Tag { Name = "Adventure" },
                new Tag { Name = "Action" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BZjgzYjRlYWEtZDE4Ny00N2FmLTkzNzgtMjBjYTMzMjEyMWE2XkEyXkFqcGc@._V1_.jpg"
        },
        new Movie
        {
            Title = "Edge of Tomorrow",
            Director = "Doug Liman",
            ReleaseDate = new DateTime(2014, 6, 6),
            Description = "A soldier caught in a time loop during an alien invasion must relive the same day repeatedly to find a way to win the war.",
            Tags = new List<Tag>
            {
                new Tag { Name = "Time Loop" },
                new Tag { Name = "Alien Invasion" },
                new Tag { Name = "Action" }
            },
            ImageUrl = "https://m.media-amazon.com/images/M/MV5BMTc5OTk4MTM3M15BMl5BanBnXkFtZTgwODcxNjg3MDE@._V1_.jpg"
        },


        };

        foreach (var movie in movies)
        {
            var existingMovie = await _context.Movies
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Title == movie.Title && m.Director == movie.Director);

            if (existingMovie == null)
            {
                var resolvedTags = movie.Tags.Select(tag =>
                    _context.Tags.FirstOrDefault(t => t.Name == tag.Name) ?? new Tag { Name = tag.Name }).ToList();

                movie.Tags = resolvedTags;

                await _context.Movies.AddAsync(movie);
            }
            else
            {
                foreach (var tag in movie.Tags)
                {
                    if (!existingMovie.Tags.Any(existingTag => existingTag.Name == tag.Name))
                    {
                        var resolvedTag = _context.Tags.FirstOrDefault(t => t.Name == tag.Name) ?? new Tag { Name = tag.Name };
                        existingMovie.Tags.Add(resolvedTag);
                    }
                }
            }
        }

        await _context.SaveChangesAsync();
    }
}
