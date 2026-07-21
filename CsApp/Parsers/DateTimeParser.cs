using System.Globalization;

namespace CsApp.Parsers
{
    public class DateTimeParser
    {
        public static readonly string BASE_DATA_FORMAT = "yyyy-MM-dd HH:mm:ss.FFFF";
        public static readonly string CUSTOM_DATA_FORMAT = "yyyy-MM-dd'T'HH:mm:ss.ffff'Z'";
        public DateTime? StringToDateTime(string data)
        {
            if (DateTime.TryParseExact(data, CUSTOM_DATA_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;
            Console.WriteLine($"Can't convert {data} to {CUSTOM_DATA_FORMAT}  DateTime format.");
            return null;
        }
    }
}
