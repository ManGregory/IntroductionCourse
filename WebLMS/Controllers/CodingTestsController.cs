using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebLMS.Data;
using WebLMS.Models;

namespace WebLMS
{
    [Authorize(Roles = "Administrator")]
    public class CodingTestsController : Controller
    {
        private readonly LMSDbContext _context;

        public CodingTestsController(LMSDbContext context)
        {
            _context = context;
        }

        // GET: CodingTests
        public async Task<IActionResult> Index()
        {
            var lMSDbContext = _context.CodingTests.Include(c => c.CodingHomework);
            return View(await lMSDbContext.ToListAsync());
        }

        // GET: CodingTests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingTest = await _context.CodingTests
                .Include(c => c.CodingHomework)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codingTest == null)
            {
                return NotFound();
            }

            return View(codingTest);
        }

        // GET: CodingTests/Create
        public IActionResult Create()
        {
            ViewData["CodingHomeworkId"] = new SelectList(_context.CodingHomeworks, "Id", "Id");
            return View();
        }

        // POST: CodingTests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodingHomeworkId,Name,InputParameters,ExpectedResult")] CodingTest codingTest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(codingTest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodingHomeworkId"] = new SelectList(_context.CodingHomeworks, "Id", "Id", codingTest.CodingHomeworkId);
            return View(codingTest);
        }

        // GET: CodingTests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingTest = await _context.CodingTests.FindAsync(id);
            if (codingTest == null)
            {
                return NotFound();
            }
            ViewData["CodingHomeworkId"] = new SelectList(_context.CodingHomeworks, "Id", "Id", codingTest.CodingHomeworkId);
            return View(codingTest);
        }

        // POST: CodingTests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodingHomeworkId,Name,InputParameters,ExpectedResult")] CodingTest codingTest)
        {
            if (id != codingTest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(codingTest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodingTestExists(codingTest.Id))
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
            ViewData["CodingHomeworkId"] = new SelectList(_context.CodingHomeworks, "Id", "Id", codingTest.CodingHomeworkId);
            return View(codingTest);
        }

        // GET: CodingTests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingTest = await _context.CodingTests
                .Include(c => c.CodingHomework)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codingTest == null)
            {
                return NotFound();
            }

            return View(codingTest);
        }

        // POST: CodingTests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var codingTest = await _context.CodingTests.FindAsync(id);
            _context.CodingTests.Remove(codingTest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodingTestExists(int id)
        {
            return _context.CodingTests.Any(e => e.Id == id);
        }
    }
}
