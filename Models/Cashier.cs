using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShaTaskApp.Models
{
    public class Cashier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string CashierName { get; set; } = "";

        [Required]
        public int BranchID { get; set; }

        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<InvoiceHeader> InvoiceHeaders { get; set; } = new List<InvoiceHeader>();

    }
}
