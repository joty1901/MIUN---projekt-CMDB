using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using interaktiva20_7.Data;
using interaktiva20_7.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace interaktiva20_7.Controllers
{
    public class SearchController : Controller
    {
        private ICmdbRepository cmdbRepository;

        public SearchController(ICmdbRepository cmdbRepository)
        {
            this.cmdbRepository = cmdbRepository;
        }

        [Route("/search")]
        [HttpGet]
        public async Task<IActionResult> Index(string ID)
        {
            var movieList = JsonConvert.DeserializeObject<List<MovieDto>>(HttpContext.Session.GetString("MovieList"));
            var viewModel = await cmdbRepository.GetMoviesBySearchString(ID, movieList);
            return View(viewModel);
        }
    }
}
