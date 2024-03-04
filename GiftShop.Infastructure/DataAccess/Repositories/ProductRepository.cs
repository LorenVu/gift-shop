using GiftShop.Domain.Entities;
using GiftShop.Infastructure.Data;
using GiftShop.Infastructure.Interfaces;

namespace GiftShop.Infastructure.DataAccess.Repositories;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
}
