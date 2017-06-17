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
        private ApplicationDbContext adb = new ApplicationDbContext();

        public ActionResult Index(string categories, string providers, string cities)
        {

            ViewBag.SkinDetails = db.SkinsDetails.ToList();
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

            var category =  offers
                .OrderBy(c => c.Category)
                .Select(c => c.Category).Distinct().ToList();

            var provider = offers
                .OrderBy(c => c.Provider)
                .Select(c => c.Provider).Distinct().ToList();



            city.Insert(0, "Не выбрано");
            provider.Insert(0, "Не выбрано");
            category.Insert(0, "Не выбрано");
            offers = offers.OrderByDescending(o => o.Rating);

            OffersListView olv = new OffersListView
            {
                Offers = offers.ToList(),

                Categories = new SelectList(category),

                Providers = new SelectList(provider),

                Cities = new SelectList(city)

            };

            return View(olv);
        }

        public ActionResult ShowOffer(int? id)
        {
            List<String> Images = new List<string>();
            string[] splitResult;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offers offer = db.Offers.Find(id);
            splitResult = offer.Images.Split(';');
            Images = splitResult.ToList();
            Images.RemoveAt(Images.Count - 1);
            if (offer == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Images = Images;
            ViewBag.SkinDetails = db.SkinsDetails.FirstOrDefault(sk => sk.OfferId == id);

            return View(offer);
        }

        [HttpPost]
        public ActionResult SendRating(int id, int rating)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userVote = db.UsersVotes.FirstOrDefault(uv => uv.UserName == User.Identity.Name & uv.OfferId == id);
                if (userVote == null)
                {
                    var offer = db.Offers.Find(id);
                    if (offer != null)
                    {
                        if (offer.Rating == 0)
                        {
                            offer.Rating = rating;
                        }
                        else
                            offer.Rating = Convert.ToInt32(Math.Ceiling(Convert.ToDouble((offer.Rating + rating) / 2)));

                        db.UsersVotes.Add(new UsersVotes { OfferId = id, UserName = User.Identity.Name });
                        offer.VotesCount += 1;
                        db.SaveChanges();
                        return Json(new { success = true, responseText = "Ваш голос учтён." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { success = false, responseText = "Призошла ошибка." }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { success = false, responseText = "Вы уже голосовали." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { success = false, responseText = "Авторизуйтесь для голосования" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddReview(int OfferId, string name, string title, string review)
        {
            db.UsersReviews.Add(new Reviews { OfferId = OfferId, UserId = User.Identity.Name, Review = review, Author = name, Title = title, Date = DateTime.Now});
            db.SaveChanges();

            var reviews = db.UsersReviews.Where(rv => rv.OfferId == OfferId)
                .OrderByDescending(rv => rv.Date)
                .ToList();
            return PartialView("AddReview",reviews);
        }

        public ActionResult LoadReviews(int id)
        {
            var reviews = db.UsersReviews.Where(rv => rv.OfferId == id)
                 .OrderByDescending(rv => rv.Date)
                 .ToList();
            return PartialView("AddReview", reviews);
        }

        public ActionResult DeleteReview (int id)
        {
            var review = db.UsersReviews.Find(id);
            if (review != null)
            {
                var offerId = review.OfferId;
                db.UsersReviews.Remove(review);
                db.SaveChanges();

                return RedirectToAction("ShowOffer", new { id = offerId });
            }
            else
                return HttpNotFound();

        }
    }
}