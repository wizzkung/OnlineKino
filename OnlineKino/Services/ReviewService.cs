using Microsoft.EntityFrameworkCore;
using OnlineKino.Context;
using OnlineKino.Models;

namespace OnlineKino.Services
{
    public class ReviewService : IService<Reviews>
    {
        private readonly DbContextOptions<MyContext> _options;

        public ReviewService(DbContextOptions<MyContext> options)
        {
            _options = options;
        }

        public async Task AddAsync(Reviews entity)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pAddReview {entity.UserId}, {entity.MovieId}, {entity.Rating}, {entity.Comment}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pDeleteReview {id}");
            }
        }

        public async Task<IEnumerable<Reviews>> GetAllAsync()
        {
            using (var db = new MyContext(_options))
            {
                return await db.Reviews
                           .FromSqlInterpolated($"EXEC pShowAllReviews")
                           .ToListAsync();
            }
        }

        public async Task<Reviews> GetByIdAsync(int id)
        {
            using (var db = new MyContext(_options))
            {
                var reviews = await db.Reviews
             .FromSqlInterpolated($"EXEC pReviewsById {id}")
             .ToListAsync();
                return reviews.FirstOrDefault();
            }
        }

        public async Task UpdateAsync(Reviews entity)
        {
            using (var db = new MyContext(_options))
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC pUpdateReview {entity.id}, {entity.UserId}, {entity.MovieId},  {entity.Rating}, {entity.Comment}");
            }
        }
    }
}
