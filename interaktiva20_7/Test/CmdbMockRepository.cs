//using interaktiva20_7.Data;
//using interaktiva20_7.Models.DTO;
//using interaktiva20_7.ViewModels;
//using Microsoft.AspNetCore.Hosting;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using System.Threading.Tasks;

//namespace interaktiva20_7.Test
//{
//    public class CmdbMockRepository : ICmdbRepository
//    {
//        private string basePath;
//        public CmdbMockRepository(IWebHostEnvironment webHostEnvironment)
//        {
//            basePath = $"{webHostEnvironment.ContentRootPath}\\Test\\";
//        }
//        public async Task<MovieDto> GetMovieByImdbId(string imdbId)
//        {
//            var file = File.ReadAllText(basePath + "OmdbMockRepository.json");
//            var result = JsonConvert.DeserializeObject<List<MovieDto>>(file);
//            var movie = result.Where(m => m.ImdbID.Equals(imdbId)).FirstOrDefault();
//            var movieWithLikes = await GetLikesAndDislikes(movie);
//            await Task.Delay(100);
//            return movieWithLikes;
//        }

//        public async Task<MovieDto> GetLikesAndDislikes(MovieDto movie)
//        {
//            var file = File.ReadAllText(basePath + "CmdbMockRepository.json");
//            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(file);

//            foreach (var m in result)
//            {
//                if (m.ImdbID.Equals(movie.ImdbID))
//                {
//                    movie.numberOfLikes = m.numberOfLikes;
//                    movie.numberOfDislikes = m.numberOfDislikes;
//                }
//            }

//            await Task.Delay(0);
//            return movie;
//        }

//        public async Task<MoviesViewModel> GetMoviesBySearchString(string searchstring)
//        {
//            var file = File.ReadAllText(basePath + "OmdbMockRepository.json");
//            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(file);
//            List<MovieDto> searchresult = new List<MovieDto>();

//            foreach (var movie in result)
//            {
//                if (movie.Title.Contains(searchstring))
//                {
//                    searchresult.Add(movie);
//                }
//            }
//            await Task.Delay(0);
//            return new MoviesViewModel(searchresult);
//        }

//        public async Task<MoviesViewModel> GetMovieViewModel()
//        {
//            var file = File.ReadAllText(basePath + "CmdbMockRepository.json");
//            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(file);
//            List<MovieDto> topFourMoviesList = new List<MovieDto>();
//            topFourMoviesList = GetShortList(result);
//            var topFourMoviesWithInfo = await GetMovieInfoFromOmdb(topFourMoviesList);

//            for (int i = 0; i < topFourMoviesList.Count; i++)
//            {
//                int numberOfLikes = topFourMoviesList[i].numberOfLikes;
//                int numberOfDislikes = topFourMoviesList[i].numberOfDislikes;
//                topFourMoviesList[i] = await GetMovieByImdbId(topFourMoviesList[i].ImdbID);
//                topFourMoviesList[i].numberOfDislikes = numberOfDislikes;
//                topFourMoviesList[i].numberOfLikes = numberOfLikes;
//            }

//            await Task.Delay(0);
//            return new MoviesViewModel(topFourMoviesList);
//        }

//        public List<MovieDto> GetShortList(IEnumerable<MovieDto> movies)
//        {
//            List<MovieDto> moviesOrderByDescending = movies.OrderByDescending(x => (x.numberOfLikes - x.numberOfDislikes)).ToList();
//            List<MovieDto> topFourMoviesList = new List<MovieDto>();

//            for (int i = 0; i < 4; i++)
//            {
//                topFourMoviesList.Add(moviesOrderByDescending[i]);
//            }

//            return topFourMoviesList;
//        }

//        public async Task<List<MovieDto>> GetMovieInfoFromOmdb(List<MovieDto> listOfMovies)
//        {
//            for (int i = 0; i < listOfMovies.Count; i++)
//            {
//                int numberOfLikes = listOfMovies[i].numberOfLikes;
//                int numberOfDislikes = listOfMovies[i].numberOfDislikes;
//                listOfMovies[i] = await GetMovieByImdbId(listOfMovies[i].ImdbID);
//                listOfMovies[i].numberOfDislikes = numberOfDislikes;
//                listOfMovies[i].numberOfLikes = numberOfLikes;
//            }
//            return listOfMovies;
//        }
//    }
//}
