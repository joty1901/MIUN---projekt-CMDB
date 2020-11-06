﻿using interaktiva20_7.Models.DTO;
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

        public async Task<MoviesViewModel> GetMovieViewModel()
        {
            string endpoint = $"{cmdbBaseUrl}/api/movie";
            var result = await apiClient.GetASync<List<MovieDto>>(endpoint);
            List<MovieDto> movies = await GetMovieInfoFromOmdb(result);

            // Nedan kodrad är till för att uppdatera OmdbMockRepo med färsk data
            //File.WriteAllText("C:/Users/jonat/source/repos/interaktiva20_7/interaktiva20_7/Test/OmdbMockRepository.json", JsonConvert.SerializeObject(movies));

            return new MoviesViewModel(movies);
        }

        public async Task<List<MovieDto>> GetCmdbMovies()
        {
            string endpoint = $"{cmdbBaseUrl}/api/movie";
            var result = await apiClient.GetASync<List<MovieDto>>(endpoint);

            return result;
        }


        public async Task<MoviesViewModel> GetMovieViewModelFromSession(List<MovieDto> savedList)
        {
            var cmdbResult = await GetCmdbMovies();
            var savedListUpdatedWithLikes = RecoverMissingLikes(savedList, cmdbResult);
            return new MoviesViewModel(savedListUpdatedWithLikes);
        }

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

            movies = RecoverMissingLikes(movies, cmdbResult);

            return movies;
        }

        public List<MovieDto> RecoverMissingLikes(List<MovieDto> movies, List<MovieDto> cmdbResult)
        {
            foreach(var movie in movies)
            {
                foreach (var cmdbMovie in cmdbResult)
                {
                    if (movie.ImdbID == cmdbMovie.ImdbID)
                    {
                        movie.numberOfLikes = cmdbMovie.numberOfLikes;
                        movie.numberOfDislikes = cmdbMovie.numberOfDislikes;
                        movie.ShortPlot = GetShortPlot(movie.Plot);
                    }
                }
            }
            return movies;
        }

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
