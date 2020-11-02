using interaktiva20_7.Data;
using interaktiva20_7.Models.DTO;
using interaktiva20_7.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace interaktiva20_7.Test
{
    public class CmdbMockRepository : ICmdbRepository
    {
        private string basePath;
        public CmdbMockRepository(IWebHostEnvironment webHostEnvironment)
        {
            basePath = $"{webHostEnvironment.ContentRootPath}\\Test\\";
        }
        public async Task<MoviesViewModel> GetMovieByImdbId(string imdbId, List<MovieDto> savedMovies)
        {
            var file = File.ReadAllText(basePath + "OmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<List<MovieDto>>(file);
            var movie = result.Where(m => m.ImdbID.Equals(imdbId)).FirstOrDefault();
            var movieWithLikes = await GetLikesAndDislikes(movie);
            await Task.Delay(0);
            return new MoviesViewModel(savedMovies, movieWithLikes);
        }

        public async Task<MovieDto> GetLikesAndDislikes(MovieDto movie)
        {
            var file = File.ReadAllText(basePath + "CmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(file);

            foreach (var m in result)
            {
                if (m.ImdbID.Equals(movie.ImdbID))
                {
                    movie.numberOfLikes = m.numberOfLikes;
                    movie.numberOfDislikes = m.numberOfDislikes;
                }
            }

            await Task.Delay(0);
            return movie;
        }

        public async Task<MoviesViewModel> GetMoviesBySearchString(string searchstring, List<MovieDto> movies)
        {
            var file = File.ReadAllText(basePath + "OmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(file);
            List<MovieDto> searchresult = new List<MovieDto>();

            foreach (var movie in result)
            {
                if (movie.Title.Contains(searchstring))
                {
                    searchresult.Add(movie);
                }
            }
            await Task.Delay(0);
            return new MoviesViewModel(searchresult, movies);
        }

        public async Task<MoviesViewModel> GetMovieViewModel()
        {
            var file = File.ReadAllText(basePath + "CmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<List<MovieDto>>(file);
            List<MovieDto> movies;
            movies = await GetMovieInfoFromOmdb(result);


            await Task.Delay(0);
            return new MoviesViewModel(movies);
        }

        public MoviesViewModel GetMovieViewModelFromSession(List<MovieDto> savedList)
        {
            return new MoviesViewModel(savedList);
        }

        public List<MovieDto> GetShortList(IEnumerable<MovieDto> movies)
        {
            List<MovieDto> moviesOrderByDescending = movies.OrderByDescending(x => (x.numberOfLikes - x.numberOfDislikes)).ToList();
            List<MovieDto> topFourMoviesList = new List<MovieDto>();

            for (int i = 0; i < 4; i++)
            {
                topFourMoviesList.Add(moviesOrderByDescending[i]);
            }

            return topFourMoviesList;
        }

        public async Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> cmdbResult)
        {
            var file = File.ReadAllText(basePath + "OmdbMockRepository.json");
            var movies = JsonConvert.DeserializeObject<List<MovieDto>>(file);

           
            movies = RecoverMissingLikes(movies, cmdbResult);
            await Task.Delay(0);
            return movies;
        }

        private List<MovieDto> RecoverMissingLikes(List<MovieDto> movies, List<MovieDto> cmdbResult)
        {
            foreach (var movie in movies)
            {
                foreach (var cmdbMovie in cmdbResult)
                {
                    if (movie.ImdbID == cmdbMovie.ImdbID)
                    {
                        movie.numberOfLikes = cmdbMovie.numberOfLikes;
                        movie.numberOfDislikes = cmdbMovie.numberOfDislikes;
                    }
                }
            }
            return movies;
        }
    }
}
