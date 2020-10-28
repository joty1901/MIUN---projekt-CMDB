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
        public List<MovieDto> topMovies;

        public MoviesViewModel(List<MovieDto> movies)
        {
            this.movies = movies;
            topMovies = GetShortList(movies);
        }

        private List<MovieDto> GetShortList(List<MovieDto> movies)
        {
            List<MovieDto> moviesOrderByDescending = movies.OrderByDescending(x => (x.numberOfLikes - x.numberOfDislikes)).ToList();
            List<MovieDto> topFourMoviesList = new List<MovieDto>();

            for (int i = 0; i < 4; i++)
            {
                topFourMoviesList.Add(moviesOrderByDescending[i]);
            }

            return topFourMoviesList;
        }

    }
}
