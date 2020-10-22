using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using interaktiva20_7.Data;
using Microsoft.AspNetCore.Mvc;

namespace interaktiva20_7.Controllers
{
    public class StartController : Controller
    {
        private IOmdbRepository omdbRepository;
        private ICmdbRepository cmdbRepository;

        public StartController(IOmdbRepository omdbRepository, ICmdbRepository cmdbRepository)
        {
            this.omdbRepository = omdbRepository;
            this.cmdbRepository = cmdbRepository;
        }

        //public async Task<IActionResult> Movies()
        //{
        //    //TODO: Ändra namn på metoden Movies till något vettigt.
        //    //TODO: Ändra en hårdkodade söksträngen som GetMovieBySearch just nu använder.
        //    var model = await omdbRepository.GetMovieBySearch("Star");
        //    return View(model);
        //}

        public async Task<IActionResult> Index()
        {
            var model = await omdbRepository.GetMovieBySearch("Star");
            return View(model);
        }
    }
}
