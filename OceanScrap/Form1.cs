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
            int portStart = int.Parse(numericUpDownFrom.Value.ToString());
            int portEnd = int.Parse(numericUpDownTo.Value.ToString());


            for(var i = portStart; i <= portEnd; i++ )
            {
                toolStripStatusLabel1.Text = "Port: " + i.ToString();

                var html = @"https://www.marinetraffic.com/en/ais/details/ports/" + i.ToString();

                Port myPort = new Port(html);
                myPort.Id = i;

                if(myPort.Scrapped)
                {
                    dataGridViewElements.Rows.Add(myPort.Id,myPort.Name, myPort.Locode, myPort.Country, myPort.Latitude, myPort.Longitude, myPort.Link);
                }
                else
                {
                    listBoxErrors.Items.Add("No data port Nº " + i.ToString());
                }
            }

        }

        
    }
}
