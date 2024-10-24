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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace EventsMVC.Controllers
{
    public class UserMastersController : Controller
    {
        private readonly UniversityEventManagementContext _context;
        private string baseurl = "https://localhost:7191/api/UserMasters/";
        HttpClient client = new HttpClient();

        public UserMastersController(UniversityEventManagementContext context)
        {
            _context = context;
        }

        // GET: UserMasters
        public async Task<IActionResult> Index()
        {
            var universityEventManagementContext = _context.UserMasters.Include(u => u.Role);
            return View(await universityEventManagementContext.ToListAsync());
        }

        // GET: UserMasters/Details/5
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserMaster users)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var data = _context.UserMasters.Where(u => u.Email == users.Email && u.Password == users.Password).FirstOrDefault();
                    if (data != null)
                    {
                        bool isValid = (data.Email == users.Email && data.Password == users.Password);
                        if (isValid)
                        {
                            var claims = new List<Claim>
                            { 
                                new Claim(ClaimTypes.Name, users.Email),
                                new Claim(ClaimTypes.NameIdentifier, data.UserId.ToString()),
                                new Claim(ClaimTypes.Role, data.RoleId.ToString()) // Assuming RoleId is the property name
                             
                            };

                            // Create identity for the user
                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);
                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                             
							HttpContext.Session.SetString("UserEmail", data.Email);
                            HttpContext.Session.SetString("UserId", data.UserId.ToString());
                            HttpContext.Session.SetString("UserName", data.FullName);
                           
							if (data.RoleId == 1)
                            { 
                                return RedirectToAction("DisplayCounts", "Admin");
                            }
                            else
                            { 
                                return RedirectToAction("Index", "Home");
                            }

                        }

                        else
                        {
                            TempData["errorPassword"] = "Invalid Password";
                            Console.WriteLine("Invalid password");
                            return RedirectToAction("Login", "UserMasters");
                        }
                    }

                    else
                    {
                        TempData["errorMessage"] = "User Not Found";
                        Console.WriteLine("Not found");
                        return View(users);
                    }
                }
                return View(users);
            }
            catch
            {
                return View();
            }
        }


        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.Session.Clear();

                return RedirectToAction("Login", "UserMasters");
            }
            catch
            {
                // Handle any exceptions
                return NotFound("View Not Found Logput not working");
            }
        }

        // GET: UserMasters/Create
        public IActionResult Create()
        {
			
			return View();
        }

        // POST: UserMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserMaster userMaster)
        {
            try
            {
                var JsonData = JsonConvert.SerializeObject(userMaster);
                var ResponseValue = await client.PostAsync(baseurl, new StringContent(JsonData, Encoding.UTF8, "application/json"));
                Console.WriteLine(ResponseValue);

                return RedirectToAction(nameof(Login));
            }
            catch
            {
                return View(userMaster);
            }
        }

        // GET: UserMasters/Edit/5
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

        // POST: UserMasters/Edit/5
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

        // GET: UserMasters/Delete/5
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

        // POST: UserMasters/Delete/5
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
