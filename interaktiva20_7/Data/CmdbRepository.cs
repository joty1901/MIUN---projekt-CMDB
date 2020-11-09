using interaktiva20_7.Models.DTO;
using interaktiva20_7.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace interaktiva20_7.Data
{
    public class CmdbRepository : ICmdbRepository
    {
        private readonly string cmdbBaseUrl;
        private readonly string omdbBaseUrl;
        private readonly string apiKey;
        private IApiClient apiClient;

        public CmdbRepository(IConfiguration configuration, IApiClient apiClient)
        {
            cmdbBaseUrl = configuration.GetValue<string>("CMDBApi:BaseUrl");
            omdbBaseUrl = configuration.GetValue<string>("OMDBApi:BaseUrl");
            apiKey = configuration.GetValue<string>("APIKey:key");
            this.apiClient = apiClient;
        }

        #region Returns a viewModel that can be used in the view
        /// <summary>
        /// Metod som hämtar alla filmer från CMDB. Skickar sedan vidare den listan till GetMovieInfoFromOmdb() som kompletterar listan med info från Omdb
        /// </summary>
        /// <returns>new MoviesViewModel</returns>
        public async Task<MoviesViewModel> GetMovieViewModel()
        {
            var cmdbMovies = await GetCmdbMovies();
            List<MovieDto> movies = await GetMovieInfoFromOmdb(cmdbMovies);

            // Nedan kodrad är till för att uppdatera OmdbMockRepo med färsk data
            //File.WriteAllText("C:/Users/jonat/source/repos/interaktiva20_7/interaktiva20_7/Test/OmdbMockRepository.json", JsonConvert.SerializeObject(movies));

            return new MoviesViewModel(movies);
        }

        public async Task<MoviesViewModel> GetMovieViewModelFromSession(List<MovieDto> savedList)
        {
            var cmdbResult = await GetCmdbMovies();
            var savedListUpdatedWithLikes = GetAllLikesAndDislikes(savedList, cmdbResult);
            return new MoviesViewModel(savedListUpdatedWithLikes);
        }

        public async Task<MoviesViewModel> GetMoviesBySearchString(string searchstring, List<MovieDto> movies)
        {
            string endpoint = $"{omdbBaseUrl}/?s='{searchstring}'&apikey={apiKey}";
            var omdbMovies = await apiClient.GetASync<SearchDto>(endpoint);
            return new MoviesViewModel(omdbMovies.Search, movies);
        }

        public async Task<MoviesViewModel> GetMovieByImdbId(string imdbId, List<MovieDto> savedMovies)
        {
            string endpoint = $"{omdbBaseUrl}/?apikey={apiKey}&i={imdbId}&plot=full";
            var omdbMovie = await apiClient.GetASync<MovieDto>(endpoint);
            var movieWithLikes = await GetLikesAndDislikesForSingleMovie(omdbMovie);

            return new MoviesViewModel(savedMovies, movieWithLikes);
        }
        #endregion

        #region Get all Movies from API (CMDB and OMDB)
        public async Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> cmdbResult)
        {
            var tasks = new List<Task<MovieDto>>();
            List<MovieDto> movies = new List<MovieDto>();

            for (int i = 0; i < cmdbResult.Count(); i++)
            {
                string omdbMovies = $"{omdbBaseUrl}/?i={cmdbResult.ElementAt(i).ImdbID}&apikey={apiKey}&plot=full";
                var task = apiClient.GetASync<MovieDto>(omdbMovies);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            for (int i = 0; i < tasks.Count; i++)
            {
                movies.Add(tasks[i].Result);
            }

            movies = GetAllLikesAndDislikes(movies, cmdbResult);

            return movies;
        }

        public async Task<List<MovieDto>> GetCmdbMovies()
        {
            string endpoint = $"{cmdbBaseUrl}/api/movie";
            var cmdbMovies = await apiClient.GetASync<List<MovieDto>>(endpoint);

            return cmdbMovies;
        }
        #endregion

        #region Get Likes/Dislikes
        public async Task<MovieDto> GetLikesAndDislikesForSingleMovie(MovieDto omdbMovie)
        {
            var cmdbMovies = await GetCmdbMovies();

            foreach (var cmdbMovie in cmdbMovies)
            {
                if (cmdbMovie.ImdbID.Equals(omdbMovie.ImdbID))
                {
                    omdbMovie.numberOfLikes = cmdbMovie.numberOfLikes;
                    omdbMovie.numberOfDislikes = cmdbMovie.numberOfDislikes;
                }
            }
            return omdbMovie;
        }
        public List<MovieDto> GetAllLikesAndDislikes(List<MovieDto> omdbMovies, List<MovieDto> cmdbResult)
        {
            foreach (var omdbMovie in omdbMovies)
            {
                foreach (var cmdbMovie in cmdbResult)
                {
                    if (omdbMovie.ImdbID == cmdbMovie.ImdbID)
                    {
                        omdbMovie.numberOfLikes = cmdbMovie.numberOfLikes;
                        omdbMovie.numberOfDislikes = cmdbMovie.numberOfDislikes;
                        omdbMovie.ShortPlot = GetShortPlot(omdbMovie.Plot);
                    }
                }
            }
            return omdbMovies;
        }
        #endregion

        #region Get short versions
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
