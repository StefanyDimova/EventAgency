using System.Linq.Expressions;

namespace EventAgency.Data.Repository.Interfaces
{
    public interface IAsyncRepository<TEntity, TKey>
    {
        ValueTask<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task AddAsync(TEntity item);

        Task<int> CountAsync();
        Task AddRangeAsync(IEnumerable<TEntity> items);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> HardDeleteAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity item);
        Task SaveChangesAsync();
    }
}
