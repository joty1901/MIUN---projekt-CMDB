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

        [Route("")]
        [Route("/home")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("MovieList")))
            {
                var viewModel = await cmdbRepository.GetMovieViewModel();
                HttpContext.Session.SetString("MovieList", JsonConvert.SerializeObject(viewModel.movies));
                viewModel.savedList = JsonConvert.DeserializeObject<List<MovieDto>>(HttpContext.Session.GetString("MovieList"));
                return View(viewModel);
            }
            else { 
                var viewModel = cmdbRepository.GetMovieViewModelFromSession(JsonConvert.DeserializeObject<List<MovieDto>>(HttpContext.Session.GetString("MovieList")));
                return View(viewModel);
            }

        }
    }
}
