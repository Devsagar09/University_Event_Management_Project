using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventsMVC.Models;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;

namespace EventsMVC.Controllers
{
    public class FeedbackTablesController : Controller
    {
        private readonly UniversityEventManagementContext _context;
        private string baseurl = "https://localhost:7191/api/FeedbackTables/";
        HttpClient client = new HttpClient();

        public FeedbackTablesController(UniversityEventManagementContext context)
        {
            _context = context;
        }

        // GET: FeedbackTables
        public async Task<IActionResult> Index()
        {
            var universityEventManagementContext = _context.FeedbackTables.Include(f => f.User);
            return View(await universityEventManagementContext.ToListAsync());
        }

        // GET: FeedbackTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FeedbackTables == null)
            {
                return NotFound();
            }

            var feedbackTable = await _context.FeedbackTables
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (feedbackTable == null)
            {
                return NotFound();
            }

            return View(feedbackTable);
        }

        // GET: FeedbackTables/Create
        public IActionResult Create()
        {

            //var userIdString = HttpContext.Session.GetString("UserId");

            //if (!int.TryParse(userIdString, out int userId))
            //{
            //	return RedirectToAction("Login", "UserMasters");
            //}

            //         FeedbackTable model = new FeedbackTable
            //         {
            //             UserId = userId
            //         };

            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            return View();
        }

        // POST: FeedbackTables/Create                      
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackTable feedbackTable)
        {

            var userIdString = HttpContext.Session.GetString("UserId");

            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "UserMasters");
            }

            feedbackTable.UserId = userId;

            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail"); 
            

            _context.Add(feedbackTable);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create", "FeedbackTables"); // Redirect to an appropriate action



        }

        // GET: FeedbackTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FeedbackTables == null)
            {
                return NotFound();
            }

            var feedbackTable = await _context.FeedbackTables.FindAsync(id);
            if (feedbackTable == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.UserMasters, "UserId", "UserId", feedbackTable.UserId);
            return View(feedbackTable);
        }

        // POST: FeedbackTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeedbackId,UserId,Message")] FeedbackTable feedbackTable)
        {
            if (id != feedbackTable.FeedbackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedbackTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackTableExists(feedbackTable.FeedbackId))
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
            ViewData["UserId"] = new SelectList(_context.UserMasters, "UserId", "UserId", feedbackTable.UserId);
            return View(feedbackTable);
        }

        // GET: FeedbackTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FeedbackTables == null)
            {
                return NotFound();
            }

            var feedbackTable = await _context.FeedbackTables
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (feedbackTable == null)
            {
                return NotFound();
            }

            return View(feedbackTable);
        }

        // POST: FeedbackTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FeedbackTables == null)
            {
                return Problem("Entity set 'UniversityEventManagementContext.FeedbackTables'  is null.");
            }
            var feedbackTable = await _context.FeedbackTables.FindAsync(id);
            if (feedbackTable != null)
            {
                _context.FeedbackTables.Remove(feedbackTable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackTableExists(int id)
        {
            return (_context.FeedbackTables?.Any(e => e.FeedbackId == id)).GetValueOrDefault();
        }
    }
}
