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
    public class OpeningHoursController : ApiController
    {
        private SkiEntities db = new SkiEntities();

        // GET: api/OpeningHours
        public IQueryable<OpeningHours> GetOpeningHours()
        {
            return db.OpeningHours;
        }

        // GET: api/OpeningHours/5
        [ResponseType(typeof(OpeningHours))]
        public IHttpActionResult GetOpeningHours(int id)
        {
            OpeningHours openingHours = db.OpeningHours.Find(id);
            if (openingHours == null)
            {
                return NotFound();
            }

            return Ok(openingHours);
        }

        // PUT: api/OpeningHours/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOpeningHours(int id, OpeningHours openingHours)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != openingHours.OpeningHoursId)
            {
                return BadRequest();
            }

            db.Entry(openingHours).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpeningHoursExists(id))
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

        // POST: api/OpeningHours
        [ResponseType(typeof(OpeningHours))]
        public IHttpActionResult PostOpeningHours(OpeningHours openingHours)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OpeningHours.Add(openingHours);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = openingHours.OpeningHoursId }, openingHours);
        }

        // DELETE: api/OpeningHours/5
        [ResponseType(typeof(OpeningHours))]
        public IHttpActionResult DeleteOpeningHours(int id)
        {
            OpeningHours openingHours = db.OpeningHours.Find(id);
            if (openingHours == null)
            {
                return NotFound();
            }

            db.OpeningHours.Remove(openingHours);
            db.SaveChanges();

            return Ok(openingHours);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OpeningHoursExists(int id)
        {
            return db.OpeningHours.Count(e => e.OpeningHoursId == id) > 0;
        }
    }
}