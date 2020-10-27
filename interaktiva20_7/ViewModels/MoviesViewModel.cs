using interaktiva20_7.Data;
using interaktiva20_7.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace interaktiva20_7.ViewModels
{
    public class MoviesViewModel
    {
        public List<MovieDto> movies;
    
        public MoviesViewModel(List<MovieDto> movies)
        {
            this.movies = movies;
        }
    }
}
