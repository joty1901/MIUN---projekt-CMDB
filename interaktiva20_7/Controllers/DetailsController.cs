using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace interaktiva20_7.Controllers
{

    public class DetailsController : Controller
    {
        public DetailsController()
        {

        }

        [Route("/details")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
