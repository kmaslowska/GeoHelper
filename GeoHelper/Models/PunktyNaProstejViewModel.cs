using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class PunktyNaProstejViewModel
    {
        public int ID { set; get; }
        public String name1 { get; set; }
        public double x1 { get; set; }
        public double y1 { get; set; }
        public String name2 { get; set; }
        public double x2 { get; set; }
        public double y2 { get; set; }
        public double distanceFrom1 { set; get; }
        public List<Point> pointList { set; get; }
        public int selectedId1 { set; get; }
        public int selectedId2 { set; get; }
        public double calculatedX { get; set; }
        public double calculatedY { get; set; }

        internal void obliczPunktNaProstej()
        {
            double roznicaXAB = x2 - x1;
            double roznicaYAB = y2 - y1;
            double odlegloscAB= Math.Sqrt(roznicaXAB * roznicaXAB + roznicaYAB * roznicaYAB);
            double przyrostXAN = distanceFrom1 * (roznicaXAB / odlegloscAB);
            double przyrostYAN= distanceFrom1 * (roznicaYAB / odlegloscAB);
            calculatedX = x1 + przyrostXAN;
            calculatedY = y1 + przyrostYAN;
        }
    }
}
