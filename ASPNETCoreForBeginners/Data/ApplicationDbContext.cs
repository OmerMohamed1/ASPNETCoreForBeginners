using ASPNETCoreForBeginners.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreForBeginners.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserPermission>().ToTable("UserPermissions")
                .HasKey(x => new { x.UserId, x.PermissionId });
        }
    }
}
