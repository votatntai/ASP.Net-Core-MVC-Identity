using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenShop.Data;
using AuthenShop.Entities;
using AuthenShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenShop.Controllers
{
    public class ShoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shoes
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var shoes = from s in _context.Products where s.Category.Equals("Shoes") select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                shoes = shoes.Where(s => s.Price.ToString().Contains(searchString) || s.Title.Contains(searchString));
            }

            int pageSize = 6;
            return View(await PaginatedList<Product>.CreateAsync(shoes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Shoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoes = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoes == null)
            {
                return NotFound();
            }

            return View(shoes);
        }

        // GET: Shoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoes = await _context.Products.FindAsync(id);
            if (shoes == null)
            {
                return NotFound();
            }
            return View(shoes);
        }

        // POST: Shoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,Description,Image,Status,Price")] Product shoes)
        {
            if (id != shoes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!shoesExists(shoes.Id))
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
            return View(shoes);
        }

        // GET: Shoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoes = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoes == null)
            {
                return NotFound();
            }

            return View(shoes);
        }

        // POST: Shoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoes = await _context.Products.FindAsync(id);
            _context.Products.Remove(shoes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool shoesExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}