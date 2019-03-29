using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class WorkTime
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("user")]
        public int idUser { get; set; }
        public User user { get; set; }
        
        [Required]
        public DateTime date { get; set; }

        [ForeignKey("project")]
        public int idProject { get; set; }
        public Project project { get; set; }

        [Required]
        public int hours { get; set; }

        [Required]
        public string type { get; set; }

        public string comment { get; set; }
    }
}