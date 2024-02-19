using System.Security.Cryptography;

namespace GiftShop.Domain.Commons.Extentions.Security;

public static class SecurityExtensions
{
    public static string MD5Hash(string input)
    {
        // Use input string to calculate MD5 hash
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}
