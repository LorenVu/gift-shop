using GiftShop.Domain.Entities;
using GiftShop.Infastructure.Data;
using GiftShop.Infastructure.Interfaces;

namespace GiftShop.Infastructure.DataAccess.Repositories;

public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
{
    public BrandRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
}
