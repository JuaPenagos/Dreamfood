
namespace DreamFood.API.Controllers
{
    using DreamFood.API.Helpers;
    using DreamFood.Common.Models;
    using DreamFood.Domain.Models;
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    [Authorize]
    public class RecommendationsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Recommendations
        public IQueryable<Recommendation> GetRecommendations()
        {
            return db.Recommendations.OrderBy(p=>p.DateRecomm);
        }

        // GET: api/Recommendations/5
        [ResponseType(typeof(Recommendation))]
        public async Task<IHttpActionResult> GetRecommendation(int id)
        {
            var recommendation = await this.db.Recommendations.Where(p => p.IdRestaurant == id).
                ToListAsync();

            return Ok(recommendation);
        }

        // PUT: api/Recommendations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRecommendation(int id, Recommendation recommendation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recommendation.IdRecommendation)
            {
                return BadRequest();
            }

            if (recommendation.ImageArray != null && recommendation.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(recommendation.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Recommendations";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);
                if (response) { recommendation.ImagePathRecomm = fullPath; }
            }

            this.db.Entry(recommendation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecommendationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(recommendation);
        }

        // POST: api/Recommendations
        [ResponseType(typeof(Recommendation))]
        public async Task<IHttpActionResult> PostRecommendation(Recommendation recommendation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (recommendation.ImageArray != null && recommendation.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(recommendation.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Recommendations";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);
                if (response) { recommendation.ImagePathRecomm = fullPath; }
            }

            this.db.Recommendations.Add(recommendation);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = recommendation.IdRecommendation }, recommendation);
        }

        // DELETE: api/Recommendations/5
        [ResponseType(typeof(Recommendation))]
        public async Task<IHttpActionResult> DeleteRecommendation(int id)
        {
            Recommendation recommendation = await db.Recommendations.FindAsync(id);
            if (recommendation == null)
            {
                return NotFound();
            }

            db.Recommendations.Remove(recommendation);
            await db.SaveChangesAsync();

            return Ok(recommendation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RecommendationExists(int id)
        {
            return db.Recommendations.Count(e => e.IdRecommendation == id) > 0;
        }
    }
}