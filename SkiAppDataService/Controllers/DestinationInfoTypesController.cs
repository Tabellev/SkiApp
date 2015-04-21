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
    public class DestinationInfoTypesController : ApiController
    {
        private SkiEntities db = new SkiEntities();

        // GET: api/DestinationInfoTypes
        public IQueryable<DestinationInfoType> GetDestinationInfoTypes()
        {
            return db.DestinationInfoTypes;
        }

        // GET: api/DestinationInfoTypes/5
        [ResponseType(typeof(DestinationInfoType))]
        public IHttpActionResult GetDestinationInfoType(int id)
        {
            DestinationInfoType destinationInfoType = db.DestinationInfoTypes.Find(id);
            if (destinationInfoType == null)
            {
                return NotFound();
            }

            return Ok(destinationInfoType);
        }

        // PUT: api/DestinationInfoTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDestinationInfoType(int id, DestinationInfoType destinationInfoType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != destinationInfoType.DestinationInfoTypeId)
            {
                return BadRequest();
            }

            db.Entry(destinationInfoType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationInfoTypeExists(id))
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

        // POST: api/DestinationInfoTypes
        [ResponseType(typeof(DestinationInfoType))]
        public IHttpActionResult PostDestinationInfoType(DestinationInfoType destinationInfoType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DestinationInfoTypes.Add(destinationInfoType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = destinationInfoType.DestinationInfoTypeId }, destinationInfoType);
        }

        // DELETE: api/DestinationInfoTypes/5
        [ResponseType(typeof(DestinationInfoType))]
        public IHttpActionResult DeleteDestinationInfoType(int id)
        {
            DestinationInfoType destinationInfoType = db.DestinationInfoTypes.Find(id);
            if (destinationInfoType == null)
            {
                return NotFound();
            }

            db.DestinationInfoTypes.Remove(destinationInfoType);
            db.SaveChanges();

            return Ok(destinationInfoType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DestinationInfoTypeExists(int id)
        {
            return db.DestinationInfoTypes.Count(e => e.DestinationInfoTypeId == id) > 0;
        }
    }
}