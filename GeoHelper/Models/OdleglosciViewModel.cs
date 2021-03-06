﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class OdleglosciViewModel
    {
        public int ID { set; get; }
        public String name1 { get; set; }
        public double x1 { get; set; }
        public double y1 { get; set; }
        public double z1 { get; set; }
        public String name2 { get; set; }
        public double x2 { get; set; }
        public double y2 { get; set; }
        public double z2 { get; set; }
        public double score { set; get; }
        public List<Point> pointList1 { set;get; }
        public List<Point> pointList2 { set; get; }
        public int selectedId1 { set; get; }
        public int selectedId2 { set; get; }

        public void obliczDlugosc()
        {
            double roznicaX = x2 - x1;
            double roznicaY = y2 - y1;
            double roznicaZ = z2 - z1;
            score = Math.Sqrt(roznicaX * roznicaX + roznicaY * roznicaY + roznicaZ * roznicaZ);
        }


    }
}
