using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Models
{
    public class ZmianaMiarKatowychViewModel
    {
        public int ID { set; get; }
        public String name { get; set; }
        public double value { get; set; }
        public String typeOfChange { get; set; }
        public double result { get; set; }
    }
}
