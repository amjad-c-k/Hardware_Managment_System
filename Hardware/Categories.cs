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
    public partial class Categories : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-5JUMML4L\\SQLEXPRESS;Initial Catalog=HardwareDb;Integrated Security=True");
        string Category;
        public Categories()
        {
            InitializeComponent();
            GetCityRecord();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        public void UpdateItemsFormCategoryComboBox()
        {
            if (Application.OpenForms["items"] is items itemsForm)
            {
                itemsForm.comboBox1.Items.Clear();
                // Fetch categories from the database and add them to the ComboBox
                string query = "SELECT * FROM CategoryTb1";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    itemsForm.comboBox1.Items.Add(row["CatName"].ToString());
                }
            }
        }
        private void GetCityRecord()
        {
            string query = "SELECT * from CategoryTb1";

            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            guna2DataGridView1.DataSource = dt;
        }


        private void button3_Click(object sender, EventArgs e)
        {


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

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            Category = textBox1.Text;
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

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string Category1 = textBox1.Text;
            //string CatID = textBox2.Text;

            string query = "INSERT INTO CategoryTb1 VALUES (@category)";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            //  cmd.Parameters.AddWithValue("@catCode", CatID);
            cmd.Parameters.AddWithValue("@category", Category1);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                MessageBox.Show("Data Successfully Added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateItemsFormCategoryComboBox();
            }
            else
            {
                MessageBox.Show("Failed to Add", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            con.Close();
            GetCityRecord();
            reset();
        }

        private void reset()
        {
            textBox1.Clear();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            string categoryToDelete = textBox1.Text;

            // Delete dependent records in ItemsTb1
            string deleteItemsQuery = "DELETE FROM ItemsTb1 WHERE ItCategory = @CategoryToDelete";
            SqlCommand deleteItemsCmd = new SqlCommand(deleteItemsQuery, con);
            deleteItemsCmd.Parameters.AddWithValue("@CategoryToDelete", categoryToDelete);

            con.Open();
            int rowsDeletedItems = deleteItemsCmd.ExecuteNonQuery();
            con.Close();

            // Now delete the category in CategoryTb1
            string deleteCategoryQuery = "DELETE FROM CategoryTb1 WHERE CatName = @CategoryToDelete";
            SqlCommand deleteCategoryCmd = new SqlCommand(deleteCategoryQuery, con);
            deleteCategoryCmd.Parameters.AddWithValue("@CategoryToDelete", categoryToDelete);

            con.Open();
            int rowsDeletedCategory = deleteCategoryCmd.ExecuteNonQuery();
            con.Close();

            if (rowsDeletedCategory > 0)
            {
                MessageBox.Show("Data Successfully Deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateItemsFormCategoryComboBox();
            }
            else
            {
                MessageBox.Show("Failed to Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            GetCityRecord();
            reset();
        }
    }
}
