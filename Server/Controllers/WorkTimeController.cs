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
    public class WorkTimeController : ControllerBase
    {

        #region DEPENDENCY INJECTION

        private WebAPIContext db;
        public WorkTimeController(WebAPIContext context)
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
        public async Task<IActionResult> GetWorkTimes(int? id, string project, string client, string type, int hours, DateTime dateBegin, DateTime? dateFinish)
        {
            IQueryable<WorkTime> query = db.WorkTimes.OrderByDescending(c => c.id);

            if (id > 0 && id != null)
                query = query.Where(c => c.id == id);

            if (project != null)
                query = query.Where(c => c.project.name == project);

            if (client != null)
                query = query.Where(c => c.project.client == client);

            if (type != null)
                query = query.Where(c => c.type == type);

            if (dateBegin != null)
                query = query.Where(c => c.date >= dateBegin);

            if (dateFinish != null)
                query = query.Where(c => c.date <= dateFinish);

            try
            {
                var workTimes = await query.AsNoTracking().Include(c=>c.user).Include(c=>c.project).ToListAsync();

                if (workTimes != null)
                    return new ObjectResult(workTimes);

                return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region HTTP POST 
        
        [HttpPost]
        public async Task<IActionResult> PostWorkTime([FromBody] WorkTime workTime)
        {
            try
            {
                bool isIDUserCorrect = await db.Users.AnyAsync(i => i.id == workTime.idUser);
                bool isIDProjectCorrect = await db.Projects.AnyAsync(i => i.id == workTime.idProject);
                bool isIDworkTimeExist = await db.WorkTimes.AnyAsync(i => i.id == workTime.id);

                if (!isIDUserCorrect || !isIDProjectCorrect || isIDworkTimeExist)
                    return BadRequest();

                db.WorkTimes.Add(workTime);
                if(workTime.user != null)
                    db.Entry(workTime.user).State = EntityState.Unchanged;
                if (workTime.project != null)
                    db.Entry(workTime.project).State = EntityState.Unchanged;

                await db.SaveChangesAsync();

                return CreatedAtAction("GetWorkTimes", new { id = workTime.id }, workTime);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        
        #endregion

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkTime([FromRoute] int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest();

                var workTime = await db.WorkTimes.FindAsync(id);
                if (workTime == null) 
                    return NotFound();

                db.WorkTimes.Remove(workTime);
                await db.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}