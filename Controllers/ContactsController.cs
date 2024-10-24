using EventsMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EventsMVC.Controllers
{
    public class ContactsController : Controller
    {
        private string baseurl = "https://localhost:7191/api/ContactTables/";
        HttpClient client = new HttpClient();

        // GET: ContactsController
        public async Task<ActionResult> Index()
        {
            var reponse = await client.GetAsync(baseurl);
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var contact = JsonConvert.DeserializeObject<List<ContactTable>>(reposenValue);
            return View(contact);
        }

        public async Task<ContactTable> GetByID(int id)
        {
            var reponse = await client.GetAsync(baseurl + id.ToString());
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var contact = JsonConvert.DeserializeObject<ContactTable>(reposenValue);
            return contact;
        }

        // GET: ContactsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var display = await GetByID(id);
            return View(display);
        }

        // GET: ContactsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContactTable collection)
        {
            try
            {
                var JsonData = JsonConvert.SerializeObject(collection);
                var ReponseValue = await client.PostAsync(baseurl, new StringContent(JsonData, Encoding.UTF8, "application/json"));
                Console.Write(ReponseValue);
                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContactsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var contact = await GetByID(id);
            return View(contact);
        }

        // POST: ContactsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ContactTable collection)
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

        // GET: ContactsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var contact = await GetByID(id);
            return View(contact);
        }

        // POST: ContactsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ContactTable collection)
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
