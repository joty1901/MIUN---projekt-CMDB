using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace interaktiva20_7.Data
{
    public partial class ApiClient : IApiClient
    {
        public async Task<T> GetASync<T>(string endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(data);
                return result;
            }
        }
    }
}
