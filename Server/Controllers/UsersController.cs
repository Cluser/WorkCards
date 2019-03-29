using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        #region DEPENDENCY INJECTION

        private WebAPIContext db;
        public UsersController(WebAPIContext context)
        {
            this.db = context;
        }

        #endregion

        // GET api/projects
        #region HTTP GET
        /// <summary>Returns projects registred in system.</summary>
        /// <param name="id">ID of project</param>
        /// <param name="name">Project name</param>
        /// <param name="client">Client name</param>
        /// <param name="totalHours">Total hours in projects</param>
        [HttpGet]
        public async Task<IActionResult> GetUsers(int? id, string name, string surname, string department)
        {
            IQueryable<User> query = db.Users.OrderByDescending(c => c.id);

            if (id > 0 && id != null)
                query = query.Where(c => c.id == id);

            if (name != null)
                query = query.Where(c => c.name == name);

            if (surname != null)
                query = query.Where(c => c.surname == surname);

            if (department != null)
                query = query.Where(c => c.department == department);

            try
            {
                var users = await query.AsNoTracking().ToListAsync();

                if (users != null)
                    return new ObjectResult(users);

                return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        // POST api/projects
        #region HTTP POST
        /// <summary>Adds new user to system.</summary>
        // POST api/users/
        [HttpPost]
        public User Post([FromBody]User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }

        #endregion

        // DELETE api/projects
        #region HTTP DELETE
        /// <summary>Deletes existing user from system.</summary>
        /// <param name="id">ID of user</param>
        /// <response code="400" type="ErrorResponse">Bad request</response>
        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest();

                var user = await db.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        #endregion
    }
}