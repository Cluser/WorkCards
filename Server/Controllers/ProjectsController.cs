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
    public class ProjectsController : ControllerBase
    {

        #region DEPENDENCY INJECTION

        private WebAPIContext db;
        public ProjectsController(WebAPIContext context)
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
        public async Task<IActionResult> GetProjects(int? id, string name, string client, int totalHours)
        {
            IQueryable<Project> query = db.Projects.OrderByDescending(c => c.id);

            if (id > 0 && id != null)
                query = query.Where(c => c.id == id);

            if (name != null)
                query = query.Where(c => c.name == name);

            if (client != null)
                query = query.Where(c => c.client == client);

            if (totalHours != 0)
                query = query.Where(c => c.totalHours == totalHours);

            try
            {
                var projects = await query.AsNoTracking().ToListAsync();

                if (projects != null)
                    return new ObjectResult(projects);

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
        public Project Post([FromBody]Project project)
        {
            db.Projects.Add(project);
            db.SaveChanges();
            return project;
        }

        #endregion

        // DELETE api/projects
        #region HTTP DELETE
        /// <summary>Deletes existing user from system.</summary>
        /// <param name="id">ID of user</param>
        /// <response code="400" type="ErrorResponse">Bad request</response>
        // DELETE api/users/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     if (id < 0)
        //     {
        //         return BadRequest();
        //     }

        //     Project project = new Project() { id = id };
        //     db.Projects.Attach(project);
        //     db.Projects.Remove(project);
        //     db.SaveChanges();
        //     return new ObjectResult(db.Projects.ToList());
        // }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest();

                var project = await db.Projects.FindAsync(id);
                if (project == null)
                    return NotFound();

                db.Projects.Remove(project);
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
