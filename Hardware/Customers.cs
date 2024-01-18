using Guna.UI2.WinForms;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hardware
{
    public partial class Customers : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-5JUMML4L\\SQLEXPRESS;Initial Catalog=HardwareDb;Integrated Security=True");
        int CustCode;
        public Customers()
        {
            InitializeComponent();
            GetRecord();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void reset()
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.Text = "Select";
        }

        private void GetRecord()
        {
            string query = "SELECT * from CustomerTb1";

            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            guna2DataGridView1.DataSource = dt;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Categories c = new Categories();
            c.Show();
            if (c.Visible)
            {
                this.Close();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            items i = new items();
            i.Show();
            if (i.Visible)
            {
                this.Close();
            }
        }

        private void guna2DataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            CustCode = int.Parse(guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            textBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            comboBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox2.Text = guna2DataGridView1.SelectedRows[0].Cells[3].Value.ToString();

        }

        private void label5_Click(object sender, EventArgs e)
        {
            Billing billing = new Billing();
            billing.Show();
            if (billing.Visible)
            {
                this.Close();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            if (dashboard.Visible)
            {
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            if (login.Visible)
            {
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string gender = comboBox1.Text;
            string phone = textBox2.Text;

            string query = "INSERT INTO CustomerTb1 VALUES (@name, @gender, @phone, @date)";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);



            int rows = cmd.ExecuteNonQuery();
            con.Close();

            if (rows > 0)
            {
                MessageBox.Show("Successfully Added");
            }
            else
            {
                MessageBox.Show("Failed to add");
            }

            GetRecord();
            reset();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string gender = comboBox1.Text;
            string phone = textBox2.Text;

            string query = "UPDATE CustomerTb1 SET CustName = @Name, Gender = @Gender, Phone = @Phone WHERE CustCode = @CustCode";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@CustCode", CustCode);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Phone", phone);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                MessageBox.Show("Successfully Modified");
            }
            else
            {
                MessageBox.Show("Failed to Modify");
            }
            con.Close();
            GetRecord();
            reset();
        }

        private void Customers_Load(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {

            string query = "DELETE from CustomerTb1 WHERE CustCode = @CustCode";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@CustCode", CustCode);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                MessageBox.Show("Successfully Deleted");
            }
            else
            {
                MessageBox.Show("Failed to Delete");
            }
            con.Close();
            GetRecord();
            reset();
        }
    }
}
