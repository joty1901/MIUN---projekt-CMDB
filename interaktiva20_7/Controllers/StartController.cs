    using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using interaktiva20_7.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using interaktiva20_7.Models.DTO;

namespace interaktiva20_7.Controllers
{
    public class StartController : Controller
    {
        private ICmdbRepository cmdbRepository;

        public StartController(ICmdbRepository cmdbRepository)
        {
            this.cmdbRepository = cmdbRepository;
        }

        //public async Task<IActionResult> Movies()
        //{
        //    //TODO: Ändra namn på metoden Movies till något vettigt.
        //    //TODO: Ändra en hårdkodade söksträngen som GetMovieBySearch just nu använder.
        //    var model = await omdbRepository.GetMovieBySearch("Star");
        //    return View(model);
        //}
        [Route("")]
        [Route("/home")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = await cmdbRepository.GetMovieViewModel();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("MovieList")))
            {
                HttpContext.Session.SetString("MovieList", JsonConvert.SerializeObject(viewModel.movies));
            }
            viewModel.savedList = JsonConvert.DeserializeObject<List<MovieDto>>(HttpContext.Session.GetString("MovieList"));

            return View(viewModel);
        }

    }
}
