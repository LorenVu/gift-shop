using GiftShop.Infastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GiftShop.Infastructure.DataAccess.Repositories;

public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    protected RepositoryBase(ApplicationDbContext dataContext)
    {
        _dbContext = dataContext;
        _dbSet = _dbContext.Set<T>();
    }

    public void Add(T entity)
    => _dbSet.Add(entity);

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities)
        => await _dbSet.AddRangeAsync(entities);

    public void AddRange(IEnumerable<T> entities)
    => _dbSet.AddRange(entities);

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
        _dbContext.Entry(entities).State = EntityState.Modified;
    }

    public void Remove(T entity)
        => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities)
        => _dbSet.RemoveRange(entities);

    public virtual T? FindById(Guid id)
        => _dbSet.Find(id);

    public async virtual Task<T?> FindByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public virtual T? GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
    {
        if (includes != null && includes.Count() > 0)
        {
            var query = _dbContext.Set<T>().Include(includes.First());
            foreach (var include in includes.Skip(1))
                query = query.Include(include);
            return query.FirstOrDefault(expression);
        }
        return _dbContext.Set<T>().FirstOrDefault(expression);
    }

    public void SaveChanges()
        => _dbContext.SaveChanges();

    public async Task<int> SaveAsync()
        => await _dbContext.SaveChangesAsync();

    public IQueryable<T> GetAll()
        => _dbContext.Set<T>().AsQueryable();

    public IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> predicate) 
        => _dbSet.Where(predicate);

}
