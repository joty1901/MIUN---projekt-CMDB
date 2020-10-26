using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using interaktiva20_7.Data;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index(string searchString)
        {
            var searchresult = await cmdbRepository.GetMoviesBySearchString(searchString);
            return View(searchresult);
        }
    }
}
