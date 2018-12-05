using EMS.DataAccess.EntityFrameworkCore.Tests.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EMS.DataAccess.EntityFrameworkCore.Tests.Contexts
{
    public class DestinationDbContext : DbContext
    {
        public DestinationDbContext(SqliteConnection connection) : this(
            new DbContextOptionsBuilder<DestinationDbContext>()
                .UseSqlite(connection).Options)
        {
        }

        public DestinationDbContext(DbContextOptions<DestinationDbContext> options) : base(options)
        {
        }

        public DbSet<DestinationUser> Users { get; set; }
    }
}