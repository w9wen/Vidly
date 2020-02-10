
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index()
        {
            // var movies = GetMovies();
            var movies = await this.dbContext.Movies.Include(m => m.Genre).ToListAsync();
            return View(movies);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var movie = await this.dbContext.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
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

        public async Task<IActionResult> New()
        {
            var genres = await this.dbContext.Genre.ToListAsync();
            var newMovie = new MovieFormViewModel()
            {
                Genres = genres
            };
            return View("MovieForm", newMovie);

        }
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await this.dbContext.Movies.SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound();

            var viewModel = new MovieFormViewModel()
            {
                Movie = movie,
                Genres = await dbContext.Genre.ToListAsync(),
            };

            return View("MovieForm", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Save(Movie movie)
        {
            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                await this.dbContext.Movies.AddAsync(movie);
            }
            else
            {
                var movieInDb = await this.dbContext.Movies.SingleAsync(m => m.Id == movie.Id);
                movieInDb.Name = movie.Name;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.NumberInStock = movie.NumberInStock;
                movieInDb.ReleaseDate = movie.ReleaseDate;
            }
            try
            {

                await this.dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return RedirectToAction("Index", "Movies");
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