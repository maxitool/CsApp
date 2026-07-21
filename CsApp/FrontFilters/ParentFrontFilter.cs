using ParentFilter = CsApp.DB.Queries.Core.Filters.ParentFilter;

namespace CsApp.FrontFilters
{
    public abstract class ParentFrontFilter
    {
        public bool IsGood { get { return _isGood; } }
        public ParentFilter? Filer { get { return _filter; } }
        protected ParentFilter? _filter;
        protected bool _isGood;

        public ParentFrontFilter() { }
        public ParentFrontFilter(ParentFilter filter)
        {
            this._filter = filter;
        }
        public ParentFilter GetFilter() { return _filter; }
    }
}
