using ShaTaskApp.Models;
using ShaTaskApp.ViewModels.Invoice;

namespace ShaTaskApp.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<List<InvoiceHeader>> GetInvoicesAsync();
        Task<InvoiceHeader?> GetInvoiceDetailsAsync(long id);
        Task CreateInvoiceAsync(InvoiceCreateEditViewModel viewModel);
        Task EditInvoiceAsync(InvoiceCreateEditViewModel viewModel);
        Task DeleteInvoiceAsync(long id);
        Task<List<Cashier>> GetCashiersByBranchAsync(int branchId);
        Task<List<Branch>> GetAllBranchesAsync();
    }
}