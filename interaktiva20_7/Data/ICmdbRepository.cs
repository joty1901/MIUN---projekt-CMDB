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
        Task<MovieDto> GetMovieByImdbId(string id);
        Task<MoviesViewModel> GetMovieViewModel();
        List<MovieDto> GetShortList(IEnumerable<MovieDto> movies);
        Task<MoviesViewModel> GetMoviesBySearchString(string searchstring);
        Task<MovieDto> GetLikesAndDislikes(MovieDto movie);
        Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> listOfMovies);
    }
}
