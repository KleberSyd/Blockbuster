using Blockbuster.Application.Interface;
using Blockbuster.Application.Models;
using Blockbuster.Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;
using Blockbuster.Domain.Entities;

namespace Blockbuster.Application.Services;

public class MovieService(BlockbusterDbContext context) : IMovieService
{
    public async Task<MovieDto?> GetMovieAsync(int movieId)
    {
        return await context.Movies
            .AsNoTracking()
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
            .AsNoTracking()
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

    public async Task<IEnumerable<MovieSummaryDto>> GetMovieSuggestionsAsync(string searchText, int limit = 5)
    {
        return await context.Movies
            .AsNoTracking()
            .Where(m => EF.Functions.Like(m.Title, $"%{searchText}%") ||
                        EF.Functions.Like(m.Description, $"%{searchText}%") ||
                        EF.Functions.Like(m.Director, $"%{searchText}%"))
            .OrderBy(m => m.Title)
            .Take(limit)
            .Select(m => new MovieSummaryDto
            {
                Id = m.Id,
                ImageUrl = m.ImageUrl,
                Title = m.Title
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<MovieDto>> SearchMoviesAsync(string query)
    {
        query = query.ToLower();

        var minutes = ConvertQueryToMinutes(query);

        var movies = await context.Movies
            .AsNoTracking()
            .Where(m => m.Title.Contains(query) ||
                        m.Description.Contains(query) ||
                        (query.Contains("minutes") && m.Timespan == minutes) ||
                        (query.Contains("hours") && m.Timespan == minutes))
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
                        .ToListAsync();

        return movies;

    }

    public async Task<IEnumerable<MovieEditDto>> GetMoviesAdminAsync(int limit, int offset)
    {
        return await context.Movies
            .OrderByDescending(m => m.Rating)
            .Skip(offset)
            .Take(limit)
            .Select(m => new MovieEditDto
            {
                Id = m.Id,
                ImageUrl = m.ImageUrl,
                Title = m.Title,
                Rating = m.Rating,
                Timespan = m.Timespan
            })
            .ToListAsync();
    }

    public async Task SaveMovieAsync(MovieDto movie)
    {
        var entity = await context.Movies.FindAsync(movie.Id);
        if (entity == null)
        {
            entity = new Movie(movie.Title, movie.Genre, movie.Director, movie.Year, movie.Rating, movie.ImageUrl, movie.TrailerUrl, movie.Description, movie.IsAvailable);
            context.Movies.Add(entity);
        }

        await context.SaveChangesAsync();
    }

    public async Task<string?> UploadImageAsync(IBrowserFile? file)
    {
        if (file is null) return null;
        ArgumentNullException.ThrowIfNull(file);

        var path = Path.Combine("wwwroot", "movie-images", file.Name);
        var savePath = Path.Combine(Directory.GetCurrentDirectory(), path);

        await using var fileStream = file.OpenReadStream(maxAllowedSize: 1024 * 1024);
        await using var saveFileStream = new FileStream(savePath, FileMode.Create);
        await fileStream.CopyToAsync(saveFileStream);

        return path.Replace("wwwroot", "");

    }

    private static int ConvertQueryToMinutes(string query)
    {
        var numberMatch = Regex.Match(query, @"\d+");
        if (!numberMatch.Success) return 0;

        if (!int.TryParse(numberMatch.Value, out var number))
            return 0;
        if (query.Contains("hour"))
        {
            return number * 60;
        }

        return query.Contains("minutes") ? number : 0;
    }

}