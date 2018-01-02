using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class DomiaryViewModel
    {
        public int ID { set; get; }
        public String name1 { get; set; }
        public double x1 { get; set; }
        public double y1 { get; set; }
        public String name2 { get; set; }
        public double x2 { get; set; }
        public double y2 { get; set; }
        public double distance1NPrim { set; get; }
        public double distanceNPrimN { set; get; }
        public List<SelectListItem> typeOfPoint { get; set; }
        public List<Point> pointList { set; get; }
        public string selectedTypeOfPoint { set; get; }
        public int selectedId1 { set; get; }
        public int selectedId2 { set; get; }
        public double calculatedX { get; set; }
        public double calculatedY { get; set; }

        internal void obliczWspolrzednePunktu()
        {
            
        }
    }
}
