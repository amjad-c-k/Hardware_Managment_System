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
    public partial class items : Form
    {
        int ItCode;
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-5JUMML4L\\SQLEXPRESS;Initial Catalog=HardwareDb;Integrated Security=True");
        public items()
        {
            InitializeComponent();
            UpdateItemsFormCategoryComboBox();
            GetCityRecord();

        }

        private void label15_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void items_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {


        }
        private void GetCityRecord()
        {
            string query = "SELECT * from ItemsTb1";

            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            guna2DataGridView1.DataSource = dt;
        }
        public void reset()
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
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



        private void button1_Click(object sender, EventArgs e)
        {

        }
        public void UpdateItemsFormCategoryComboBox()
        {
            /* if (Application.OpenForms["items"] is items itemsForm)
             {*/
            comboBox1.Items.Clear();
            // Fetch categories from the database and add them to the ComboBox
            string query = "SELECT * FROM CategoryTb1";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                comboBox1.Items.Add(row["CatName"].ToString());
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Customers c = new Customers();
            c.Show();
            if (c.Visible)
            {
                this.Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Billing b = new Billing();
            b.Show();
            if (b.Visible)
            {
                this.Close();
            }

        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ItCode = int.Parse(guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            textBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            comboBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = guna2DataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox4.Text = guna2DataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox5.Text = guna2DataGridView1.SelectedRows[0].Cells[5].Value.ToString();

        }

        private void dashboardLable_Click(object sender, EventArgs e)
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

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM ItemsTb1 WHERE ItCode = @ItCode";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ItCode", ItCode);
            con.Open();
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                MessageBox.Show("Data Successfully Deleted");

            }
            else
            {
                MessageBox.Show("Failed to Delete");
            }
            con.Close();
            GetCityRecord();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string Item = textBox1.Text;
            string category = comboBox1.Text;
            double Price = double.Parse(textBox3.Text);
            string Stock = textBox4.Text;
            string Manufacturer = textBox5.Text;
            DateTime selectedDate = guna2DateTimePicker1.Value;

            DateTime currentDateAndTime = DateTime.Now;
            DateTime combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, currentDateAndTime.Hour, currentDateAndTime.Minute, currentDateAndTime.Second);


            string categoryQuery = "SELECT CatName FROM CategoryTb1 WHERE CatName = @category";
            SqlCommand categoryCmd = new SqlCommand(categoryQuery, con);
            categoryCmd.Parameters.AddWithValue("@category", category);
            con.Open();
            object catCodeObj = categoryCmd.ExecuteScalar();
            con.Close();

            if (catCodeObj != null)
            {
                string itCategory = catCodeObj.ToString();

                string query = "INSERT INTO ItemsTb1 (ItName, ItCategory, Price, Stock, Manufacturer,Date) VALUES (@item, @category, @price, @stock, @manufacturer,@Date)";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@item", Item);
                cmd.Parameters.AddWithValue("@category", itCategory);
                cmd.Parameters.AddWithValue("@price", Price);
                cmd.Parameters.AddWithValue("@stock", Stock);
                cmd.Parameters.AddWithValue("@manufacturer", Manufacturer);
                cmd.Parameters.AddWithValue("@Date", combinedDateTime);

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

                GetCityRecord();
                reset();
            }
            else
            {
                MessageBox.Show("Category does not exist. Please add the category first.");
            }
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            string Item = textBox1.Text;
            string category = comboBox1.Text;
            double price = double.Parse(textBox3.Text);
            string stock = textBox4.Text;
            string manufacturer = textBox5.Text;
            DateTime Date = guna2DateTimePicker1.Value;

            string query = "UPDATE ItemsTb1 SET ItName = @Item, ItCategory = @category, Price = @price, Stock = @stock, Manufacturer = @manufacturer, Date = @Date WHERE ItCode = @ItCode";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@ItCode", ItCode);
            cmd.Parameters.AddWithValue("@item", Item);
            cmd.Parameters.AddWithValue("@category", category);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@stock", stock);
            cmd.Parameters.AddWithValue("@manufacturer", manufacturer);
            cmd.Parameters.AddWithValue("@Date", Date);

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
            GetCityRecord();
            reset();

        }
    }
}
