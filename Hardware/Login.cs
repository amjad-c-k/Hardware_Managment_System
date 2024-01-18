using System;
using System.Data.SqlClient;
using System.Security;
using System.Windows.Forms;

namespace Hardware
{
    public partial class Login : Form
    {
        private SqlConnection con = new SqlConnection("Data Source=LAPTOP-5JUMML4L\\SQLEXPRESS;Initial Catalog=HardwareDb;Integrated Security=True");

        public Login()
        {
            InitializeComponent();
            maskedTextBox1.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private SecureString GetSecurePassword()
        {
            SecureString securePassword = new SecureString();
            foreach (char c in maskedTextBox1.Text)
            {
                securePassword.AppendChar(c);
            }
            return securePassword;
        }

        private string ConvertToUnsecureString(SecureString secureString)
        {
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secureString);
            try
            {
                return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(maskedTextBox1.Text))
            {
                string Email = textBox1.Text;
                SecureString passwordSecure = GetSecurePassword();
                // Convert the SecureString to plain text
                string password = ConvertToUnsecureString(passwordSecure);

                string query = "INSERT INTO [user] VALUES(@email, @password)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@email", Email);
                cmd.Parameters.AddWithValue("@password", password); // Use the password variable

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Successfully Registered");
                }
                else
                {
                    MessageBox.Show("Failed to Register");
                }
                con.Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(maskedTextBox1.Text))
            {
                string email = textBox1.Text;
                SecureString passwordSecure = GetSecurePassword();

                string query = "SELECT * FROM [user] WHERE Email=@email AND Password=@password";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", ConvertToUnsecureString(passwordSecure));

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                items i = new items();
             
                if (reader.HasRows)
                {
                   
                    i.Show();
                 
                }
                else
                {
                    MessageBox.Show("Failed to Sign in");
                }

              

                reader.Close();
                con.Close();

             
            }
        }
    }
}
