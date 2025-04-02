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

        public Task AddAsync(Reviews entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reviews>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Reviews> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Reviews entity)
        {
            throw new NotImplementedException();
        }
    }
}
