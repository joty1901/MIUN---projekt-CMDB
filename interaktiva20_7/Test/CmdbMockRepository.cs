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
        public async Task<MovieDto> GetMovieByImdbId(string id)
        {

            var file = File.ReadAllText(basePath + "OmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<List<MovieDto>>(file);
            var movie = result.Where(m => m.ImdbID.Equals(id)).FirstOrDefault();
            await Task.Delay(100);
            return movie;

            //foreach (var movie in result)
            //{
            //    if (id == movie.ImdbID)
            //    {
            //        await Task.Delay(0);
            //        return movie;
            //    }
            //}
            //return null;
        }

        public async Task<MoviesViewModel> GetMoviesBySearchString(string searchstring)
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
            return new MoviesViewModel(searchresult);
        }

        public Task<SearchDto> GetMovieBySearch(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task<MoviesViewModel> GetMovieViewModel()
        {
            var file = File.ReadAllText(basePath + "CmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(file);
            List<MovieDto> topFourMoviesList = new List<MovieDto>();
            topFourMoviesList = ShortenList(result);

            for (int i = 0; i < topFourMoviesList.Count; i++)
            {
                //TODO: fixa så att den inte skriver över MovieDto
                int numberOfLikes = topFourMoviesList[i].numberOfLikes;
                int numberOfDislikes = topFourMoviesList[i].numberOfDislikes;
                topFourMoviesList[i] = await GetMovieByImdbId(topFourMoviesList[i].ImdbID);
                topFourMoviesList[i].numberOfDislikes = numberOfDislikes;
                topFourMoviesList[i].numberOfLikes = numberOfLikes;
            }

            await Task.Delay(0);
            return new MoviesViewModel(topFourMoviesList);
        }

        public List<MovieDto> ShortenList(IEnumerable<MovieDto> movies)
        {
            List<MovieDto> temp1List = movies.OrderByDescending(x => (x.numberOfLikes - x.numberOfDislikes)).ToList();
            List<MovieDto> topFourMoviesList = new List<MovieDto>();

            for (int i = 0; i < 4; i++)
            {
                topFourMoviesList.Add(temp1List[i]);
            }

            return topFourMoviesList;
        }
    }
}
