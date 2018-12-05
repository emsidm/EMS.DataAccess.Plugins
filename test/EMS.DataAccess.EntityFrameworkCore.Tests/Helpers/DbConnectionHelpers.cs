using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EMS.DataAccess.EntityFrameworkCore.Tests.Helpers
{
    internal static class DbConnectionHelpers
    {
        private static void InitializeConnection(out SqliteConnection connection)
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
        }

        public static void SeedContext<TContext>(out SqliteConnection connection)
            where TContext : DbContext
        {
            InitializeConnection(out connection);

            using (var context = (TContext) Activator.CreateInstance(typeof(TContext), connection))
            {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
        }
    }
}