using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class KatPoziomyViewModel
    {
        public int ID { set; get; }
        public String name1 { get; set; }
        public double x1 { get; set; }
        public double y1 { get; set; }
        public String name2 { get; set; }
        public double x2 { get; set; }
        public double y2 { get; set; }
        public String name3 { get; set; }
        public double x3 { get; set; }
        public double y3 { get; set; }
        public double score { set; get; }
        public List<Point> pointList { set; get; }
        public int selectedId1 { set; get; }
        public int selectedId2 { set; get; }
        public int selectedId3 { set; get; }

        public void obliczAzymut()
        {
            double roznicaX = x2 - x1;
            double roznicaY = y2 - y1;
            double czwartak = Math.Atan(roznicaX / roznicaY);

            if(roznicaX >= 0 && roznicaY >= 0)
            {
                score = czwartak;
            }
            if (roznicaX < 0 && roznicaY < 0)
            {
                score = 200 + czwartak;
            }
            if (roznicaX < 0 && roznicaY >= 0)
            {
                score = 200 + czwartak;
            }
            if (roznicaX >= 0 && roznicaY < 0)
            {
                score =400+ czwartak;
            }
            score = Math.Round(score, 3);


        }
    }
}
