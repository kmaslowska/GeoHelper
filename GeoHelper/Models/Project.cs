using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class Project
    {
        public int ID { get; set; }
        [Display(Name = "Właściciel")]
        public String owner { get; set; }
        [Display(Name = "Nazwa")]
        public String name { get; set; }
        [Display(Name = "Opis")]
        public String description { get; set; }
        [Display(Name = "Układ odniesienia")]
        public String frameOfReference { get; set; }
    }
}
