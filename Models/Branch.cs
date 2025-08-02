using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShaTaskApp.Models
{
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string BranchName { get; set; } = "";

        [Required]
        public int CityID { get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        public virtual ICollection<Cashier> Cashiers { get; set; } = new List<Cashier>();
        public virtual ICollection<InvoiceHeader> InvoiceHeaders { get; set; } = new List<InvoiceHeader>();

    }
}
