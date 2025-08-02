using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShaTaskApp.Models
{
    public class InvoiceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        public long InvoiceHeaderID { get; set; }

        [Required]
        [MaxLength(200)]
        public string ItemName { get; set; } = "";

        [Required]
        [Column(TypeName = "float")]
        public double ItemCount { get; set; } = 0;

        [Required]
        [Column(TypeName = "float")]
        public double ItemPrice { get; set; } = 0;

        [ForeignKey("InvoiceHeaderID")]
        public virtual InvoiceHeader InvoiceHeader { get; set; }

        [NotMapped]
        public double TotalPrice => ItemCount * ItemPrice;
    }
}
