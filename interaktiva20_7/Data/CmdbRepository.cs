using interaktiva20_7.Models.DTO;
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

        private string baseUrl;

        public CmdbRepository(IConfiguration configuration)
        {
            baseUrl = configuration.GetValue<string>("CMDBApi:BaseUrl");
        }

        public async Task<IEnumerable<MovieDto>> GetTopRatedMovies()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string endpoint = $"{baseUrl}api/movie";
                    var response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(data);
                    result.OrderByDescending(x => x.Like - x.Dislike);
                    return result;
                }catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
