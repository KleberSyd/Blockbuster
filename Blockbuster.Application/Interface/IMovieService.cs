using Blockbuster.Application.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Blockbuster.Application.Interface;

public interface IMovieService
{
    /// <summary>
    /// Retrieves a movie by its ID.
    /// </summary>
    /// <param name="movieId">The ID of the movie to retrieve.</param>
    /// <returns>The movie details as a MovieDto object.</returns>
    Task<MovieDto?> GetMovieAsync(int movieId);

    /// <summary>
    /// Retrieves a list of top-rated movies.
    /// </summary>
    /// <param name="limit">The maximum number of movies to retrieve.</param>
    /// <param name="offset">The number of movies to skip before retrieving.</param>
    /// <returns>A list of top-rated movies as MovieSummaryDto objects.</returns>
    Task<IEnumerable<MovieSummaryDto>?> GetTopRatedMoviesAsync(int limit, int offset);

    /// <summary>
    /// Retrieves a list of movie suggestions based on the search text.
    /// </summary>
    /// <param name="searchText">The search text to match against movie titles, descriptions, and directors.</param>
    /// <param name="limit">The maximum number of movie suggestions to retrieve.</param>
    /// <returns>A list of movie suggestions as MovieSummaryDto objects.</returns>
    Task<IEnumerable<MovieSummaryDto>> GetMovieSuggestionsAsync(string searchText, int limit = 5);

    /// <summary>
    /// Searches for movies based on the given query.
    /// </summary>
    /// <param name="query">The query to search for movies.</param>
    /// <returns>A list of movies that match the query as MovieDto objects.</returns>
    Task<IEnumerable<MovieDto>> SearchMoviesAsync(string query);

    /// <summary>
    /// Retrieves a list of movies for administration purposes.
    /// </summary>
    /// <param name="limit">The maximum number of movies to retrieve.</param>
    /// <param name="offset">The number of movies to skip before retrieving.</param>
    /// <returns>A list of movies for administration as MovieEditDto objects.</returns>
    Task<IEnumerable<MovieEditDto>> GetMoviesAdminAsync(int limit, int offset);

    /// <summary>
    /// Saves a movie to the database.
    /// </summary>
    /// <param name="movie">The movie to save as a MovieDto object.</param>
    Task SaveMovieAsync(MovieDto movie);

    /// <summary>
    /// Uploads an image file for a movie.
    /// </summary>
    /// <param name="uploadedFile">The image file to upload as an IBrowserFile object.</param>
    /// <returns>The path of the uploaded image file.</returns>
    Task<string?> UploadImageAsync(IBrowserFile? uploadedFile);

    /// <summary>
    /// Deletes movies from the database.
    /// </summary>
    /// <param name="selectedMovieIds">The IDs of the movies to delete.</param>
    Task DeleteMoviesAsync(List<int> selectedMovieIds);
}