using Blockbuster.Application.Models;

namespace Blockbuster.Application.Interface;

public interface IMovieService
{
    Task<MovieDto?> GetMovieAsync(int movieId);
    Task<IEnumerable<MovieSummaryDto>?> GetTopRatedMoviesAsync();

}