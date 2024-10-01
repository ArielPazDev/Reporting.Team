using Backend.API.RESTful.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.RESTful.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RolModel> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasKey(user => user.IDUser);
            modelBuilder.Entity<RolModel>().HasKey(rol => rol.IDRol);

            base.OnModelCreating(modelBuilder);
        }
    }
}
