using GiftShop.Domain.Entities;
using GiftShop.Infastructure.Data;
using GiftShop.Infastructure.Interfaces;

namespace GiftShop.Infastructure.DataAccess.Repositories;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
}
