using System.Globalization;
using Values = CsApp.DB.Models.Values;

namespace CsApp.Validators
{
    public class ValueValidator
    {
        public static readonly DateTime MIN_DATETIME = DateTime.ParseExact("01.01.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture);
        public static bool ValidateValue(Values value)
        {
            if (value == null ||
                value.date < MIN_DATETIME || value.date > DateTime.Now || // maybe DateTime.UtcNow 
                value.execution_time < 0 ||
                value.value < 0) 
                return false;
            return true;
        }
    }
}
