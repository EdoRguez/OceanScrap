using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanScrap
{
    class Port
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Locode { get; set; }
        public string Country { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Link { get; set; }

        public bool Scrapped { get; set; }

        public Port() {  }

        public Port(string html)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(html);

                var nodeName = htmlDoc.DocumentNode.SelectSingleNode("//h1");
                getPortName(nodeName.InnerText);


                var nodeLocode = htmlDoc.DocumentNode.SelectNodes("//b");
                Locode = nodeLocode[2].InnerText;


                var nodeCountry = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'font-120')]");
                getCountry(nodeCountry[0].InnerText);

                var nodeCoordenates = htmlDoc.DocumentNode.SelectNodes("//b");
                getCoordenates(nodeCoordenates[0].InnerText);

                Link = html;

                Scrapped = true;
            }
            catch (Exception ex)
            {
                Scrapped = false;
            }
        }

        public void getPortName(string text)
        {
            int end;

            end = text.IndexOf("(");

            var name = text.Substring(0, end).Trim();

            Name = name;
        }

        public void getCountry(string text)
        {
            int start = text.IndexOf("Country: ") + 9;
            int end = text.IndexOf("(");

            var country = text.Substring(start, end - start).Trim();

            Country = country;
        }

        public void getCoordenates(string text)
        {
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
