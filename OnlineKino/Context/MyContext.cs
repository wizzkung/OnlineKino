using Microsoft.EntityFrameworkCore;
using OnlineKino.Models;

namespace OnlineKino.Context
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Admins> Admins { get; set; }
        public DbSet<TokenResultDTO> TokenResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<TokenResultDTO>()
                .HasNoKey()
                .ToView(null);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(Program.config["db"]);
            // optionsBuilder.UseLazyLoadingProxies();// работает только при условии что навигационные поля виртуальные
            optionsBuilder.UseSqlServer("Server=LERA;Database=onlineKino;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=30");
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }



    }
}