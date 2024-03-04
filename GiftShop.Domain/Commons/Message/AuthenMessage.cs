namespace GiftShop.Domain.Common.Message;

public class AuthenMessage
{
    public static readonly string INPUT_INVALID = "Input invalid";

    public static readonly string DEVICE_ID_CANNOT_BLANK = "Device ID cannot be blank";

    public static readonly string EMAIL_CANNOT_BLANK = "Email cannot be blank";
    public static readonly string EMAIL_ISVALID = "Email is valid";

    public static readonly string PASSWORD_CANNOT_BLANK = "Password cannot be blank";
    public static readonly string PASSWORD_NOT_MATCH = "Repassword not match";
    public static readonly string PASSWORD_ISVALID = "Password is valid";

    public static readonly string NOT_EXIST_ACCOUNT = "Not exist account with email above"; 

    public static readonly string TOKEN_CANNOT_BLANK = "Token cannot be blank";
    public static readonly string TOKEN_INVALID = "Token is valid"; 
}
