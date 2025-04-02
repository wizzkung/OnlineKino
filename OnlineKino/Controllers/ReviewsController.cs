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
        private readonly Procedures _procedures;
        public ReviewsController(MyContext context)
        {
            _context = context;
            _procedures = new Procedures(new DbContextOptions<MyContext>());
        }

      


    }
}
