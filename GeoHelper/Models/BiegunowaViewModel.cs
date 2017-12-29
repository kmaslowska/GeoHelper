using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class BiegunowaViewModel
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
        public double distance1P { set; get; }
        public double kierunek12 { set; get; }
        public double kierunek13 { set; get; }
        public double kierunek1P { set; get; }
        public List<Point> pointList { set; get; }
        public int selectedId1 { set; get; }
        public int selectedId2 { set; get; }
        public int selectedId3 { set; get; }
        public double calculatedX { get; set; }
        public double calculatedY { get; set; }

        public double obliczAzymut(double x1, double y1, double x2, double y2)
        {
            double roznicaX = x2 - x1;
            double roznicaY = y2 - y1;
            double czwartak = (Math.Atan(roznicaX / roznicaY) * 400) / (2 * Math.PI);
            double score=0;

            if (roznicaX >= 0 && roznicaY >= 0)
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
                score = 400 + czwartak;
            }
            score = Math.Round(score, 3);
            return score;


        }

        internal void obliczWspolrzednePunktu()
        {
            double gammaPrim = obliczAzymut(x1, y1, x2, y2) - kierunek12;
            double gammaPrim2 = obliczAzymut(x1, y1, x3, y3) - kierunek13;
            double gamma = (gammaPrim + gammaPrim2) / 2;
            double azymut1P = kierunek1P + gamma;
            double azymut1Pradiany= (azymut1P * 2 * Math.PI) / 400;
            double roznica1PX = distance1P * ((Math.Cos(azymut1Pradiany) * 400) / (2 * Math.PI));
            double roznica1PY = distance1P * ((Math.Sin(azymut1Pradiany) * 400) / (2 * Math.PI));
            calculatedX = x1 + roznica1PX;
            calculatedY = y1 + roznica1PY;
        }
    }
}
