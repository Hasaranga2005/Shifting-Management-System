using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eShiftManagementSystem
{
    public partial class frmcustomersignup : Form
    {
        string connectionString = "Server=DESKTOP-L03EVJF\\SQLEXPRESS;Database=E-ShiftDB;Trusted_Connection=True;";
        public frmcustomersignup()
        {
            InitializeComponent();
            toolTip1.SetToolTip(pbClose, "Close");
            toolTip1.SetToolTip(pbback, "Back");
            toolTip1.SetToolTip(button4, "Hide Password");
            toolTip1.SetToolTip(button3, "Show Password");
            toolTip1.SetToolTip(button1, "Hide Password");
            toolTip1.SetToolTip(button2, "Show Password");
            toolTip1.BackColor = Color.LightYellow;
            toolTip1.ForeColor = Color.Black;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmcustomersignup_Load(object sender, EventArgs e)
        {
            lblFirstNameError.BringToFront();
            lblLastNameError.BringToFront();
            lblEmailError.BringToFront();
            lblUsernameError.BringToFront();
            lblPasswordError.BringToFront();
            lblConfirmPasswordError.BringToFront();
        }

        private void ClearAllErrors()
        {
            lblFirstNameError.Text = "";
            lblLastNameError.Text = "";
            lblEmailError.Text = "";
            lblUsernameError.Text = "";
            lblPasswordError.Text = "";
            lblConfirmPasswordError.Text = "";

            panelFirstName.BackColor = Color.Gray;
            panelLastName.BackColor = Color.Gray;
            panelEmail.BackColor = Color.Gray;
            panelUsername.BackColor = Color.Gray;
            panelPassword.BackColor = Color.Gray;
            panelConfirmPassword.BackColor = Color.Gray;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private async void btnAdminLogin_Click(object sender, EventArgs e)
        {
            lblFirstNameError.Text = "";
            lblLastNameError.Text = "";
            lblEmailError.Text = "";
            lblUsernameError.Text = "";
            lblPasswordError.Text = "";
            lblConfirmPasswordError.Text = "";
            lblError.Text = "";

            ClearAllErrors();

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(txtfirstname.Text))
            {
                panelFirstName.BackColor = Color.Red;
                lblFirstNameError.Text = "First name is required.";
                picTickFirstName.Visible = false;
                hasError = true;
            }
            else
            {
                panelFirstName.BackColor = Color.SeaGreen;
                picTickFirstName.Visible = true;
            }

            if (string.IsNullOrWhiteSpace(txtlastname.Text))
            {
                panelLastName.BackColor = Color.Red;
                lblLastNameError.Text = "Last name is required.";
                picTickLastName.Visible = false;
                hasError = true;
            }
            else
            {
                panelLastName.BackColor = Color.SeaGreen;
                picTickLastName.Visible = true;
            }

            if (!IsValidEmail(txtemail.Text))
            {
                panelEmail.BackColor = Color.Red;
                lblEmailError.Text = "Invalid email address.";
                picTickEmail.Visible = false;
                hasError = true;
            }
            else
            {
                panelEmail.BackColor = Color.SeaGreen;
                picTickEmail.Visible = true;
            }

            if (string.IsNullOrWhiteSpace(txtusername.Text))
            {
                panelUsername.BackColor = Color.Red;
                lblUsernameError.Text = "Username is required.";
                picTickUsername.Visible = false;
                hasError = true;
            }
            else
            {
                panelUsername.BackColor = Color.SeaGreen;
                picTickUsername.Visible = true;
            }

            string password = txtpassword.Text;
            if (!ValidatePasswordField())
                hasError = true;


            if (string.IsNullOrWhiteSpace(txtconfirmpassword.Text))
            {
                panelConfirmPassword.BackColor = Color.Red;
                lblConfirmPasswordError.Text = "Please confirm your password.";
                picTickConfirmPass.Visible = false;
                hasError = true;
            }
            else if (password != txtconfirmpassword.Text)
            {
                panelConfirmPassword.BackColor = Color.Red;
                lblConfirmPasswordError.Text = "Passwords do not match.";
                picTickConfirmPass.Visible = false;
                hasError = true;
            }
            else 
            {
                panelConfirmPassword.BackColor = Color.SeaGreen;
                picTickConfirmPass.Visible= true;
            }

            if (hasError)
                return;

            panelOverlay.Visible = true;
            panelOverlay.BringToFront();
            lblProcessing.Text = "Registering...";
            lblProcessing.Visible = true;
            picLoading.Visible = true;
            Application.DoEvents(); 

            await Task.Delay(2000); 


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string emailCheckQuery = "SELECT COUNT(*) FROM Customers WHERE Email = @Email";
                    using (SqlCommand emailCmd = new SqlCommand(emailCheckQuery, conn))
                    {
                        emailCmd.Parameters.AddWithValue("@Email", txtemail.Text);
                        int emailExists = (int)emailCmd.ExecuteScalar();
                        if (emailExists > 0)
                        {
                            panelEmail.BackColor = Color.Red;
                            lblEmailError.Text = "E-mail address already Exists.";
                            lblProcessing.Visible = false;
                            picLoading.Visible = false;
                            panelOverlay.Visible = false;
                            MessageBox.Show("This email is already registered. Please use a different email.", "Email Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;

                        }
                    }

                    string usernameCheckQuery = "SELECT COUNT(*) FROM Customers WHERE Username = @Username";
                    using (SqlCommand usernameCmd = new SqlCommand(usernameCheckQuery, conn))
                    {
                        usernameCmd.Parameters.AddWithValue("@Username", txtusername.Text);
                        int usernameExists = (int)usernameCmd.ExecuteScalar();
                        if (usernameExists > 0)
                        {
                            panelUsername.BackColor = Color.Red;
                            lblUsernameError.Text = "Username already exists.";
                            lblProcessing.Visible = false;
                            picLoading.Visible = false;
                            panelOverlay.Visible = false;
                            MessageBox.Show("This username is already taken. Please choose another one.", "Username Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string insertQuery = @"
                INSERT INTO Customers (FirstName, LastName, Email, Username, Password)
                VALUES (@FirstName, @LastName, @Email, @Username, @Password)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@FirstName", txtfirstname.Text);
                        insertCmd.Parameters.AddWithValue("@LastName", txtlastname.Text);
                        insertCmd.Parameters.AddWithValue("@Email", txtemail.Text);
                        insertCmd.Parameters.AddWithValue("@Username", txtusername.Text);
                        insertCmd.Parameters.AddWithValue("@Password", txtpassword.Text.Trim());

                        int rowsInserted = insertCmd.ExecuteNonQuery();
                        if (rowsInserted > 0)
                        {
                            lblProcessing.Visible = false;
                            picLoading.Visible = false;
                            panelOverlay.Visible = false;

                            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields(); 

                            this.Close();

                            frmcustomerlogin loginForm = new frmcustomerlogin(); 
                            loginForm.Show();

                        }
                        else
                        {
                            lblError.Text = "Registration failed. Try again.";
                            panelOverlay.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    panelOverlay.Visible = false;
                }
            }
        }
        private bool IsStrongPassword(string password)
        {
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$";
            return Regex.IsMatch(password, pattern);
        }

        private void ClearFields()
        {
            txtfirstname.Clear();
            txtlastname.Clear();
            txtemail.Clear();
            txtusername.Clear();
            txtpassword.Clear();
            txtconfirmpassword.Clear();
            picTickFirstName.Visible = false;
            picTickLastName.Visible = false;
            picTickEmail.Visible = false;
            picTickUsername.Visible = false;
            picTickPassword.Visible = false;
            picTickConfirmPass.Visible = false;

            panelFirstName.BackColor = panelLastName.BackColor = panelEmail.BackColor =
            panelUsername.BackColor = panelPassword.BackColor = panelConfirmPassword.BackColor = Color.Silver;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtpassword.PasswordChar == '*')
            {
                button4.BringToFront();
                txtpassword.PasswordChar = '\0';
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtpassword.PasswordChar == '\0')
            {
                button3.BringToFront();
                txtpassword.PasswordChar = '*';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtconfirmpassword.PasswordChar == '*')
            {
                button1.BringToFront();
                txtconfirmpassword.PasswordChar = '\0';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtconfirmpassword.PasswordChar == '\0')
            {
                button2.BringToFront();
                txtconfirmpassword.PasswordChar = '*';
            }
        }

        private void pbback_Click(object sender, EventArgs e)
        {
            frmcustomerlogin start = new frmcustomerlogin();
            start.Show();
            this.Hide();
        }

        private void pbback_MouseEnter(object sender, EventArgs e)
        {
            pbback.BackColor = Color.Silver;
        }

        private void pbback_MouseLeave(object sender, EventArgs e)
        {
            pbback.BackColor = Color.Transparent;
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;
        }

        private void llblregister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmcustomerlogin signUp = new frmcustomerlogin();
            signUp.Show();
            this.Close();
        }

        private void txtfirstname_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtfirstname.Text))
            {
                panelFirstName.BackColor = Color.Red;
                lblFirstNameError.Text = "First name is required.";
                picTickFirstName.Visible = false;
                picCrossFirstName.Visible = true;

            }
            else
            {
                panelFirstName.BackColor = Color.SeaGreen;
                lblFirstNameError.Text = "";
                picTickFirstName.Visible = true;
                picCrossFirstName.Visible = false;
            }
        }

        private void txtemail_Leave(object sender, EventArgs e)
        {

            if (!IsValidEmail(txtemail.Text))
            {
                panelEmail.BackColor = Color.Red;
                lblEmailError.Text = "Invalid email address.";
                picTickEmail.Visible = false;
                picCrossEmail.Visible = true;
            }
            else
            {
                panelEmail.BackColor = Color.SeaGreen;
                lblEmailError.Text = "";
                picTickEmail.Visible = true;
                picCrossEmail.Visible = false;
            }
        }

        private void txtpassword_Leave(object sender, EventArgs e)
        {
            ValidatePasswordField();
        }

        private void txtconfirmpassword_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtconfirmpassword.Text))
            {
                panelConfirmPassword.BackColor = Color.Red;
                lblConfirmPasswordError.Text = "Please confirm your password.";
                picTickConfirmPass.Visible = false;
                picCrossConfirmPassword.Visible = true;
            }
            else if (txtconfirmpassword.Text != txtpassword.Text)
            {
                panelConfirmPassword.BackColor = Color.Red;
                lblConfirmPasswordError.Text = "Passwords do not match.";
                picTickConfirmPass.Visible = false;
                picCrossConfirmPassword.Visible = true;
            }
            else
            {
                panelConfirmPassword.BackColor = Color.SeaGreen;
                lblConfirmPasswordError.Text = "";
                picTickConfirmPass.Visible = true;
                picCrossConfirmPassword.Visible = false;
            }
        }

        private void txtlastname_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtlastname.Text))
            {
                panelLastName.BackColor = Color.Red;
                lblLastNameError.Text = "Last name is required.";
                picTickLastName.Visible = false;
                picCrossLastName.Visible = true;
            }
            else
            {
                panelLastName.BackColor = Color.SeaGreen;
                lblLastNameError.Text = "";
                picTickLastName.Visible = true;
                picCrossLastName.Visible = false;
            }
        }

        private void txtusername_Leave(object sender, EventArgs e)
        {

            string username = txtusername.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                panelUsername.BackColor = Color.Red;
                lblUsernameError.Text = "Username is required.";
                picTickUsername.Visible = false;
                picCrossUsername.Visible = true;
            }
            else if (username.Length < 4 || !Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                panelUsername.BackColor = Color.Red;
                lblUsernameError.Text = "Use at least 4 letters, no spaces/symbols.";
                picTickUsername.Visible = false;
                picCrossUsername.Visible = true;
            }
            else
            {
                panelUsername.BackColor = Color.SeaGreen;
                lblUsernameError.Text = "";
                picTickUsername.Visible = true;
                picCrossUsername.Visible = false;
            }
        }

        private bool ValidatePasswordField()
        {
            string password = txtpassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(password))
            {
                panelPassword.BackColor = Color.Red;
                lblPasswordError.Text = "Password is required.";
                picTickPassword.Visible = false;
                picCrossPassword.Visible = true;
                return false;
            }
            else if (!IsStrongPassword(password))
            {
                panelPassword.BackColor = Color.Red;
                lblPasswordError.Text = "Password must include uppercase, lowercase, number, and special character.";
                picTickPassword.Visible = false;
                picCrossPassword.Visible = true;
                return false;
            }
            else
            {
                panelPassword.BackColor = Color.SeaGreen;
                lblPasswordError.Text = "";
                picTickPassword.Visible = true;
                picCrossPassword.Visible = false;
                return true;
            }
        }
    }
}
