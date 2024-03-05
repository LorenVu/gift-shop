using System.Linq.Expressions;

namespace GiftShop.Infastructure.Interfaces;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    T? FindById(Guid id);
    Task<T?> FindByIdAsync(Guid id);
    T? GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);
    void SaveChanges();
    Task<int> SaveAsync();
    IQueryable<T> GetAll();
    IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> predicate);
}
