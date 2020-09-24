using System;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;

namespace Generate
{
    public partial class Sizebox : Form
    {
        public Sizebox()
        {
            InitializeComponent();
        }
        /* Decrypt
        USE nTest
        GO
        SELECT dist, CONVERT(varchar, DECRYPTBYKEY(enckey)) AS 'ENC'
        FROM idlist;
        GO
        */
        /* Add single
        use nTest
        INSERT INTO idlist (dist, enckey)
        VALUES ('aab_', ENCRYPTBYKEY(KEY_GUID('AES128Key'), 'aab_'));
         */
        Thread _t;
        ThreadStart _st;
        public int i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, say;
        //private string id1, id2, id3, id4, id5, id6, id7, id8, id9, id10, id11, id12;
        public string sqltable = "nTest";
        public string allchars, dist, myInsertQuery, encvalue;
        public string[] tt;
        public bool _shouldStop;
        //private System.IO.StreamWriter file = new System.IO.StreamWriter(".\\export.txt");

        delegate void SetTextDelegate(string value);
        delegate void SetTimeDelegate(string value);
        delegate void SetBtnDelegate();
        delegate void SetCountDelegate(string value);
        delegate void SetShowDelegate(string value);

        public void SetShow(string value)
        {
            if (showbox.InvokeRequired)
                showbox.Invoke(new SetShowDelegate(SetShow), value);
            else
            {
                showbox.Text = value;
            }
        }

        public void SetCount(string value)
        {
            if (InvokeRequired)
                Invoke(new SetCountDelegate(SetCount), value);
            else
            {
                count.Text = value;
            }
        }

        public void RunBtn()
        {
            if (InvokeRequired)
                Invoke(new SetBtnDelegate(RunBtn));
            else
            {
                dur.PerformClick();
            }
        }

        public void SetTime(string value)
        {
            if (InvokeRequired)
                Invoke(new SetTimeDelegate(SetTime), value);
            else
            {
                totaltime.Text = value;
            }
        }

        public void SetText(string value)
        {
            if (InvokeRequired)
                Invoke(new SetTextDelegate(SetText), value);
            else
            {
                mixbox.Text = value;
            }
        }

        public string RandomKeys(int size)
        {
            char[] cr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            string result = string.Empty;
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                result += cr[r.Next(0, cr.Length - 1)].ToString();
            }
            return result;
        }

