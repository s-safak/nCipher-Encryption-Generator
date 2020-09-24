using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Generate
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public bool SaveOK;
        
        private void Form2Load(object sender, EventArgs e)
        {
            string sqlstring = Properties.Settings.Default.EnigmaConnectionString;
            var acon = new SqlConnectionStringBuilder(sqlstring);
            server_box.Text= acon.DataSource;
            user_box.Text = acon.UserID;
            password_box.Text = acon.Password;
            table_box.Text = acon.InitialCatalog; 
            //acon.
            //Properties.Settings.Default.EnigmaConnectionString = "bla bla";
            //Properties.Settings.Default.Save()

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //connectionString="Data Source=172.16.0.82;Initial Catalog=nTest;User ID=sa;Password=q1p0w2o9"

            label6.Text = "Connection Checking...";
            label6.Visible = true;
            Application.DoEvents();
            Properties.Settings.Default.EnigmaConnectionString = "Data Source="+server_box.Text+";Initial Catalog="+table_box.Text+";User ID="+user_box.Text+";Password="+password_box.Text;

            SqlConnection testconn = new SqlConnection(Properties.Settings.Default.EnigmaConnectionString.ToString());
            try
            {

                testconn.Open();
                if (testconn.State == ConnectionState.Open)
                {
                    Properties.Settings.Default.Save();
                    label6.Text = "Settings are saved!";
                    label6.Visible = true;
                    of.loadexec();
                }
            }
            catch
            {

                    MessageBox.Show("Database is not Accesible!");
                    label6.Text="Connection Problem!";
               
            }
            //configur configur.RefreshSection(Properties.Settings.Default.EnigmaConnectionString);
        }

        Sizebox of;

        public Sizebox Of
        {
            get { return of; }
            set { of = value; }
        }

    }
}
