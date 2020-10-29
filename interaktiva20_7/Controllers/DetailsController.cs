using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using interaktiva20_7.Data;
using interaktiva20_7.Models.DTO;
using interaktiva20_7.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace interaktiva20_7.Controllers
{

    public class DetailsController : Controller
    {
        private ICmdbRepository cmdbRepository;

        public DetailsController(ICmdbRepository cmdbRepository)
        {
            this.cmdbRepository = cmdbRepository;
        }

        [Route("/details")]
        [HttpGet]
        public async Task<IActionResult> Index(string ID)
        {
            var movieList = JsonConvert.DeserializeObject<List<MovieDto>>(HttpContext.Session.GetString("MovieList"));
            var movieDetails = await cmdbRepository.GetMovieByImdbId(ID, movieList);
            return View(movieDetails);
        }
    }
}
