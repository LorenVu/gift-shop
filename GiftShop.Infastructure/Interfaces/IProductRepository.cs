using GiftShop.Domain.Entities;
using System.Linq.Expressions;

namespace GiftShop.Infastructure.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> FindProductByIDWithProperty(Guid id);
    IQueryable<Product> GetProductWithProperty(Expression<Func<Product, bool>> predicate);
    Task<int> CreateProduct(Product product);
    Task<int> UpdateProduct(Product product);
}
