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
        public IQueryable<SkiDay> GetSkidays()
        {
            return db.SkiDays;
        }

        // GET: api/SkiDays/5
        [ResponseType(typeof(SkiDay))]
        public IHttpActionResult GetSkiDay(int id)
        {
            SkiDay skiDay = db.SkiDays.Find(id);
            if (skiDay == null)
            {
                return NotFound();
            }

            return Ok(skiDay);
        }

        // PUT: api/SkiDays/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSkiDay(int id, SkiDay skiDay)
        {
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
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkiDayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SkiDays
        [ResponseType(typeof(SkiDay))]
        public IHttpActionResult PostSkiDay(SkiDay skiDay)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SkiDays.Add(skiDay);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = skiDay.SkiDayId }, skiDay);
        }

        // DELETE: api/SkiDays/5
        [ResponseType(typeof(SkiDay))]
        public IHttpActionResult DeleteSkiDay(int id)
        {
            SkiDay skiDay = db.SkiDays.Find(id);
            if (skiDay == null)
            {
                return NotFound();
            }

            db.SkiDays.Remove(skiDay);
            db.SaveChanges();

            return Ok(skiDay);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SkiDayExists(int id)
        {
            return db.SkiDays.Count(e => e.SkiDayId == id) > 0;
        }
    }
}