using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class Point
    {
        public int ID { get; set; }
        public String name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public int projectId { get; set; }
    }
}
