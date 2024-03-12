using GiftShop.Domain.Entities;
using GiftShop.Infastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.Infastructure.DataAccess.Repositories;

public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
{
    public UserRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public async Task<ApplicationUser> GetUserInfo(string id)
        => await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
}
