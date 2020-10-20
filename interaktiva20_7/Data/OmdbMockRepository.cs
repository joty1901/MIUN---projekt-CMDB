using interaktiva20_7.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace interaktiva20_7.Data
{
    public class OmdbMockRepository : IOmdbRepository
    {
        public Task<IEnumerable<MovieDto>> GetMovieBySearch(string searchString)
        {
            
        }



    }
}
