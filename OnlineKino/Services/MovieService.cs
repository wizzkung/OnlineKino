using Microsoft.EntityFrameworkCore;
using OnlineKino.Context;
using OnlineKino.Models;

namespace OnlineKino.Services
{
    public class MovieService : IService<Movies>
    {

        private readonly DbContextOptions<MyContext> _options;

        public MovieService(DbContextOptions<MyContext> options)
        {
            _options = options;
        }

        public async Task AddAsync(Movies entity)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pAddMovie {entity.Name}, {entity.Genres}, {entity.Duration}, {entity.Link}, {entity.PosterUrl}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pDeleteMovie {id}");
            }
        }

        public async Task<IEnumerable<Movies>> GetAllAsync()
        {
            using (var db = new MyContext(_options))
            {
                return await db.Movies
                           .FromSqlInterpolated($"EXEC pShowMovies")
                           .ToListAsync();
            }
        }

        public async Task<Movies> GetByIdAsync(int id)
        {
            using (var db = new MyContext(_options))
            {
                var movies = await db.Movies
             .FromSqlInterpolated($"EXEC pShowById {id}")
             .ToListAsync();
                return movies.FirstOrDefault();
            }
        }

        public async Task UpdateAsync(Movies entity)
        {
            using (var db = new MyContext(_options))
            {
              await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pUpdateMovie {entity.id}, {entity.Name}, {entity.Genres},  {entity.Duration}, {entity.Link}, {entity.PosterUrl}");
            }

            
        }



       
    }
}
