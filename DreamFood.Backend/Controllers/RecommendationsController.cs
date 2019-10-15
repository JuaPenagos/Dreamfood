using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DreamFood.Backend.Models;
using DreamFood.Common.Models;
using DreamFood.Backend.Helpers;

namespace DreamFood.Backend.Controllers
{
    public class RecommendationsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Recommendations
        public async Task<ActionResult> Index()
        {
            var recommendations = db.Recommendations.Include(r => r.Restaurant);
            return View(await recommendations.ToListAsync());
        }

        // GET: Recommendations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recommendation recommendation = await db.Recommendations.FindAsync(id);
            if (recommendation == null)
            {
                return HttpNotFound();
            }
            return View(recommendation);
        }

        // GET: Recommendations/Create
        public ActionResult Create()
        {
            ViewBag.IdRestaurant = new SelectList(db.Restaurants, "IdRestaurant", "Name");
            return View();
        }

        // POST: Recommendations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RecommendationView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Recommendations";

                if (view.ImageFileRecomm != null)
                {
                    pic = FileHelper.UploadPhoto(view.ImageFileRecomm, folder);
                    pic = $"{folder}/{pic}";
                }
                var recommendation = this.ToRecommendation(view, pic);

                db.Recommendations.Add(recommendation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdRestaurant = new SelectList(db.Restaurants, "IdRestaurant", "Name", view.IdRestaurant);
            return View(view);
        }

        private Recommendation ToRecommendation(RecommendationView view, string pic)
        {
            return new Recommendation
            {
                IdRecommendation = view.IdRecommendation,
                IdRestaurant = view.IdRestaurant,
                RecommendationUser = view.RecommendationUser,
                DateRecomm = view.DateRecomm,
                ImagePathRecomm = pic,
                Score = view.Score,
                UserId=view.UserId
            };
        }

        // GET: Recommendations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recommendation recommendation = await db.Recommendations.FindAsync(id);
            if (recommendation == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRestaurant = new SelectList(db.Restaurants, "IdRestaurant", "Name", recommendation.IdRestaurant);

            var view = this.ToView(recommendation);
            return View(view);
        }

        private RecommendationView ToView(Recommendation recommendation)
        {
            return new RecommendationView
            {
                IdRecommendation = recommendation.IdRecommendation,
                IdRestaurant = recommendation.IdRestaurant,
                RecommendationUser = recommendation.RecommendationUser,
                DateRecomm = recommendation.DateRecomm,
                ImagePathRecomm = recommendation.ImagePathRecomm,
                Score = recommendation.Score,
                UserId=recommendation.UserId
            };
          
        }

        // POST: Recommendations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RecommendationView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.ImagePathRecomm;
                var folder = "~/Content/Recommendations";

                if (view.ImageFileRecomm != null)
                {
                    pic = FileHelper.UploadPhoto(view.ImageFileRecomm, folder);
                    pic = $"{folder}/{pic}";
                }
                var recommendation = this.ToRecommendation(view, pic);

                db.Entry(recommendation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdRestaurant = new SelectList(db.Restaurants, "IdRestaurant", "Name", view.IdRestaurant);
            return View(view);
        }

        // GET: Recommendations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recommendation recommendation = await db.Recommendations.FindAsync(id);
            if (recommendation == null)
            {
                return HttpNotFound();
            }
            return View(recommendation);
        }

        // POST: Recommendations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Recommendation recommendation = await db.Recommendations.FindAsync(id);
            db.Recommendations.Remove(recommendation);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
