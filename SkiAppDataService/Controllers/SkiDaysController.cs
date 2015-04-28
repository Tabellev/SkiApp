using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using skiAppDatamodel;

namespace SkiAppDataService.Controllers
{
    public class SkiDaysController : ApiController
    {
        private SkiEntities db = new SkiEntities();

        // GET: api/SkiDays
        /// <summary>
        /// Gets the skidays.
        /// </summary>
        /// <returns></returns>
        public IQueryable<SkiDay> GetSkidays()
        {
            return db.SkiDays.Include(s => s.Lifts).Include(s => s.Slopes).Include(s => s.SkiDayUser);
        }

        // GET: api/SkiDays/5
        /// <summary>
        /// Gets a ski day.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(SkiDay))]
        public IHttpActionResult GetSkiDay(int id)
        {
            SkiDay skiDay = db.SkiDays.Find(id);
            if (skiDay == null)
            {
                return NotFound();
            }
            db.Entry(skiDay).Collection(b => b.Lifts).Load(); 
            return Ok(skiDay);
        }

        // PUT: api/SkiDays/5
        /// <summary>
        /// Changes/updates a ski day.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="skiDay">The ski day.</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSkiDay(int id, SkiDay skiDay)
        {
            //Litt høy på Lines of code(12), men kan ikke ta bort eller flytte noe.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != skiDay.SkiDayId)
            {
                return BadRequest();
            }

            db.Entry(skiDay).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkiDayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        // POST: api/SkiDays
        /// <summary>
        /// Saves a new ski day.
        /// </summary>
        /// <param name="skiDay">The ski day.</param>
        /// <returns></returns>
        [ResponseType(typeof(SkiDay))]
        public IHttpActionResult PostSkiDay(SkiDay skiDay)
        {
            //Høy på Lines of code(14) og litt lav på Maintainability Index(57). Har flyttet ut det jeg kan i egne metoder.
            var user = skiDay.SkiDayUser;
            User skiDayUser = db.Users.Find(user.UserId);
            skiDay.SkiDayUser = skiDayUser;

            AddSkiDayLifts(skiDay);
            AddSkiDaySlopes(skiDay);
           
            ModelState.Clear();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SkiDays.Add(skiDay);

            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = skiDay.SkiDayId }, skiDay);
            }
            catch (DataException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/SkiDays/5
        /// <summary>
        /// Deletes a ski day.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(SkiDay))]
        public IHttpActionResult DeleteSkiDay(int id)
        {
            //Litt høy på Lines of code(11), men kan ikke ta bort eller flytte noe.
            SkiDay skiDay = db.SkiDays.Find(id);
            if (skiDay == null)
            {
                return NotFound();
            }

            db.SkiDays.Remove(skiDay);

            try
            {
                db.SaveChanges();
                return Ok(skiDay);
            }
            catch (DataException)
            {
                if (!SkiDayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks if a ski day exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool SkiDayExists(int id)
        {
            return db.SkiDays.Count(e => e.SkiDayId == id) > 0;
        }

        private void AddSkiDayLifts(SkiDay skiDay)
        {
            var lifts = skiDay.Lifts.ToList<Lift>();
            skiDay.Lifts.Clear();

            foreach (var l in lifts)
            {
                Lift lift = db.Lifts.Find(l.LiftId);
                skiDay.Lifts.Add(lift);
            }
        }

        private void AddSkiDaySlopes(SkiDay skiDay)
        {
            var slopes = skiDay.Slopes.ToList<Slope>();
            skiDay.Slopes.Clear();

            foreach (var s in slopes)
            {
                Slope slope = db.Slopes.Find(s.SlopeId);
                skiDay.Slopes.Add(slope);
            }
        }
    }
}