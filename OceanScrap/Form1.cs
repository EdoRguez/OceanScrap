using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OceanScrap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Port myPort = new Port();

            var html = @"https://www.marinetraffic.com/en/ais/details/ports/1";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);

            var nodeName = htmlDoc.DocumentNode.SelectSingleNode("//h1");
            myPort.getPortName(nodeName.InnerText);


            var nodeLocode = htmlDoc.DocumentNode.SelectNodes("//b");
            myPort.Locode = nodeLocode[2].InnerText;


            var nodeCountry = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'font-120')]");
            myPort.Country = nodeCountry[0].InnerText;

            var nodeCoordenates = htmlDoc.DocumentNode.SelectNodes("//b");
            myPort.getCoordenates(nodeCoordenates[0].InnerText);

            textBoxPrueba.Text = myPort.Latitude + " " + myPort.Longitude;
            //textBoxPrueba.Text = myPort.Latitude + " " + myPort.Longitude;



            //textBoxPrueba.Text = node.Name + " ---- " + node.OuterHtml + "||||||| " + node.InnerText;
        }

        
    }
}
