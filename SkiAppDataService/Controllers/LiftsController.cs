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
    public class LiftsController : ApiController
    {
        private SkiEntities db = new SkiEntities();

        // GET: api/Lifts
        /// <summary>
        /// Gets the lifts.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Lift> GetLifts()
        {
            return db.Lifts.Include(l => l.LiftDestination);
        }

        // GET: api/Lifts/5
        /// <summary>
        /// Gets a lift.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(Lift))]
        public IHttpActionResult GetLift(int id)
        {
            Lift lift = db.Lifts.Find(id);
            
            if (lift == null)
            {
                return NotFound();
            }

            return Ok(lift);
        }

        // PUT: api/Lifts/5
        /// <summary>
        /// Changes/updates a lift.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="lift">The lift.</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLift(int id, Lift lift)
        {
            //Litt høy på Lines of code(12), men kan ikke ta bort eller flytte noe.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lift.LiftId)
            {
                return BadRequest();
            }

            db.Entry(lift).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiftExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        // POST: api/Lifts
        /// <summary>
        /// Saves a new lift.
        /// </summary>
        /// <param name="lift">The lift.</param>
        /// <returns></returns>
        [ResponseType(typeof(Lift))]
        public IHttpActionResult PostLift(Lift lift)
        {
            //Litt høy på Lines of code(12) og Maintainability Index(59), men kan ikke ta bort eller flytte noe.
            var destination = lift.LiftDestination;
            Destination liftDestination = db.Destinations.Find(destination.DestinationId);
            lift.LiftDestination = liftDestination;
            ModelState.Clear();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Lifts.Add(lift);

            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = lift.LiftId }, lift);
            }
            catch (DataException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Lifts/5
        /// <summary>
        /// Deletes a lift.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(Lift))]
        public IHttpActionResult DeleteLift(int id)
        {
            //Litt høy på Lines of code(11), men kan ikke ta bort eller flytte noe.
            Lift lift = db.Lifts.Find(id);
            if (lift == null)
            {
                return NotFound();
            }

            db.Lifts.Remove(lift);

            try
            {
                db.SaveChanges();
                return Ok(lift);
            }
            catch (DataException)
            {
                if (!LiftExists(id))
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
        /// Checks if a lift exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool LiftExists(int id)
        {
            return db.Lifts.Count(e => e.LiftId == id) > 0;
        }
    }
}