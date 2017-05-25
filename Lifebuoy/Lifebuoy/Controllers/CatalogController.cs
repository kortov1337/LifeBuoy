using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lifebuoy.DAL;
using Lifebuoy.Models;
using System.Net;

namespace Lifebuoy.Controllers
{
    public class CatalogController : Controller
    {
        private OffersContext db = new OffersContext();
        private OffersContext db1 = new OffersContext();

        public ActionResult Index(string categories, string providers, string cities)
        {
            IQueryable<Offers> offers = db.Offers;
            List<Offers> moderatedOffers = new List<Offers>();
            List<int> offIds = new List<int>();
            var mOffers = db.ModeratedOffers;
            foreach ( var q in mOffers)
            {
                offIds.Add(q.OfferId);
            }
           
            foreach (var q in offIds)
            {
                var offer = offers.FirstOrDefault(o => o.Id == q);
                if(offer!=null)
                    moderatedOffers.Add(offer);
            }
            offers = moderatedOffers.AsQueryable();

            if (!String.IsNullOrEmpty(categories)&&!categories.Equals("Не выбрано"))
            {
                offers = offers.Where(o => o.Category == categories);
            }

            if (!String.IsNullOrEmpty(providers) && !providers.Equals("Не выбрано"))
            {
                offers = offers.Where(o => o.Provider == providers);
            }

            if (!String.IsNullOrEmpty(cities) && !cities.Equals("Не выбрано"))
            {
                offers = offers.Where(o => o.City == cities);
            }

           
            var city = db.Offers
                .OrderBy(c => c.City)
                .Select(c => c.City).Distinct().ToList();

            var category = db.Offers
                .OrderBy(c => c.Category)
                .Select(c => c.Category).Distinct().ToList();

            var provider = db.Offers
                .OrderBy(c => c.Provider)
                .Select(c => c.Provider).Distinct().ToList();

            city.Insert(0, "Не выбрано");
            provider.Insert(0, "Не выбрано");
            category.Insert(0, "Не выбрано");

            OffersListView olv = new OffersListView
            {
                Offers = offers.ToList(),

                Categories = new SelectList(category),

                Providers = new SelectList(provider),

                Cities = new SelectList(city)

            };

            return View(olv);
        }
        public ActionResult ShowOffer(string id)
        {
            List<String> Images = new List<string>();
            string[] splitResult;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           // IQueryable<Offers> offer = db.Offers.Where(a => a.catalogID == id);
            Offers offer = db.Offers.FirstOrDefault(a => a.catalogID == id);
            splitResult = offer.Images.Split(';');
            Images = splitResult.ToList();
            Images.RemoveAt(Images.Count - 1);
            if (offer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Images = Images;
            return View(offer);
        }
    }
}