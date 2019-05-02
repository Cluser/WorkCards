using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Statistic
    {
        public Project project { get; set; }
        public int totalHours { get; set; }
    }
}