namespace GiftShop.Domain.Commons.Extentions.DateTimes;

public static class DateTimeExtension
{
    public static DateTime ToDateTime(this string input, string format)
    {
        DateTime date;
        try
        {
            return DateTime.ParseExact(input, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception|DateTimeExtension|ToDateTime|Error: ${ex.Message}");

            return DateTime.Now;
        }
    }
}
