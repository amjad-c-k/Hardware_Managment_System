using iTextSharp.text.pdf;
using iTextSharp.text;
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
using Guna.UI2.WinForms;

namespace Hardware
{
    public partial class Dashboard : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-5JUMML4L\\SQLEXPRESS;Initial Catalog=HardwareDb;Integrated Security=True");
        public Dashboard()
        {
            InitializeComponent();
            ITEMS_DASHBOARD();
            Category_DASHBOARD();

            redArrow.Visible = false;
            greenArrow.Visible = false;
        }

        private void ITEMS_DASHBOARD()
        {
            string query = "SELECT ItCode, ItName FROM ItemsTb1";

            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            guna2DataGridView2.DataSource = dt; // Assuming guna2DataGridView2 is the name of your DataGridView control

            con.Close();
        }

        private void Category_DASHBOARD()
        {
            string query = "SELECT [CatName] FROM [dbo].[CategoryTb1]";

            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            guna2DataGridView1.DataSource = dt; // Assuming guna2DataGridView2 is the name of your DataGridView control

            con.Close();

        }

        private void cutomDataBtn_Click(object sender, EventArgs e)
        {
            updateCounts();
        }

        private void updateCounts()
        {
            DateTime startDate = new DateTime(startDateTime.Value.Year, startDateTime.Value.Month, startDateTime.Value.Day, 0, 0, 0);
            DateTime endDate = new DateTime(endDateTime.Value.Year, endDateTime.Value.Month, endDateTime.Value.Day, 23, 59, 59);

            //ITEM COUNT
            string itemquery = "SELECT COUNT(*) AS ItemCount FROM ItemsTb1 WHERE Date >= @startDate AND Date <= @endDate;";
            SqlCommand cmd = new SqlCommand(itemquery, con);

            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);

            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();

            if (result != null)
            {
                int itemCount = Convert.ToInt32(result);
                itemLabel.Text = itemCount.ToString();
            }

            //ORDER COUNT
            string orderquery = "SELECT COUNT(*) AS OrderCount FROM BillTb WHERE Date >= @startDate AND Date <= @endDate;";
            SqlCommand ordercmd = new SqlCommand(orderquery, con);

            ordercmd.Parameters.AddWithValue("@startDate", startDate);
            ordercmd.Parameters.AddWithValue("@endDate", endDate);

            con.Open();
            object orderResult = ordercmd.ExecuteScalar();
            con.Close();

            if (orderResult != null)
            {
                int OrderCount = Convert.ToInt32(orderResult);
                orderLable.Text = OrderCount.ToString();
            }


            //REVENUE COUNT
            string revenue = "SELECT SUM([Quantity] * [Price]) AS GrandTotal FROM BillTb WHERE Date >= @startDate AND Date <= @endDate;";
            SqlCommand revenuecmd = new SqlCommand(revenue, con);

            revenuecmd.Parameters.AddWithValue("@startDate", startDate);
            revenuecmd.Parameters.AddWithValue("@endDate", endDate);

            con.Open();
            object revenueResult = revenuecmd.ExecuteScalar();
            con.Close();

            if (revenueResult != DBNull.Value && revenueResult != null)
            {
                int RevenueCount = Convert.ToInt32(revenueResult);
                revenueLable.Text = RevenueCount.ToString();

                if (RevenueCount >= 300000)
                {
                    greenArrow.Visible = true;
                    redArrow.Visible = false;
                    revenueLable.ForeColor = Color.White;
                }
                else
                {
                    redArrow.Visible = true;
                    greenArrow.Visible = false;

                    if (redArrow.Visible)
                    {
                        revenueLable.ForeColor = Color.Red;
                    }

                }
            }


            //CUSTOMER COUNT
            string customerquery = "SELECT COUNT(*) FROM CustomerTb1 WHERE Date >= @startDate AND Date <= @endDate;";
            SqlCommand customercmd = new SqlCommand(customerquery, con);

            customercmd.Parameters.AddWithValue("@startDate", startDate);
            customercmd.Parameters.AddWithValue("@endDate", endDate);

            con.Open();
            object customerResult = customercmd.ExecuteScalar();
            con.Close();

            if (customerResult != null)
            {
                int CustomerCount = Convert.ToInt32(customerResult);
                customerLable.Text = CustomerCount.ToString();
            }

            printToPdf();
        }

        private void todayDataBtn_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            // Set the startDateTime to the beginning of the day
            startDateTime.Value = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            // Set the endDateTime to the end of the day
            endDateTime.Value = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            updateCounts();

        }

        private void thisWeekDataBtn_Click(object sender, EventArgs e)
        {
            startDateTime.Value = DateTime.Now.AddDays(-7);
            endDateTime.Value = DateTime.Now;

            updateCounts();
        }

        private void thisMonthDataBtn_Click(object sender, EventArgs e)
        {
            startDateTime.Value = DateTime.Now.AddDays(-30);
            endDateTime.Value = DateTime.Now;

            updateCounts();
        }


        private void printToPdf()
        {
            string order, totalRevenue, customers, totalItems;
            order = orderLable.Text;
            totalRevenue = revenueLable.Text;
            customers = customerLable.Text;
            totalItems = itemLabel.Text;

            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream("C:\\Users\\Amjad Khalil\\Desktop\\PDF\\a.pdf", FileMode.Create));
            document.Open();
            document.Add(new Paragraph("Number of Orders: " + order));
            document.Add(new Paragraph("Total Revenue: " + totalRevenue));
            document.Add(new Paragraph("Number of Customer: " + customers));
            document.Add(new Paragraph("Total Items: " + totalItems));
            document.Close();

        }


        private void label15_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            items i = new items();
            i.Show();
            if (i.Visible)
            {
                this.Close();
            }
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            Categories categories = new Categories();
            categories.Show();
            if (categories.Visible)
            {
                this.Close();
            }
        }

        private void label4_Click_1(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            if (customers.Visible)
            {
                this.Close();
            }
        }

        private void label5_Click_1(object sender, EventArgs e)
        {
            Billing billing = new Billing();
            billing.Show();
            if (billing.Visible)
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

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
