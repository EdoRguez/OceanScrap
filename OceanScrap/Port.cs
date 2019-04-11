using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanScrap
{
    class Port
    {
        public string PortName { get; set; }
        public string Locode { get; set; }
        public string Country { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string TimeZone { get; set; }


        public void getPortName(string text)
        {
            int end;

            end = text.IndexOf("(");

            var name = text.Substring(0, end).Trim();

            PortName = name;
        }

        public void getCoordenates(string text)
        {
            // 37.94606&deg; / 23.615965&deg;

            int start;
            int end;

            start = 0;
            end = text.IndexOf("&deg");
            var latitude = text.Substring(start, end).Trim();
            Latitude = float.Parse(latitude);



            start = text.IndexOf("/ ") + 2;
            end = text.LastIndexOf("&deg");
            var longitude = text.Substring(start, end - start).Trim();
            Longitude = float.Parse(longitude);

        }
    }
}
