using Blockbuster.Application.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Blockbuster.Application.Interface;

public interface IMovieService
{
    Task<MovieDto?> GetMovieAsync(int movieId);
    Task<IEnumerable<MovieSummaryDto>?> GetTopRatedMoviesAsync(int limit, int offset);
    Task<IEnumerable<MovieSummaryDto>> GetMovieSuggestionsAsync(string searchText, int limit = 5);
    Task<IEnumerable<MovieDto>> SearchMoviesAsync(string query);
    Task<IEnumerable<MovieEditDto>> GetMoviesAdminAsync(int limit, int offset);
    Task SaveMovieAsync(MovieDto movie);
    Task<string?> UploadImageAsync(IBrowserFile? uploadedFile);
}