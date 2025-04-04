using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineKino.Context;
using OnlineKino.Models;
using OnlineKino.Services;

namespace OnlineKino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly MovieService _movieService;

        public MovieController(MyContext context, MovieService movieService)
        {
            _context = context;
            _movieService = movieService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Movies>>> GetMovies()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Movies>> GetMovies(int id)
        {
            var movies = await _movieService.GetByIdAsync(id);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost("Update{id}")]
        public async Task<IActionResult> PutMovies(int id, Movies movies)
        {
            if (id != movies.id)
            {
                return BadRequest();
            }

            try
            {
                await _movieService.UpdateAsync(movies);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoviesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost("AddMovie")]
        public async Task<ActionResult<Movies>> PostMovies(Movies movies)
        {
            await _movieService.AddAsync(movies);
            return CreatedAtAction("GetMovies", new { id = movies.id }, movies);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteMovies(int id)
        {
            var movies = await _movieService.GetByIdAsync(id);
            if (movies == null)
            {
                return NotFound();
            }

            await _movieService.DeleteAsync(id);
            return Ok();
        }

        private bool MoviesExists(int id)
        {
            return _context.Movies.Any(e => e.id == id);
        }
    }
}
