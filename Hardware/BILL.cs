using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hardware
{
    public partial class BILL : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-5JUMML4L\\SQLEXPRESS;Initial Catalog=HardwareDb;Integrated Security=True");
        private string customer;
        int total;

        public BILL(string customer, string itemName, int Price, int Quantity, DateTime DATED)
        {
            InitializeComponent();
            this.customer = customer;
            GetCityRecord();
            CalculateTotalBill();

            total = Price * Quantity;
            label9.Text = itemName;
            label8.Text = Quantity.ToString();
            label7.Text = Price.ToString();
            label6.Text = total.ToString();
            label11.Text = DATED.ToString();
        }



        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GetCityRecord()
        {
            string query = "SELECT * from BillTb WHERE Customer = @Customer";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Customer", this.customer);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            guna2DataGridView2.DataSource = dt;
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void CalculateTotalBill()
        {
            int totalBill = 0;

            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                if (row.Cells["Price"].Value != null && row.Cells["Quantity"].Value != null &&
                    int.TryParse(row.Cells["Price"].Value.ToString(), out int price) &&
                    int.TryParse(row.Cells["Quantity"].Value.ToString(), out int quantity))
                {
                    int itemTotal = price * quantity;
                    totalBill += itemTotal;
                }
            }

            totalLabel.Text = $"Total Bill: {totalBill}";
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void SaveBillToFile()
        {
            string billContent = GetBillContent();

            string folderPath = "C:\\User\\Amjad Khalil";
            string filePath = Path.Combine(folderPath, "Bill.txt");

            try
            {
                Directory.CreateDirectory(folderPath);

                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.Write(billContent);
                }

                MessageBox.Show("Bill has been saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save the bill: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetBillContent()
        {
            string billContent = string.Empty;

            foreach (Control control in guna2CustomGradientPanel1.Controls)
            {
                if (control is Label)
                {
                    Label label = (Label)control;
                    billContent += $"{label.Text}\n";
                }
            }

            return billContent;
        }
        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            SaveBillToFile();
        }
    }
}
