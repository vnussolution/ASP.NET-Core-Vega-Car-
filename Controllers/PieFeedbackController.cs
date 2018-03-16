using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vega.Models.PieShop;
using Vega.ViewModels;

namespace Vega.Controllers {

    [Authorize]
    public class PieFeedbackController : Controller {

        private readonly IFeedbackRepository _feedbackRepository;

        public PieFeedbackController (IFeedbackRepository feedbackRepository) {
            _feedbackRepository = feedbackRepository;
        }
        public IActionResult Index () {
            //throw new Exception ();
            return View ();
        }

        [HttpPost]
        public IActionResult Index (Feedback feedback) {
            if (ModelState.IsValid) {
                _feedbackRepository.AddFeedback (feedback);
                return RedirectToAction ("FeedbackComplete");
            } else {
                return View (feedback);
            }

        }

        public IActionResult FeedbackComplete () {
            return View ();
        }

        public IActionResult Error () {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View ();
        }
    }
}