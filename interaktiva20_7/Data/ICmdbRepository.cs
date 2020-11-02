using interaktiva20_7.Models.DTO;
using interaktiva20_7.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace interaktiva20_7.Data
{
    public interface ICmdbRepository
    {
        
        Task<MoviesViewModel> GetMovieViewModel();

        MoviesViewModel GetMovieViewModelFromSession(List<MovieDto> savedList);

        Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> cmdbResult);

        List<MovieDto> RecoverMissingLikes(List<MovieDto> movies, List<MovieDto> cmdbResult);

        string GetShortPlot(string plot);

        List<MovieDto> GetShortList(IEnumerable<MovieDto> movies);

        Task<MoviesViewModel> GetMoviesBySearchString(string searchstring, List<MovieDto> movies);

        Task<MoviesViewModel> GetMovieByImdbId(string imdbId, List<MovieDto> savedMovies);

        Task<MovieDto> GetLikesAndDislikes(MovieDto movie);
 
    }
}
