using ShaTaskApp.Models;

namespace ShaTaskApp.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        List<InvoiceHeader> GetAll();
        InvoiceHeader? GetById(long id);
        void Add(InvoiceHeader invoice);
        void Update(InvoiceHeader invoice);
        void Delete(InvoiceHeader invoice);
    }
}
