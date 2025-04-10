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
    public class AdminController : ControllerBase
    {
        private readonly IService<Admins> _adminService;
        private readonly JwtService _jwtService;
        private readonly IPasswordService _passwordService;
        public AdminController(IService<Admins> service, JwtService jwt, IPasswordService passwordService)
        {
            _adminService = service;
            _jwtService = jwt;
            _passwordService = passwordService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Admins>>> GetAdmins()
        {
            var admins = await _adminService.GetAllAsync();
            return Ok(admins);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Admins>> GetAdmins(int id)
        {
            var admins = await _adminService.GetByIdAsync(id);
            if (admins == null)
            {
                return NotFound();
            }
            return Ok(admins);
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

        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin(Admins admin)
        {
            try
            {
                await _adminService.AddAsync(admin);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            try
            {
                await _adminService.DeleteAsync(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, Admins admin)
        {
            if (id != admin.id)
            {
                return BadRequest();
            }

            try
            {
                await _adminService.UpdateAsync(admin);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _adminService.GetByIdAsync(id) != null;
                if (!exists)
                    return NotFound();
                throw;
            }
            return Ok();
        }


        [HttpGet("Auth")]
        public async Task<ActionResult<Admins>> Auth(string login, string password)
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
