using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hardware
{

    public partial class Billing : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-5JUMML4L\\SQLEXPRESS;Initial Catalog=HardwareDb;Integrated Security=True");
        int ItCode;

        string itemBill;
        string categoryBill;
        int priceBill;
        int quantityBill;
        DateTime combinedDateTime;
        public Billing()
        {
            InitializeComponent();
            InitializeCustomerCombo();
            GetCityRecord();
            billingCategoryTB.Text = "Select";
            customerCombo.Text = "Select";
        }

        private void Billing_Load(object sender, EventArgs e)
        {

        }

        private void InitializeCustomerCombo()
        {
            string query = "SELECT * from CustomerTb1";

            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                customerCombo.Items.Add(row["CustName"].ToString());
            }
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
        private void label15_Click(object sender, EventArgs e)
        {
            this.Close();
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
        int stock = 0;
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ItCode = int.Parse(guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            billinItemTB.Text = guna2DataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            billingCategoryTB.Text = guna2DataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            billingPriceTB.Text = guna2DataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            billingQuantityTB.Text = guna2DataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            // MessageBox.Show(ItCode.ToString());
            stock = int.Parse(billingQuantityTB.Text);
        }
        public void reset()
        {
            billinItemTB.Clear();
            billingPriceTB.Clear();
            billingQuantityTB.Clear();

        }

        private void NEWBILL()
        {

        }
        //int TotalPrice = 0;
        private void billCal()
        {
            itemBill = billinItemTB.Text;
            categoryBill = billingCategoryTB.Text;
            priceBill = int.Parse(billingPriceTB.Text);
            quantityBill = int.Parse(billingQuantityTB.Text);

            DateTime selectedDate = guna2DateTimePicker1.Value;
            DateTime currentDateAndTime = DateTime.Now;
            combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, currentDateAndTime.Hour, currentDateAndTime.Minute, currentDateAndTime.Second);


            string query = "INSERT INTO BillTb VALUES (@itemBill, @categoryBill, @priceBill,@quantityBill, @Date, @Customer)";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.Parameters.AddWithValue("@itemBill", itemBill);
            cmd.Parameters.AddWithValue("@categoryBill", categoryBill);
            cmd.Parameters.AddWithValue("@priceBill", priceBill);
            cmd.Parameters.AddWithValue("@quantityBill", quantityBill);
            cmd.Parameters.AddWithValue("@Date", combinedDateTime);
            cmd.Parameters.AddWithValue("@Customer", customerCombo.Text);

            int rows = cmd.ExecuteNonQuery();
            con.Close();

            // BILL bill = new BILL(itemBill,priceBill,quantityBill);

            string fileName = "C:\\Users\\Amjad Khalil\\Desktop\\BillRecord.html";

            try
            {
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = new StreamWriter(fileName, true))
                    {
                        sw.WriteLine("<html><head><style>");
                        sw.WriteLine("body { font-family: 'Arial', sans-serif; background-color: #008080; margin: 0; padding: 0; }");
                        sw.WriteLine("h2 { color: #333; text-align: center; padding-top: 20px; }");
                        sw.WriteLine("table { border-collapse: collapse; width: 70%; margin: 20px auto; background-color: #fff; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }");
                        sw.WriteLine("th, td { border: 1px solid #dddddd; padding: 15px; text-align: left; }");
                        sw.WriteLine("th { background-color: #4CAF50; color: white; }");
                        sw.WriteLine("tr:nth-child(even) { background-color: #f2f2f2; }");
                        sw.WriteLine("tr:nth-child(odd) { background-color: #ffffff; }");
                        sw.WriteLine("</style></head><body>");
                        sw.WriteLine("<h2>Billing Record</h2>");
                        sw.WriteLine("<table><tr><th>Item</th><th>Category</th><th>Price</th><th>Quantity</th><th>TotalPrice</th><th>Date</th>");
                    }
                }


                int TotalPrice = priceBill * quantityBill;

                using (StreamWriter sw = new StreamWriter(fileName, true))
                {
                    sw.WriteLine($"<tr><td>{itemBill}</td><td>{categoryBill}</td><td>{priceBill}</td><td>{quantityBill}</td><td>{TotalPrice}</td><td>{combinedDateTime}</td></tr>");
                }

                MessageBox.Show("Successfully Added");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add: {ex.Message}");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {


        }

        private void label3_Click(object sender, EventArgs e)
        {
            Categories categories = new Categories();
            categories.Show();
            if (categories.Visible)
            {
                this.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            if (customers.Visible)
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
            GetCityRecord();
            BILL bill = new BILL(customerCombo.Text, itemBill, priceBill, quantityBill, combinedDateTime);
            bill.Show();

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            billinItemTB.Clear();
            billingQuantityTB.Clear();
            billingPriceTB.Clear();
            billingCategoryTB.Text = "Select";
            customerCombo.Text = "Select";
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void addToBillBtn_Click(object sender, EventArgs e)
        {
            int quantity = int.Parse(billingQuantityTB.Text);

            if (quantity > 0 && quantity <= stock)
            {
                int stockLeft = stock - quantity;


                string query = "UPDATE ItemsTb1 SET Stock = @stock WHERE ItCode = @ItCode";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                cmd.Parameters.AddWithValue("@ItCode", ItCode);
                cmd.Parameters.AddWithValue("@stock", stockLeft);


                int rows = cmd.ExecuteNonQuery();

                con.Close();
                billCal();
            }
            else
            {
                MessageBox.Show("Enter Valid entry");
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
