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
    public class UsersController : ApiController
    {
        private SkiEntities db = new SkiEntities();

        // GET: api/Users
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        /// <summary>
        /// Gets a user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        /// <summary>
        /// Changes/updates a user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            //Litt høy på Lines of code(12), men kan ikke ta bort eller flytte noe.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }
        }

        // POST: api/Users
        /// <summary>
        /// Saves a new user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
            }
            catch (DataException)
            {
                return BadRequest();
            }
            
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            //Litt høy på Lines of code(11), men kan ikke ta bort eller flytte noe.
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);

            try
            {
                db.SaveChanges();
                return Ok(user);
            }
            catch (DataException)
            {
                if (!UserExists(id))
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
        /// Checks if a user exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}