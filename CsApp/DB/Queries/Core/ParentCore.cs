using CsApp.DB;

namespace CsApp.DB.Queries.Core
{
    public abstract class ParentCore
    {
        public static readonly char SPACE = ' ';
        public static readonly char SCREENING_CHAR = '\'';

        public static string StrProcessing(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            return data.Replace("\"", "").Replace("\'", "").Trim();
        }
    }
}
