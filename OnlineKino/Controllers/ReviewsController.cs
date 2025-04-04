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
    public class ReviewsController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly ReviewService reviewService;
        public ReviewsController(MyContext context)
        {
            _context = context;
            reviewService = new ReviewService(new DbContextOptions<MyContext>());
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Movies>>> GetReviews()
        {
            var movies = await reviewService.GetAllAsync();
            return Ok(movies);
        }


        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Movies>> GetReviews(int id)
        {
            var movies = await reviewService.GetByIdAsync(id);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateReviews(int id, Reviews reviews)
        {
            if (id != reviews.id)
            {
                return BadRequest();
            }

            try
            {
                await reviewService.UpdateAsync(reviews);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewsExist(id))
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


        [HttpPost("AddReviews")]
        public async Task<ActionResult<Movies>> PostReviews(Reviews reviews)
        {
            await reviewService.AddAsync(reviews);
            return CreatedAtAction("GetMovies", new { id = reviews.id }, reviews);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var reviews = await reviewService.GetByIdAsync(id);
            if (reviews == null)
            {
                return NotFound();
            }

            await reviewService.DeleteAsync(id);
            return Ok();
        }

        private bool ReviewsExist(int id)
        {
            return _context.Reviews.Any(e => e.id == id);
        }



    }
}
