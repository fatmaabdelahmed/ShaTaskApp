using System.ComponentModel.DataAnnotations;

namespace ShaTaskApp.ViewModels.Invoice
{
    public class InvoiceCreateEditViewModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "اسم العميل مطلوب")]
        [StringLength(200, ErrorMessage = "اسم العميل لا يجب أن يزيد عن 200 حرف")]
        [Display(Name = "اسم العميل")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاريخ الفاتورة مطلوب")]
        [Display(Name = "تاريخ الفاتورة")]
        [DataType(DataType.Date)]
        public DateTime Invoicedate { get; set; } = DateTime.Now;

        [Display(Name = "الكاشير")]
        public int? CashierID { get; set; }

        [Required(ErrorMessage = "الفرع مطلوب")]
        [Display(Name = "الفرع")]
        public int BranchID { get; set; }

        [Display(Name = "أصناف الفاتورة")]
        public List<InvoiceDetailsViewModel> InvoiceDetails { get; set; } = new List<InvoiceDetailsViewModel>();

        public double TotalPrice => InvoiceDetails?.Sum(d => d.LineTotal) ?? 0;
    }

    public class InvoiceDetailsViewModel
    {
        public long ID { get; set; }

        [Display(Name = "اسم الصنف")]
        [StringLength(200, ErrorMessage = "اسم الصنف لا يجب أن يزيد عن 200 حرف")]
        public string ItemName { get; set; } = string.Empty;

        [Display(Name = "الكمية")]
        [Range(0.01, double.MaxValue, ErrorMessage = "الكمية يجب أن تكون أكبر من صفر")]
        public double ItemCount { get; set; } = 1;

        [Display(Name = "السعر")]
        [Range(0.01, double.MaxValue, ErrorMessage = "السعر يجب أن يكون أكبر من صفر")]
        public double ItemPrice { get; set; }

        public double LineTotal => ItemCount * ItemPrice;
    }
}

