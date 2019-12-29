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
    public class CodingHomeworksController : Controller
    {
        private readonly LMSDbContext _context;

        public CodingHomeworksController(LMSDbContext context)
        {
            _context = context;
        }

        // GET: CodingHomeworks
        public IActionResult Index()
        {
            var codingHomeworks = _context.CodingHomeworks.Include(c => c.Lecture).ToList().GroupBy(c => c.Lecture.Id).ToList();
            return View(codingHomeworks);
        }

        // GET: CodingHomeworks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingHomework = await _context.CodingHomeworks
                .Include(c => c.Lecture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codingHomework == null)
            {
                return NotFound();
            }

            return View(codingHomework);
        }

        // GET: CodingHomeworks/Create
        public IActionResult Create()
        {
            ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Id");
            return View();
        }

        // POST: CodingHomeworks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LectureId,Subject,Description,TemplateCode,CodingTestType,EntryType,EntryMethodName,MaxAttempts")] CodingHomework codingHomework)
        {
            if (ModelState.IsValid)
            {
                _context.Add(codingHomework);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Id", codingHomework.LectureId);
            return View(codingHomework);
        }

        // GET: CodingHomeworks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingHomework = await _context.CodingHomeworks.FindAsync(id);
            if (codingHomework == null)
            {
                return NotFound();
            }
            ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Id", codingHomework.LectureId);
            return View(codingHomework);
        }

        // POST: CodingHomeworks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LectureId,Subject,Description,TemplateCode,CodingTestType,EntryType,EntryMethodName,MaxAttempts")] CodingHomework codingHomework)
        {
            if (id != codingHomework.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(codingHomework);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodingHomeworkExists(codingHomework.Id))
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
            ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Id", codingHomework.LectureId);
            return View(codingHomework);
        }

        // GET: CodingHomeworks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingHomework = await _context.CodingHomeworks
                .Include(c => c.Lecture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codingHomework == null)
            {
                return NotFound();
            }

            return View(codingHomework);
        }

        // POST: CodingHomeworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var codingHomework = await _context.CodingHomeworks.FindAsync(id);
            _context.CodingHomeworks.Remove(codingHomework);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodingHomeworkExists(int id)
        {
            return _context.CodingHomeworks.Any(e => e.Id == id);
        }
    }
}
