using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace eShiftManagementSystem
{
    public partial class frmstart : Form
    {
        public frmstart()
        {
            InitializeComponent();
            toolTip1.SetToolTip(pbClose, "Close");
            toolTip1.SetToolTip(pbMinimize, "Minimize");
            toolTip1.BackColor = Color.LightYellow;
            toolTip1.ForeColor = Color.Black;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
       
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            frmAdminLogin adminLogin = new frmAdminLogin();
            adminLogin.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmcustomerlogin customerLogin = new frmcustomerlogin();
            customerLogin.Show();
            this.Hide();
        }

        private void pbClose_MouseEnter_1(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave_1(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;
        }

        private void pbMinimize_MouseEnter_1(object sender, EventArgs e)
        {
            pbMinimize.BackColor = Color.Silver;
        }

        private void pbMinimize_MouseLeave_1(object sender, EventArgs e)
        {
            pbMinimize.BackColor = Color.Transparent;
        }
    }
}
