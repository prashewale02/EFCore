#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCore.InventoryModels;
using EFCore.WebUI.Data;

namespace EFCore.WebUI.Controllers
{
    public class CategoryDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategoryDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CategoryDetails.Include(c => c.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CategoryDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryDetails = await _context.CategoryDetails
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryDetails == null)
            {
                return NotFound();
            }

            return View(categoryDetails);
        }

        // GET: CategoryDetails/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Categories, "Id", "CreatedByUserId");
            return View();
        }

        // POST: CategoryDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ColorValue,ColorName")] CategoryDetails categoryDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Categories, "Id", "CreatedByUserId", categoryDetails.Id);
            return View(categoryDetails);
        }

        // GET: CategoryDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryDetails = await _context.CategoryDetails.FindAsync(id);
            if (categoryDetails == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Categories, "Id", "CreatedByUserId", categoryDetails.Id);
            return View(categoryDetails);
        }

        // POST: CategoryDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ColorValue,ColorName")] CategoryDetails categoryDetails)
        {
            if (id != categoryDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryDetailsExists(categoryDetails.Id))
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
            ViewData["Id"] = new SelectList(_context.Categories, "Id", "CreatedByUserId", categoryDetails.Id);
            return View(categoryDetails);
        }

        // GET: CategoryDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryDetails = await _context.CategoryDetails
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryDetails == null)
            {
                return NotFound();
            }

            return View(categoryDetails);
        }

        // POST: CategoryDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryDetails = await _context.CategoryDetails.FindAsync(id);
            _context.CategoryDetails.Remove(categoryDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryDetailsExists(int id)
        {
            return _context.CategoryDetails.Any(e => e.Id == id);
        }
    }
}
