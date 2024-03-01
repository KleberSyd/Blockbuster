using Blockbuster.Application.Interface;
using Blockbuster.Application.Models;
using Blockbuster.Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Blockbuster.Application.Services;

public class MovieService(BlockbusterDbContext context) : IMovieService
{
    public async Task<MovieDto?> GetMovieAsync(int movieId)
    {
        return await context.Movies
            .Where(m => m.Id == movieId)
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Genre = m.Genre,
                Director = m.Director,
                Year = m.Year,
                Rating = m.Rating,
                ImageUrl = m.ImageUrl,
                TrailerUrl = m.TrailerUrl,
                Description = m.Description,
                IsAvailable = m.IsAvailable,
                Timespan = m.Timespan,
            })
            .SingleAsync();
    }

    public async Task<IEnumerable<MovieSummaryDto>?> GetTopRatedMoviesAsync(int limit, int offset)
    {
        return await context.Movies
            .OrderByDescending(m => m.Rating)
            .Skip(offset)
            .Take(limit)
            .Select(m => new MovieSummaryDto
            {
                Id = m.Id,
                ImageUrl = m.ImageUrl,
                Title = m.Title
            })
            .ToListAsync();
    }
}