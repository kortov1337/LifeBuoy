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
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

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
                ViewBag.Roles = adb.Roles.ToList();
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(users.ToPagedList(pageNumber, pageSize));
            }
            else
                return View("AccessDenied");
        }

       public async Task<ActionResult> SaveRole(string id, string roleName)
        {
            if (User.IsInRole("Admin"))
            {
                var user = adb.Users.Find(id);          
                var role = adb.Roles.Where(r => r.Name.Equals(roleName));
                var rid = user.Roles.First().RoleId;
                var oldRole = adb.Roles.Where(r => r.Id == rid);

                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                if (role!=null)
                {
                    var result = await userManager.RemoveFromRoleAsync(id, oldRole.First().Name);
                    var result1 = await userManager.AddToRoleAsync(id, role.First().Name);
                    adb.SaveChanges();
                }
                return RedirectToAction("AdministratorPage", "Admin");
            }
            else
                return View("AccessDenied");
        }

        [HttpGet]
        public ActionResult DeleteUser (string id)
        {
            if (User.IsInRole("Admin"))
            {
                var user = adb.Users.Find(id);
                if(user!= null)
                {
                    return View(user);
                }
                else
                    return HttpNotFound();
            }
            else
                return View("AccessDenied");
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (User.IsInRole("Admin"))
            {
                var user = adb.Users.Find(id);
                if (user != null)
                    adb.Users.Remove(user);
                adb.SaveChanges();
                return RedirectToAction("AdministratorPage");
            }
            else
                return View("AccessDenied");
        }

        public ActionResult DeleteOffer(int? id)
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

        [HttpPost, ActionName("DeleteOffer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOfferConfirmed(int? id)
        {
            if (User.IsInRole("Moderator"))
            {
                Offers offers = db.Offers.Find(id);
                ModeratedOffers offer = db.ModeratedOffers.FirstOrDefault(o => o.OfferId == id);
                if (offer != null)
                    db.ModeratedOffers.Remove(offer);
                db.Offers.Remove(offers);
                db.SaveChanges();
                return RedirectToAction("ModeratorPage");
            }
            else
                return View("AccessDenied");
        }

        public ActionResult ModeratorPage(int? page)
        {
            if (User.IsInRole("Moderator"))
            {
                int pageSize = 5;
                int pageNumber = (page ?? 1);

                ViewBag.AllOffers = db.Offers.ToList();
                List<Offers> NonModered = new List<Offers>();
                foreach( var q in db.Offers)
                {
                    var item = db2.ModeratedOffers.FirstOrDefault(c => c.OfferId == q.Id);
                    if(item==null)
                    {
                        NonModered.Add(q);
                    }
                }
                return View(NonModered.ToPagedList(pageNumber, pageSize));
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
                item.isModerated = true;
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