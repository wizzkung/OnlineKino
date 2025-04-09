using Microsoft.EntityFrameworkCore;
using OnlineKino.Context;
using OnlineKino.Models;

namespace OnlineKino.Services
{
    public class UserService : IService<Users>, IPasswordService
    {
        private readonly DbContextOptions<MyContext> _options;
        public UserService(DbContextOptions<MyContext> options)
        {
            _options = options;
        }
        public async Task AddAsync(Users entity)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pAddUser {entity.Email}, {entity.Login}, {entity.Password}");
            }
        }
        public async Task DeleteAsync(int id)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pDeleteUser {id}");
            }
        }
        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            using (var db = new MyContext(_options))
            {
                return await db.Users
                           .FromSqlInterpolated($"EXEC pShowUsers")
                           .ToListAsync();
            }
        }
        public async Task<Users> GetByIdAsync(int id)
        {
            using (var db = new MyContext(_options))
            {
                var users = await db.Users
             .FromSqlInterpolated($"EXEC pShowUserById {id}")
             .ToListAsync();
                return users.FirstOrDefault();
            }
        }
        public async Task UpdateAsync(Users entity)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pUpdateUser {entity.id}, {entity.Email}, {entity.Login}");
            }
        }

        public async Task UpdatePasswordAsync(UserUpdatePassword dto)
        {
            using (var db = new MyContext(_options))    
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pUpdateUserPass {dto.Login}, {dto.OldPassword}, {dto.NewPassword}");
             
            }
        }
    }

}

