using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaTaskApp.Models;
using ShaTaskApp.Services.Interfaces;
using ShaTaskApp.ViewModels.Invoice;

namespace ShaTaskApp.Controllers
{
    [Authorize(Roles = "Admin,User")]

    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetInvoicesAsync();
            return View(invoices);
        }

        public async Task<IActionResult> Details(long id)
        {
            var invoice = await _invoiceService.GetInvoiceDetailsAsync(id);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new InvoiceCreateEditViewModel();
            await PopulateViewBags();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _invoiceService.CreateInvoiceAsync(viewModel);
                return RedirectToAction(nameof(Index));
            }

            await PopulateViewBags();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(long id)
        {
            var invoice = await _invoiceService.GetInvoiceDetailsAsync(id);
            if (invoice == null)
                return NotFound();

            var viewModel = new InvoiceCreateEditViewModel
            {
                ID = invoice.ID,
                CustomerName = invoice.CustomerName,
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

            await PopulateViewBags();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InvoiceCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _invoiceService.EditInvoiceAsync(viewModel);
                return RedirectToAction(nameof(Index));
            }

            await PopulateViewBags();
            return View(viewModel);
        }

        //public async Task<IActionResult> Delete(long id)
        //{
        //    var invoice = await _invoiceService.GetInvoiceDetailsAsync(id);
        //    if (invoice == null)
        //        return NotFound();

        //    return View(invoice);
        //}

        
        [HttpGet]

        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _invoiceService.DeleteInvoiceAsync(id);
            return RedirectToAction(nameof(Index), new { deleted = true });
        }


        [HttpGet]
        public async Task<JsonResult> GetCashiersByBranch(int branchId)
        {
            var cashiers = await _invoiceService.GetCashiersByBranchAsync(branchId);
            return Json(cashiers.Select(c => new { c.ID, Name = c.CashierName }));
        }

        private async Task PopulateViewBags()
        {
            ViewBag.Branches = await _invoiceService.GetAllBranchesAsync();
        }

        private async Task<List<Branch>> GetBranchesAsync()
        {
            return await _invoiceService.GetAllBranchesAsync();
        }
    }
}