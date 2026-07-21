using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsApp.DB.Models
{
    [Table("Files", Schema = "public")]
    public class Files
    {
        [Key]
        public int id { get; set; }
        public string filename { get; set; }
    }
}
