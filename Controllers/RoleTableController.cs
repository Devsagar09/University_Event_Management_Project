using EventsMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EventsMVC.Controllers
{
    public class RoleTableController : Controller
    {
        private string baseurl = "https://localhost:7191/api/RoleTables/";
        HttpClient client = new HttpClient();

        // GET: RoleTableController
        public  async Task<ActionResult> Index()
        {
            var reponse = await client.GetAsync(baseurl);
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<RoleTable>>(reposenValue);
            return View(roles);
             
        }

        //Get : RoleID
        public async Task<RoleTable> GetByID(int id)
        {
            var reponse = await client.GetAsync(baseurl + id.ToString());
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<RoleTable>(reposenValue);
            return roles;
        }


        // GET: RoleTableController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var DiplayData = await GetByID(id);
            return View(DiplayData);

        }

        // GET: RoleTableController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleTableController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleTable collection)
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

        // GET: RoleTableController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var roles = await GetByID(id);
            return View(roles);

        }

        // POST: RoleTableController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, RoleTable collection)
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

        // GET: RoleTableController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var roles = await GetByID(id);
            return View(roles);

        }

        // POST: RoleTableController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, RoleTable rolesdelete)
        {
            try
            {
                var ReponseValue = await client.DeleteAsync(baseurl+ id.ToString());
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
