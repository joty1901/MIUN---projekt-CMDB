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
        public IApiClient apiClient;

        public CmdbRepository(IConfiguration configuration)
        {
            cmdbBaseUrl = configuration.GetValue<string>("CMDBApi:BaseUrl");
            omdbBaseUrl = configuration.GetValue<string>("OMDBApi:BaseUrl");
            apiClient = new ApiClient();
        }

        public async Task<MoviesViewModel> GetMovieViewModel()
        {

        
            string cmdbMovies = $"{cmdbBaseUrl}/api/movie";
            var result = await apiClient.GetASync<IEnumerable<MovieDto>>(cmdbMovies);
            List<MovieDto> shortResultList = new List<MovieDto>();
            shortResultList = ShortenList(result);

            for (int i = 0; i < shortResultList.Count; i++)
            {
                //TODO: fixa så att den inte skriver över MovieDto
                int numberOfLikes = shortResultList[i].numberOfLikes;
                int numberOfDislikes = shortResultList[i].numberOfDislikes;
                string omdbMovies = $"{omdbBaseUrl}/?i={shortResultList[i].ImdbID}&apikey=398aa398";
                shortResultList[i] = await apiClient.GetASync<MovieDto>(omdbMovies);
                shortResultList[i].numberOfDislikes = numberOfDislikes;
                shortResultList[i].numberOfLikes = numberOfLikes;
            }

            await Task.Delay(0);
                return new MoviesViewModel(shortResultList);


            
        }

        public async Task<MovieDto> GetMovieByImdbId(string imdbId)
        {
            string endpoint = $"{omdbBaseUrl}/?apikey=398aa398&i={imdbId}";
            var result = await apiClient.GetASync<MovieDto>(endpoint);
            return result;
        }


        public async Task<SearchDto> GetMovieBySearch(string searchString)
        {
            using (HttpClient client = new HttpClient())
            {

                string endpoint = $"{omdbBaseUrl}/?t={searchString}&s={searchString}&apikey=398aa398";
                var response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SearchDto>(data);
                return result;
            }

        }

       

        public List<MovieDto> ShortenList(IEnumerable<MovieDto> movies)
        {
            List<MovieDto> temp1List = movies.OrderByDescending(x => (x.numberOfLikes - x.numberOfDislikes)).ToList();
            List<MovieDto> shortList = new List<MovieDto>();

            for (int i = 0; i < 4; i++)
            {
                shortList.Add(temp1List[i]);
            }

            return shortList;
        }


    }
}
