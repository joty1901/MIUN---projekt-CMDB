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
        Task<SearchDto> GetMovieBySearch(string searchString);

        Task<MovieDto> GetMovieByImdbId(string id);
        Task<MoviesViewModel> GetMovieViewModel();
        List<MovieDto> ShortenList(IEnumerable<MovieDto> movies);
        Task<List<MovieDto>> GetMoviesBySearchString(string searchstring);
    }
}
