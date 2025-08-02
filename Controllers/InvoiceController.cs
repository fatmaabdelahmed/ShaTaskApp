using Microsoft.AspNetCore.Mvc;
using ShaTaskApp.Models;
using ShaTaskApp.Services.Interfaces;
using ShaTaskApp.ViewModels;

namespace ShaTaskApp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: /Invoice
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetInvoicesAsync();
            return View(invoices);
        }

        // GET: /Invoice/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var invoice = await _invoiceService.GetInvoiceDetailsAsync(id);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        // GET: /Invoice/Create
        public IActionResult Create()
        {
            var viewModel = new InvoiceCreateEditViewModel();
            return View(viewModel);
        }

        // POST: /Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _invoiceService.CreateInvoiceAsync(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: /Invoice/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var invoice = await _invoiceService.GetInvoiceDetailsAsync(id);
            if (invoice == null)
                return NotFound();

            // Convert to ViewModel
            var viewModel = new InvoiceCreateEditViewModel
            {
                ID = invoice.ID,
                CashierID = invoice.CashierID,
                Invoicedate = invoice.Invoicedate,
                BranchID = invoice.Cashier?.BranchID ?? 0,
                InvoiceDetails = invoice.InvoiceDetails?.Select(d => new InvoiceDetailsViewModel
                {
                    ID = d.ID,
                    ItemName = d.ItemName,
                    ItemCount = d.ItemCount,
                    ItemPrice = d.ItemPrice
                }).ToList() ?? new List<InvoiceDetailsViewModel>()
            };

            return View(viewModel);
        }

        // POST: /Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InvoiceCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _invoiceService.EditInvoiceAsync(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: /Invoice/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var invoice = await _invoiceService.GetInvoiceDetailsAsync(id);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        // POST: /Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _invoiceService.DeleteInvoiceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // AJAX method to get cashiers by branch
        [HttpGet]
        public async Task<JsonResult> GetCashiersByBranch(int branchId)
        {
            var cashiers = await _invoiceService.GetCashiersByBranchAsync(branchId);
            return Json(cashiers.Select(c => new { c.ID, c.CashierName }));
        }
    }
}