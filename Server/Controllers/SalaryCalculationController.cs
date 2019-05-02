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
    public class SalaryCalculationController : ControllerBase
    {

        #region DEPENDENCY INJECTION

        private WebAPIContext db;
        public SalaryCalculationController(WebAPIContext context)
        {
            this.db = context;
        }

        #endregion

        [HttpGet("GetSalary")]
        public async Task<IActionResult> GetSalary(int userId, int workHourValue) {
            List<int> _projectHours = new List<int>();

            foreach (Project project in await db.Projects.ToAsyncEnumerable().ToList()) {
                _projectHours.Add(db.WorkTimes.AsEnumerable().Where(c => c.idUser == userId && c.idProject == project.id).Sum(row => row.hours));
            }



            if (_projectHours != null)
                return new ObjectResult(_projectHours.Sum() * workHourValue);

            return NotFound();
        }

  
    }
}
