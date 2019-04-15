using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OceanScrap
{
    public partial class Form1 : Form
    {
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OceanScrap.Properties.Settings.ocean_connection"].ConnectionString;
        SqlConnection sqlConexion;
        SqlCommand comando;


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

            dataGridViewElements.Rows.Clear();
            listBoxErrors.Items.Clear();

            sqlConexion.Open();
            string sqlQuery = "INSERT INTO Ports(Id, Name, Locode, Country, Latitude, Longitude, Link) VALUES(@Id, @Name, @Locode, @Country, @Latitude, @Longitude, @Link)";
            comando = new SqlCommand(sqlQuery, sqlConexion);

            for (var i = portStart; i <= portEnd; i++ )
            {
                toolStripStatusLabel1.Text = "Scrapping..";

                var html = @"https://www.marinetraffic.com/en/ais/details/ports/" + i.ToString();

                Port myPort = new Port(html);
                myPort.Id = i;

                if(myPort.Scrapped)
                {
                    toolStripStatusLabel1.Text = "Scrapping.. Current port: " + i.ToString();

                    dataGridViewElements.Rows.Add(myPort.Id,myPort.Name, myPort.Locode, myPort.Country, myPort.Latitude, myPort.Longitude, myPort.Link);


                    comando.Parameters.AddWithValue("@Id", myPort.Id);
                    comando.Parameters.AddWithValue("@Name", myPort.Name);
                    comando.Parameters.AddWithValue("@Locode", myPort.Locode);
                    comando.Parameters.AddWithValue("@Country", myPort.Country);
                    comando.Parameters.AddWithValue("@Latitude", myPort.Latitude);
                    comando.Parameters.AddWithValue("@Longitude", myPort.Longitude);
                    comando.Parameters.AddWithValue("@Link", myPort.Link);

                    try
                    {
                        comando.ExecuteNonQuery();
                        sqlQuery = "SELECT @@IDENTITY";
                        comando = new SqlCommand(sqlQuery, sqlConexion);
                        var ultimoId = Convert.ToInt32(comando.ExecuteScalar());

                        MessageBox.Show("Los datos han sido ingresados satisfactoriamente", "Editar", MessageBoxButtons.OK);
                        this.DialogResult = DialogResult.OK;
                        Dispose();

                    }
                    catch (SqlException ex)
                    {
                        //tran.Rollback();

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
                else
                {
                    toolStripStatusLabel1.Text = "Scrapping.. No data from port: " + i.ToString();
                    listBoxErrors.Items.Add("No data port Nº " + i.ToString());
                }

                Refresh();
            }

            toolStripStatusLabel1.Text = "Scrapping finished";

            sqlConexion.Close();

        }

        
    }
}
