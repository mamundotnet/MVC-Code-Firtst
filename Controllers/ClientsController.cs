using ProjectMVC.Models.ViewModels;
using ProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.Controllers
{
    public class ClientsController : Controller
    {
        private BookingDbContext db = new BookingDbContext();
        // GET: Clients
        int size = 2;
        public ActionResult Index(string usertext, string sortOrder, int page = 1)
        {
            ViewBag.currentPage = page;
            ViewBag.clientName = usertext;

            if (string.IsNullOrEmpty(usertext))
            {
                ViewBag.totalPages = (int)Math.Ceiling((double)db.Clients.Count() / size);
                var clients = db.Clients.Include(c => c.BookingEntries.Select(s => s.Spot)).OrderBy(x => x.ClientId).Skip((page - 1) * size).Take(size).ToList();
                return View(clients);

            }
            else
            {
                ViewBag.totalPages = (int)Math.Ceiling((double)db.Clients.Count() / size);
                var clients = db.Clients.Where(c => c.ClientName.ToLower().Contains(usertext.ToLower())).Include(c => c.BookingEntries.Select(s => s.Spot)).OrderBy(x => x.ClientId).Skip((page - 1) * size).Take(size).ToList();

                return View(clients);
            }
        }
        public ActionResult AddNewSpot(int? id)
        {
            ViewBag.spots = new SelectList(db.Spots.ToList(), "SpotId", "SpotName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewSpot");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ClientVM clientVM, int[] spotId)
        {
            if (ModelState.IsValid)
            {
                Client client = new Client()
                {
                    ClientName = clientVM.ClientName,
                    BirthDate = clientVM.BirthDate,
                    MonthlyIncome = clientVM.MonthlyIncome,
                    MaritalStatus = clientVM.MaritalStatus
                };

                //Image
                HttpPostedFileBase file = clientVM.PictureFile;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images", DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    client.Picture = filePath;
                }
                //All Spot 

                foreach (var item in spotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        Client = client,
                        ClientId = client.ClientId,
                        SpotId = item
                    };
                    db.BookingEntries.Add(bookingEntry);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            Client client = db.Clients.First(x => x.ClientId == id);
            var clientSpots = db.BookingEntries.Where(x => x.ClientId == id).ToList();

            ClientVM clientVM = new ClientVM()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                MonthlyIncome = client.MonthlyIncome,
                BirthDate = client.BirthDate,
                Picture = client.Picture,
                MaritalStatus = client.MaritalStatus
            };

            if (clientSpots.Count() > 0)
            {
                foreach (var item in clientSpots)
                {
                    clientVM.SpotList.Add(item.SpotId);
                }
            }
            return View(clientVM);
        }

        [HttpPost]
        public ActionResult Edit(ClientVM clientVM, int[] SpotId)
        {
            if (ModelState.IsValid)
            {
                Client client = new Client()
                {
                    ClientId = clientVM.ClientId,
                    ClientName = clientVM.ClientName,
                    BirthDate = clientVM.BirthDate,
                    MonthlyIncome = clientVM.MonthlyIncome,
                    MaritalStatus = clientVM.MaritalStatus
                };

                //Image
                HttpPostedFileBase file = clientVM.PictureFile;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images", DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    client.Picture = filePath;
                }
                else
                {
                    //string fName= clientVM.PictureFile.FileName.ToString();
                    client.Picture = clientVM.Picture;
                }

                //spot delete
                var existsSpotEntry = db.BookingEntries.Where(x => x.ClientId == client.ClientId).ToList();

                foreach (var bookingEntry in existsSpotEntry)
                {
                    db.BookingEntries.Remove(bookingEntry);
                }

                //Add Spot
                foreach (var item in SpotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        Client = client,
                        ClientId = client.ClientId,
                        SpotId = item
                    };
                    db.BookingEntries.Add(bookingEntry);
                }
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}