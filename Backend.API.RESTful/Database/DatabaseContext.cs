using Backend.API.RESTful.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.RESTful.Database
{
    public class DatabaseContext: DbContext
    {
        // public DatabaseContext(DbContextOptions options): base(options) { }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<UsersModel> Users { get; set; }
        public DbSet<RolesModel> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersModel>().HasKey(user => user.IDUser);
            modelBuilder.Entity<RolesModel>().HasKey(rol => rol.IDRol);

            base.OnModelCreating(modelBuilder);
        }
    }
}
