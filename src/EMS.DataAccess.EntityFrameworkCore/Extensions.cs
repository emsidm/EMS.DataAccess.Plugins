using System;
using System.Linq;
using EMS.DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.DataAccess.EntityFrameworkCore
{
    public static class Extensions
    {
        public static void AddDataSource<T>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options) where T : DbContext
        {
            services.AddDbContext<T>(options);
            services.AddTransient<IDataSource, EfDataContext<T>>();
        }

        public static void AddDataTarget<T>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options) where T : DbContext
        {
            services.AddDbContext<T>(options);
            services.AddTransient<IDataTarget, EfDataContext<T>>();
        }

        public static void AddDataContext<T>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options) where T : DbContext
        {
            services.AddDbContext<T>(options);
            services.AddTransient<IDataSource, EfDataContext<T>>();
            services.AddTransient<IDataTarget, EfDataContext<T>>();
        }

        public static bool EntityExists<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            context.Entry(entity).Metadata.FindPrimaryKey();

            return context.Entry(entity).IsKeySet &&
                   context.Set<TEntity>()
                       .Find(typeof(TEntity)
                           .GetProperty(context.FindPrimaryKeyName<TEntity>())?.GetValue(entity)) != null;
        }

        private static string FindPrimaryKeyName<TEntity>(this DbContext context)
        {
            return context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey()
                .Properties.Select(x => x.Name).Single();
        }
    }
}