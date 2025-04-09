using Microsoft.EntityFrameworkCore;
using OnlineKino.Context;
using OnlineKino.Models;

namespace OnlineKino.Services
{
    public class AdminService : IService<Admins>, IPasswordService
    {

        private readonly DbContextOptions<MyContext> _options;
        public AdminService(DbContextOptions<MyContext> options)
        {
            _options = options;
        }
        public async Task AddAsync(Admins entity)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pAddAdmin {entity.Email}, {entity.Login}, {entity.Password}, {entity.Role}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pDeleteAdmin {id}");
            }
        }

        public async Task<IEnumerable<Admins>> GetAllAsync()
        {
            using (var db = new MyContext(_options))
            {
                return await db.Admins
                           .FromSqlInterpolated($"EXEC pShowAdmins")
                           .ToListAsync();
            }
        }

        public async Task<Admins> GetByIdAsync(int id)
        {
            using (var db = new MyContext(_options))
            {
                var admins = await db.Admins
             .FromSqlInterpolated($"EXEC pShowAdminById {id}")
             .ToListAsync();
                return admins.FirstOrDefault();
            }
        }

        public async Task UpdateAsync(Admins entity)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pUpdateAdmin {entity.id}, {entity.Email}, {entity.Login}, {entity.Role}");
            }
        }

        public async Task UpdatePasswordAsync(UserUpdatePassword dto)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pUpdateAdminPass {dto.Login}, {dto.OldPassword}, {dto.NewPassword}");

            }
        }
    }
}
