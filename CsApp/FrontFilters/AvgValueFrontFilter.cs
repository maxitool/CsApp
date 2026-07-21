using RangeFilter = CsApp.DB.Queries.Core.Filters.RangeFilter;

namespace CsApp.FrontFilters
{
    public class AvgValueFrontFilter : ParentFrontFilter
    {
        public AvgValueFrontFilter(string fromAvgValue, string toAvgValue) : base()
        {
            if (!decimal.TryParse(fromAvgValue, out decimal from))
                return;
            if (!decimal.TryParse(toAvgValue, out decimal to))
                return;
            if (from < 0 || to < 0)
            {
                Console.WriteLine($"fromAvgValue < 0 or toAvgValue < 0.");
                return;
            }
            _filter = new RangeFilter("avg_value", fromAvgValue, toAvgValue, typeof(decimal));
            _isGood = true;
        }
    }
}
