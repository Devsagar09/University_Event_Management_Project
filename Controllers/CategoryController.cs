using EventsMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EventsMVC.Controllers
{
    public class CategoryController : Controller
    {
        private string baseurl = "https://localhost:7191/api/EventCategories/";
        HttpClient client = new HttpClient();

        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            var reponse = await client.GetAsync(baseurl);
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var category = JsonConvert.DeserializeObject<List<EventCategory>>(reposenValue);
            return View(category);
        }

        public async Task<EventCategory> GetByID(int id)
        {
            var reponse = await client.GetAsync(baseurl + id.ToString());
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var category = JsonConvert.DeserializeObject<EventCategory>(reposenValue);
            return category;
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var display = await GetByID(id);
            return View(display);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventCategory collection)
        {   
            try
            {
                var JsonData = JsonConvert.SerializeObject(collection);
                var ReponseValue = await client.PostAsync(baseurl, new StringContent(JsonData, Encoding.UTF8, "application/json"));
                Console.Write(ReponseValue);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var category = await GetByID(id);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EventCategory collection)
        {
            try
            {
                var JsonData = JsonConvert.SerializeObject(collection);
                var ReponseValue = await client.PutAsync(baseurl + id.ToString(), new StringContent(JsonData, Encoding.UTF8, "application/json"));
                Console.Write(ReponseValue);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var category = await GetByID(id);
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, EventCategory collection)
        {
            try
            {
                var ReponseValue = await client.DeleteAsync(baseurl + id.ToString());
                Console.Write(ReponseValue);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
