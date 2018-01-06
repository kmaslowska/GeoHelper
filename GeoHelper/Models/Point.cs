using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class Point
    {
        public int ID { get; set; }
        [Display(Name = "Nazwa")]
        public String name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        [Display(Name = "Id projektu")]
        public int projectId { get; set; }
    }
}
