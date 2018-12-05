using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;
using static EMS.DataAccess.Abstractions.ProvisioningState;

namespace EMS.DataAccess.EntityFrameworkCore
{
    public class EfDataContext<TContext> : IDataContext where TContext : DbContext
    {
        private readonly TContext _context;

        public EfDataContext(TContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Entities<TEntity>() where TEntity : class => _context.Set<TEntity>();
        public string Name { get; set; }

        public async Task<IProvisioningStatus<TEntity>> ProvisionAsync<TEntity>(TEntity entity)
            where TEntity : class => _context.EntityExists(entity)
            ? await UpdateEntity(entity)
            : await CreateEntity(entity);

        private async Task<IProvisioningStatus<TEntity>> UpdateEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return new ProvisioningStatus<TEntity>(Updated, entity);
        }

        private async Task<ProvisioningStatus<TEntity>> CreateEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return new ProvisioningStatus<TEntity>(Created, entity);
        }

        public async Task<IProvisioningStatus<TEntity>> BulkProvisionAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            var entitiesList = entities.ToList();
            try
            {
                await _context.Set<TEntity>().AddRangeAsync(entitiesList);
                await _context.SaveChangesAsync();
                return new ProvisioningStatus<TEntity>(Created, entitiesList);
            }
            catch (Exception e)
            {
                return new ProvisioningStatus<TEntity>(Error, entitiesList, e.Message);
            }
        }

        public async Task<IProvisioningStatus<TEntity>> DeprovisionAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return new ProvisioningStatus<TEntity>(Deleted, entity);
        }

        public async Task<IProvisioningStatus<TEntity>> BulkDeprovisionAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            var entitiesList = entities.ToList();
            try
            {
                _context.Set<TEntity>().RemoveRange(entitiesList);
                await _context.SaveChangesAsync();

                return new ProvisioningStatus<TEntity>(Deleted, entitiesList);
            }
            catch (Exception e)
            {
                return new ProvisioningStatus<TEntity>(Error, entitiesList, e.Message);
            }
        }

        public async Task<IProvisioningStatus<TEntity>> GetProvisioningStatusAsync<TEntity>(TEntity entity)
            where TEntity : class => _context.EntityExists(entity)
            ? new ProvisioningStatus<TEntity>(Unmodified, entity)
            : new ProvisioningStatus<TEntity>(Inexistent, entity);

        public void Dispose() => _context.Dispose();
    }
}