using GiftShop.Application.Constrants.Requests;

namespace GiftShop.Application.Interfaces;

public interface ISendMailService
{
    Task SendEmailAsync(MailContent mailContent);
}