        public string RandomHEX(int size)
        {
            char[] cr = "0123456789ABCDEF".ToCharArray();
            string result = string.Empty;
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                result += cr[r.Next(0, cr.Length - 1)].ToString();
            }
            return result;
        }

        private void Random_worker()
        {
            SqlConnection myConn = new SqlConnection(Properties.Settings.Default.EnigmaConnectionString);
            int i = 0;
            if (myConn.State != ConnectionState.Open)
            {
                myConn.Open();
            }
            string key_gen;
            while (i < 1)
            {
                
                try
                {
                    if (encvalue != "")
                    {
                        key_gen = RandomKeys(Convert.ToInt32(size_box.Text));
                        myInsertQuery = "INSERT INTO idlist ([i1], [i2], [i3], [i4], [i5], [i6], [i7], [i8], [i9], [i10], [i11], [i12], [dist], [enckey]) VALUES ('0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', @cr, " + encvalue.ToString() + ", '" + key_gen + "'))";
                    }
                    else
                    {
                        key_gen = RandomKeys(Convert.ToInt32(size_box.Text));
                        myInsertQuery = "INSERT INTO idlist ([i1], [i2], [i3], [i4], [i5], [i6], [i7], [i8], [i9], [i10], [i11], [i12], [dist]) VALUES ('0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', @cr )";
                    }
                    var comm = new SqlCommand(myInsertQuery) {Connection = myConn};
                    comm.Parameters.AddWithValue("cr", key_gen);
                    //comm.Parameters.Add("@crypt", cr.ToString());
                    if (myConn.State != ConnectionState.Open) myConn.Open();
                    comm.ExecuteNonQuery();
                }
                finally
                {
                    myConn.Close();
                }
                say++;
                if ((counter_count.Text != "0") && (say == Convert.ToInt32(counter_count.Text))) RunBtn();
                SetCount(say.ToString());
                while (_shouldStop)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(33);
                    timer1.Enabled = false;
                }
            }
        }


        private void Hex_worker()
        {
            SqlConnection myConn = new SqlConnection(Properties.Settings.Default.EnigmaConnectionString);
            int i = 0;
            while (i < 1)
            {
                try
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    if (encvalue != "")
                    {
                        //SetShow(RandomHEX(Convert.ToInt32(size_box.Text)));
                        int LenCode = Convert.ToInt32(size_box.Text);
                        string strCode = RandomHEX(LenCode);
                        SetShow(strCode);
                        //showbox.Text = strCode;
                        myInsertQuery = "INSERT INTO idlist ([i1], [i2], [i3], [i4], [i5], [i6], [i7], [i8], [i9], [i10], [i11], [i12], [dist], [enckey]) VALUES ('0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', @cr, " + encvalue.ToString() + ", '" + showbox.Text + "'))";
                    }
                    SetShow(RandomHEX(Convert.ToInt32(size_box.Text)));
                    myInsertQuery = "INSERT INTO idlist ([i1], [i2], [i3], [i4], [i5], [i6], [i7], [i8], [i9], [i10], [i11], [i12], [dist]) VALUES ('0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', @cr )";

                    SqlCommand comm = new SqlCommand(myInsertQuery);
                    comm.Parameters.AddWithValue("cr", showbox.Text);
                    //comm.Parameters.Add("@crypt", cr.ToString());
                    comm.Connection = myConn;
                    comm.ExecuteNonQuery();
                }
                finally
                {
                    myConn.Close();
                }




                say++;
                if ((counter_count.Text != "0") && (say == Convert.ToInt32(counter_count.Text))) RunBtn();
                SetCount(say.ToString());
                while (_shouldStop)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(33);
                    timer1.Enabled = false;
                }
            }

        }


        private void worker()
        {
            //string myConnectionString = "Data Source=172.16.0.82,1433;database=nTest;User ID=sa;Password=q1p0w2o9";
            SqlConnection myConn = new SqlConnection(Properties.Settings.Default.EnigmaConnectionString);
            //Richtext Save
            //myInsertQuery = "INSERT INTO idlist ([i1], [i2], [i3], [i4], [i5], [i6], [i7], [i8], [i9], [i10], [i11], [i12], [dist], [enckey]) VALUES (@id1, @id2, @id3, @id4, @id5, @id6, @id7, @id8, @id9, @id10, @id11, @id12, @cr, ENCRYPTBYKEY(KEY_GUID('AES256Key'), @crypt))";

            while (i1 < allchars.Length)
            {
                while (i2 < allchars.Length)
                {
                    while (i3 < allchars.Length)
                    {
                        while (i4 < allchars.Length)
                        {
                            while (i5 < allchars.Length)
                            {
                                while (i6 < allchars.Length)
                                {
                                    while (i7 < allchars.Length)
                                    {
                                        while (i8 < allchars.Length)
                                        {
                                            while (i9 < allchars.Length)
                                            {
                                                while (i10 < allchars.Length)
                                                {
                                                    while (i11 < allchars.Length)
                                                    {
                                                        while (i12 < allchars.Length)
                                                        {

                                                            //if (_shouldStop != false)
                                                            string cr = tt[i1] + tt[i2] + tt[i3] + tt[i4] + tt[i5] + tt[i6] + tt[i7] + tt[i8] + tt[i9] + tt[i10] + tt[i11] + tt[i12];
                                                            cr = cr.Trim();
                                                            try
                                                            {
                                                                if (myConn.State != ConnectionState.Open)
                                                                {
                                                                    myConn.Open();
                                                                }
                                                                if (encvalue != "")
                                                                {
                                                                    myInsertQuery = "INSERT INTO idlist ([i1], [i2], [i3], [i4], [i5], [i6], [i7], [i8], [i9], [i10], [i11], [i12], [dist], [enckey]) VALUES (@id1, @id2, @id3, @id4, @id5, @id6, @id7, @id8, @id9, @id10, @id11, @id12, @cr, " + encvalue.ToString() + ", '" + cr.ToString() + "'))";
                                                                }

                                                                SqlCommand comm = new SqlCommand(myInsertQuery);
                                                                comm.Parameters.AddWithValue("id1", i1.ToString());
                                                                comm.Parameters.AddWithValue("id2", i2.ToString());
                                                                comm.Parameters.AddWithValue("id3", i3.ToString());
                                                                comm.Parameters.AddWithValue("id4", i4.ToString());
                                                                comm.Parameters.AddWithValue("id5", i5.ToString());
                                                                comm.Parameters.AddWithValue("id6", i6.ToString());
                                                                comm.Parameters.AddWithValue("id7", i7.ToString());
                                                                comm.Parameters.AddWithValue("id8", i8.ToString());
                                                                comm.Parameters.AddWithValue("id9", i9.ToString());
                                                                comm.Parameters.AddWithValue("id10", i10.ToString());
                                                                comm.Parameters.AddWithValue("id11", i11.ToString());
                                                                comm.Parameters.AddWithValue("id12", i12.ToString());
                                                                comm.Parameters.AddWithValue("cr", cr.ToString());
                                                                //comm.Parameters.Add("@crypt", cr.ToString());
                                                                comm.Connection = myConn;
                                                                comm.ExecuteNonQuery();
                                                            }
                                                            finally
                                                            {
                                                                myConn.Close();
                                                            }
                                                            //file.WriteLine(cr);
                                                            SetText(cr);
                                                            say++;
                                                            if ((counter_count.Text != "0") && (say == Convert.ToInt32(counter_count.Text))) RunBtn();
                                                            SetCount(say.ToString());
                                                            i12++;
                                                            while (_shouldStop)
                                                            {
                                                                if (myConn.State == ConnectionState.Open)
                                                                {
                                                                    myConn.Close();
                                                                }
                                                                myConn.ConnectionString = Properties.Settings.Default.EnigmaConnectionString.ToString();
                                                                timer1.Enabled = false;
                                                                Application.DoEvents();
                                                                System.Threading.Thread.Sleep(33);
                                                            }
                                                        }
                                                        i12 = 1;
                                                        i11++;
                                                    }
                                                    i12 = 1;
                                                    i11 = 1;
                                                    i10++;
                                                }
                                                i12 = 1;
                                                i11 = 1;
                                                i10 = 1;
                                                i9++;
                                            }
                                            i12 = 1;
                                            i11 = 1;
                                            i10 = 1;
                                            i9 = 1;
                                            i8++;
                                        }
                                        i12 = 1;
                                        i11 = 1;
                                        i10 = 1;
                                        i9 = 1;
                                        i8 = 1;
                                        i7++;
                                    }
                                    i12 = 1;
                                    i11 = 1;
                                    i10 = 1;
                                    i9 = 1;
                                    i8 = 1;
                                    i7 = 1;
                                    i6++;
                                }
                                i12 = 1;
                                i11 = 1;
                                i10 = 1;
                                i9 = 1;
                                i8 = 1;
                                i7 = 1;
                                i6 = 1;
                                i5++;
                            }
                            i12 = 1;
                            i11 = 1;
                            i10 = 1;
                            i9 = 1;
                            i8 = 1;
                            i7 = 1;
                            i6 = 1;
                            i5 = 1;
                            i4++;
                        }
                        i12 = 1;
                        i11 = 1;
                        i10 = 1;
                        i9 = 1;
                        i8 = 1;
                        i7 = 1;
                        i6 = 1;
                        i5 = 1;
                        i4 = 1;
                        i3++;
                    }
                    i12 = 1;
                    i11 = 1;
                    i10 = 1;
                    i9 = 1;
                    i8 = 1;
                    i7 = 1;
                    i6 = 1;
                    i5 = 1;
                    i4 = 1;
                    i3 = 1;
                    i2++;
                }
                i12 = 1;
                i11 = 1;
                i10 = 1;
                i9 = 1;
                i8 = 1;
                i7 = 1;
                i6 = 1;
                i5 = 1;
                i4 = 1;
                i3 = 1;
                i2 = 1;
                i1++;
            }
            //etime.Text = DateTime.Now.ToString("hh:mm:ss.FFF");
            //TimeSpan ts = new TimeSpan();
            //DateTime dt1 = Convert.ToDateTime(stime.Text);
            //DateTime dt2 = Convert.ToDateTime(etime.Text);
            //ts = dt1 - dt2;
            //SetTime(ts.ToString());
            //MessageBox.Show("Bitti");
            //file.Close();
        }

        private void start_Click(object sender, EventArgs e)
        {
            //AES128Key, AES192Key,AES256Key, RSA1024Key, RSA2048Key
            if (tripledes_radio.Checked)
            {
                //myInsertQuery = "INSERT INTO idlist ([i1], [i2], [i3], [i4], [i5], [i6], [i7], [i8], [i9], [i10], [i11], [i12], [dist], [enckey]) VALUES (@id1, @id2, @id3, @id4, @id5, @id6, @id7, @id8, @id9, @id10, @id11, @id12, @cr, ENCRYPTBYKEY(KEY_GUID('TRIPLE_DESKey'), @crypt))";
                encvalue = "ENCRYPTBYKEY(KEY_GUID('TRIPLE_DESKey')";
                cbox.Text = "";
            }
            else if (tripledes3_radio.Checked)
            {
                encvalue = "ENCRYPTBYKEY(KEY_GUID('TRIPLE_DES_3KEY')";
                cbox.Text = "";
            }
            else if (aes128_radio.Checked)
            {
                encvalue = "ENCRYPTBYKEY(KEY_GUID('AES128Key')";
                cbox.Text = "";
            }
            else if (aes192_radio.Checked)
            {
                encvalue = "ENCRYPTBYKEY(KEY_GUID('AES192Key')";
                cbox.Text = "";
            }
            else if (aes256_radio.Checked)
            {
                encvalue = "ENCRYPTBYKEY(KEY_GUID('AES256Key')";
                cbox.Text = "";
            }
            else if (rsa1024_radio.Checked)
            {
                encvalue = "ENCRYPTBYASYMKEY(ASYMKEY_ID('RSA1024Key')";
                cbox.Text = "";
            }
            else if (rsa2048_radio.Checked)
            {
                encvalue = "ENCRYPTBYASYMKEY(ASYMKEY_ID('RSA2048Key')";
                cbox.Text = "";
            }
            else
            {
                encvalue = "";
                myInsertQuery = "INSERT INTO idlist ([i1], [i2], [i3], [i4], [i5], [i6], [i7], [i8], [i9], [i10], [i11], [i12], [dist]) VALUES (@id1, @id2, @id3, @id4, @id5, @id6, @id7, @id8, @id9, @id10, @id11, @id12, @cr)";
            }

            allchars = " " + hbox.Text + rbox.Text + cbox.Text;
            tt = new string[allchars.Length];
            for (int y = 0; y < allchars.Length; y++)
            {
                tt[y] = allchars.Substring(y, 1);
            }
            if (_shouldStop != false)
            {
                //st = new ThreadStart(worker); //create thread
                //t = new Thread(st);//create thread
                //t.Start();//thread start
                //stime.Text = DateTime.Now.ToString("hh:mm:ss.FFF");
                //start.Enabled = false;
                say = 0;
                etime.Text = "";
                totaltime.Text = "";
                if (time_chk.Checked) timer1.Enabled = true;
                stime.Text = DateTime.Now.ToString("hh:mm:ss.FFF");
                Properties.Settings.Default.Reload();
                _shouldStop = false;
            }
            else
            {

                SqlConnection subconn = new SqlConnection(Properties.Settings.Default.EnigmaConnectionString.ToString());
                string sqlsubject = "SELECT * FROM [" + sqltable.ToString() + "].[dbo].[idlist] ORDER BY iid DESC";
                try
                {
                    subconn.Open();
                    SqlDataReader idreader = null;
                    SqlCommand cmd = new SqlCommand(sqlsubject, subconn);
                    idreader = cmd.ExecuteReader();
                    if (idreader.HasRows == true)
                    {
                        while (idreader.Read())
                        {
                            //string posID = (subjectreader["ID"].ToString()).Trim();
                            i1 = Convert.ToInt32((idreader["i1"].ToString()));
                            i2 = Convert.ToInt32((idreader["i2"].ToString()));
                            i3 = Convert.ToInt32((idreader["i3"].ToString()));
                            i4 = Convert.ToInt32((idreader["i4"].ToString()));
                            i5 = Convert.ToInt32((idreader["i5"].ToString()));
                            i6 = Convert.ToInt32((idreader["i6"].ToString()));
                            i7 = Convert.ToInt32((idreader["i7"].ToString()));
                            i8 = Convert.ToInt32((idreader["i8"].ToString()));
                            i9 = Convert.ToInt32((idreader["i9"].ToString()));
                            i10 = Convert.ToInt32((idreader["i10"].ToString()));
                            i11 = Convert.ToInt32((idreader["i11"].ToString()));
                            i12 = Convert.ToInt32((idreader["i12"].ToString()));
                            dist = (idreader["dist"].ToString());
                            break;
                        }
                        //cmd.CommandType = CommandType.Text;
                        //cmd.ExecuteNonQuery();
                    }
                    else
                    {

                        load();
                    }

                }
                finally
                {
                    subconn.Close();
                }
                if ((time_chk.Checked) && (timer_time.Text != ""))
                {
                    timer1.Interval = Convert.ToInt32(timer_time.Text) * 1000;
                    timer1.Enabled = true;
                }
                idbox.Text = i1.ToString() + i2.ToString() + i3.ToString() + i4.ToString() + i5.ToString() + i6.ToString() + i7.ToString() + i8.ToString() + i9.ToString() + i10.ToString() + i11.ToString() + i12.ToString();
                mixbox.Text = dist.Trim();

                if (hex_radio.Checked) _st = new ThreadStart(Hex_worker);
                if (random_radio.Checked) _st = new ThreadStart(Random_worker);
                if (order_radio.Checked) _st = new ThreadStart(worker); //create thread
                _t = new Thread(_st);//create thread
                _t.Start();//thread start
                etime.Text = "";
                totaltime.Text = "";
                stime.Text = DateTime.Now.ToString("hh:mm:ss.FFF");
            }

        }

        private void dur_Click(object sender, EventArgs e)
        {
            _shouldStop = true;
            start.Enabled = true;
            timer1.Enabled = false;
            etime.Text = DateTime.Now.ToString("hh:mm:ss.FFF");
            TimeSpan ts = new TimeSpan();
            DateTime dt1 = Convert.ToDateTime(stime.Text);
            DateTime dt2 = Convert.ToDateTime(etime.Text);
            ts = dt2 - dt1;
            totaltime.Text = ts.ToString();
            count.Text = say.ToString();
            timer1.Enabled = false;
        }

        private void load()
        {
            allchars = " " + hbox.Text + rbox.Text + cbox.Text;
            tt = new string[allchars.Length];
            for (int p = 0; p < allchars.Length; p++)
            {
                tt[p] = allchars.Substring(p, 1);
            }
            i1 = 0; i2 = 0; i3 = 0; i4 = 0; i5 = 0; i6 = 0; i7 = 0; i8 = 0;
            i9 = 1; i10 = 1; i11 = 1; i12 = 1;
            dist = tt[i1] + tt[i2] + tt[i3] + tt[i4] + tt[i5] + tt[i6] + tt[i7] + tt[i8] + tt[i9] + tt[i10] + tt[i11] + tt[i12];
            mixbox.Text = dist;
            idbox.Text = i1.ToString() + i2.ToString() + i3.ToString() + i4.ToString() + i5.ToString() + i6.ToString() + i7.ToString() + i8.ToString() + i9.ToString() + i10.ToString() + i11.ToString() + i12.ToString();

        }

        public void loadexec()
        {
            string sqlstring = Properties.Settings.Default.EnigmaConnectionString;
            SqlConnectionStringBuilder acon = new SqlConnectionStringBuilder(sqlstring);
            serveriplbl.Text = acon.DataSource;
            Properties.Settings.Default.Reload();
            SqlConnection subconn = new SqlConnection(Properties.Settings.Default.EnigmaConnectionString.ToString());
            string sqlsubject = "SELECT * FROM [" + sqltable.ToString() + "].[dbo].[idlist] ORDER BY iid DESC";

            try
            {
                subconn.Open();
                SqlDataReader idreader = null;
                SqlCommand cmd = new SqlCommand(sqlsubject, subconn);
                idreader = cmd.ExecuteReader();
                if (idreader.HasRows == true)
                {
                    while (idreader.Read())
                    {
                        //string posID = (subjectreader["ID"].ToString()).Trim();
                        i1 = Convert.ToInt32((idreader["i1"].ToString()));
                        i2 = Convert.ToInt32((idreader["i2"].ToString()));
                        i3 = Convert.ToInt32((idreader["i3"].ToString()));
                        i4 = Convert.ToInt32((idreader["i4"].ToString()));
                        i5 = Convert.ToInt32((idreader["i5"].ToString()));
                        i6 = Convert.ToInt32((idreader["i6"].ToString()));
                        i7 = Convert.ToInt32((idreader["i7"].ToString()));
                        i8 = Convert.ToInt32((idreader["i8"].ToString()));
                        i9 = Convert.ToInt32((idreader["i9"].ToString()));
                        i10 = Convert.ToInt32((idreader["i10"].ToString()));
                        i11 = Convert.ToInt32((idreader["i11"].ToString()));
                        i12 = Convert.ToInt32((idreader["i12"].ToString()));
                        dist = (idreader["dist"].ToString());
                        break;
                    }
                    //cmd.CommandType = CommandType.Text;
                    //cmd.ExecuteNonQuery();
                }
                else
                {
                    load();
                }

            }
            finally
            {
                subconn.Close();
            }

            idbox.Text = i1.ToString() + i2.ToString() + i3.ToString() + i4.ToString() + i5.ToString() + i6.ToString() + i7.ToString() + i8.ToString() + i9.ToString() + i10.ToString() + i11.ToString() + i12.ToString();
            mixbox.Text = dist.Trim();
        }

        public void bekle_Click(object sender, EventArgs e)
        {
            loadexec();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tripledes_radio.Checked = false;
            tripledes3_radio.Checked = false;
            aes128_radio.Checked = false;
            aes192_radio.Checked = false;
            aes256_radio.Checked = false;
            rsa1024_radio.Checked = false;
            rsa2048_radio.Checked = false;
            order_radio.Checked = true;
            size_box.Text = "0";
            size_box.Enabled = false;
        }

        private void cleardb_btn_Click(object sender, EventArgs e)
        {
            SqlConnection clearconn = new SqlConnection(Properties.Settings.Default.EnigmaConnectionString.ToString());
            string clearqry = "truncate table idlist";
            clearconn.Open();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            SqlCommand clearcmd = new SqlCommand(clearqry, clearconn);
            if (clearconn.State != ConnectionState.Open)
            {
                clearconn.Open();
            }
            try
            {
                clearcmd.ExecuteNonQuery();
                load();
            }
            finally
            {
                clearconn.Close();
            }
        }

        private void time_chk_CheckedChanged(object sender, EventArgs e)
        {
            if (count_chk.Checked)
            {
                count_chk.Checked = false;
            }
            else
            {
                counter_count.Enabled = false;
                counter_count.Text = "0";
                timer_time.Enabled = true;
            }
            if (!time_chk.Checked) timer_time.Enabled = false;
        }

        private void count_chk_CheckedChanged(object sender, EventArgs e)
        {
            if (time_chk.Checked)
            {
                time_chk.Checked = false;
            }
            else
            {
                timer_time.Enabled = false;
                timer_time.Text = "0";
                counter_count.Enabled = true;
            }
            if (!count_chk.Checked)
            {
                counter_count.Enabled = false;
                counter_count.Text = "0";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dur.PerformClick();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_t != null) _t.Abort();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Form2 f2 = new Form2();
            f2.Of = this;
            f2.MdiParent = this.MdiParent;
            f2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataView f3 = new DataView();
            f3.MdiParent = this.MdiParent;
            f3.Show();
        }

        private void random_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (random_radio.Checked)
            {
                count_chk.Checked = true;
                counter_count.Text = "10";
                size_box.Enabled = true;
                size_box.Text = "10";
            }
            else
            {
                count_chk.Checked = false;
                counter_count.Text = "";
                size_box.Text = "0";
                size_box.Enabled = false;
            }
        }

        private void hex_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (hex_radio.Checked)
            {
                count_chk.Checked = true;
                counter_count.Text = "10";
                size_box.Enabled = true;
                size_box.Text = "10";
            }
            else
            {
                count_chk.Checked = false;
                counter_count.Text = "";
                size_box.Text = "0";
                size_box.Enabled = false;
            }
        }

        private void rsa1024_radio_CheckedChanged(object sender, EventArgs e)
        {
            random_radio.Checked = true;
            size_box.Enabled = true;
            size_box.Text = "128";
            hex_radio.Checked = false;
            order_radio.Checked = false;
        }

        private void Rsa2048RadioCheckedChanged(object sender, EventArgs e)
        {
            random_radio.Checked = true;
            size_box.Enabled = true;
            size_box.Text = "256";
            hex_radio.Checked = false;
            order_radio.Checked = false;
        }

        private void SizeboxShown(object sender, EventArgs e)
        {
            string sqlstring = Properties.Settings.Default.EnigmaConnectionString;
            var acon = new SqlConnectionStringBuilder(sqlstring);
            var server = acon.DataSource;
            DialogResult dialogAsk = MessageBox.Show("Connection Server : "+server+" \n Do you want to make Data connection?", "Warning!",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogAsk == DialogResult.Yes)
            {
                // TODO: This line of code loads data into the 'nTestDataSet.idlist' table. You can move, or remove it, as needed.
                //SqlConnection subconn = new SqlConnection(@"Data Source=212.174.107.7,1433;database=nTest;User ID=sa;Password=02_Rainbow20!");
                string conn_str = Properties.Settings.Default.EnigmaConnectionString;
                var s_con = new SqlConnectionStringBuilder(conn_str);
                serveriplbl.Text = s_con.DataSource;
                if (Properties.Settings.Default.EnigmaConnectionString != null)
                {
                    var subconn = new SqlConnection(Properties.Settings.Default.EnigmaConnectionString);
                    var sqlsubject = string.Format("SELECT * FROM [{0}].[dbo].[idlist] ORDER BY iid DESC", sqltable);

                    try
                    {
                        subconn.Open();
                        var cmd = new SqlCommand(sqlsubject, subconn);
                        var idreader = cmd.ExecuteReader();
                        if (idreader.HasRows)
                        {
                            while (idreader.Read())
                            {
                                //string posID = (subjectreader["ID"].ToString()).Trim();
                                i1 = Convert.ToInt32((idreader["i1"].ToString()));
                                i2 = Convert.ToInt32((idreader["i2"].ToString()));
                                i3 = Convert.ToInt32((idreader["i3"].ToString()));
                                i4 = Convert.ToInt32((idreader["i4"].ToString()));
                                i5 = Convert.ToInt32((idreader["i5"].ToString()));
                                i6 = Convert.ToInt32((idreader["i6"].ToString()));
                                i7 = Convert.ToInt32((idreader["i7"].ToString()));
                                i8 = Convert.ToInt32((idreader["i8"].ToString()));
                                i9 = Convert.ToInt32((idreader["i9"].ToString()));
                                i10 = Convert.ToInt32((idreader["i10"].ToString()));
                                i11 = Convert.ToInt32((idreader["i11"].ToString()));
                                i12 = Convert.ToInt32((idreader["i12"].ToString()));
                                dist = (idreader["dist"].ToString());
                                break;
                            }
                            //cmd.CommandType = CommandType.Text;
                            //cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            load();
                        }

                    }
                    finally
                    {
                        subconn.Close();
                    }
                }

                idbox.Text = i1.ToString() + i2.ToString() + i3.ToString() + i4.ToString() + i5.ToString() +
                             i6.ToString() + i7.ToString() + i8.ToString() + i9.ToString() + i10.ToString() +
                             i11.ToString() + i12.ToString();
                mixbox.Text = dist.Trim();
                //file.WriteLine(lines);
                //file.Close();
            }
            else
            {
                button2.PerformClick();
            }
        }

        private void Form1_load(object sender, EventArgs e)
        {

        }



    }
}