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
        /// <summary>
        /// Gets the slopes.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Slope> GetSlopes()
        {
            return db.Slopes.Include(s => s.SlopeDestination);
        }

        // GET: api/Slopes/5
        /// <summary>
        /// Gets a slope.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Changes/updates a slope.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="slope">The slope.</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSlope(int id, Slope slope)
        {
            //Litt høy på Lines of code(12), men kan ikke ta bort eller flytte noe.
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
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SlopeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        // POST: api/Slopes
        /// <summary>
        /// Saves a new slope.
        /// </summary>
        /// <param name="slope">The slope.</param>
        /// <returns></returns>
        [ResponseType(typeof(Slope))]
        public IHttpActionResult PostSlope(Slope slope)
        {
            //Litt høy på Lines of code(12) og lav på Maintainability Index(59), men kan ikke ta bort eller flytte noe.
            var destination = slope.SlopeDestination;
            Destination slopeDestination = db.Destinations.Find(destination.DestinationId);
            slope.SlopeDestination = slopeDestination;
            ModelState.Clear();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Slopes.Add(slope);

            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = slope.SlopeId }, slope);
            }
            catch (DataException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Slopes/5
        /// <summary>
        /// Deletes a slope.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(Slope))]
        public IHttpActionResult DeleteSlope(int id)
        {
            //Litt høy på Lines of code(11), men kan ikke ta bort eller flytte noe.
            Slope slope = db.Slopes.Find(id);
            if (slope == null)
            {
                return NotFound();
            }

            db.Slopes.Remove(slope);

            try
            {
                db.SaveChanges();
                return Ok(slope);
            }
            catch (DataException)
            {
                if (!SlopeExists(id))
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
        /// Checks if a slope exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool SlopeExists(int id)
        {
            return db.Slopes.Count(e => e.SlopeId == id) > 0;
        }
    }
}