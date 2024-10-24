using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventsMVC.Models;

namespace EventsMVC.Controllers
{
    public class PaymentTablesController : Controller
    {
        private readonly UniversityEventManagementContext _context;

        public PaymentTablesController(UniversityEventManagementContext context)
        {
            _context = context;
        }

        // GET: PaymentTables
        public async Task<IActionResult> Index()
        {
            var universityEventManagementContext = _context.PaymentTables.Include(p => p.Event).Include(p => p.User);
            return View(await universityEventManagementContext.ToListAsync());
        }

        // GET: PaymentTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PaymentTables == null)
            {
                return NotFound();
            }

            var paymentTable = await _context.PaymentTables
                .Include(p => p.Event)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (paymentTable == null)
            {
                return NotFound();
            }

            return View(paymentTable);
        }

        // GET: PaymentTables/Create
        public IActionResult Create(int id)
        {
            Console.WriteLine($"Received eid: {id}");

            var userIdString = HttpContext.Session.GetString("UserId");

            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "UserMasters");
            }

            //var eventId = _context.EventMasters.Find(id);

            //if (eventId == null)
            //{
            //    Console.WriteLine("Event ID Not Found");
                
            //}

            //var model = new PaymentTable
            //{
            //    UserId = userId,
            //    EventId = eventId.EventId,
            //    Date = DateTime.Now,
            //    Amount = eventId.Price
            //};

            //ViewBag.Price = eventId.Price;
            //ViewBag.EventName = eventId.EventName;
            //ViewBag.EventImage = eventId.EventImage;
            //ViewBag.EventId = eventId.EventId;
            //ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            return View();
        }

        // POST: PaymentTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentTable paymentTable,int id)
        {
            if (ModelState.IsValid)
            {
                var userIdString = HttpContext.Session.GetString("UserId");

                if (!int.TryParse(userIdString, out int userId))
                {
                    return RedirectToAction("Login", "UserMasters");
                }

                var Events = _context.EventMasters.Find(id);
                if (Events == null)
                {
                    return NotFound("Event Id is not found");
                }

                paymentTable.UserId = userId;
                paymentTable.EventId = Events.EventId;
                paymentTable.Amount = Events.Price;
                paymentTable.Date = DateTime.Now;

                ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.Price = Events.Price;

                _context.Add(paymentTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paymentTable);
        }

        // GET: PaymentTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PaymentTables == null)
            {
                return NotFound();
            }

            var paymentTable = await _context.PaymentTables.FindAsync(id);
            if (paymentTable == null)
            {
                return NotFound();
            }
            //ViewData["EventId"] = new SelectList(_context.EventMasters, "EventId", "Date", paymentTable.EventId);
            //ViewData["UserId"] = new SelectList(_context.UserMasters, "UserId", "UserId", paymentTable.UserId);
            return View(paymentTable);
        }

        // POST: PaymentTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,UserId,EventId,Date,Amount,CardholderName,Card_Number,Card_Type,Expiry,CVV")] PaymentTable paymentTable)
        {
            if (id != paymentTable.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentTableExists(paymentTable.PaymentId))
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
            ViewData["EventId"] = new SelectList(_context.EventMasters, "EventId", "Date", paymentTable.EventId);
            ViewData["UserId"] = new SelectList(_context.UserMasters, "UserId", "UserId", paymentTable.UserId);
            return View(paymentTable);
        }

        // GET: PaymentTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PaymentTables == null)
            {
                return NotFound();
            }

            var paymentTable = await _context.PaymentTables
                .Include(p => p.Event)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (paymentTable == null)
            {
                return NotFound();
            }

            return View(paymentTable);
        }

        // POST: PaymentTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PaymentTables == null)
            {
                return Problem("Entity set 'UniversityEventManagementContext.PaymentTables'  is null.");
            }
            var paymentTable = await _context.PaymentTables.FindAsync(id);
            if (paymentTable != null)
            {
                _context.PaymentTables.Remove(paymentTable);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentTableExists(int id)
        {
          return (_context.PaymentTables?.Any(e => e.PaymentId == id)).GetValueOrDefault();
        }
    }
}
