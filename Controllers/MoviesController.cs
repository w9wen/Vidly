using Microsoft.AspNetCore.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // Get: Movies/Random
        public IActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            // return View(movie);
            // return Content("Hello World!");
            // return NotFound();
            // return new EmptyResult();
            return  RedirectToAction("Index", "Home", new {page = 1, sortBy = "name"});
        }
    }
}