using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsSubirDatosMysql
{
    public partial class Form1 : Form
    { //variables importantes 
        private string path;
        private string myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=uta007008%;database=fisiedns;";
        public Form1()
        {
            InitializeComponent();
            bntInsert.Enabled = false;
            pictureBox1.Visible = false;
           label2.Text = "";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openDialog();
        }
        private void openDialog()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //propiedades del openDialogo prar subir  archivos filtrados
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Files (*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    string dir = openFileDialog1.FileName;

                    

                    //Console.WriteLine("Original string: \"{0}\"", dir);
                    //del path tomado remplazamos el \ por / para que sea legible desde mysql
                    path = dir.Replace("\\","//");
                    lblPath.Text = path;
                    if (!lblPath.Equals(""))
                    {
                        bntInsert.Enabled = true;
                    }
                    //usando el insert para la importacion de los datos 
                    
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error,no se puede leer el archivo" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public int insert(string path)
        {
            int resultado=0;

                   
         
            try
            {
               MySqlConnection conn = new MySqlConnection(myConnectionString);
                conn.Open();
               string sql1= "LOAD DATA LOCAL INFILE '" + path + "' INTO TABLE dns.t1 " +
  " FIELDS TERMINATED BY ';' " +
    " LINES TERMINATED BY '\r\n' " +
        " (fecha,cliente,puerto,dominios,addr) ";
                //MessageBox.Show(sql1);
                MySqlCommand cmd = new MySqlCommand(sql1,conn);
                //cmd.CommandTimeout = 3000;

                //comando sql que permite importar un archivo cvs a mysql

                //en el caso de Fields termined by puede ser por (, o ;) depede de la version del excel
                // entre parentesis van el nombre de las columnas que se van a introducir en la tabla




               resultado= cmd.ExecuteNonQuery();
                
                conn.Close();



            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message + " filas agregadas: " + resultado, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            
            return resultado;



        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bntInsert_Click(object sender, EventArgs e)
        {
            label2.Text = "Procesando...";
            button1.Enabled = false;
            pictureBox1.Visible = true;
            bntInsert.Enabled = false;
            int result =insert(path);
            if (result >0)
            {
                label2.Text = "Proceso finalizado !";
                pictureBox1.Visible = false;
                button1.Enabled = true;
            }
            else
            {
                pictureBox1.Visible = false;
                label2.Text = "Intente nuevamente !";
                button1.Enabled = true;
            }

           
               
                
          

            
          
        }
        private DataTable exeSql(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection cnn = new MySqlConnection(myConnectionString);
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
              
                cnn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: "+ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            return dt;

        }

        public void llenarChart()
        {
            List<string> series1 = new List<string>();
            series1.Add("men");
            series1.Add("women");
            series1.Add("boys");
            series1.Add("girlds");

            List<int> puntos = new List<int>();
            puntos.Add(10);
            puntos.Add(20);
            puntos.Add(30);
            puntos.Add(20);

            string[] seriesArray = { "Cats", "Dogs" };
            int[] pointsArray = { 1, 2 };

            // Set palette.
            chart1.Palette = ChartColorPalette.SeaGreen;

            // Set title.
            chart1.Titles.Add("Humans");

            // Add series.
            for (int i = 0; i < series1.Count; i++)
            {
                // Add series.
                Series series = chart1.Series.Add(series1[i]);

                // Add point.
                series.Points.Add(puntos[i]);
            }
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
        
            chart1.Palette = ChartColorPalette.SeaGreen;
            DataTable table = new DataTable();
            // Set title.
            chart1.Titles.Add("Results");
            table=exeSql(textsql.Text.Trim());

            chart1.DataSource = table;

            chart1.Series[0].XValueMember = "serie";
            chart1.Series[0].YValueMembers = "puntos";
            chart1.DataBind();

            /**
             * codigo para graficar extrayendo datos del datatable
             * */
            //foreach (DataRow row in table.Rows)
            //     {

            //     Series series = chart1.Series.Add(row["serie"].ToString());
            //     series.Points.Add(int.Parse(row["puntos"].ToString()));
            // }


            //row[column] = row[column].ToString().Replace(@"\", @" ").Trim();


        }

        private void PrintColumnNames(DataTable table)
        {


            // cargar el nombre de las columnas
            //foreach (DataColumn dc in table.Columns)
            //{
            //    String field1 = dc.ColumnName;
            //    MessageBox.Show(field1);
            //}

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    //                     row[column] obtiene el valor de las columnas de la fila
                    row[column] = row[0].ToString();
                   // series.Points.Add(puntos[i]);
                    row[column] = row[1].ToString().Replace(@"\", @" ").Trim();
                }
            }
        }
    }
}
