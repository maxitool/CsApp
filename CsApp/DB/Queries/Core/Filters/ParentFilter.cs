using System.Text;

namespace CsApp.DB.Queries.Core.Filters
{
    public abstract class ParentFilter : ParentCore
    {
        // Chain of Responsibility pattern
        public enum Operation
        {
            AND,
            OR
        }
        public ParentFilter? NextFilter { get { return _nextFilter; } }
        public Operation BetweenOperation { get { return _betweenOperation; } }
        protected ParentFilter? _nextFilter;
        protected Operation _betweenOperation = Operation.AND;

        public ParentFilter SetNextFilter(ParentFilter filter) 
        { 
            this._nextFilter = filter;
            return this;
        }
        public ParentFilter SetOperationWithFilter(Operation betweenOperation) 
        { 
            this._betweenOperation = betweenOperation;
            return this;
        }

        protected StringBuilder Build(StringBuilder stringBuilder)
        {
            if (_nextFilter == null)
            {
                stringBuilder.Append('(');
                return AddFilter(stringBuilder).Append(')');
            }
            stringBuilder = _nextFilter.Build(stringBuilder).Append(' ').Append(_betweenOperation).Append(" (");
            stringBuilder = AddFilter(stringBuilder).Append(')');
            return stringBuilder;
        }
        public string Build()
        {
            StringBuilder stringBuilder = new();
            return Build(stringBuilder).ToString();
        }

        protected StringBuilder ValueScreening(StringBuilder stringBuilder,string value, Type type)
        {
            switch (type)
            {
                case Type t when (t == typeof(string) || t == typeof(DateTime)):
                    return stringBuilder.Append(SCREENING_CHAR + value + SCREENING_CHAR);
                default:
                    return stringBuilder.Append(value);
            }
        }

        protected abstract StringBuilder AddFilter(StringBuilder stringBuilder);

    }
}
