using Microsoft.AspNetCore.Mvc.Rendering;
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
        public List<SelectListItem> typeOfChange { get; set; }
        public String selectedTypeOfChange { get; set; }
        public double result { get; set; }

        internal void zamienMiary()
        {
            switch (selectedTypeOfChange)
            {
                case "stopnie->grady":
                    result = (value*400)/360;
                    break;
                case "grady->stopnie":
                    result = (value * 360) / 400;
                    break;
                case "stopnie->radiany":
                    result = (value * 2* Math.PI) / 360;
                    break;
                case "radiany->stopnie":
                    result = (value * 360) / (2 * Math.PI); 
                    break;
                case "grady->radiany":
                    result = (value * 2 * Math.PI) / 400; 
                    break;
                case "radiany->grady":
                    result = (value * 400) / (2 * Math.PI);
                    break;

            }
            
        }
    }
}
