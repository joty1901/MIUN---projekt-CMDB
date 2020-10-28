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
            var movieDetails = await cmdbRepository.GetMovieByImdbId(ID);
            return View(movieDetails);
        }
    }
}
