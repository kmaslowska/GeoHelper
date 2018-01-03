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

        public double obliczAzymut(double x1, double y1, double x2, double y2)
        {
            double roznicaX = x2 - x1;
            double roznicaY = y2 - y1;
            double czwartak = (Math.Atan(roznicaY / roznicaX) * 400) / (2 * Math.PI); 
            double score=0;

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
            return score;

        }

        internal void obliczKatPoziomy()
        {
            score = obliczAzymut(x1,y1,x3,y3) - obliczAzymut(x1,y1,x2,y2);
        }
    }
}
