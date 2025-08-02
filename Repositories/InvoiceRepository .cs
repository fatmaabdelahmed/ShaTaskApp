using Microsoft.EntityFrameworkCore;
using ShaTaskApp.Models;
using ShaTaskApp.Models.Context;
using ShaTaskApp.Repositories.Interfaces;

namespace ShaTaskApp.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ShaTaskDbContext _context;

        public InvoiceRepository(ShaTaskDbContext context)
        {
            _context = context;
        }

        public List<InvoiceHeader> GetAll()
        {
            return _context.InvoiceHeaders
                           .Include(i => i.Cashier)
                           .Include(i => i.InvoiceDetails)
                           .ToList();
        }

        public InvoiceHeader? GetById(long id)
        {
            return _context.InvoiceHeaders
                           .Include(i => i.Cashier)
                           .Include(i => i.InvoiceDetails)
                           .FirstOrDefault(i => i.ID == id);
        }

        public void Add(InvoiceHeader invoice)
        {
            _context.InvoiceHeaders.Add(invoice);
            _context.SaveChanges();
        }

        public void Update(InvoiceHeader invoice)
        {
            _context.InvoiceHeaders.Update(invoice);
            _context.SaveChanges();
        }

        public void Delete(InvoiceHeader invoice)
        {
            _context.InvoiceHeaders.Remove(invoice);
            _context.SaveChanges();
        }
    }
    
}
