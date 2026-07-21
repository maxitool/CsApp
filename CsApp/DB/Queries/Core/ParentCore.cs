using CsApp.DB;

namespace CsApp.DB.Queries.Core
{
    public abstract class ParentCore
    {
        public static readonly char SPACE = ' ';
        public static readonly char SCREENING_CHAR = '\'';
        public static readonly string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.FFFF";

        public static string StrProcessing(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            return data.Replace("\"", "").Replace("\'", "").Trim();
        }
    }
}
