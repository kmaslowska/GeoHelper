using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class WciecieKatoweViewModel
    {
        public int ID { set; get; }
        public String name1 { get; set; }
        public double x1 { get; set; }
        public double y1 { get; set; }
        public String name2 { get; set; }
        public double x2 { get; set; }
        public double y2 { get; set; }
        public double angle1 { set; get; }
        public double angle2 { set; get; }
        public List<Point> pointList { set; get; }
        public int selectedId1 { set; get; }
        public int selectedId2 { set; get; }
        public double calculatedX { get; set; }
        public double calculatedY { get; set; }

        public double obliczAzymut(double x1, double y1, double x2, double y2)
        {
            double roznicaX = x2 - x1;
            double roznicaY = y2 - y1;
            double czwartak = (Math.Atan(roznicaY / roznicaX) * 400) / (2 * Math.PI);
            double score = 0;

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
        public double obliczDlugosc(double x1, double y1, double x2, double y2)
        {
            double roznicaX = x2 - x1;
            double roznicaY = y2 - y1;

            return Math.Sqrt(roznicaX * roznicaX + roznicaY * roznicaY);
        }
        internal void obliczWspolrzedne()
        {
            double kat1Radiany = (angle1 * 2 * Math.PI) / 400;
            double kat2Radiany = (angle2 * 2 * Math.PI) / 400;
            double odleglosc12 = obliczDlugosc(x1, y1, x2, y2);
            double odleglosc1N = (odleglosc12 * Math.Sin(kat2Radiany)) / (Math.Sin(kat1Radiany+kat2Radiany));
            double azymut12 = obliczAzymut(x1, y1, x2, y2);
            double azymut1N = azymut12 + angle1;
            double azymut1NRadiany= (azymut1N * 2 * Math.PI) / 400;
            double roznica1NX = odleglosc1N * Math.Cos(azymut1NRadiany);
            double roznica1NY = odleglosc1N * Math.Sin(azymut1NRadiany);
            calculatedX = x1 + roznica1NX;      
            calculatedY = y1 + roznica1NY;

        }
    }
}
