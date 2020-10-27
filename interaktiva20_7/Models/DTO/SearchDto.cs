using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace interaktiva20_7.Models.DTO
{
    public class SearchDto
    {
        public List<MovieDto> Search { get; set; }
        public int TotalResults { get; set; }

    }
}
