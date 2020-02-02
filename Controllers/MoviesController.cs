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
            ViewData["Movie"] = movie;

            return View();
            // return Content("Hello World!");
            // return NotFound();
            // return new EmptyResult();
            // return  RedirectToAction("Index", "Home", new {page = 1, sortBy = "name"});


        }

        public IActionResult Edit(int id)
        {
            return Content("id=" + id);
        }

        public IActionResult Index(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;

            if (string.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            return Content(string.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }

        [Route("Movies/Released/{year}/{month:regex(^\\d{{2}}$)}")]
        public IActionResult ByReleaseDate(int year, int month)
        {
            return Content("Date = " + year + "/" + month);
        }
        
    }
}