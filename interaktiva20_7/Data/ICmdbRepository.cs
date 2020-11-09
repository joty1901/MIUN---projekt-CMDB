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

        Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> cmdbResult);

        List<MovieDto> GetAllLikesAndDislikes(List<MovieDto> movies, List<MovieDto> cmdbResult);

        string GetShortPlot(string plot);

        Task<MoviesViewModel> GetMoviesBySearchString(string searchstring, List<MovieDto> movies);

        Task<MoviesViewModel> GetMovieByImdbId(string imdbId, List<MovieDto> savedMovies);

        Task<MovieDto> GetLikesAndDislikesForSingleMovie(MovieDto movie);

        Task<List<MovieDto>> GetCmdbMovies();

        Task<MoviesViewModel> GetMovieViewModelFromSession(List<MovieDto> savedList);
    }
}
