﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lifebuoy.DAL;
using Lifebuoy.Models;
using System.IO;
using System.Drawing;

namespace Lifebuoy.Controllers
{
    public class ConstructorController : Controller
    {
        private OffersContext db = new OffersContext();

        public ActionResult Dashboard()
        {
              if(User.IsInRole("Provider"))
                {
                    var userName = User.Identity.Name;
                    IQueryable<ProvidersOffers> providerOffers = db.ProviderOffers;
                    List<Offers> offers = new List<Offers>();
                    List<int> offIds = new List<int>();

                    providerOffers = providerOffers.Where(o => o.Owner == User.Identity.Name);
                    foreach (var q in providerOffers)
                    {
                        offIds.Add(q.OfferId);
                    }

                    foreach (var q in offIds)
                    {
                        var offer = db.Offers.FirstOrDefault(o => o.Id == q);
                        if (offer != null)
                            offers.Add(offer);
                    }

                    return View(offers);
                }
                else
                    return View("AccessDenied");
        }
        [HttpGet]
        public ActionResult SkinSettings(int? id)
        {
            List<String> Images = new List<string>();
            string[] splitResult;

            var offer = db.Offers.Find(id);
            if (offer!=null)
            {
                splitResult = offer.Images.Split(';');
                Images = splitResult.ToList();
                Images.RemoveAt(Images.Count - 1);
                ViewBag.Images = Images;

                return View(offer);
            }
                
            else
                return HttpNotFound();
        }
   
         [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SkinSettings(FormCollection fc)
         {
            string photo_Display_Type = fc.Get("pic");
            string background = fc.Get("backgr");
            string hot = fc.Get("hot");
            string hightlight = fc.Get("hightlight");
            int offerId = Convert.ToInt32(fc.Get("offerId"));
            var sd = db.SkinsDetails.FirstOrDefault(d => d.OfferId == offerId);
            if (sd == null)
            {
                db.SkinsDetails.Add(new SkinDetails
                {
                    photoDisplayMode = photo_Display_Type,
                    background = background,
                    hotOffer = hot,
                    hightlight = hightlight,
                    OfferId = offerId
                });
            }
            else
            { 
                sd.photoDisplayMode = photo_Display_Type;
                sd.background = background;
                sd.hotOffer = hot;
                sd.hightlight = hightlight;
                sd.OfferId = offerId;
            }
            db.SaveChanges();
            return RedirectToAction("Dashboard");
         }

        public ActionResult Details(int? id)
        {
            if (User.IsInRole("Provider"))
            {
                if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                Offers offers = db.Offers.Find(id);
                if (offers == null)
                    {
                        return HttpNotFound();
                    }
                return View(offers);
            }
            else
                return View("AccessDenied");
        }

        public ActionResult FillData()
        {
            if (User.IsInRole("Provider"))
            {
                return View();
            }
            else
                return View("AccessDenied");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FillData(IEnumerable<HttpPostedFileBase> files, Offers offers)
        {
            if (User.IsInRole("Provider"))
            {
                if (ModelState.IsValid)
                {
                    string images = "";

                    if (files != null)
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            byte[] imageData = null;
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                imageData = binaryReader.ReadBytes(file.ContentLength);
                            }
                            images += Convert.ToBase64String(imageData) + ";";
                        }
                    }
                    var id = offers.Provider.Split('@')[0] + "_" + offers.Category+"_" + offers.CatalogName;
                    offers.catalogID = Transliter.GetTranslit(id);
                    offers.Images = images;
                    db.Offers.Add(offers);
                    db.SaveChanges();
                    db.ProviderOffers.Add(new ProvidersOffers { OfferId = offers.Id, Owner = User.Identity.Name });
                    db.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
                return View(offers);
            }
            else
                return View("AccessDenied");
        }

        public ActionResult Edit(int? id)
        {
            if (User.IsInRole("Provider"))
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
        public ActionResult Edit(IEnumerable<HttpPostedFileBase> files, Offers offers)
        {
            if (User.IsInRole("Provider"))
            {
                if (ModelState.IsValid)
                {
                    var item = db.Offers.FirstOrDefault(c => c.Id == offers.Id);
                    string images = "";

                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                byte[] imageData = null;
                                using (var binaryReader = new BinaryReader(file.InputStream))
                                {
                                    imageData = binaryReader.ReadBytes(file.ContentLength);
                                }
                                images += Convert.ToBase64String(imageData) + ";";
                            }
                        }
                    }

                    item.Id = offers.Id;
                    item.Adress = offers.Adress;
                    item.catalogID = offers.catalogID;
                    item.CatalogImg = offers.CatalogImg;              
                    item.CatalogName = offers.CatalogName;
                    item.Category = offers.Category;
                    item.City = offers.City;
                    item.Contacts = offers.Contacts;
                    item.Description = offers.Description;
                    item.Provider = offers.Provider;
                    item.Prices = offers.Prices;
                    item.Rating = offers.Rating;
                    item.ShortDescription = offers.ShortDescription;
                    item.WorkingHours = offers.WorkingHours;
                    item.isModerated = false;
                    var tmp = db.ModeratedOffers.FirstOrDefault(o => o.OfferId == item.Id);
                    if(tmp!=null)
                        db.ModeratedOffers.Remove(tmp);

                    db.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
            return View(offers);
            }
            else
                return View("AccessDenied");
        }

        public ActionResult Delete(int? id)
        {
            if (User.IsInRole("Provider"))
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("Provider"))
            {
                Offers offers = db.Offers.Find(id);
                ModeratedOffers offer = db.ModeratedOffers.FirstOrDefault(o => o.OfferId == id);
                if (offer != null)
                    db.ModeratedOffers.Remove(offer);
                db.Offers.Remove(offers);
                db.SaveChanges();
                return RedirectToAction("Dashboard");
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
