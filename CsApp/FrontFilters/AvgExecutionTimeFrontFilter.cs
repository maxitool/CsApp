using RangeFilter = CsApp.DB.Queries.Core.Filters.RangeFilter;

namespace CsApp.FrontFilters
{
    public class AvgExecutionTimeFrontFilter : ParentFrontFilter
    {
        public AvgExecutionTimeFrontFilter(string fromAvgExecutionTime, string toAvgExecutionTime) : base()
        {
            if (!decimal.TryParse(fromAvgExecutionTime, out decimal from))
                return;
            if (!decimal.TryParse(toAvgExecutionTime, out decimal to))
                return;
            if (from < 0 || to < 0)
            {
                Console.WriteLine($"fromAvgExecutionTime < 0 or toAvgExecutionTime < 0.");
                return;
            }
            _filter = new RangeFilter("avg_execution_time", fromAvgExecutionTime, toAvgExecutionTime, typeof(decimal));
            _isGood = true;
        }
    }
}
