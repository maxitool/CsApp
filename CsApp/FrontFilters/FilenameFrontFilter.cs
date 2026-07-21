using ValueFilter = CsApp.DB.Queries.Core.Filters.ValueFilter;

namespace CsApp.FrontFilters
{
    public class FilenameFrontFilter : ParentFrontFilter
    {
        public FilenameFrontFilter(string filename) : base(new ValueFilter("filename", filename, typeof(string))) 
        {
            _isGood = true;
        }
    }
}