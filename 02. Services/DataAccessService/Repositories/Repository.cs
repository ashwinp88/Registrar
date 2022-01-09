using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessService.Repositories;

internal class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private readonly RegistrarDbContext _dbContext;
    public Repository(RegistrarDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(TEntity entity)
    {
       await _dbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
    {
       return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task<TEntity?> Get(int id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public void Remove(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }
}

