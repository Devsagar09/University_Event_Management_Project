using EventsMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Text;

namespace EventsMVC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UniversityEventManagementContext _context;
        private string baseurl = "https://localhost:7191/api/UserMasters/";
        HttpClient client = new HttpClient();

        public AdminController(UniversityEventManagementContext context)
        {
            _context = context;
        }
        // GET: AdminController
        public async Task<IActionResult> Index()
        {
            var universityEventManagementContext = _context.UserMasters.Include(u => u.Role);
            return View(await universityEventManagementContext.ToListAsync());
        }

        public async Task<IActionResult> DisplayCounts()
        {
            var response1 = await client.GetAsync(baseurl + "getTotalUser/");
            var TotalResponse = await response1.Content.ReadAsStringAsync();
            var Total = JsonConvert.DeserializeObject<int>(TotalResponse);

            var response2 = await client.GetAsync(baseurl + "getUserCount/");
            var UserResponse = await response2.Content.ReadAsStringAsync();
            var TotalUser = JsonConvert.DeserializeObject<int>(UserResponse);

            var response3 = await client.GetAsync(baseurl + "getAdminCount/");
            var AdminResponse = await response3.Content.ReadAsStringAsync();
            var TotalAdmin = JsonConvert.DeserializeObject<int>(AdminResponse);

            var viewModel = new CountViewModel
            {
                TotalUserCount = Total,
                UserCount = TotalUser,
                Adminount = TotalAdmin
            };

            return View("DisplayCounts", viewModel);
        }

        public async Task<UserMaster> GetByID(int id)
        {
            var reponse = await client.GetAsync(baseurl + id.ToString());
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var getuser = JsonConvert.DeserializeObject<UserMaster>(reposenValue);
            return getuser;
        }

        // GET: AdminController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var display = await GetByID(id);
            return View(display);
        }

        // GET:
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.RoleTables, "RoleId", "RoleName");
            return View();
        }

        // POST: 
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
                TempData["AlertMessage"] = "User Created Successfully..";
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.RoleTables, "RoleId", "RoleName", userMaster.RoleId);
            return View(userMaster);
        }

        // GET: AdminController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var editvalue = await GetByID(id);
            return View(editvalue);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UserMaster edited)
        {
            try
            {
                var JsonData = JsonConvert.SerializeObject(edited);
                var ReponseValue = await client.PutAsync(baseurl + id.ToString(), new StringContent(JsonData, Encoding.UTF8, "application/json"));
                Console.Write(ReponseValue);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //GET: AdminController/Delete/5    
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var deleted = await GetByID(id);
        //    return View(deleted);
        //}

        // POST: AdminController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var ReponseValue = await client.DeleteAsync(baseurl + id.ToString());
            Console.Write(ReponseValue);
            return RedirectToAction(nameof(Index));

        }
    }
}
