
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext dbContext;
        public MoviesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override void Dispose(bool disposing)
        {
            this.dbContext.Dispose();
        }

        public IActionResult Index()
        {
            // var movies = GetMovies();
            var movies = this.dbContext.Movies.Include(m => m.Genre).ToList();
            return View(movies);
        }

        public IActionResult Detail(int id)
        {
            var movie = this.dbContext.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound();
            return View(movie);
        }

        private IEnumerable<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie(){ Id = 1, Name = "Shrek!"},
                new Movie(){ Id = 2, Name = "Wall-E"}
            };
        }




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

        // public IActionResult Index(int? pageIndex, string sortBy)
        // {
        //     if (!pageIndex.HasValue)
        //         pageIndex = 1;

        //     if (string.IsNullOrWhiteSpace(sortBy))
        //         sortBy = "Name";

        //     return Content(string.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        // }

        [Route("Movies/Released/{year}/{month:regex(^\\d{{2}}$)}")]
        public IActionResult ByReleaseDate(int year, int month)
        {
            return Content("Date = " + year + "/" + month);
        }

    }
}