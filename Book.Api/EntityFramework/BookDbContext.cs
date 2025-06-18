using Book.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Book.Api.EntityFramework
{
    public class BookDbContext(DbContextOptions<BookDbContext> Options) : DbContext(Options)
    {
        public DbSet<BookData> Books { get; set; }
    }
}
