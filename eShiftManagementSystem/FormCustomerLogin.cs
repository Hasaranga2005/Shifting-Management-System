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

namespace eShiftManagementSystem
{
    public partial class frmcustomerlogin : Form
    {
        public frmcustomerlogin()
        {
            InitializeComponent();
            toolTip1.SetToolTip(pbClose, "Close");
            toolTip1.SetToolTip(pbback, "Back");
            toolTip1.SetToolTip(button1, "Hide Password");
            toolTip1.SetToolTip(button2, "Show Password");
            toolTip1.BackColor = Color.LightYellow;
            toolTip1.ForeColor = Color.Black;
            lblErrorUserNotFound.Text = "";
            lblErrorUserNotFound.Visible = true;
            lblErrorUserNotFound.ForeColor = Color.Red;
            lblErrorUserNotFound.AutoSize = true;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmstart start = new frmstart();
            start.Show();
            this.Hide();
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;

        }

        private void pbback_MouseEnter(object sender, EventArgs e)
        {
            pbback.BackColor = Color.Silver;
        }

        private void pbback_MouseLeave(object sender, EventArgs e)
        {
            pbback.BackColor = Color.Transparent;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtcustomerPassword.PasswordChar == '*')
            {
                button1.BringToFront();
                txtcustomerPassword.PasswordChar = '\0';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtcustomerPassword.PasswordChar == '\0')
            {
                button2.BringToFront();
                txtcustomerPassword.PasswordChar = '*';
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmcustomersignup signUp = new frmcustomersignup();
            signUp.Show();
            this.Close();
        }

        private void btnCustomerLogin_Click(object sender, EventArgs e)
        {
            lblErrorWrongPassword.Visible = false;
            lblErrorUserNotFound.Visible = false;
            lblUsernameRequired.Visible = false;
            lblPasswordRequired.Visible = false;

            string username = txtcustomerUsername.Text.Trim();
            string password = txtcustomerPassword.Text.Trim();
            bool hasEmptyField = false;

            if (string.IsNullOrWhiteSpace(username))
            {
                lblUsernameRequired.Text = "Please enter your username.";
                lblUsernameRequired.Visible = true;
                panelUsername.BackColor = Color.Red;
                hasEmptyField = true;
            }
            else
            {
                panelUsername.BackColor = Color.LightGray;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                lblPasswordRequired.Text = "Please enter your password.";
                lblPasswordRequired.Visible = true;
                panelPassword.BackColor = Color.Red;
                hasEmptyField = true;
            }
            else
            {
                panelPassword.BackColor = Color.LightGray;
            }

            if (hasEmptyField)
                return; 

            string connectionString = @"Data Source=DESKTOP-L03EVJF\SQLEXPRESS;Initial Catalog=E-ShiftDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT CustomerID, FirstName, Password, Email FROM Customers WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbPassword = reader["Password"].ToString();
                                int customerId = Convert.ToInt32(reader["CustomerID"]);
                                string customerName = reader["FirstName"].ToString();
                                string customerEmail = reader["Email"].ToString();


                                if (password == dbPassword)
                                {
                                    frmCustomerDashboard dashboard = new frmCustomerDashboard();
                                    dashboard.LoggedInCustomerID = customerId;
                                    dashboard.LoggedInCustomerName = customerName;
                                    dashboard.LoggedInCustomerEmail = customerEmail;
                                    dashboard.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    lblErrorWrongPassword.Text = "Incorrect password.";
                                    lblErrorWrongPassword.Visible = true;
                                    txtcustomerPassword.Clear();
                                    txtcustomerPassword.Focus();
                                }
                            }
                            else
                            {
                                lblErrorUserNotFound.Text = "Username not found.";
                                lblErrorUserNotFound.Visible = true;
                                txtcustomerUsername.Clear();
                                txtcustomerPassword.Clear();
                                txtcustomerUsername.Focus();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        
        private void txtcustomerUsername_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtcustomerUsername.Text))
            {
                lblUsernameRequired.Visible = false;
                panelUsername.BackColor = Color.SeaGreen;
            }
        }

        private void txtcustomerPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtcustomerPassword.Text))
            {
                lblPasswordRequired.Visible = false;
                panelPassword.BackColor = Color.SeaGreen;
            }
        }
    }
}
