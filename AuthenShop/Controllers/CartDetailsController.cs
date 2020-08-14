using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthenShop.Data;
using AuthenShop.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AuthenShop.Controllers
{
    [Authorize]
    public class CartDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            return Json(await _context.CartDetail.Where(c => c.Email.Equals(User.Identity.Name)).ToListAsync());
        }

        // GET: CartDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CartDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,ProductId,Quantity,Price")] CartDetail cartDetail)
        {
            if (ModelState.IsValid)
            {
                if (!CartExits(cartDetail.Email))
                {
                    _context.Cart.Add(new Cart
                    {
                        Email = cartDetail.Email
                    });
                }
                _context.Add(cartDetail);
                await _context.SaveChangesAsync();
                return StatusCode(200, "Success");
            }
            return View(cartDetail);
        }

        // GET: CartDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartDetail = await _context.CartDetail.FindAsync(id);
            if (cartDetail == null)
            {
                return NotFound();
            }
            return View(cartDetail);
        }

        // POST: CartDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,ProductId,Quantity,Price")] CartDetail cartDetail)
        {
            if (id != cartDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartDetailExists(cartDetail.Id))
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
            return View(cartDetail);
        }

        // GET: CartDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartDetail = await _context.CartDetail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartDetail == null)
            {
                return NotFound();
            }

            return View(cartDetail);
        }

        // POST: CartDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartDetail = await _context.CartDetail.FindAsync(id);
            _context.CartDetail.Remove(cartDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartDetailExists(int id)
        {
            return _context.CartDetail.Any(e => e.Id == id);
        }

        private bool CartExits(string email)
        {
            return _context.Cart.Any(c => c.Email == email);
        }
    }
}