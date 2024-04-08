using GiftShop.Application.Constrants.Responses;

namespace GiftShop.Application.Interfaces;

public interface IUserService
{
    Task<BaseResponse> GetUserInfo(string id);
}
