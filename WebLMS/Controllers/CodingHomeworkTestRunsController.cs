using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebLMS.Data;
using WebLMS.Models;

namespace WebLMS.Controllers
{
    public class CodingHomeworkTestRunsController : Controller
    {
        private readonly LMSDbContext _context;

        public CodingHomeworkTestRunsController(LMSDbContext context)
        {
            _context = context;
        }

        // GET: CodingHomeworkTestRuns
        public async Task<IActionResult> Index()
        {
            var lMSDbContext = _context.CodingHomeworkTestRuns.Include(c => c.CodingTest);
            return View(await lMSDbContext.ToListAsync());
        }

        // GET: CodingHomeworkTestRuns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingHomeworkTestRun = await _context.CodingHomeworkTestRuns
                .Include(c => c.CodingTest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codingHomeworkTestRun == null)
            {
                return NotFound();
            }

            return View(codingHomeworkTestRun);
        }

        // GET: CodingHomeworkTestRuns/Create
        public IActionResult Create()
        {
            ViewData["CodingTestId"] = new SelectList(_context.CodingTests, "Id", "Id");
            return View();
        }

        // POST: CodingHomeworkTestRuns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodingTestId,SourceCode,Result,StartTime,EndTime")] CodingHomeworkTestRun codingHomeworkTestRun)
        {
            if (ModelState.IsValid)
            {
                _context.Add(codingHomeworkTestRun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodingTestId"] = new SelectList(_context.CodingTests, "Id", "Id", codingHomeworkTestRun.CodingTestId);
            return View(codingHomeworkTestRun);
        }

        // GET: CodingHomeworkTestRuns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingHomeworkTestRun = await _context.CodingHomeworkTestRuns.FindAsync(id);
            if (codingHomeworkTestRun == null)
            {
                return NotFound();
            }
            ViewData["CodingTestId"] = new SelectList(_context.CodingTests, "Id", "Id", codingHomeworkTestRun.CodingTestId);
            return View(codingHomeworkTestRun);
        }

        // POST: CodingHomeworkTestRuns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodingTestId,SourceCode,Result,StartTime,EndTime")] CodingHomeworkTestRun codingHomeworkTestRun)
        {
            if (id != codingHomeworkTestRun.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(codingHomeworkTestRun);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodingHomeworkTestRunExists(codingHomeworkTestRun.Id))
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
            ViewData["CodingTestId"] = new SelectList(_context.CodingTests, "Id", "Id", codingHomeworkTestRun.CodingTestId);
            return View(codingHomeworkTestRun);
        }

        // GET: CodingHomeworkTestRuns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingHomeworkTestRun = await _context.CodingHomeworkTestRuns
                .Include(c => c.CodingTest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codingHomeworkTestRun == null)
            {
                return NotFound();
            }

            return View(codingHomeworkTestRun);
        }

        // POST: CodingHomeworkTestRuns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var codingHomeworkTestRun = await _context.CodingHomeworkTestRuns.FindAsync(id);
            _context.CodingHomeworkTestRuns.Remove(codingHomeworkTestRun);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodingHomeworkTestRunExists(int id)
        {
            return _context.CodingHomeworkTestRuns.Any(e => e.Id == id);
        }
    }
}
