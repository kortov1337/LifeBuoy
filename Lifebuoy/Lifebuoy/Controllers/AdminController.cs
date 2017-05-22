﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lifebuoy.DAL;
using Lifebuoy.Models;
using System.Net;

namespace Lifebuoy.Controllers
{
    public class AdminController : Controller
    {
        private OffersContext db = new OffersContext();
        private OffersContext db2 = new OffersContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult ModeratorPage()
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

        public ActionResult ModerateOffer(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModerateOffer(Offers offers)
        {
            var item = db.Offers.FirstOrDefault(c => c.Id == offers.Id);
            item.Id = offers.Id;
            item.Adress = offers.Adress;
            item.catalogID = offers.catalogID;
            item.CatalogImg = offers.CatalogImg;
            item.CatalogName = offers.CatalogName;
            item.Category = offers.Category;
            item.City = offers.City;
            item.Contacts = offers.Contacts;
            item.Description = offers.Description;
            // item.Images = images;
            item.Provider = offers.Provider;
            item.Rating = offers.Rating;
            item.ShortDescription = offers.ShortDescription;
            item.WorkingHours = offers.WorkingHours;

            db.ModeratedOffers.Add(new ModeratedOffers { OfferId = item.Id, isModerated = true });
            db.SaveChanges();

            return Redirect("ModeratorPage");
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