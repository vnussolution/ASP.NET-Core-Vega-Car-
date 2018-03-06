using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vega.Models.PieShop;
using Vega.ViewModels;

namespace Vega.Controllers {
    public class HomeController : Controller {

        private readonly IPieRepository _pieRepository;

        public HomeController (IPieRepository pieRepository) {
            _pieRepository = pieRepository;
        }
        public IActionResult Index () {
            //throw new Exception ();
            return View ();
        }

        public IActionResult PieIndex () {

            var pies = _pieRepository.GetAllPies ().OrderBy (p => p.Name);
            var pieViewModel = new HomePieViewModel () {
                Title = "Welcome to Pie Shop",
                Pies = pies.ToList ()
            };
            return View (pieViewModel);
        }

        public IActionResult Details (int id) {
            var pie = _pieRepository.GetPieById (id);

            if (pie == null) {
                return NotFound ();
            }
            return View (pie);
        }

        public IActionResult Error () {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View ();
        }
    }
}