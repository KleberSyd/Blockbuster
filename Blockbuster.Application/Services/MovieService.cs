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
    /// <summary>
    /// Retrieves a movie by its ID.
    /// </summary>
    /// <param name="movieId">The ID of the movie to retrieve.</param>
    /// <returns>The movie details as a MovieDto object.</returns>
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

    /// <summary>
    /// Retrieves a list of top-rated movies.
    /// </summary>
    /// <param name="limit">The maximum number of movies to retrieve.</param>
    /// <param name="offset">The number of movies to skip before retrieving.</param>
    /// <returns>A list of top-rated movies as MovieSummaryDto objects.</returns>
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

    /// <summary>
    /// Retrieves a list of movie suggestions based on the search text.
    /// </summary>
    /// <param name="searchText">The search text to match against movie titles, descriptions, and directors.</param>
    /// <param name="limit">The maximum number of movie suggestions to retrieve.</param>
    /// <returns>A list of movie suggestions as MovieSummaryDto objects.</returns>
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

    /// <summary>
    /// Searches for movies based on the given query.
    /// </summary>
    /// <param name="query">The query to search for movies.</param>
    /// <returns>A list of movies that match the query as MovieDto objects.</returns>
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

    /// <summary>
    /// Retrieves a list of movies for administration purposes.
    /// </summary>
    /// <param name="limit">The maximum number of movies to retrieve.</param>
    /// <param name="offset">The number of movies to skip before retrieving.</param>
    /// <returns>A list of movies for administration as MovieEditDto objects.</returns>
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

    /// <summary>
    /// Saves a movie to the database.
    /// </summary>
    /// <param name="movie">The movie to save as a MovieDto object.</param>
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

    /// <summary>
    /// Uploads an image file for a movie.
    /// </summary>
    /// <param name="file">The image file to upload as an IBrowserFile object.</param>
    /// <returns>The path of the uploaded image file.</returns>
    public async Task<string?> UploadImageAsync(IBrowserFile? file)
    {
        if (file is null) return null;
        ArgumentNullException.ThrowIfNull(file);

        var path = Path.Combine("movie-images", file.Name);
        var savePath = Path.Combine(Directory.GetCurrentDirectory(), path);

        await using var fileStream = file.OpenReadStream(maxAllowedSize: 1024 * 1024);
        await using var saveFileStream = new FileStream(savePath, FileMode.Create);
        await fileStream.CopyToAsync(saveFileStream);

        return path;
    }

    /// <summary>
    /// Deletes movies from the database.
    /// </summary>
    /// <param name="selectedMovieIds">The IDs of the movies to delete.</param>
    public async Task DeleteMoviesAsync(List<int> selectedMovieIds)
    {
        var moviesToDelete = await context.Movies
            .Where(m => selectedMovieIds.Contains(m.Id))
            .ToListAsync();

        context.Movies.RemoveRange(moviesToDelete);

        await context.SaveChangesAsync();
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
