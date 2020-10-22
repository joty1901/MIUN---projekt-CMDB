using interaktiva20_7.Models.DTO;
using interaktiva20_7.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace interaktiva20_7.Data
{
    public interface ICmdbRepository
    {
        Task<MovieDto> GetMovieByImdbId(string imdbId);
        Task<SearchDto> GetMovieBySearch(string searchString);
        Task<MoviesViewModel> GetMovieViewModel();
        Task<IEnumerable<MovieDto>> GetTopRatedMovies();
    }
}
