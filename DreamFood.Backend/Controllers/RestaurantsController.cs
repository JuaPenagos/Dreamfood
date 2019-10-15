using DreamFood.Backend.Helpers;
using DreamFood.Backend.Models;
using DreamFood.Common.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamFood.Backend.Controllers
{
    public class RestaurantsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Restaurants
        public async Task<ActionResult> Index()
        {
            return View(await db.Restaurants.ToListAsync());
        }

        // GET: Restaurants/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = await db.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // GET: Restaurants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RestaurantView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Restaurants";

                if (view.ImageFileMenu != null)
                {
                    pic = FileHelper.UploadPhoto(view.ImageFileMenu, folder);
                    pic = $"{folder}/{pic}";
                }
                var restaurant = this.ToRestaurant(view,pic);
                db.Restaurants.Add(restaurant);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        private Restaurant ToRestaurant(RestaurantView view,string pic)
        {
            return new Restaurant
            {
                IdRestaurant = view.IdRestaurant,
                Name = view.Name,
                Type = view.Type,
                Remarks = view.Remarks,
                Phone = view.Phone,
                ImagePathMenu = pic,
                Address = view.Address,
                Latitude = view.Latitude,
                Longitude = view.Longitude
            };
        }

        // GET: Restaurants/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = await db.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            var view = this.ToView(restaurant);

            return View(view);
        }

        private RestaurantView ToView(Restaurant restaurant)
        {
            return new RestaurantView
            {
                IdRestaurant = restaurant.IdRestaurant,
                Name = restaurant.Name,
                Type = restaurant.Type,
                Remarks = restaurant.Remarks,
                Phone = restaurant.Phone,
                ImagePathMenu = restaurant.ImagePathMenu,
                Address = restaurant.Address,
                Latitude = restaurant.Latitude,
                Longitude = restaurant.Longitude
            };
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RestaurantView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.ImagePathMenu;
                var folder = "~/Content/Restaurants";

                if (view.ImageFileMenu != null)
                {
                    pic = FileHelper.UploadPhoto(view.ImageFileMenu, folder);
                    pic = $"{folder}/{pic}";
                }

                var restaurant = this.ToRestaurant(view, pic);
                db.Entry(restaurant).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(view);
        }

        // GET: Restaurants/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = await db.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Restaurant restaurant = await db.Restaurants.FindAsync(id);
            db.Restaurants.Remove(restaurant);
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
