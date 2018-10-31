using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MCU.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IFilmRepository filmRepository)
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}