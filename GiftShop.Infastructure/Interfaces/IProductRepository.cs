using GiftShop.Domain.Entities;

namespace GiftShop.Infastructure.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<int> CreateProductWithProperty(Product product);
}
