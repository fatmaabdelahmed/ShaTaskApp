
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShaTaskApp.Models;
using ShaTaskApp.Models.Context;

namespace ShaTask.Controllers
{
    [Authorize]
    public class CashierController : Controller
    {
        private readonly ShaTaskDbContext _context;

        public CashierController(ShaTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cashiers = await _context.Cashiers
                .Include(c => c.Branch)
                .ThenInclude(b => b.City)
                .ToListAsync();
            return View(cashiers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers
                .Include(c => c.Branch)
                .ThenInclude(b => b.City)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        public IActionResult Create()
        {
            ViewData["BranchID"] = new SelectList(_context.Branches.Include(b => b.City), "ID", "BranchName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CashierName,BranchID")] Cashier cashier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cashier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchID"] = new SelectList(_context.Branches.Include(b => b.City), "ID", "BranchName", cashier.BranchID);
            return View(cashier);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return NotFound();
            }
            ViewData["BranchID"] = new SelectList(_context.Branches.Include(b => b.City), "ID", "BranchName", cashier.BranchID);
            return View(cashier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CashierName,BranchID")] Cashier cashier)
        {
            if (id != cashier.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cashier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashierExists(cashier.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchID"] = new SelectList(_context.Branches.Include(b => b.City), "ID", "BranchName", cashier.BranchID);
            return View(cashier);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers
                .Include(c => c.Branch)
                .ThenInclude(b => b.City)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier != null)
            {
                _context.Cashiers.Remove(cashier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CashierExists(int id)
        {
            return _context.Cashiers.Any(e => e.ID == id);
        }
    }
}