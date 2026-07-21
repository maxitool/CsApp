using CsApp.Parsers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsApp.DB.Models
{
    [Table("Values", Schema = "public")]
    public class Values
    {
        [Key]
        public int id { get; set; }
        public int result_id { get; set; }
        public DateTime date { get; set; }
        public int execution_time { get; set; }
        public decimal value { get; set; }

        public Results? result { get; set; }

        public override string ToString()
        {
            return @$"Values(
        id = {id},
        result_id = {result_id},
        date = {date.ToString(DateTimeParser.CUSTOM_DATA_FORMAT)},
        execution_time = {execution_time},
        value = {value}
)";
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(id, result_id, date, execution_time, value);
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;
            Values o = obj as Values;
            if (o.GetHashCode() != GetHashCode()) return false;
            return o.id == id && o.result_id == result_id && o.date.Equals(date)
                && o.execution_time == execution_time && o.value == value;
        }
    }
}
