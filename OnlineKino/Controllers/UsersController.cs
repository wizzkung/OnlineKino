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
    public class UsersController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly UserService userService;
        public UsersController(MyContext context)
        {
            _context = context;
            userService = new UserService(new DbContextOptions<MyContext>());
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            var users = await userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await userService.GetByIdAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpPost("UpdatePass")]
        public async Task<IActionResult> UpdatePassword(UserUpdatePassword dto)
        {
            try
            {
                await userService.UpdatePasswordAsync(dto);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(Users users)
        {
            try
            {
                await userService.AddAsync(users);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await userService.DeleteAsync(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return Ok();
        }


        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, Users user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            try
            {
                await userService.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExist(id))
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

        private bool UserExist(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }

    }
}
