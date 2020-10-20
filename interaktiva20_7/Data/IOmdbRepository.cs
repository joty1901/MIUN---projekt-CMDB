using interaktiva20_7.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace interaktiva20_7.Data
{
    public interface IOmdbRepository
    {
        Task<SearchDto> GetMovieBySearch(string searchString);
    }
}
