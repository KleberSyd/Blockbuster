using Blockbuster.Application.Models;

namespace Blockbuster.Application.Interface;

public interface IMovieService
{
    Task<MovieDto?> GetMovieAsync(int movieId);
    Task<IEnumerable<MovieSummaryDto>?> GetTopRatedMoviesAsync(int limit, int offset);
    Task<IEnumerable<MovieSummaryDto>> GetMovieSuggestionsAsync(string searchText, int limit = 5);
    Task<IEnumerable<MovieDto>> SearchMoviesAsync(string query);

}