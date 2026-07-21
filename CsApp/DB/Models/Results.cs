using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsApp.DB.Models
{
    [Table("Results", Schema = "public")]
    public class Results
    {
        [Key]
        public int id { get; set; }
        public int id_file { get; set; }
        public TimeSpan delta_date { get; set; }
        public DateTime min_date { get; set; }
        public decimal avg_execution_time { get; set; }
        public decimal avg_value { get; set; }
        public decimal median_value { get; set; }
        public decimal max_value { get; set; }
        public decimal min_value { get; set; }
    }
}
