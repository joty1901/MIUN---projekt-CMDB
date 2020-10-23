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

        public CmdbRepository(IConfiguration configuration)
        {
            cmdbBaseUrl = configuration.GetValue<string>("CMDBApi:BaseUrl");
            omdbBaseUrl = configuration.GetValue<string>("OMDBApi:BaseUrl");
        }

        public async Task<MoviesViewModel> GetMovieViewModel()
        {

            using (HttpClient client = new HttpClient())
            {
                
                string endpoint = $"{cmdbBaseUrl}api/movie";
                var response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(data);
                List<MovieDto> shortResultList = new List<MovieDto>();
                shortResultList = ShortenList(result);

                for (int i = 0; i < shortResultList.Count; i++)
                {
                    //TODO: fixa så att den inte skriver över MovieDto
                    int numberOfLikes = shortResultList[i].numberOfLikes;
                    int numberOfDislikes = shortResultList[i].numberOfDislikes;
                    shortResultList[i] = await GetMovieByImdbId(shortResultList[i].ImdbID);
                    shortResultList[i].numberOfDislikes = numberOfDislikes;
                    shortResultList[i].numberOfLikes = numberOfLikes;
                }
                

                return new MoviesViewModel(shortResultList);


            }
        }


        public async Task<IEnumerable<MovieDto>> GetTopRatedMovies()
        {
            using (HttpClient client = new HttpClient())
            {
                
                    string endpoint = $"{cmdbBaseUrl}api/movie";
                    var response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(data);

                    return result;
             
            }
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

        public async Task<MovieDto> GetMovieByImdbId(string imdbId)
        {
            using (HttpClient client = new HttpClient())
            {

                string endpoint = $"{omdbBaseUrl}/?i={imdbId}&apikey=398aa398";
                var response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<MovieDto>(data);
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
