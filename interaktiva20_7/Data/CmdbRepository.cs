using interaktiva20_7.Models.DTO;
using interaktiva20_7.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            var result = await apiClient.GetASync<IEnumerable<MovieDto>>(endpoint);
            List<MovieDto> topFourMoviesList = new List<MovieDto>();
            topFourMoviesList = GetShortList(result);
            var topFourMoviesWithInfo = await GetMovieInfoFromOmdb(topFourMoviesList);

            return new MoviesViewModel(topFourMoviesWithInfo);
        }

        public async Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> listOfMovies)
        {
            for (int i = 0; i < listOfMovies.Count; i++)
            {
                int numberOfLikes = listOfMovies[i].numberOfLikes;
                int numberOfDislikes = listOfMovies[i].numberOfDislikes;
                string omdbMovies = $"{omdbBaseUrl}/?i={listOfMovies[i].ImdbID}&apikey={apiKey}";
                listOfMovies[i] = await apiClient.GetASync<MovieDto>(omdbMovies);
                listOfMovies[i].numberOfDislikes = numberOfDislikes;
                listOfMovies[i].numberOfLikes = numberOfLikes;
            }
            return listOfMovies;
        }


        //public async Task<MoviesViewModel> GetMovieViewModel()
        //{
        //    string endpoint = $"{cmdbBaseUrl}/api/movie";
        //    var cmdbResult = await apiClient.GetASync<IEnumerable<MovieDto>>(endpoint);
        //    List<MovieDto> movies = new List<MovieDto>();
        //    movies = await GetMovieInfoFromOmdb(cmdbResult);

        //    return new MoviesViewModel(movies);
        //}

        //public async Task<List<MovieDto>> GetMovieInfoFromOmdb(IEnumerable<MovieDto> cmdbResult)
        //{
        //    var tasks = new List<Task<MovieDto>>();
        //    List<MovieDto> movies = new List<MovieDto>();

        //    for (int i = 0; i < cmdbResult.Count(); i++)
        //    {
        //        //int numberOfLikes = cmdbResult.ElementAt(i).numberOfLikes;
        //        //int numberOfDislikes = cmdbResult.ElementAt(i).numberOfDislikes;
        //        string omdbMovies = $"{omdbBaseUrl}/?i={cmdbResult.ElementAt(i).ImdbID}&apikey={apiKey}";
        //        var task = apiClient.GetASync<MovieDto>(omdbMovies);
        //        //task.Result.numberOfLikes = numberOfLikes;
        //        //task.Result.numberOfDislikes = numberOfDislikes;
        //        tasks.Add(task);
        //    }

        //    await Task.WhenAll(tasks);
        //    for (int i = 0; i < tasks.Count; i++)
        //    {
        //        movies.Add(tasks[i].Result);
        //    }
        //    return movies;
        //}

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

        public async Task<MoviesViewModel> GetMoviesBySearchString(string searchstring)
        {
            string endpoint = $"{omdbBaseUrl}/?t={searchstring}&s={searchstring}&apikey={apiKey}";
            var result = await apiClient.GetASync<SearchDto>(endpoint);
            return new MoviesViewModel(result.Search);
        }

        public async Task<MovieDto> GetMovieByImdbId(string imdbId)
        {
            string endpoint = $"{omdbBaseUrl}/?apikey={apiKey}&i={imdbId}";
            var result = await apiClient.GetASync<MovieDto>(endpoint);
            var movieWithLikes = await GetLikesAndDislikes(result);

            return movieWithLikes;
        }

        public List<MovieDto> SortListOrderByLikes(IEnumerable<MovieDto> movies)
        {
            List<MovieDto> moviesOrderBydescending = movies.OrderByDescending(x => (x.numberOfLikes - x.numberOfDislikes)).ToList();
            List<MovieDto> topFourMoviesList = new List<MovieDto>();

            for (int i = 0; i < moviesOrderBydescending.Count; i++)
            {
                topFourMoviesList.Add(moviesOrderBydescending[i]);
            }

            return topFourMoviesList;
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
