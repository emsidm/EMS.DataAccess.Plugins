using EMS.DataAccess.EntityFrameworkCore.Tests.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EMS.DataAccess.EntityFrameworkCore.Tests.Contexts
{
    public class SourceDbContext : DbContext
    {
        public SourceDbContext(SqliteConnection connection) : this(
            new DbContextOptionsBuilder<SourceDbContext>()
                .UseSqlite(connection).Options)
        {
        }

        public SourceDbContext(DbContextOptions<SourceDbContext> options) : base(options)
        {
        }

        public DbSet<SourceUser> Users { get; set; }
    }
}