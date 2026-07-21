using CsApp.Parsers;
using RangeFilter = CsApp.DB.Queries.Core.Filters.RangeFilter;

namespace CsApp.FrontFilters
{
    public class MinDateFrontFilter : ParentFrontFilter
    {
        public MinDateFrontFilter(string fromDate, string toDate) : base()
        {
            DateTimeParser dateTimeParser = new();
            DateTime? fromDateTime = dateTimeParser.StringToDateTime(fromDate);
            if (fromDateTime == null)
            {
                Console.WriteLine("Can't convert fromDate string to DateTime format.");
                return;
            }
            DateTime? toDateTime = dateTimeParser.StringToDateTime(toDate);
            if (toDateTime == null)
            {
                Console.WriteLine("Can't convert toDateTime string to DateTime format.");
                return;
            }
            _filter = new RangeFilter("min_date", ((DateTime)fromDateTime).ToString(DateTimeParser.BASE_DATA_FORMAT), ((DateTime)toDateTime).ToString(DateTimeParser.BASE_DATA_FORMAT), typeof(DateTime));
            _isGood = true;
        }
    }
}
