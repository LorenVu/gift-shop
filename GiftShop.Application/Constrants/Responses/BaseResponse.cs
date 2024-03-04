using System.Text.Json.Serialization;

namespace GiftShop.Application.Constrants.Responses;

public class BaseResponse
{
    public BaseResponse() { }

    public BaseResponse(int status, bool error, string message, object data, int errorCode)
    {
        Status = status;
        Error = error;
        Message = message;
        Data = data;
        ErrorCode = errorCode;
    }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("errorCode")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("error")]
    public bool Error { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public object Data { get; set; } = new object();
}
