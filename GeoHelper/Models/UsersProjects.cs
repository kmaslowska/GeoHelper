using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class UsersProjects
    {
        public int ID { get; set; }
        [Display(Name = "Id projektu")]
        public int projectId { get; set; }
        [Display(Name = "Użytkownik")]
        public String user { get; set; }
        [Display(Name = "Czy główny")]
        public bool leading { get; set; }
    }
}
