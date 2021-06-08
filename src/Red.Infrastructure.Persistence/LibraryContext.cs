using Microsoft.EntityFrameworkCore;
using Red.Core.Domain.Models;
using Red.Infrastructure.Persistence.Configurations;

namespace Red.Infrastructure.Persistence
{
    internal sealed class LibraryContext : DbContext
    {
        #pragma warning disable 8618
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<SwitchGame> Games { get; init; }
        #pragma warning restore 8618

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SwitchGameTypeConfiguration());
        }
    }
}