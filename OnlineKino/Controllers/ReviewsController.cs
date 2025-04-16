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
        private readonly IService<Reviews> _reviewsService;
        private readonly MyContext _context;
        public ReviewsController(IService<Reviews> service, MyContext context)
        {
            _reviewsService = service;
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Movies>>> GetReviews()
        {
            var movies = await _reviewsService.GetAllAsync();
            return Ok(movies);
        }


        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Movies>> GetReviews(int id)
        {
            var movies = await _reviewsService.GetByIdAsync(id);
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
                await _reviewsService.UpdateAsync(reviews);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _reviewsService.GetByIdAsync(id) != null;
                if (!exists)
                    return NotFound();
                throw;
            }
            return Ok();
        }


        [HttpPost("AddReviews")]
        public async Task<ActionResult<Movies>> PostReviews(Reviews reviews)
        {
            await _reviewsService.AddAsync(reviews);
            return Ok(reviews);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var reviews = await _reviewsService.GetByIdAsync(id);
            if (reviews == null)
            {
                return NotFound();
            }

            await _reviewsService.DeleteAsync(id);
            return Ok();
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
