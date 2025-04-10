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
        private readonly IService<Movies> _moviesService;
        public MovieController(IService<Movies> service)
        {
            _moviesService = service;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Movies>>> GetMovies()
        {
            var movies = await _moviesService.GetAllAsync();
            return Ok(movies);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Movies>> GetMovies(int id)
        {
            var movies = await _moviesService.GetByIdAsync(id);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutMovies(int id, Movies movies)
        {
            if (id != movies.id)
            {
                return BadRequest();
            }

            try
            {
                await _moviesService.UpdateAsync(movies);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _moviesService.GetByIdAsync(id) != null;
                if (!exists)
                    return NotFound();
                throw;
            }
            return Ok();
 
        }

        [HttpPost("AddMovie")]
        public async Task<ActionResult<Movies>> PostMovies(Movies movies)
        {
            await _moviesService.AddAsync(movies);
            return CreatedAtAction("GetMovies", new { id = movies.id }, movies);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteMovies(int id)
        {
            var movies = await _moviesService.GetByIdAsync(id);
            if (movies == null)
            {
                return NotFound();
            }

            await _moviesService.DeleteAsync(id);
            return Ok();
        }

    }
}
