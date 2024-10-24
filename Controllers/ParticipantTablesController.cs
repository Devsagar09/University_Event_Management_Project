using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventsMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventsMVC.Controllers
{
    public class ParticipantTablesController : Controller
    {
        private readonly UniversityEventManagementContext _context;

        public ParticipantTablesController(UniversityEventManagementContext context)
        {
            _context = context;
        }

        // GET: ParticipantTables
        [Authorize] 
        public async Task<IActionResult> Index()
        {
            var universityEventManagementContext = _context.ParticipantTables.Include(p => p.Event).Include(p => p.User);

            var userIdString = HttpContext.Session.GetString("UserId");
			if (!int.TryParse(userIdString, out int userId))
			{
				return RedirectToAction("Login", "UserMasters");
			}

            ViewBag.UserID = userId;

			return View(await universityEventManagementContext.ToListAsync());
        }


		public async Task<IActionResult> Admin()
		{
			var universityEventManagementContext = _context.ParticipantTables.Include(p => p.Event).Include(p => p.User);

			return View(await universityEventManagementContext.ToListAsync());
		}



		// GET: ParticipantTables/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ParticipantTables == null)
            {
                return NotFound();
            }

            var participantTable = await _context.ParticipantTables
                .Include(p => p.Event)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participantTable == null)
            {
                return NotFound();
            }

            return View(participantTable);
        }

        // GET: ParticipantTables/Create
        public IActionResult Create()
        {
             

            //var userid = HttpContext.Session.GetString("UserId");
            var userIdString = HttpContext.Session.GetString("UserId");

            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "UserMasters");
            }


            var model = new ParticipantTable
            {
                UserId = userId, 

                Date = DateTime.Now
            };

            ViewData["EventId"] = new SelectList(_context.EventMasters, "EventId", "EventName");

            return View(model);
        }

        // POST: ParticipantTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParticipantId,UserId,EventId,Date")] ParticipantTable participantTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participantTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.EventMasters, "EventId", "EventName", participantTable.EventId); 
            return View(participantTable);
        }

        // GET: ParticipantTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ParticipantTables == null)
            {
                return NotFound();
            }

            var participantTable = await _context.ParticipantTables.FindAsync(id);
            if (participantTable == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.EventMasters, "EventId", "Date", participantTable.EventId);
            ViewData["UserId"] = new SelectList(_context.UserMasters, "UserId", "UserId", participantTable.UserId);
            return View(participantTable);
        }

        // POST: ParticipantTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParticipantId,UserId,EventId,Date")] ParticipantTable participantTable)
        {
            if (id != participantTable.ParticipantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participantTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantTableExists(participantTable.ParticipantId))
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
            ViewData["EventId"] = new SelectList(_context.EventMasters, "EventId", "Date", participantTable.EventId);
            ViewData["UserId"] = new SelectList(_context.UserMasters, "UserId", "UserId", participantTable.UserId);
            return View(participantTable);
        }

        // GET: ParticipantTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ParticipantTables == null)
            {
                return NotFound();
            }

            var participantTable = await _context.ParticipantTables
                .Include(p => p.Event)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participantTable == null)
            {
                return NotFound();
            }

            return View(participantTable);
        }

        // POST: ParticipantTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ParticipantTables == null)
            {
                return Problem("Entity set 'UniversityEventManagementContext.ParticipantTables'  is null.");
            }
            var participantTable = await _context.ParticipantTables.FindAsync(id);
            if (participantTable != null)
            {
                _context.ParticipantTables.Remove(participantTable);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantTableExists(int id)
        {
          return (_context.ParticipantTables?.Any(e => e.ParticipantId == id)).GetValueOrDefault();
        }
    }
}
