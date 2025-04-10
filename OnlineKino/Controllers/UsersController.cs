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
        private readonly IService<Users> _userService;
        private readonly JwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public UsersController(IService<Users> userService, JwtService jwt, IPasswordService passwordService)
        {
            _userService = userService;
            _jwtService = jwt;
            _passwordService = passwordService;

        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _userService.GetByIdAsync(id);
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
                await _passwordService.UpdatePasswordAsync(dto);
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
                await _userService.AddAsync(users);
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
                await _userService.DeleteAsync(id);
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
                await _userService.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _userService.GetByIdAsync(id) != null;
                if (!exists)
                    return NotFound();
                throw;
            }
            return Ok();
        }

        [HttpGet ("Auth")]
        public async Task<ActionResult<Users>> Auth(string login, string password)
        {
            var user = await _jwtService.GenerateTokenAsync(login, password);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

      

    }
}
