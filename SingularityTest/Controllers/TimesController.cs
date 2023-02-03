using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SingularityTest.Data;
using SingularityTest.Models;

namespace SingularityTest.Controllers
{
    public class TimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Times
        public async Task<IActionResult> Index()
        {
              return View(await _context.Times.ToListAsync());
        }

        // GET: Times/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Times == null)
            {
                return NotFound();
            }

            var time = await _context.Times
                .FirstOrDefaultAsync(m => m.Id == id);
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // GET: Times/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Times/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Hour,Minute")] Time time)
        {
            //if (ModelState.IsValid)
            //{
                var currentTime = DateTime.Now;
                var hour = currentTime.Hour % 12;
                var minute = currentTime.Minute;

                string[] timeToText = new string[] { "zero", "One", "Two", "Three", "Four", "Five", "six", "Seven", "Eight","Nine", "Ten", "Eleven", "Tweelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Tweenty", "Tweenty One", "Tweenty Two", "Tweenty Three", "Tweenty Four", "Tweenty Five", "Tweenty Six", "Tweenty Seven", "Tweenty Eight", "Tweenty Nine" };
                if(minute == 0)
                {
                    time.TimeToText = $"{timeToText[hour]} o' clock";
                }
                else if(minute == 1)
                {
                    time.TimeToText = $"One minute pass {timeToText[hour]} ";
                }
                else if(minute == 15)
                {
                    time.TimeToText = $"Quater pass {timeToText[hour]} ";
                }
                else if (minute == 30)
                {
                    time.TimeToText = $"Half pass {timeToText[hour]} ";
                }
                else if (minute == 45)
                {
                    time.TimeToText = $"Quater to {timeToText[(hour % 12) +1]} ";
                }
                else if (minute == 59)
                {
                    time.TimeToText = $"One minute to {timeToText[(hour % 12) + 1]} ";
                }
                else if(minute < 30)
                {
                    time.TimeToText = $"{timeToText[minute]} minutes pass {timeToText[hour]} ";
                }
                else if(minute > 30)
                {
                    time.TimeToText = $" {timeToText[60 - minute]} minutes to {timeToText[(hour % 12) + 1]} ";
                }
                TempData["Time"] = time.TimeToText;
                _context.Add(time);
                await _context.SaveChangesAsync();
                return View();
                //return RedirectToAction(nameof(Index));
            //}
            //return View();
        }

        // GET: Times/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Times == null)
            {
                return NotFound();
            }

            var time = await _context.Times.FindAsync(id);
            if (time == null)
            {
                return NotFound();
            }
            return View(time);
        }

        // POST: Times/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hour,Minute,TimeToText")] Time time)
        {
            if (id != time.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(time);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeExists(time.Id))
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
            return View(time);
        }

        // GET: Times/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Times == null)
            {
                return NotFound();
            }

            var time = await _context.Times
                .FirstOrDefaultAsync(m => m.Id == id);
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // POST: Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Times == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Times'  is null.");
            }
            var time = await _context.Times.FindAsync(id);
            if (time != null)
            {
                _context.Times.Remove(time);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeExists(int id)
        {
          return _context.Times.Any(e => e.Id == id);
        }
    }
}
