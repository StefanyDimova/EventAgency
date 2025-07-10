using EventAgency.Data.Repository.Interfaces;
using EventAgencyFinalProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using static EventAgency.GCommon.ApplicationConstants;
using static EventAgency.Data.Common.ExceptionMessages;

namespace EventAgency.Data.Repository
{
    public abstract class BaseRepository<TEntity, TKey>
        : IRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly EventAgencyDbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        protected BaseRepository(EventAgencyDbContext dbContext)
        {
            this.dbContext = dbContext;
            // метод , който връща dbSet-a отговарящ на подаденото Entity
            this.dbSet = this.dbContext.Set<TEntity>();
        }

        public TEntity? GetById(TKey id)
        {
            return this.dbSet
                .Find(id);
        }
        public ValueTask<TEntity?> GetByIdAsync(TKey id)
        {
            return this.dbSet
                .FindAsync(id);
        }

        public TEntity? SingleOrDefault(Func<TEntity, bool> predicate)
        {
            return this.dbSet
                .SingleOrDefault(predicate);
        }

        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet
                .SingleOrDefaultAsync(predicate);
        }
        public TEntity? FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return this.dbSet
                .FirstOrDefault(predicate);
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet
                .FirstOrDefaultAsync(predicate);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return this.dbSet.ToArray();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            TEntity[] entities = await this.dbSet
                .ToArrayAsync();

            return entities;
        }

        public IQueryable<TEntity> GetAllAttached()
        {
            return this.dbSet.AsQueryable();
        }
        public void Add(TEntity item)
        {
            this.dbSet.Add(item);
            this.dbContext.SaveChanges();
        }

        public async Task AddAsync(TEntity item)
        {
            await this.dbSet.AddAsync(item);
            await this.dbContext.SaveChangesAsync();
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            this.dbSet.AddRange(items);
            this.dbContext.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await this.dbSet.AddRangeAsync(items);
            await this.dbContext.SaveChangesAsync();
        }

        public bool Delete(TEntity entity)
        {
            this.PerformSoftDeleteOfEntity(entity);
            return this.Update(entity);
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            this.PerformSoftDeleteOfEntity(entity);
            return this.UpdateAsync(entity);
        }

        public bool HardDelete(TEntity entity)
        {
            this.dbSet.Remove(entity);
            int updatedCount = this.dbContext.SaveChanges();
            return updatedCount > 0;
        }
        public async Task<bool> HardDeleteAsync(TEntity entity)
        {
            this.dbSet.Remove(entity);
            int updatedCount = await this.dbContext.SaveChangesAsync();
            return updatedCount > 0;
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync();
        }

        public bool Update(TEntity item)
        {
            try
            {
                this.dbSet.Attach(item);
                this.dbSet.Entry(item).State = EntityState.Modified;
                this.dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TEntity item)
        {
            try
            {
                this.dbSet.Attach(item);
                this.dbSet.Entry(item).State = EntityState.Modified;
                await this.dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void PerformSoftDeleteOfEntity(TEntity entity)
        {
            PropertyInfo? isDeletedProperty =
                this.GetIsDeletedProperty(entity);

            if (isDeletedProperty == null)
            {
                throw new InvalidOperationException(SoftDeleteOnNonSoftDeletableEntity);
            }

            isDeletedProperty.SetValue(entity, true);
        }
        private PropertyInfo? GetIsDeletedProperty(TEntity entity)
        {
            return typeof(TEntity)
                .GetProperties()
                .FirstOrDefault(pi => pi.PropertyType == typeof(bool) &&
                                pi.Name == IsDeletedPropertyName);
        }

        public int Count()
        {
            return this.dbSet.Count();
        }

        public Task<int> CountAsync()
        {
            return this.dbSet.CountAsync();
        }
    }
}
