using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsApp.DB.Models
{
    [Table("Values", Schema = "public")]
    public class Values
    {
        [Key]
        public int id { get; set; }
        public int id_file { get; set; }
        public DateTime date { get; set; }
        public int execution_time { get; set; }
        public decimal value { get; set; }
    }
}
