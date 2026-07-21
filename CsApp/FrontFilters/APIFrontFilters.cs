using CsApp.DB.Queries.Core.Filters;

namespace CsApp.FrontFilters
{
    public class APIFrontFilters
    {
        public string WorkFilters { get 
            {
                if (_countFilters <= 0)
                    return "No used filters.";
                return _workFilters;  
            } }
        public bool HaveFilenameFilter { get; set; } //= false;
        public string? Filename { get; set; } //= ".csv";
        public bool HaveMinDateFilter { get; set; } //= false;
        public string? FromDate { get; set; } //= "2025-07-15T14:30:45.1234Z";
        public string? ToDate { get; set; } //= "2025-07-15T14:30:45.1234Z";
        public bool HaveAvgValueFilter { get; set; } //= false;
        public string? FromAvgValue { get; set; } //= "230";
        public string? ToAvgValue { get; set; } //= "235";
        public bool HaveAvgExecutionFilter { get; set; } //= false;
        public string? FromAvgExecution { get; set; } //= "213123122.1";
        public string? ToAvgExecution { get; set; } //= "213123123.2";
        protected string _workFilters = "Used filters:\n";
        protected int _countFilters = 0;

        public ParentFilter GetFilter()
        {
            List<ParentFilter> filters = new(); ParentFrontFilter frontFilter;
            if (HaveFilenameFilter && Filename != null)
            {
                filters.Add(new FilenameFrontFilter(Filename).Filer);
                _workFilters += "  FilenameFilter\n";
                _countFilters += 1;
            }
            if (HaveMinDateFilter && FromDate != null && ToDate != null)
            {
                frontFilter = new MinDateFrontFilter(FromDate, ToDate);
                if (frontFilter.IsGood)
                    filters.Add(frontFilter.GetFilter());
                _workFilters += "  MinDateFilter\n";
                _countFilters += 1;
            }
            if (HaveAvgValueFilter && FromAvgValue != null && ToAvgValue != null)
            {
                frontFilter = new AvgValueFrontFilter(FromAvgValue, ToAvgValue);
                if (frontFilter.IsGood)
                    filters.Add(frontFilter.GetFilter());
                _workFilters += "  AvgValueFilter\n";
                _countFilters += 1;
            }
            if (HaveAvgExecutionFilter && FromAvgExecution != null && ToAvgExecution != null)
            {
                frontFilter = new AvgExecutionTimeFrontFilter(FromAvgExecution, ToAvgExecution);
                if (frontFilter.IsGood)
                    filters.Add(frontFilter.GetFilter());
                _workFilters += "  AvgExecutionFilter\n";
                _countFilters += 1;
            }
            if (filters.Count <= 0)
                return null;
            if (filters.Count <= 1)
                return filters[0];
            for (int i = filters.Count - 1; i > 0; i--)
                filters[i - 1].SetNextFilter(filters[i]);
            return filters[0];
        }
    }
}
