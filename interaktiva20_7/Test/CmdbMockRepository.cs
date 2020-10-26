using interaktiva20_7.Data;
using interaktiva20_7.Models.DTO;
using interaktiva20_7.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            foreach (var movie in result)
            {
                if (id == movie.ImdbID)
                {
                    await Task.Delay(0);
                    return movie;
                }
            }
            return null;

        }

        public Task<SearchDto> GetMovieBySearch(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task<MoviesViewModel> GetMovieViewModel()
        {
            var file = File.ReadAllText(basePath + "CmdbMockRepository.json");
            var result = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(file);
            List<MovieDto> shortResultList = new List<MovieDto>();
            shortResultList = ShortenList(result);

            for (int i = 0; i < shortResultList.Count; i++)
            {
                //TODO: fixa så att den inte skriver över MovieDto
                int numberOfLikes = shortResultList[i].numberOfLikes;
                int numberOfDislikes = shortResultList[i].numberOfDislikes;
                shortResultList[i] = await GetMovieByImdbId(shortResultList[i].ImdbID);
                shortResultList[i].numberOfDislikes = numberOfDislikes;
                shortResultList[i].numberOfLikes = numberOfLikes;
            }

            await Task.Delay(0);
            return new MoviesViewModel(shortResultList);
        }

        public List<MovieDto> ShortenList(IEnumerable<MovieDto> movies)
        {
            List<MovieDto> temp1List = movies.OrderByDescending(x => (x.numberOfLikes - x.numberOfDislikes)).ToList();
            List<MovieDto> shortList = new List<MovieDto>();

            for (int i = 0; i < 4; i++)
            {
                shortList.Add(temp1List[i]);
            }

            return shortList;
        }


    }
}
