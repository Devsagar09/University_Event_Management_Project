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
    public class SampleController : Controller
    {
        private readonly UniversityEventManagementContext _context;

        public SampleController(UniversityEventManagementContext context)
        {
            _context = context;
        }

        // GET: Sample
        public async Task<IActionResult> Index()
        {
            var universityEventManagementContext = _context.UserMasters.Include(u => u.Role);
            return View(await universityEventManagementContext.ToListAsync());
        }

        // GET: Sample/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserMasters == null)
            {
                return NotFound();
            }

            var userMaster = await _context.UserMasters
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userMaster == null)
            {
                return NotFound();
            }

            return View(userMaster);
        }

        // GET: Sample/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.RoleTables, "RoleId", "RoleId");
            return View();
        }

        // POST: Sample/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserMaster userMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.RoleTables, "RoleId", "RoleId", userMaster.RoleId);
            return View(userMaster);
        }

        // GET: Sample/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserMasters == null)
            {
                return NotFound();
            }

            var userMaster = await _context.UserMasters.FindAsync(id);
            if (userMaster == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.RoleTables, "RoleId", "RoleId", userMaster.RoleId);
            return View(userMaster);
        }

        // POST: Sample/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FullName,Email,Password,Address,RoleId")] UserMaster userMaster)
        {
            if (id != userMaster.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserMasterExists(userMaster.UserId))
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
            ViewData["RoleId"] = new SelectList(_context.RoleTables, "RoleId", "RoleId", userMaster.RoleId);
            return View(userMaster);
        }

        // GET: Sample/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserMasters == null)
            {
                return NotFound();
            }

            var userMaster = await _context.UserMasters
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userMaster == null)
            {
                return NotFound();
            }

            return View(userMaster);
        }

        // POST: Sample/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserMasters == null)
            {
                return Problem("Entity set 'UniversityEventManagementContext.UserMasters'  is null.");
            }
            var userMaster = await _context.UserMasters.FindAsync(id);
            if (userMaster != null)
            {
                _context.UserMasters.Remove(userMaster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserMasterExists(int id)
        {
          return (_context.UserMasters?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
