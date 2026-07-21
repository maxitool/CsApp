using System.Text;

namespace CsApp.DB.Queries.Core.Filters
{
    public class ValueFilter : ParentFilter
    {
        private string _key, _value;
        private Type _type;

        public ValueFilter(string key, string value, Type type) : base()
        {
            this._key = StrProcessing(key);
            this._value = StrProcessing(value);
            this._type = type;
        }

        protected override StringBuilder AddFilter(StringBuilder stringBuilder)
        {
            stringBuilder.Append(_key).Append('=');
            return ValueScreening(stringBuilder, _value, _type);
        }
    }
}
