using Newtonsoft.Json;
using OceanScrap.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OceanScrap
{
    public partial class Form1 : Form
    {
        string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["OceanScrap.Properties.Settings.ocean_connection"].ConnectionString;
        SqlConnection sqlDb;
        SqlCommand comando;


        public Form1()
        {
            InitializeComponent();
            sqlDb = new SqlConnection(dbString);
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

            dataGridViewElements.Rows.Clear();
            listBoxErrors.Items.Clear();

            sqlDb.Open();

            for (var i = portStart; i <= portEnd; i++ )
            {
                Application.DoEvents();

                toolStripStatusLabel1.Text = "Scrapping..";

                var html = @"https://www.marinetraffic.com/en/ais/details/ports/" + i.ToString();

                Port myPort = new Port(html);
                myPort.Id = i;

                if(myPort.Scrapped)
                {
                    toolStripStatusLabel1.Text = "Scrapping.. Current port: " + i.ToString();

                    dataGridViewElements.Rows.Add(myPort.Id,myPort.Name, myPort.Locode, myPort.Country, myPort.Latitude, myPort.Longitude, myPort.Link);

                    if (checkBoxSave.Checked)
                    {



                        string sqlQuery = "INSERT INTO Port(Id, PortName, Locode, Country, Latitude, Longitude, Link) VALUES(@Id, @PortName, @Locode, @Country, @Latitude, @Longitude, @Link)";
                        comando = new SqlCommand(sqlQuery, sqlDb);
                        comando.Parameters.AddWithValue("@Id", myPort.Id);
                        comando.Parameters.AddWithValue("@PortName", myPort.Name);
                        comando.Parameters.AddWithValue("@Locode", myPort.Locode);
                        comando.Parameters.AddWithValue("@Country", myPort.Country);
                        comando.Parameters.AddWithValue("@Latitude", myPort.Latitude);
                        comando.Parameters.AddWithValue("@Longitude", myPort.Longitude);
                        comando.Parameters.AddWithValue("@Link", myPort.Link);

                        try
                        {
                            comando.ExecuteNonQuery();
                            sqlQuery = "SELECT @@IDENTITY";
                            comando = new SqlCommand(sqlQuery, sqlDb);

                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2627)
                            {
                                MessageBox.Show("Algunos datos están duplicados en la base de datos", "Error", MessageBoxButtons.OK);
                            }
                            else
                            {
                                MessageBox.Show("Otro error de base de datos " + ex.ToString());
                            }
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show("Ha sucedido un error: " + error.ToString());
                        }
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Scrapping.. No data from port: " + i.ToString();
                    listBoxErrors.Items.Add("No data port Nº " + i.ToString());
                }

                Refresh();
            }

            toolStripStatusLabel1.Text = "Scrapping finished";

            sqlDb.Close();

        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            var portsList = new List<Port>();

            foreach (DataGridViewRow row in dataGridViewElements.Rows)
            {
                Port dataGridPort = new Port()
                {
                    Id = int.Parse(row.Cells["Id"].Value.ToString()),
                    Name = row.Cells["File"].Value.ToString(),
                    Locode = row.Cells["Locode"].Value.ToString(),
                    Country = row.Cells["Country"].Value.ToString(),
                    Latitude = float.Parse(row.Cells["Latitude"].Value.ToString()),
                    Longitude = float.Parse(row.Cells["Longitude"].Value.ToString()),
                    Link = row.Cells["Link"].Value.ToString()
                };
                portsList.Add(dataGridPort);
            }

            //Write out JSON file
            //string export = JsonConvert.SerializeObject(new { types = portsList }, Formatting.Indented);
            //File.WriteAllText(@Settings.Default.folder + "\\" + "upload" + "\\" + "export.json", export);

            textBoxPrueba.Text = portsList[1].ToString();

            //string output = JsonConvert.SerializeObject(dataGridViewElements.DataSource);
            //System.IO.File.WriteAllText("json.json", output);
        }
    }
}
