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
    public class DestinationsController : ApiController
    {
        private SkiEntities db = new SkiEntities();

        // GET: api/Destinations
        /// <summary>
        /// Gets the destinations.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Destination> GetDestinations()
        {
            return db.Destinations;
        }

        // GET: api/Destinations/5
        /// <summary>
        /// Gets a destination.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(Destination))]
        public IHttpActionResult GetDestination(int id)
        {
            Destination destination = db.Destinations.Find(id);
            if (destination == null)
            {
                return NotFound();
            }

            return Ok(destination);
        }

        // PUT: api/Destinations/5
        /// <summary>
        /// Changes/updates a destination.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDestination(int id, Destination destination)
        {
            //Er litt høy på Lines of code(12), men ingentin jeg kan ta bort eller flytte.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != destination.DestinationId)
            {
                return BadRequest();
            }

            db.Entry(destination).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DataException)
            {
                if (!DestinationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        // POST: api/Destinations
        /// <summary>
        /// Saves a new destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        [ResponseType(typeof(Destination))]
        public IHttpActionResult PostDestination(Destination destination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Destinations.Add(destination);

            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = destination.DestinationId }, destination);
            }
            catch (DataException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Destinations/5
        /// <summary>
        /// Deletes a destination.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(Destination))]
        public IHttpActionResult DeleteDestination(int id)
        {
            //Litt høy på Lines of code(11), men kan ikke ta bort eller flytte noe.
            Destination destination = db.Destinations.Find(id);
            if (destination == null)
            {
                return NotFound();
            }

            db.Destinations.Remove(destination);

            try
            {
                db.SaveChanges();
                return Ok(destination);
            }
            catch (DataException)
            {
                if (!DestinationExists(id))
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
        /// Checks if a destination exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool DestinationExists(int id)
        {
            return db.Destinations.Count(e => e.DestinationId == id) > 0;
        }
    }
}