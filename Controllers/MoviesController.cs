
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // Get: Movies/Random
        public IActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            // ViewData["RandomMovie"] = movie;
            // ViewBag.Movie = movie;

            // var viewResult = new ViewResult();
            // viewResult.ViewData.Model = 

            var customers = new List<Customer>
            {
                new Customer {Name = "Customer 1"},
                new Customer {Name = "Customer 2"}
            };
            var viewModel = new RandomMovieViewModel()
            {
                Movie = movie,
                Customers = customers
            };
            
            return View(viewModel);
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