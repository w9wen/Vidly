using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private ApplicationDbContext dbContext;
        private IMapper mapper;
        public MoviesController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            // var movieList = await this.dbContext.Movies.ToListAsync();
            var movieList = 
                await this.dbContext.Movies
                .Include(m => m.Genre)
                .ToListAsync();
            var movieListDto = this.mapper.Map<List<MovieDto>>(movieList);
            return movieListDto;
        }

        // GET: api/movies/2
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await this.dbContext.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            var movieDto = this.mapper.Map<MovieDto>(movie);

            return movieDto;
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = this.mapper.Map<Movie>(movieDto);
            await this.dbContext.Movies.AddAsync(movie);
            await this.dbContext.SaveChangesAsync();
            movieDto.Id = movie.Id;

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // PUT: api/movies/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovie(int id, MovieDto movieDto)
        {
            if (id != movieDto.Id)
                return BadRequest();

            var movie = this.mapper.Map<Movie>(movieDto);
            this.dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else throw;
            }
            return NoContent();
        }

        // DELETE: api/movies/3
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            var movie = await this.dbContext.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();
            this.dbContext.Movies.Remove(movie);
            await this.dbContext.SaveChangesAsync();

            return movie;
        }

        private bool MovieExists(int id) => this.dbContext.Movies.Any(m => m.Id == id);
    }
}