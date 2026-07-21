using CsApp.DB.Models;
using System.Text;

namespace CsApp.DB.Queries.Core.Filters
{
    public class RangeFilter : ParentFilter
    {
        private string _key, _valueFrom, _valueTo;
        private Type _type;

        public RangeFilter(string key, string valueFrom, string valueTo, Type type) : base()
        {
            this._key = StrProcessing(key);
            this._valueFrom = StrProcessing(valueFrom);
            this._valueTo = StrProcessing(valueTo);
            this._type = type;
        }

        protected override StringBuilder AddFilter(StringBuilder stringBuilder)
        {
            stringBuilder.Append(_key).Append("BETWEEN");
            stringBuilder = ValueScreening(stringBuilder, _valueFrom, _type).Append(" AND ");
            return ValueScreening(stringBuilder, _valueTo, _type);
        }
    }
}
