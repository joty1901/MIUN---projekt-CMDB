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
        Task<MoviesViewModel> GetMovieByImdbId(string id, List<MovieDto> savedMovies);
        Task<MoviesViewModel> GetMovieViewModel();
        List<MovieDto> GetShortList(IEnumerable<MovieDto> movies);
        Task<MoviesViewModel> GetMoviesBySearchString(string searchstring, List<MovieDto> movies);
        Task<MovieDto> GetLikesAndDislikes(MovieDto movie);
        MoviesViewModel GetMovieViewModelFromSession(List<MovieDto> savedList);
        //Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> listOfMovies);
    }
}
