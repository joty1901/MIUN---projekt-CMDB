using interaktiva20_7.Models.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace interaktiva20_7.Data
{
    public class OmdbRepository : IOmdbRepository
    {
        private string baseUrl;

        public OmdbRepository(IConfiguration configuration)
        {
            baseUrl = configuration.GetValue<string>("OMDBApi:BaseUrl");
        }

        public async Task<IEnumerable<MovieDto>> GetMovieBySearch(string searchString)
        {
            using(HttpClient client = new HttpClient())
            {
                try
                {
                    string endpoint = $"{baseUrl}/?t={searchString}&s={searchString}&apikey=398aa398";
                    var response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(data);
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }
}
