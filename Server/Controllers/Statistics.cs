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
    public class Statistics : ControllerBase
    {

        #region DEPENDENCY INJECTION

        private WebAPIContext db;
        public Statistics(WebAPIContext context)
        {
            this.db = context;
        }

        #endregion


        // [HttpGet("user")]
        // public int GetUserTotalHours(int userId)
        // {
        //     return db.WorkTimes.AsEnumerable().Where(c => c.idUser == userId).Sum(row => row.hours);
        // }

        // [HttpGet("userForProject")]
        // public int GetUserTotalHoursForProject(int userId, int projectId)
        // {
        //     return db.WorkTimes.AsEnumerable().Where(c => c.idUser == userId && c.idProject == projectId).Sum(row => row.hours);;
        // }

        [HttpGet("UserTotalHours")]
        public async Task<IActionResult> GetUserTotalHours()
        {
            List<StatisticUser> _statisticsUser = new List<StatisticUser>();
            

            try
            {
                foreach(User user in await db.Users.ToAsyncEnumerable().ToList()) {
                    List<Statistic> _statistics = new List<Statistic>();
                    foreach(Project project in await db.Projects.ToAsyncEnumerable().ToList()) {
                        if ( db.WorkTimes.AsEnumerable().Where(c => c.idUser == user.id && c.idProject == project.id).Sum(row => row.hours) > 0)
                        {
                            Statistic statistic = new Statistic();
                            statistic.project = project;
                            statistic.totalHours = db.WorkTimes.AsEnumerable().Where(c => c.idUser == user.id && c.idProject == project.id).Sum(row => row.hours);
                            _statistics.Add(statistic);
                        }
                    }
                    StatisticUser statisticUser = new StatisticUser();
                    statisticUser.usertId = user.id;
                    statisticUser.statistic = _statistics;
                    _statisticsUser.Add(statisticUser);
                }
                    
                if (_statisticsUser != null)
                    return new ObjectResult(_statisticsUser);

                return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpGet("ProjectTotalHours")]
        public async Task<IActionResult> GetProjectTotalHours()
        {
            List<Statistic> _statistics = new List<Statistic>();

            try
            {
                foreach(Project project in await db.Projects.ToAsyncEnumerable().ToList()) {
                    Statistic statistic = new Statistic();
                    statistic.project = project;
                    statistic.totalHours = db.WorkTimes.AsEnumerable().Where(c => c.idProject == project.id).Sum(row => row.hours);
                    _statistics.Add(statistic);
                }
                    
                if (_statistics != null)
                    return new ObjectResult(_statistics);

                return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}