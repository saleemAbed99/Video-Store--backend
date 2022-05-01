using Microsoft.EntityFrameworkCore;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<FavoriteList> FavoriteLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => new { x.Id, x.Username });
            modelBuilder.Entity<User>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}