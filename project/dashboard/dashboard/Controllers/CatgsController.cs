using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dashboard.Models;

namespace dashboard.Controllers
{
    public class CatgsController : Controller
    {
        private readonly mydbContext _context;

        public CatgsController(mydbContext context)
        {
            _context = context;
        }

        // GET: Catgs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Catgs.ToListAsync());
        }

        // GET: Catgs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catg = await _context.Catgs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catg == null)
            {
                return NotFound();
            }

            return View(catg);
        }

        // GET: Catgs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catgs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Namee")] Catg catg)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catg);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catg);
        }

        // GET: Catgs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catg = await _context.Catgs.FindAsync(id);
            if (catg == null)
            {
                return NotFound();
            }
            return View(catg);
        }

        // POST: Catgs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Namee")] Catg catg)
        {
            if (id != catg.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catg);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatgExists(catg.Id))
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
            return View(catg);
        }

        // GET: Catgs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catg = await _context.Catgs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catg == null)
            {
                return NotFound();
            }

            return View(catg);
        }

        // POST: Catgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catg = await _context.Catgs.FindAsync(id);
            _context.Catgs.Remove(catg);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatgExists(int id)
        {
            return _context.Catgs.Any(e => e.Id == id);
        }
    }
}
