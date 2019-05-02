using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class StatisticUser
    {
        public int usertId { get; set; }
        public ICollection<Statistic> statistic { get; set; }
    }
}