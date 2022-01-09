using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories;
public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> Get(int id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
    Task Add(TEntity entity);
    void Remove(TEntity entity);
}