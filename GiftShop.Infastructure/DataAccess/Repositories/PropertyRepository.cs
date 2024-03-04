using GiftShop.Domain.Entities;
using GiftShop.Infastructure.Data;
using GiftShop.Infastructure.Interfaces;

namespace GiftShop.Infastructure.DataAccess.Repositories;

public class PropertyRepository : RepositoryBase<Property>, IPropertyRepository
{
    public PropertyRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
}
