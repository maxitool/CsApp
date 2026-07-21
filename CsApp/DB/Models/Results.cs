using CsApp.Parsers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsApp.DB.Models
{
    [Index(nameof(filename), IsUnique = true)]
    [Table("Results", Schema = "public")]
    public class Results
    {
        [Key]
        public int id { get; set; }
        public string filename { get; set; }
        public TimeSpan delta_date { get; set; } = new();
        public DateTime min_date { get; set; } = new();
        public decimal avg_execution_time { get; set; }
        public decimal avg_value { get; set; }
        public decimal median_value { get; set; }
        public decimal max_value { get; set; }
        public decimal min_value { get; set; }
        public List<Values> values { get; set; } = new();

        public override string ToString()
        {
            return @$"Results(
        id = {id},
        filename = {filename},
        delta_date = {delta_date.ToString()},
        min_date = {min_date.ToString(DateTimeParser.CUSTOM_DATA_FORMAT)},
        avg_execution_time = {avg_execution_time},
        avg_value = {avg_value},
        median_value = {median_value},
        max_value = {max_value},
        min_value = {min_value}
)";
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(id, filename, delta_date, avg_execution_time, avg_value, median_value,
                max_value, min_value);
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;
            Results o = obj as Results;
            if (o.GetHashCode() != GetHashCode()) return false;
            return o.id == id && o.filename.Equals(filename) &&
                o.delta_date.Equals(delta_date) && o.min_date.Equals(min_date) &&
                o.avg_execution_time == avg_execution_time && o.avg_value == avg_value &&
                o.median_value == median_value && o.max_value == max_value && o.min_value == min_value;
        }
    }
}
