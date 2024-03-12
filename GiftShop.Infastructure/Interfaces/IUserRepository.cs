using GiftShop.Domain.Entities;

namespace GiftShop.Infastructure.Interfaces;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<ApplicationUser> GetUserInfo(string id);
}
