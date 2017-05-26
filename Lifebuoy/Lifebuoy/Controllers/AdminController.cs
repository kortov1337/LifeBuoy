using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lifebuoy.DAL;
using Lifebuoy.Models;
using System.Net;
using PagedList.Mvc;
using PagedList;

namespace Lifebuoy.Controllers
{
    public class AdminController : Controller
    {
        private OffersContext db = new OffersContext();
        private OffersContext db2 = new OffersContext();
        private ApplicationDbContext adb = new ApplicationDbContext();
        // GET: Admin
        public ActionResult AdministratorPage(int? page)
        {
            if (User.IsInRole("Admin"))
            {
                var users = adb.Users.ToList();
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(users.ToPagedList(pageNumber, pageSize));
            }
            else
                return View("AccessDenied");
        }

       
        public ActionResult ModeratorPage()
        {
            if (User.IsInRole("Moderator"))
            {
                List<Offers> NonModered = new List<Offers>();
                foreach( var q in db.Offers)
                {
                    var item = db2.ModeratedOffers.FirstOrDefault(c => c.OfferId == q.Id);
                    if(item==null)
                    {
                        NonModered.Add(q);
                    }
                }
                return View(NonModered);
            }
            else
                return View("AccessDenied");
        }

        public ActionResult ModerateOffer(int? id)
        {
            if (User.IsInRole("Moderator"))
            {
                List<String> Images = new List<string>();
                string[] splitResult;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Offers offers = db.Offers.Find(id);
                if (offers == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    splitResult = offers.Images.Split(';');
                    Images = splitResult.ToList();
                    Images.RemoveAt(Images.Count - 1);
                }
                ViewBag.Images = Images;       
                return View(offers);
            }
            else
                return View("AccessDenied");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModerateOffer(Offers offers)
        {
            if (User.IsInRole("Moderator"))
            {
                var item = db.Offers.FirstOrDefault(c => c.Id == offers.Id);
                item.Id = offers.Id;
                item.Adress = offers.Adress;
                item.catalogID = offers.catalogID;
                item.CatalogName = offers.CatalogName;
                item.Category = offers.Category;
                item.City = offers.City;
                item.Contacts = offers.Contacts;
                item.Description = offers.Description;
                item.Provider = offers.Provider;
                item.Rating = offers.Rating;
                item.ShortDescription = offers.ShortDescription;
                item.WorkingHours = offers.WorkingHours;
                db.ModeratedOffers.Add(new ModeratedOffers { OfferId = item.Id, isModerated = true });
                db.SaveChanges();
                return Redirect("ModeratorPage");
            }
            else
                return View("AccessDenied");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}