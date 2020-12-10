using Microsoft.EntityFrameworkCore;

namespace KidsList.Data
{
    public class KidsListContext : DbContext
    {
        public KidsListContext(DbContextOptions<KidsListContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This allows the user email column to use case insensitive operations.
            modelBuilder.HasPostgresExtension("citext");
        }

        public DbSet<Family> Families { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Kid> Kids { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
    }
}