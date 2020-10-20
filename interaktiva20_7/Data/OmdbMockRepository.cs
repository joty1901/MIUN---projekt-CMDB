using interaktiva20_7.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace interaktiva20_7.Data
{
    public class OmdbMockRepository : IOmdbRepository
    {
        public async Task<IEnumerable<MovieDto>> GetMovieBySearch(string searchString)
        {
            var response = new WebClient().DownloadString("./Test/StarWarsMock.json");
            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(response);
            return result;
        }
    }
}
