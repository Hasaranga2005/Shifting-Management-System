using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace eShiftManagementSystem
{
    public partial class frmAdminLogin : Form
    {
        string connectionString = "Server=DESKTOP-L03EVJF\\SQLEXPRESS;Database=E-ShiftDB;Trusted_Connection=True;";

        public frmAdminLogin()
        {
            InitializeComponent();
            toolTip1.SetToolTip(pbClose, "Close");
            toolTip1.SetToolTip(pictureBox1, "Back");
            toolTip1.SetToolTip(button1, "Hide Password");
            toolTip1.SetToolTip(button2, "Show Password");
            toolTip1.BackColor = Color.LightYellow;
            toolTip1.ForeColor = Color.Black;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private async void btnAdminLogin_Click(object sender, EventArgs e)
        {
            string username = txtAdminUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            lblAdminUsernameRequired.Visible = false;
            lblAdminPasswordRequired.Visible = false;
            lblAdminUserNotFound.Visible = false;
            lblAdminIncorrectPassword.Visible = false;

            panelAdminUsername.BackColor = Color.Gray;
            panelAdminPassword.BackColor = Color.Gray;

            bool hasEmpty = false;

            if (string.IsNullOrWhiteSpace(username))
            {
                lblAdminUsernameRequired.Visible = true;
                panelAdminUsername.BackColor = Color.Red;
                await Shake(panelAdminUsername);
                hasEmpty = true;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                lblAdminPasswordRequired.Visible = true;
                panelAdminPassword.BackColor = Color.Red;
                await Shake(panelAdminPassword);
                hasEmpty = true;
            }

            if (hasEmpty) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string checkUserQuery = "SELECT Password FROM Admins WHERE Username = @Username";
                    SqlCommand cmd = new SqlCommand(checkUserQuery, conn);
                    cmd.Parameters.AddWithValue("@Username", username);

                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        lblAdminUserNotFound.Visible = true;
                        panelAdminUsername.BackColor = Color.Red;
                        panelAdminPassword.BackColor = Color.Red;
                        txtAdminUsername.Clear();
                        txtPassword.Clear();
                        await Shake(panelAdminUsername);
                        await Shake(panelAdminPassword);
                        return;
                    }

                    string correctPassword = result.ToString();

                    if (correctPassword.Trim() != password)
                    {
                        lblAdminIncorrectPassword.Visible = true;
                        panelAdminPassword.BackColor = Color.Red;
                        txtPassword.Clear();
                        await Shake(panelAdminPassword);
                        return;
                    }

                    MessageBox.Show("Login successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmAdminDashboard dashboard = new frmAdminDashboard();
                    dashboard.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private async Task Shake(Control control)
        {
            var original = control.Location;
            for (int i = 0; i < 4; i++)
            {
                control.Location = new Point(original.X + 5, original.Y);
                await Task.Delay(40);
                control.Location = new Point(original.X - 5, original.Y);
                await Task.Delay(40);
            }
            control.Location = original;
        }
        private void txtAdminUsername_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAdminUsername.Text))
            {
                lblAdminUsernameRequired.Visible = false;
                lblAdminUserNotFound.Visible = false;
                panelAdminUsername.BackColor = Color.SeaGreen;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblAdminPasswordRequired.Visible = false;
                lblAdminIncorrectPassword.Visible = false;
                panelAdminPassword.BackColor = Color.SeaGreen;
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmstart start = new frmstart();
            start.Show();
            this.Hide();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Silver;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPassword.PasswordChar == '\0')
            {
                button2.BringToFront();
                txtPassword.PasswordChar = '*';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(txtPassword.PasswordChar == '*')
            {
                button1.BringToFront();
                txtPassword.PasswordChar = '\0';
            }
        }
    }
}
