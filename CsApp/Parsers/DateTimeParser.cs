using System.Globalization;

namespace CsApp.Parsers
{
    public class DateTimeParser
    {
        protected readonly string _dataFormat = "yyyy-MM-dd'T'HH:mm:ss.ffff'Z'";
        public DateTime? StringToDateTime(string data)
        {
            if (DateTime.TryParseExact(data, _dataFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;
            // info
            return null;
        }
    }
}
