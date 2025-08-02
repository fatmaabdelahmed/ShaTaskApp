using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShaTaskApp.Models
{
    public class InvoiceHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string CustomerName { get; set; } = "";

        [Required]
        public DateTime Invoicedate { get; set; } = DateTime.Now;

        public int? CashierID { get; set; }

        [Required]
        public int BranchID { get; set; }

        [ForeignKey("CashierID")]
        public virtual Cashier Cashier { get; set; }

        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    }
}
