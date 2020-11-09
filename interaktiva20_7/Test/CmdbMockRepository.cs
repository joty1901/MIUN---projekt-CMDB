using interaktiva20_7.Data;
using interaktiva20_7.Models.DTO;
using interaktiva20_7.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace interaktiva20_7.Test
{
    public class CmdbMockRepository : ICmdbRepository
    {
        private string basePath;
        public CmdbMockRepository(IWebHostEnvironment webHostEnvironment)
        {
            basePath = $"{webHostEnvironment.ContentRootPath}\\Test\\";
        }


        #region Returns a viewModel that can be used in the view
        public async Task<MoviesViewModel> GetMovieByImdbId(string imdbId, List<MovieDto> savedMovies)
        {
            var file = File.ReadAllText(basePath + "OmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<List<MovieDto>>(file);
            var movie = result.Where(m => m.ImdbID.Equals(imdbId)).FirstOrDefault();
            var movieWithLikes = await GetLikesAndDislikesForSingleMovie(movie);
            await Task.Delay(0);
            return new MoviesViewModel(savedMovies, movieWithLikes);
        }


        public async Task<MoviesViewModel> GetMoviesBySearchString(string searchstring, List<MovieDto> movies)
        {
            var file = File.ReadAllText(basePath + "OmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(file);
            List<MovieDto> searchresult = new List<MovieDto>();

            foreach (var movie in result)
            {
                if (movie.Title.Contains(searchstring))
                {
                    searchresult.Add(movie);
                }
            }
            await Task.Delay(0);
            return new MoviesViewModel(searchresult, movies);
        }

        public async Task<MoviesViewModel> GetMovieViewModel()
        {
            var file = File.ReadAllText(basePath + "CmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<List<MovieDto>>(file);
            List<MovieDto> movies;
            movies = await GetMovieInfoFromOmdb(result);

            await Task.Delay(0);
            return new MoviesViewModel(movies);
        }


        public async Task<MoviesViewModel> GetMovieViewModelFromSession(List<MovieDto> savedList)
        {
            var cmdbResult = await GetCmdbMovies();
            var savedListUpdatedWithLikes = GetAllLikesAndDislikes(savedList, cmdbResult);
            await Task.Delay(0);
            return new MoviesViewModel(savedListUpdatedWithLikes);
        }
        #endregion

        #region Get all Movies From API (CMDB and OMDB)
        public async Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> cmdbResult)
        {
            var file = File.ReadAllText(basePath + "OmdbMockRepository.json");
            var movies = JsonConvert.DeserializeObject<List<MovieDto>>(file);

            movies = GetAllLikesAndDislikes(movies, cmdbResult);
            await Task.Delay(0);
            return movies;
        }



        public async Task<List<MovieDto>> GetCmdbMovies()
        {
            var file = File.ReadAllText(basePath + "CmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<List<MovieDto>>(file);
            await Task.Delay(0);

            return result;
        }
        #endregion

        #region Get Likes/Dislikes
        public async Task<MovieDto> GetLikesAndDislikesForSingleMovie(MovieDto movie)
        {
            var file = File.ReadAllText(basePath + "CmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(file);

            foreach (var m in result)
            {
                if (m.ImdbID.Equals(movie.ImdbID))
                {
                    movie.numberOfLikes = m.numberOfLikes;
                    movie.numberOfDislikes = m.numberOfDislikes;
                    movie.ShortPlot = GetShortPlot(movie.Plot);
                }
            }

            await Task.Delay(0);
            return movie;
        }
        public List<MovieDto> GetAllLikesAndDislikes(List<MovieDto> movies, List<MovieDto> cmdbResult)
        {
            foreach (var movie in movies)
            {
                foreach (var cmdbMovie in cmdbResult)
                {
                    if (movie.ImdbID == cmdbMovie.ImdbID)
                    {
                        movie.numberOfLikes = cmdbMovie.numberOfLikes;
                        movie.numberOfDislikes = cmdbMovie.numberOfDislikes;
                    }
                }
            }
            return movies;
        }
        #endregion

        #region Short versions
        public string GetShortPlot(string plot)
        {
            if (plot != null)
            {
                string shortPlot = "";

                for (int i = 0; i < plot.Length; i++)
                {
                    if (i <= 202)
                    {
                        shortPlot += plot[i];
                    }
                    else
                    {
                        return shortPlot += "..."; ;
                    }
                }

                return shortPlot += "...";
            }
            return null;
        }

        #endregion

    }
}
