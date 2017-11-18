using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class Project
    {
        public int ID { get; set; }
        public String owner { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public String frameOfReference { get; set; }
    }
}
