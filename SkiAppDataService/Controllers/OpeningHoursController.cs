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
        /// <summary>
        /// Gets the opening hours.
        /// </summary>
        /// <returns></returns>
        public IQueryable<OpeningHours> GetOpeningHours()
        {
            return db.OpeningHours;
        }

        // GET: api/OpeningHours/5
        /// <summary>
        /// Gets a opening hours.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Changes/updates a opening hours.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="openingHours">The opening hours.</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOpeningHours(int id, OpeningHours openingHours)
        {
            //Litt høy på Lines of code(12), men kan ikke ta bort eller flytte noe.
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
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpeningHoursExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }
        }

        // POST: api/OpeningHours
        /// <summary>
        /// Saves a new opening hours.
        /// </summary>
        /// <param name="openingHours">The opening hours.</param>
        /// <returns></returns>
        [ResponseType(typeof(OpeningHours))]
        public IHttpActionResult PostOpeningHours(OpeningHours openingHours)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OpeningHours.Add(openingHours);

            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = openingHours.OpeningHoursId }, openingHours);
            }
            catch (DataException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/OpeningHours/5
        /// <summary>
        /// Deletes a opening hours.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(OpeningHours))]
        public IHttpActionResult DeleteOpeningHours(int id)
        {
            //Litt høy på Lines of code(11), men kan ikke ta bort eller flytte noe.
            OpeningHours openingHours = db.OpeningHours.Find(id);
            if (openingHours == null)
            {
                return NotFound();
            }

            db.OpeningHours.Remove(openingHours);

            try
            {
                db.SaveChanges();
                return Ok(openingHours);
            }
            catch (DataException)
            {
                if (!OpeningHoursExists(id))
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
        /// Checks if a opening hours exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool OpeningHoursExists(int id)
        {
            return db.OpeningHours.Count(e => e.OpeningHoursId == id) > 0;
        }
    }
}