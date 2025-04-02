using Microsoft.EntityFrameworkCore;
using OnlineKino.Context;

namespace OnlineKino.Services
{
    public class Procedures
    {
        private readonly DbContextOptions<MyContext> _options;

        public Procedures(DbContextOptions<MyContext> options)
        {
            _options = options;
        }

        public void pShowMovies()
        {
            using (var db = new MyContext(_options))
            {
                db.Database.ExecuteSqlRaw("EXEC pShowMovies");
            }
        }

        public void pDeleteMovies(int id)
        {
            using (var db = new MyContext(_options))
            {
                db.Database.ExecuteSqlInterpolated($"EXEC pDeleteMovie {id}");
            }
        }

        public void pShowMovieById(int id)
        {
            using (var db = new MyContext(_options))
            {
                db.Database.ExecuteSqlInterpolated($"EXEC pShowById {id}");
            }
        }

        public void pAddMovie(string name, string genre, int duration, string link, string posterURL)
        {
            using (var db = new MyContext(_options))
            {
                db.Database.ExecuteSqlInterpolated($"EXEC pAddMovie {name}, {genre},  {duration}, {link}, {posterURL}");
            }
        }

        public void pUpdateMovie(int id, string name, string genre, int duration, string link, string posterURL)
        {
            using (var db = new MyContext(_options))
            {
                db.Database.ExecuteSqlInterpolated($"EXEC pUpdateMovie {id}, {name}, {genre},  {duration}, {link}, {posterURL}");
            }
        }

        public void pAddReview(int userId, int movieId, int rating, string comment)
        {
            using (var db = new MyContext(_options))
            {
                db.Database.ExecuteSqlInterpolated($"EXEC pAddReview {userId}, {movieId}, {rating}, {comment}");
            }
        }

        public void pDeleteReview(int id)
        {
            using (var db = new MyContext(_options))
            {
                db.Database.ExecuteSqlInterpolated($"EXEC pDeleteReview {id}");
            }
        }

        public void pShowAllReviews()
        {
            using (var db = new MyContext(_options))
            {
                db.Database.ExecuteSqlRaw("EXEC pShowAllReviews");
            }
        }
    }
}
