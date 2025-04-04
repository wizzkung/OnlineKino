
using Microsoft.EntityFrameworkCore;
using OnlineKino.Context;
using OnlineKino.Models;
using OnlineKino.Services;

namespace OnlineKino
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          
            builder.Services.AddDbContext<MyContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IService<Movies>, MovieService>();
            builder.Services.AddScoped<MovieService>();
            builder.Services.AddScoped<IService<Reviews>, ReviewService>();
            builder.Services.AddScoped<ReviewService>();    

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

         
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
