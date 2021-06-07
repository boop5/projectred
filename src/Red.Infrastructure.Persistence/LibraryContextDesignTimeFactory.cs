using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Red.Infrastructure.Persistence
{
    internal class LibraryContextDesignTimeFactory : IDesignTimeDbContextFactory<LibraryContext>
    {
        public LibraryContext CreateDbContext(string[] args)
        {
            Console.WriteLine($"CLI ARGUMENTS: [ {string.Join(",", args.Select(x => $"\"{x}\""))} ]");

            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer("use --connection argument when using efcore tools");
            // dotnet ef database update --connection "..."

            return new LibraryContext(optionsBuilder.Options);
        }
    }
}