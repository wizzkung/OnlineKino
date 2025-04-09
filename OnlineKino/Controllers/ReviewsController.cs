using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineKino.Context;
using OnlineKino.Models;
using OnlineKino.Services;
using System.Data;

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
            return Ok();
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

        [HttpGet("ExcelLastDay")]
        public async Task<IActionResult> ExcelLastDay()
        {
            var reviews = await _context.Reviews
                .FromSqlInterpolated($"EXEC pReviewsLast24")
                .ToListAsync();

            if (reviews == null || !reviews.Any())
                return BadRequest();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Data");
            var dataTable = new DataTable();
            var props = typeof(Reviews).GetProperties();

            foreach (var prop in props)
                dataTable.Columns.Add(prop.Name);

            foreach (var item in reviews)
            {
                var row = dataTable.NewRow();
                foreach (var prop in props)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            worksheet.Cell(1, 1).InsertTable(dataTable);
            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Report.xlsx");
        }
        [HttpGet("ExcelLastMonth")]
        public async Task<IActionResult> ExcelLastMonth()
        {
            var reviews = await _context.Reviews
               .FromSqlInterpolated($"EXEC pReviewsLastMonth")
               .ToListAsync();

            if (reviews == null || !reviews.Any())
                return BadRequest();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Data");
            var dataTable = new DataTable();
            var props = typeof(Reviews).GetProperties();

            foreach (var prop in props)
                dataTable.Columns.Add(prop.Name);

            foreach (var item in reviews)
            {
                var row = dataTable.NewRow();
                foreach (var prop in props)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            worksheet.Cell(1, 1).InsertTable(dataTable);
            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Report.xlsx");
        }
    }
}
