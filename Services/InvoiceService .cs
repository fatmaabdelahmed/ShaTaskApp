using Microsoft.EntityFrameworkCore;
using ShaTaskApp.Models;
using ShaTaskApp.Models.Context;
using ShaTaskApp.Services.Interfaces;
using ShaTaskApp.ViewModels;

namespace ShaTaskApp.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ShaTaskDbContext _context;

        public InvoiceService(ShaTaskDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceHeader>> GetInvoicesAsync()
        {
            return await _context.InvoiceHeaders
                .Include(h => h.Cashier)
                .Include(h => h.Cashier.Branch)
                .ToListAsync();
        }

        public async Task<InvoiceHeader?> GetInvoiceDetailsAsync(long id)
        {
            return await _context.InvoiceHeaders
                .Include(h => h.Cashier)
                .Include(h => h.Cashier.Branch)
                .Include(h => h.InvoiceDetails)
                .FirstOrDefaultAsync(h => h.ID == id);
        }

        public async Task CreateInvoiceAsync(InvoiceCreateEditViewModel vm)
        {
            var header = new InvoiceHeader
            {
                CashierID = vm.CashierID,
                Invoicedate = vm.Invoicedate,
                CustomerName = vm.CustomerName 
            };

            _context.InvoiceHeaders.Add(header);
            await _context.SaveChangesAsync();

            foreach (var detail in vm.InvoiceDetails)
            {
                var invoiceDetail = new InvoiceDetail 
                {
                    InvoiceHeaderID = header.ID, 
                    ItemName = detail.ItemName,
                    ItemCount = detail.ItemCount,
                    ItemPrice = detail.ItemPrice
                };
                _context.InvoiceDetails.Add(invoiceDetail);
            }

            await _context.SaveChangesAsync();
        }

        public async Task EditInvoiceAsync(InvoiceCreateEditViewModel vm)
        {
            var existingHeader = await _context.InvoiceHeaders
                .Include(h => h.InvoiceDetails)
                .FirstOrDefaultAsync(h => h.ID == vm.ID);

            if (existingHeader == null) return;

            existingHeader.CashierID = vm.CashierID;
            existingHeader.Invoicedate = vm.Invoicedate;
            existingHeader.CustomerName = vm.CustomerName; 

            _context.InvoiceDetails.RemoveRange(existingHeader.InvoiceDetails);

            foreach (var detail in vm.InvoiceDetails)
            {
                var invoiceDetail = new InvoiceDetail
                {
                    InvoiceHeaderID = existingHeader.ID,
                    ItemName = detail.ItemName,
                    ItemCount = detail.ItemCount,
                    ItemPrice = detail.ItemPrice
                };
                _context.InvoiceDetails.Add(invoiceDetail);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteInvoiceAsync(long id)
        {
            var invoice = await _context.InvoiceHeaders
                .Include(h => h.InvoiceDetails)
                .FirstOrDefaultAsync(h => h.ID == id);

            if (invoice != null)
            {
                _context.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);
                _context.InvoiceHeaders.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Cashier>> GetCashiersByBranchAsync(int branchId)
        {
            return await _context.Cashiers
                .Where(c => c.BranchID == branchId)
                .ToListAsync();
        }

        public async Task<List<Branch>> GetAllBranchesAsync()
        {
            return await _context.Branches.ToListAsync();
        }
    }
}