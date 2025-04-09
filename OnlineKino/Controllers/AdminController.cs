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
        private readonly MyContext _context;
        private readonly AdminService adminService;
        public AdminController(MyContext context)
        {
            _context = context;
            adminService = new AdminService(new DbContextOptions<MyContext>());
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Admins>>> GetAdmins()
        {
            var admins = await adminService.GetAllAsync();
            return Ok(admins);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Admins>> GetAdmins(int id)
        {
            var admins = await adminService.GetByIdAsync(id);
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
                await adminService.UpdatePasswordAsync(dto);
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
                await adminService.AddAsync(admin);
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
                await adminService.DeleteAsync(id);
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
                await adminService.UpdateAsync(admin);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminsExist(id))
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

        private bool AdminsExist(int id)
        {
            return _context.Admins.Any(e => e.id == id);
        }

    }
}
