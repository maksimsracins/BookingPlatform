using BookingPlatform.Core.Interfaces.Repositories;
using BookingPlatform.Infrastructure.Persistence.Context;

namespace BookingPlatform.Infrastructure.Repositories;

public class Repository<TEntity>(BookingDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    protected readonly BookingDbContext Context = context;

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    public Task UpdateAsync(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }
}