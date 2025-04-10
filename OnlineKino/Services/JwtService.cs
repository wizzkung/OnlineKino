using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineKino.Context;
using OnlineKino.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineKino.Services
{
    public class JwtService
    {
        private readonly DbContextOptions<MyContext> _options;
        IConfiguration config;
        public JwtService(DbContextOptions<MyContext> options, IConfiguration configuration)
        {
            _options = options;
            config = configuration;
        }

        public async Task<TokenResultDTO> GenerateTokenAsync(string login, string password)
        {
            using var db = new MyContext(_options);

            // Изменение запроса: не используем 'Token' в выборке
            var resultList = await db.TokenResults
                .FromSqlRaw("EXEC pGenerateToken @login, @password",
                            new SqlParameter("@login", login),
                            new SqlParameter("@password", password))
                .ToListAsync();

            var user = resultList.FirstOrDefault();
            if (user == null || user.Status == 0)
            {
                // Возвращаем пустой объект, если статус 0 или пользователь не найден
                return new TokenResultDTO
                {
                    Status = 0,
                    Token = null,
                    Role = null,
                    Login = null,
                    Email = null
                };
            }

            // Генерация токена на C# стороне
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Login),
        new Claim(ClaimTypes.Role, user.UserType),
        new Claim("id", user.Id.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                                issuer: config["Jwt:Issuer"],
                                audience: config["Jwt:Issuer"],
                                claims: claims,
                                expires: DateTime.UtcNow.AddMinutes(30),
                                signingCredentials: creds);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Теперь добавляем токен в результат
            return new TokenResultDTO
            {
                Status = 1,
                UserType = user.UserType,
                Id = user.Id,
                Email = user.Email,
                Login = user.Login,
                Role = user.Role,
                Token = tokenString // Генерация и возврат токена
            };
        }


    }
}
