using interaktiva20_7.Data;
using interaktiva20_7.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace interaktiva20_7.ViewModels
{
    public class MoviesViewModel
    {
        public MovieDto SelectedMovie;
        public List<MovieDto> savedList;
        public List<MovieDto> movies;
        public List<MovieDto> topMovies;

        public List<MovieDto> SessionMovieList { get; set; }

        public MoviesViewModel(List<MovieDto> movies)
        {
            this.movies = SortListOrderByLikes(movies);
            this.savedList = movies;
            topMovies = GetShortList(movies);
        }
        public MoviesViewModel(List<MovieDto> movies, List<MovieDto> savedList) { 
            this.movies = movies;
            this.savedList = savedList;
        }

        public MoviesViewModel(List<MovieDto> savedList, MovieDto selectedMovie)
        {
            this.savedList = savedList;
            this.SelectedMovie = selectedMovie;
            topMovies = GetShortList(savedList);
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

        public List<MovieDto> SortListOrderByLikes(IEnumerable<MovieDto> movies)
        {
            List<MovieDto> moviesOrderBydescending = movies.OrderByDescending(x => (x.numberOfLikes - x.numberOfDislikes)).ToList();

            return moviesOrderBydescending;
        }

    }
}
