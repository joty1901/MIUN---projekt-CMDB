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
        public IApiClient apiClient;

        public CmdbRepository(IConfiguration configuration)
        {
            cmdbBaseUrl = configuration.GetValue<string>("CMDBApi:BaseUrl");
            omdbBaseUrl = configuration.GetValue<string>("OMDBApi:BaseUrl");
            apiKey = configuration.GetValue<string>("APIKey:key");
            apiClient = new ApiClient();
        }

        public async Task<MoviesViewModel> GetMovieViewModel()
        {
            string endpoint = $"{cmdbBaseUrl}/api/movie";
            var result = await apiClient.GetASync<List<MovieDto>>(endpoint);
            List<MovieDto> movies;
            movies = await GetMovieInfoFromOmdb(result);

            return new MoviesViewModel(movies);
        }

        public MoviesViewModel GetMovieViewModelFromSession(List<MovieDto> savedList)
        {
            return new MoviesViewModel(savedList);
        }

        public async Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> cmdbResult)
        {
            var tasks = new List<Task<MovieDto>>();
            List<MovieDto> movies = new List<MovieDto>();

            for (int i = 0; i < cmdbResult.Count(); i++)
            {
                string omdbMovies = $"{omdbBaseUrl}/?i={cmdbResult.ElementAt(i).ImdbID}&apikey={apiKey}";
                var task = apiClient.GetASync<MovieDto>(omdbMovies);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            for (int i = 0; i < tasks.Count; i++)
            {
                movies.Add(tasks[i].Result);
            }

            movies = RecoverMissingLikes(movies, cmdbResult);

            return movies;
        }

        private List<MovieDto> RecoverMissingLikes(List<MovieDto> movies, List<MovieDto> cmdbResult )
        {
            foreach(var movie in movies)
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
 
        public List<MovieDto> GetShortList(IEnumerable<MovieDto> movies)
        {
            List<MovieDto> moviesOrderByDescending = movies.OrderByDescending(x => (x.numberOfLikes - x.numberOfDislikes)).ToList();
            List<MovieDto> topFourMoviesList = new List<MovieDto>();

            for (int i = 0; i < 4; i++)
            {
                topFourMoviesList.Add(moviesOrderByDescending[i]);
            }

            return topFourMoviesList;
        }

        public async Task<MoviesViewModel> GetMoviesBySearchString(string searchstring, List<MovieDto> movies)
        {
            string endpoint = $"{omdbBaseUrl}/?s='{searchstring}'&apikey={apiKey}";
            var result = await apiClient.GetASync<SearchDto>(endpoint);
            return new MoviesViewModel(result.Search, movies);
        }

        public async Task<MoviesViewModel> GetMovieByImdbId(string imdbId, List<MovieDto> savedMovies)
        {
            string endpoint = $"{omdbBaseUrl}/?apikey={apiKey}&i={imdbId}&plot=full";
            var result = await apiClient.GetASync<MovieDto>(endpoint);
            var movieWithLikes = await GetLikesAndDislikes(result);

            return new MoviesViewModel(savedMovies, movieWithLikes);
        }

        public async Task<MovieDto> GetLikesAndDislikes(MovieDto movie)
        {
            string endpoint = $"{cmdbBaseUrl}/api/movie";
            var result = await apiClient.GetASync<IEnumerable<MovieDto>>(endpoint);

            foreach (var m in result)
            {
                if (m.ImdbID.Equals(movie.ImdbID))
                {
                    movie.numberOfLikes = m.numberOfLikes;
                    movie.numberOfDislikes = m.numberOfDislikes;
                }
            }
            return movie;
        }
    }
}
