using EventsMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace EventsMVC.Controllers
{
    public class EventMasterController : Controller
    {
        private readonly UniversityEventManagementContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventMasterController(UniversityEventManagementContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private string baseurl = "https://localhost:7191/api/EventMasters/";
        HttpClient client = new HttpClient();



        // GET: EventMasterController
        public async Task<ActionResult> Index()
        {
            var reponse = await client.GetAsync(baseurl);
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventMaster>>(reposenValue);
            return View(events);
        }

		public async Task<ActionResult> Tech_event()
		{
			var reponse = await client.GetAsync(baseurl);
			var reposenValue = await reponse.Content.ReadAsStringAsync();
			var events = JsonConvert.DeserializeObject<List<EventMaster>>(reposenValue);
			return View(events);

		}

		public async Task<ActionResult> Index2()
        {
            var reponse = await client.GetAsync(baseurl);
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventMaster>>(reposenValue);
            return View(events);
        }

		public async Task<ActionResult> Index3()
		{
			var reponse = await client.GetAsync(baseurl);
			var reposenValue = await reponse.Content.ReadAsStringAsync();
			var events = JsonConvert.DeserializeObject<List<EventMaster>>(reposenValue);
			return View(events);
		}

		public async Task<ActionResult> In_door()
        {
            var reponse = await client.GetAsync(baseurl);
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventMaster>>(reposenValue);
            return View(events);
        }

        public async Task<ActionResult> out_door()
        {
            var reponse = await client.GetAsync(baseurl);
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventMaster>>(reposenValue);
            return View(events);
        }

        public async Task<EventMaster> GetByID(int id)
        {
            var reponse = await client.GetAsync(baseurl + id.ToString());
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<EventMaster>(reposenValue);
            return events;
        }

        // GET: EventMasterController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var GetDetails = await GetByID(id);
            return View(GetDetails);
        }

        // GET: EventMasterController/Create
        public ActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.EventCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: EventMasterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventDTO addEvent)
        {

            if (ModelState.IsValid)
            {
                string newFileName = string.Empty;

                if (addEvent.Image != null && addEvent.Image.Length > 0)
                {
                    newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(addEvent.Image.FileName);

                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                    Directory.CreateDirectory(uploadFolder);

                    string imageFullPath = Path.Combine(uploadFolder, newFileName);

                    using (var stream = new FileStream(imageFullPath, FileMode.Create))
                    {
                        await addEvent.Image.CopyToAsync(stream);
                    }


                }

                EventMaster eventmaster = new EventMaster()
                {
                    EventName = addEvent.EventName,
                    EventImage = newFileName,
                    Description = addEvent.Description,
                    Date = addEvent.Date,
                    Price = addEvent.Price,
                    CategoryId = addEvent.CategoryId,
                };

                _context.EventMasters.Add(eventmaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryID"] = new SelectList(_context.EventCategories, "CategoryId", "CategoryName", addEvent.CategoryId);
            return View(addEvent);

        }

        //GET : Event_Details
        public ActionResult Event_Details(int id)
        {

            var events = _context.EventMasters.Find(id);

            if (events == null)
            {
                return RedirectToAction(nameof(Index));
            }


            var EventDTO = new EventDTO()
            {
                EventName = events.EventName,
                Description = events.Description,
                Date = events.Date,
                Price = events.Price,
                CategoryId = events.CategoryId,
            };

            ViewBag.EventID = events.EventId;
            ViewBag.EventName = events.EventName;
            ViewBag.Desciption = events.Description;
            ViewBag.Price = events.Price;
            ViewBag.ImageFile = events.EventImage;
            ViewBag.Date = events.Date;

            ViewData["CategoryID"] = new SelectList(_context.EventCategories, "CategoryId", "CategoryName", events.CategoryId);
            return View(EventDTO);
        }




        public ActionResult Participant(int id)
        {
            Console.WriteLine($"Received eid: {id}");

            var userIdString = HttpContext.Session.GetString("UserId");

            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "UserMasters");
            }

            var eventId = _context.EventMasters.Find(id);

            if (eventId == null)
            {
                Console.WriteLine("Event not found");
                return RedirectToAction("Error", "Home");
            }

            var exisitingUser = _context.ParticipantTables.FirstOrDefault(u => u.UserId == userId && u.EventId == eventId.EventId);

            if (exisitingUser != null)
            {
                TempData["ErrorMessage"] = "You have already participated in this event.";
                return RedirectToAction("Index", "ParticipantTables");
            }

            var model = new ParticipantTable
            {
                UserId = userId,
                EventId = eventId.EventId,
                Date = DateTime.Now
            };

            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            ViewBag.Eventname = eventId.EventName;
            ViewBag.EventID = eventId.EventId;
            ViewBag.Price = eventId.Price;

            return View(model);

        }

   //     [Authorize]
   //     [HttpPost]
   //     [ValidateAntiForgeryToken]
   //     public async Task<IActionResult> Participant(ParticipantTable addparticipant)
   //     {
   //         if (ModelState.IsValid)
   //         {
   //             var exisitingUser = _context.ParticipantTables.FirstOrDefault(u => u.UserId == addparticipant.UserId
   //             && u.EventId == addparticipant.EventId);

   //             if (exisitingUser != null)
   //             {
   //                 TempData["ErrorMessage"] = "You have already participated in this event.";
   //                 return RedirectToAction("Index", "ParticipantTables");
   //             }

   //             _context.ParticipantTables.Add(addparticipant);
   //             await _context.SaveChangesAsync();
			//	return RedirectToAction(nameof(Payment));

			//}

   //         return View(addparticipant);

   //     }

        public ActionResult Payment(int id)
        {
            Console.WriteLine($"Received eid: {id}");

            var userIdString = HttpContext.Session.GetString("UserId");

            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "UserMasters");
            }

            var eventId = _context.EventMasters.Find(id);

            if (eventId == null)
            {
                Console.WriteLine("Event not found");
                return RedirectToAction("Error", "Home");
            }

            var model = new PaymentTable
            {
                UserId = userId,
                EventId = eventId.EventId,
                Date = DateTime.Now,
                Amount = eventId.Price
            };

            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            ViewBag.Eventname = eventId.EventName;
            ViewBag.Image = eventId.EventImage;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(PaymentTable paymenttable)
        {

			_context.PaymentTables.Add(paymenttable);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index", "Home");
			
        }

        //     public ActionResult Favorite(int id)
        //     {
        //Console.WriteLine($"Received eid: {id}");
        //var userIdString = HttpContext.Session.GetString("UserId");

        //if (!int.TryParse(userIdString, out int userId))
        //{
        //	return RedirectToAction("Login", "UserMasters");
        //}

        //var eventMaster = _context.EventMasters.Find(id);

        //if (eventMaster == null)
        //{
        //	Console.WriteLine("Event not found");
        //	return RedirectToAction("Error", "Home");
        //}


        //var favorite = new HistoryTable
        //{
        //	UserId = userId,
        //	EventId = eventMaster.EventId
        //};

        //ViewBag.EventName = eventMaster.EventName;
        //ViewBag.EventImage = eventMaster.EventImage;
        //ViewBag.EventPrice = eventMaster.Price;
        //ViewBag.EventDate = eventMaster.Date;


        //var favorites = _context.HistoryTables.Where(f => f.UserId == userId).ToList();
        //         return View(favorites);
        //     }

        public ActionResult Favorite(int id)
        {
            Console.WriteLine($"Received eid: {id}");

            var userIdString = HttpContext.Session.GetString("UserId");

            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "UserMasters");
            }

            var events = _context.EventMasters.Find(id);

            if (events == null)
            {
                Console.WriteLine("Event not found");
                return RedirectToAction("Error", "Home");
            }

            

            var model = new HistoryTable
            {
                UserId = userId,
                EventId = events.EventId
            };

            ViewBag.EventID = events.EventId;
            ViewBag.EventName = events.EventName;
            ViewBag.Image = events.EventImage;
            ViewBag.Price = events.Price;

			return RedirectToAction("Favorite");
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Favorite(HistoryTable favoritList, int id)
        {
			Console.WriteLine($"Received eid: {id}");

			var userIdString = HttpContext.Session.GetString("UserId");

            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "UserMasters");
            }

			var events = _context.EventMasters.Find(id);

			if (events == null)
			{
				Console.WriteLine("Event not found");
				return RedirectToAction("Error", "Home");
			}

            var exisitingUser = _context.ParticipantTables.FirstOrDefault(u => u.UserId == userId && u.EventId == events.EventId);

            if (exisitingUser != null)
            {
                TempData["ErrorMessage"] = "You have already participated in this event.";
                return RedirectToAction("FavoriteList");
            }

            favoritList.UserId = userId;
            favoritList.EventId = events.EventId;

            ViewBag.EvetName = events.EventName;
            ViewBag.Image = events.EventImage;
            ViewBag.EventID = events.EventId;

            _context.HistoryTables.Add(favoritList);
            _context.SaveChanges();

            return RedirectToAction("FavoriteList");
        }

        public async Task<IActionResult> FavoriteList()
        { 

            var userIdString = HttpContext.Session.GetString("UserId");

            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "UserMasters");
            }

            ViewBag.UserID = userId;

            var favoriteEvents = await _context.HistoryTables
                .Include(h => h.Event)
                .Include(h => h.User)
                .Where(h => h.UserId == userId)
                .ToListAsync();

            return View(favoriteEvents);
        }

        public async Task<IActionResult> RemoveFavorite(int id)
        {
            if (_context.HistoryTables == null)
            {
                return NotFound();
            }

            var favoritelist = await _context.HistoryTables
                .FirstOrDefaultAsync(m => m.HistoryId == id);

            if (favoritelist == null)
            {
                return NotFound();
            }

            _context.HistoryTables.Remove(favoritelist);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(FavoriteList));
        }

            // GET: EventMasterController/Edit/5

            public ActionResult Edit(int id)
        {

            var events = _context.EventMasters.Find(id);

            if (events == null)
            {
                return RedirectToAction(nameof(Index));
            }


            var EventDTO = new EventDTO()
            {
                EventName = events.EventName,
                Description = events.Description,
                Date = events.Date,
                Price = events.Price,
                CategoryId = events.CategoryId,
            };

            ViewBag.EventID = events.EventId;
            ViewBag.ImageFile = events.EventImage;

            ViewData["CategoryID"] = new SelectList(_context.EventCategories, "CategoryId", "CategoryName", events.CategoryId);
            return View(EventDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EventDTO eventDTO)
        {
            var events = _context.EventMasters.Find(id);

            if (events == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var eventMaster = await _context.EventMasters.FindAsync(id);
                    if (eventMaster == null)
                    {
                        return NotFound();
                    }

                    eventMaster.EventName = eventDTO.EventName;
                    eventMaster.Description = eventDTO.Description;
                    eventMaster.Date = eventDTO.Date;
                    eventMaster.Price = eventDTO.Price;
                    eventMaster.CategoryId = eventDTO.CategoryId;

                    ViewBag.EventID = events.EventId;
                    ViewBag.ImageFile = events.EventImage;

                    if (eventDTO.Image != null && eventDTO.Image.Length > 0)
                    {
                        // Delete the old image if exists
                        if (!string.IsNullOrEmpty(eventMaster.EventImage))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, eventMaster.EventImage);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save the new image
                        var newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(eventDTO.Image.FileName);
                        var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                        Directory.CreateDirectory(uploadFolder); // Ensure the directory exists
                        var newImagePath = Path.Combine(uploadFolder, newFileName);

                        using (var stream = new FileStream(newImagePath, FileMode.Create))
                        {
                            await eventDTO.Image.CopyToAsync(stream);  // Use async method
                        }

                        eventMaster.EventImage = Path.Combine("Uploads", newFileName);
                    }

                    _context.Update(eventMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventMasterExists(events.EventId))
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

            ViewData["CategoryID"] = new SelectList(_context.EventCategories, "CategoryId", "CategoryName", eventDTO.CategoryId);
            return View(eventDTO);

        }

        private bool EventMasterExists(int id)
        {
            return (_context.EventMasters?.Any(e => e.EventId == id)).GetValueOrDefault();
        }



        public async Task<ActionResult> Delete(int id)
        {
            var ReponseValue = await client.DeleteAsync(baseurl + id.ToString());
            Console.Write(ReponseValue);
            return RedirectToAction(nameof(Index));

        }
    }
}
