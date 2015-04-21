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
    public class SlopesController : ApiController
    {
        private SkiEntities db = new SkiEntities();

        // GET: api/Slopes
        public IQueryable<Slope> GetSlopes()
        {
            return db.Slopes;
        }

        // GET: api/Slopes/5
        [ResponseType(typeof(Slope))]
        public IHttpActionResult GetSlope(int id)
        {
            Slope slope = db.Slopes.Find(id);
            if (slope == null)
            {
                return NotFound();
            }

            return Ok(slope);
        }

        // PUT: api/Slopes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSlope(int id, Slope slope)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != slope.SlopeId)
            {
                return BadRequest();
            }

            db.Entry(slope).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SlopeExists(id))
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

        // POST: api/Slopes
        [ResponseType(typeof(Slope))]
        public IHttpActionResult PostSlope(Slope slope)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Slopes.Add(slope);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = slope.SlopeId }, slope);
        }

        // DELETE: api/Slopes/5
        [ResponseType(typeof(Slope))]
        public IHttpActionResult DeleteSlope(int id)
        {
            Slope slope = db.Slopes.Find(id);
            if (slope == null)
            {
                return NotFound();
            }

            db.Slopes.Remove(slope);
            db.SaveChanges();

            return Ok(slope);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SlopeExists(int id)
        {
            return db.Slopes.Count(e => e.SlopeId == id) > 0;
        }
    }
}