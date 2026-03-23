using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Linq;


namespace eShiftManagementSystem
{
    public partial class frmAdminDashboard : Form
    {
        string connectionString = @"Data Source=DESKTOP-L03EVJF\SQLEXPRESS;Initial Catalog=E-ShiftDB;Integrated Security=True";

        public frmAdminDashboard()
        {

            InitializeComponent();
            toolTip1.SetToolTip(pbClose, "Back");
            btnSubmit.Click += btnSubmit_Click;
        }

        private void frmAdminDashboard_Load(object sender, EventArgs e)
        {
            Console.WriteLine(panelAdminContainer.Parent.Name);

            panelNavHome.MouseClick += NavPanel_Click;
            panelNavAdmins.MouseClick += NavPanel_Click;
            panelNavShiftRequests.MouseClick += NavPanel_Click;
            panelNavCustomers.MouseClick += NavPanel_Click;
            panelNavUpdateStatus.MouseClick += NavPanel_Click;
            panelNavTransportUnits.MouseClick += NavPanel_Click;
            panelNavReports.MouseClick += NavPanel_Click;

            HighlightSidebar(panelNavHome);
            panelHomeContainer.Visible = true;
            panelAdminContainer.Visible = false;
            panelShiftRequestsContainer.Visible = false;
            panelCustomersContainer.Visible = false;
            panelShiftStatusContainer.Visible = false;
            panelTransportContainer.Visible = false;
            panelReportsContainer.Visible = false;

            LoadHomeDashboardData();
            LoadCustomerData();
            txtSearchCustomers.TextChanged += txtSearchCustomers_TextChanged;

            panelSearchPrompt.Visible = true;
            cmbUpdateStatus.Items.Clear();
            cmbUpdateStatus.Items.AddRange(new string[] { "Pending", "Ongoing", "Completed", "Cancelled" });

            lblNoRequestSelected.Visible = true;

            cmbStatusFilter.Items.Clear();
            cmbStatusFilter.Items.AddRange(new string[] { "All", "Pending", "Completed", "Rejected" });
            cmbStatusFilter.SelectedIndex = 0;

            cmbDateSort.Items.Clear();
            cmbDateSort.Items.AddRange(new string[] { "Latest First", "Oldest First" });
            cmbDateSort.SelectedIndex = 0;
            cmbDateSort.SelectedIndexChanged += (s, args) => btnSubmit.PerformClick();
            cmbStatusFilter.SelectedIndexChanged += (s, args) => btnSubmit.PerformClick();
            txtCustomerSearch.TextChanged += txtCustomerSearch_TextChanged;

            LoadPendingShiftRequests();

            LoadAdmins();
            cmbAdminRole.Items.AddRange(new string[] { "Admin", "SuperAdmin" });

            btnVehicleInfo.MouseClick += TransportNav_Click;
            btnAssignShift.MouseClick += TransportNav_Click;
            btnAddVehicle.MouseClick += TransportNav_Click;
            btnDrivers.MouseClick += TransportNav_Click;
            btnAddDriver.MouseClick += TransportNav_Click;

            btnVehicleInfo.MouseEnter += SidebarButton_MouseEnter;
            btnAssignShift.MouseEnter += SidebarButton_MouseEnter;
            btnAddVehicle.MouseEnter += SidebarButton_MouseEnter;
            btnDrivers.MouseEnter += SidebarButton_MouseEnter;
            btnAddDriver.MouseEnter += SidebarButton_MouseEnter;

            btnVehicleInfo.MouseLeave += SidebarButton_MouseLeave;
            btnAssignShift.MouseLeave += SidebarButton_MouseLeave;
            btnAddVehicle.MouseLeave += SidebarButton_MouseLeave;
            btnDrivers.MouseLeave += SidebarButton_MouseLeave;
            btnAddDriver.MouseLeave += SidebarButton_MouseLeave;

            labelVehicleInfo.Click += (s, e2) => TransportNav_Click(btnVehicleInfo, null);
            labelAssignShift.Click += (s, e2) => TransportNav_Click(btnAssignShift, null);
            lblAddVehicle.Click += (s, e2) => TransportNav_Click(btnAddVehicle, null);
            labelDrivers.Click += (s, e2) => TransportNav_Click(btnDrivers, null);
            lblAddDriver.Click += (s, e2) => TransportNav_Click(btnAddDriver, null);

            TransportNav_Click(btnVehicleInfo, null);

            cmbVehicleType.Items.Clear();
            cmbVehicleType.Items.AddRange(new string[] {
                "Bike", "Three Wheeler", "Van", "Lorry", "Container", "Truck"
            });
            cmbVehicleType.SelectedIndex = 0;

            btnAddVehicles.Click += btnAddVehicles_Click;
            btnRefreshVehicles.Click += btnRefreshVehicles_Click;
            txtSearchVehicles.TextChanged += txtSearchVehicles_TextChanged;
            btnClearSearchVehicle.Click += btnClearSearchVehicle_Click;

            LoadPendingShiftsForAssignment(); 
            LoadAssignShiftResources();
            txtSearchDriver.TextChanged += txtSearchDriver_TextChanged;
            btnResetDriver.Click += btnResetDriver_Click;
            cmbReportType.Items.AddRange(new string[] {
                "All Jobs",
                "Pending Jobs",
                "Completed Jobs",
                "Customer List",
                "Load Summary",
                "Transport Unit Summary"
            });

            cmbReportType.SelectedIndex = 0;
            LoadReportTypes();

        }

        ////////////////////////////////////////////////////Navigation Panel///////////////////////////////////////////////////////////////////
        private void NavPanel_Click(object sender, MouseEventArgs e)
        {
            if (sender is Panel clickedPanel)
            {
                HighlightSidebar(clickedPanel);

                panelHomeContainer.Visible = false;
                panelAdminContainer.Visible = false;
                panelShiftRequestsContainer.Visible = false;
                panelCustomersContainer.Visible = false;
                panelShiftStatusContainer.Visible = false;
                panelTransportContainer.Visible = false;
                panelReportsContainer.Visible = false;

                if (clickedPanel == panelNavHome)
                    panelHomeContainer.Visible = true;
                else if (clickedPanel == panelNavAdmins)
                    panelAdminContainer.Visible = true;
                else if (clickedPanel == panelNavShiftRequests)
                    panelShiftRequestsContainer.Visible = true;
                else if (clickedPanel == panelNavCustomers)
                    panelCustomersContainer.Visible = true;
                else if (clickedPanel == panelNavUpdateStatus)
                    panelShiftStatusContainer.Visible = true;
                else if (clickedPanel == panelNavTransportUnits)
                    panelTransportContainer.Visible = true;
                else if(clickedPanel == panelNavReports)
                    panelReportsContainer.Visible = true;
            }
        }

        private void HighlightSidebar(Panel selectedPanel)
        {

            Color defaultColor = Color.FromArgb(45, 45, 45);
            panelNavHome.BackColor = defaultColor;
            panelNavAdmins.BackColor = defaultColor;
            panelNavShiftRequests.BackColor = defaultColor;
            panelNavCustomers.BackColor = defaultColor;
            panelNavUpdateStatus.BackColor = defaultColor;
            panelNavTransportUnits.BackColor = defaultColor;
            panelNavReports.BackColor = defaultColor;

            selectedPanel.BackColor = Color.FromArgb(70, 130, 180);
        }
        //////////////////////////////////////////////////// Navigation Panel END ///////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////// Home Section /////////////////////////////////////////////////////////////////////////

        private void LoadHomeDashboardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmdCustomers = new SqlCommand("SELECT COUNT(*) FROM Customers", conn);
                    lblTotalCustomers.Text = cmdCustomers.ExecuteScalar()?.ToString() ?? "0";

                    SqlCommand cmdCompleted = new SqlCommand("SELECT COUNT(*) FROM Shifts WHERE Status = 'Completed'", conn);
                    lblCompletedShifts.Text = cmdCompleted.ExecuteScalar()?.ToString() ?? "0";

                    SqlCommand cmdPending = new SqlCommand("SELECT COUNT(*) FROM Shifts WHERE Status = 'Pending'", conn);
                    lblPendingShifts.Text = cmdPending.ExecuteScalar()?.ToString() ?? "0";

                    SqlCommand cmdTransport = new SqlCommand("SELECT COUNT(*) FROM TransportUnits WHERE Status = 'Active'", conn);
                    lblActiveUnits.Text = cmdTransport.ExecuteScalar()?.ToString() ?? "0";

                    SqlCommand cmdStatusCounts = new SqlCommand("SELECT Status, COUNT(*) FROM Shifts GROUP BY Status", conn);
                    using (SqlDataReader reader = cmdStatusCounts.ExecuteReader())
                    {
                        chartStatusDistribution.Series[0].Points.Clear();
                        chartStatusDistribution.Series[0]["PieLabelStyle"] = "Outside";
                        chartStatusDistribution.Series[0].IsValueShownAsLabel = true;
                        chartStatusDistribution.ChartAreas[0].Area3DStyle.Enable3D = false;
                        chartStatusDistribution.ChartAreas[0].InnerPlotPosition = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(10, 10, 80, 80);

                        while (reader.Read())
                        {
                            string status = reader[0].ToString();
                            int count = Convert.ToInt32(reader[1]);
                            chartStatusDistribution.Series[0].Points.AddXY(status, count);
                        }
                    }

                    chartDailyRequests.Series[0].Points.Clear();
                    chartDailyRequests.Series[0].ChartType = SeriesChartType.Column;
                    chartDailyRequests.Series[0].IsValueShownAsLabel = true;
                    chartDailyRequests.Series[0].LabelForeColor = Color.Black;
                    chartDailyRequests.Legends.Clear();

                    chartDailyRequests.ChartAreas[0].AxisX.Title = "Date";
                    chartDailyRequests.ChartAreas[0].AxisX.Interval = 1;
                    chartDailyRequests.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    chartDailyRequests.ChartAreas[0].AxisX.LabelStyle.Angle = -45; 

                    chartDailyRequests.ChartAreas[0].AxisY.Title = "Shift Requests";
                    chartDailyRequests.ChartAreas[0].AxisY.Interval = 1;
                    chartDailyRequests.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                    chartDailyRequests.ChartAreas[0].AxisY.LabelStyle.Format = "0";

                    SqlCommand cmdByDay = new SqlCommand(@"
                        SELECT TOP 7 CONVERT(date, ShiftDate) AS ShiftDay, COUNT(*) AS Total 
                        FROM Shifts 
                        GROUP BY CONVERT(date, ShiftDate) 
                        ORDER BY ShiftDay ASC", conn);

                    using (SqlDataReader dayReader = cmdByDay.ExecuteReader())
                    {
                        while (dayReader.Read())
                        {
                            string date = Convert.ToDateTime(dayReader["ShiftDay"]).ToString("MM/dd");
                            int count = Convert.ToInt32(dayReader["Total"]);
                            chartDailyRequests.Series[0].Points.AddXY(date, count);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data:\n" + ex.Message);
            }
        }

        /////////////////////////////////////////////////////// Home Section End ////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////// Customers Section ///////////////////////////////////////////////////////////////////

        private void LoadCustomerData()
        {
            string query = @"
            SELECT 
                C.CustomerID, 
                C.FirstName + ' ' + C.LastName AS FullName,
                C.Email,
                C.Username,
                C.CreatedAt,
                COUNT(S.ShiftID) AS TotalShifts
            FROM 
                Customers C
            LEFT JOIN 
                Shifts S ON C.CustomerID = S.CustomerID
            GROUP BY 
                C.CustomerID, C.FirstName, C.LastName, C.Email, C.Username, C.CreatedAt
            ORDER BY 
                C.CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvCustomers.DataSource = dt;

                StyleCustomerGrid();

                lblTotalCUstomersCustomers.Text = "Total: " + dt.Rows.Count + " Customers";
            }
        }

        private void StyleCustomerGrid()
        {
            dgvCustomers.BorderStyle = BorderStyle.None;
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToResizeRows = false;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.EnableHeadersVisualStyles = false;
            dgvCustomers.GridColor = Color.Gainsboro;

            dgvCustomers.DefaultCellStyle.BackColor = Color.White;
            dgvCustomers.DefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30);
            dgvCustomers.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvCustomers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255); // light blue
            dgvCustomers.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCustomers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvCustomers.RowTemplate.Height = 40;

            dgvCustomers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 70, 100); // deep steel blue
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvCustomers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvCustomers.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            if (!dgvCustomers.Columns.Contains("Delete"))
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvCustomers.Columns.Add(btnDelete);
            }
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && dgvCustomers.Columns[e.ColumnIndex].Name == "Delete")
            {
                int customerId = Convert.ToInt32(dgvCustomers.Rows[e.RowIndex].Cells["CustomerID"].Value);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string deleteQuery = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                        SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@CustomerID", customerId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    LoadCustomerData();
                }
            }
        }

        private void txtSearchCustomers_TextChanged(object sender, EventArgs e)
        {
            string filter = txtSearchCustomers.Text.Trim().ToLower();

            if (dgvCustomers.DataSource is DataTable dt)
            {
                dt.DefaultView.RowFilter = $@"
            CONVERT(FullName, System.String) LIKE '%{filter}%'
            OR CONVERT(Email, System.String) LIKE '%{filter}%'
            OR CONVERT(Username, System.String) LIKE '%{filter}%'
            OR CONVERT(CustomerID, System.String) LIKE'%{filter}%'";
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchCustomers.Text = "";
        }

        ///////////////////////////////////////////////////////////////// End Of Customer Section //////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////// Update Section //////////////////////////////////////////////////////////
        private void btnSearchShifts_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtSearchShiftID.Text))
            {
                MessageBox.Show("Please enter a Shift ID.");
                return;
            }

            int shiftID;
            if (!int.TryParse(txtSearchShiftID.Text, out shiftID))
            {
                MessageBox.Show("Invalid Shift ID.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT 
            PickupContactName, PickupContactPhone, PackageDescription,
            PickupAddress, DeliveryAddress, ServiceType, Status
            FROM Shifts WHERE ShiftID = @ShiftID", conn);

                cmd.Parameters.AddWithValue("@ShiftID", shiftID);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    panelSearchPrompt.Visible = false; 

                    lblContactName.Text = reader["PickupContactName"].ToString();
                    lblContactPhone.Text = reader["PickupContactPhone"].ToString();
                    lblPackageDesc.Text = reader["PackageDescription"].ToString();
                    lblPickupAddresss.Text = reader["PickupAddress"].ToString();
                    lblDeliveryAddresss.Text = reader["DeliveryAddress"].ToString();
                    lblServiceTypes.Text = reader["ServiceType"].ToString();
                    lblCurrentStatus.Text = reader["Status"].ToString();

                    string status = reader["Status"].ToString();
                    switch (status)
                    {
                        case "Pending":
                            lblCurrentStatus.BackColor = Color.Gold;
                            break;
                        case "Ongoing":
                            lblCurrentStatus.BackColor = Color.LightBlue;
                            break;
                        case "Completed":
                            lblCurrentStatus.BackColor = Color.LightGreen;
                            break;
                        case "Rejected":
                            lblCurrentStatus.BackColor = Color.IndianRed;
                            break;
                        default:
                            lblCurrentStatus.BackColor = Color.Gray;
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("No shift found with this ID.");
                    panelSearchPrompt.Visible = true;
                }

                reader.Close();
            }
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchShiftID.Text) || cmbUpdateStatus.SelectedItem == null)
            {
                MessageBox.Show("Please enter a Shift ID and select a new status.");
                return;
            }

            int shiftID = Convert.ToInt32(txtSearchShiftID.Text);
            string newStatus = cmbUpdateStatus.SelectedItem.ToString();
            string notes = txtNotes.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Shifts SET Status = @Status WHERE ShiftID = @ShiftID", conn);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@ShiftID", shiftID);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Shift status updated.");
            btnSearchShifts.PerformClick(); 
        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            txtSearchShiftID.Clear();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchShiftID.Clear();
            cmbUpdateStatus.SelectedIndex = -1;
            txtNotes.Clear();
            panelSearchPrompt.Visible = true;

            lblContactName.Text = "";
            lblContactPhone.Text = "";
            lblPackageDesc.Text = "";
            lblPickupAddresss.Text = "";
            lblDeliveryAddresss.Text = "";
            lblServiceTypes.Text = "";
            lblCurrentStatus.Text = "";
            lblCurrentStatus.BackColor = Color.Transparent;
        }
        /////////////////////////////////////////////////////// End Of Update Shift Status ///////////////////////////////////////////////////////

        /////////////////////////////////////////////////////// Shift Requests Section ///////////////////////////////////////////////////////////

        private void LoadPendingShiftRequests(string dateFilter = "", string statusFilter = "", string customerSearch = "")
        {

            string selectedStatus = statusFilter;
            string selectedSort = dateFilter;
            string customers = customerSearch;

            string query = @"
                SELECT 
                    S.ShiftID,
                    C.FirstName + ' ' + C.LastName AS Customer,
                    S.ShiftDate,
                    S.ServiceType,
                    S.PickupCity,
                    S.DeliveryCity,
                    S.PackageDescription,
                    S.Status
                FROM Shifts S
                INNER JOIN Customers C ON S.CustomerID = C.CustomerID
                WHERE 1=1";

            if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "All")
            {
                query += " AND S.Status = @Status";
            }

            if (!string.IsNullOrEmpty(customers))
            {
                query += " AND (ISNULL(C.FirstName, '') + ' ' + ISNULL(C.LastName, '') LIKE @CustomerName)";
            }

            if (selectedSort == "Oldest First")
            {
                query += " ORDER BY S.ShiftDate ASC";
            }
            else
            {
                query += " ORDER BY S.ShiftDate DESC";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "All")
                    cmd.Parameters.AddWithValue("@Status", selectedStatus);

                if (!string.IsNullOrEmpty(customers))
                    cmd.Parameters.AddWithValue("@CustomerName", "%" + customers + "%");

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                dgvShiftRequests.DataSource = dt;
                StyleGrid(dgvShiftRequests);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string dateFilter = cmbDateSort.SelectedItem?.ToString() ?? "Latest First";
            string statusFilter = cmbStatusFilter.SelectedItem?.ToString() ?? "All";
            string customers = txtCustomerSearch.Text.Trim();

            LoadPendingShiftRequests(dateFilter, statusFilter, customers);
        }

        private void dgvShiftRequests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvShiftRequests.Rows[e.RowIndex];
                var statusCell = row.Cells["Status"];

                if (statusCell?.Value != null)
                {
                    string status = statusCell.Value.ToString();

                    if (status == "Rejected" || status == "Rejected")
                    {
                        btnreject.Visible = false;
                    }
                    else
                    {
                        btnreject.Visible = true;
                    }
                }

                var value = row.Cells["ShiftID"].Value;
                if (value != null && value != DBNull.Value)
                {
                    int shiftId = Convert.ToInt32(value);
                    ShowShiftDetails(shiftId);

                }
            }
        }

        private void ShowShiftDetails(int shiftId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Shifts WHERE ShiftID = @id", conn);
                cmd.Parameters.AddWithValue("@id", shiftId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblPickupName.Text = reader["PickupContactName"].ToString();
                    lblPickupPhone.Text = reader["PickupContactPhone"].ToString();
                    lblPickupAddress.Text = reader["PickupAddress"].ToString();
                    lblDeliveryAddress.Text = reader["DeliveryAddress"].ToString();
                    lblRecipientName.Text = reader["RecipientName"].ToString();
                    lblRecipientPhone.Text = reader["RecipientPhone"].ToString();

                }

                reader.Close();
            }

            panelShiftRequestDetails.Visible = true;
            lblNoRequestSelected.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdateShiftStatus("Ongoing");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpdateShiftStatus("Rejected");
        }

        private void UpdateShiftStatus(string newStatus)
        {
            int shiftId = GetSelectedShiftID();
            if (shiftId == -1) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Shifts SET Status = @status WHERE ShiftID = @id", conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@id", shiftId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Shift status updated to " + newStatus);
            LoadPendingShiftRequests();
            panelShiftRequestDetails.Visible = false;
            lblNoRequestSelected.Visible = true;
        }

        private int GetSelectedShiftID()
        {
            if (dgvShiftRequests.CurrentRow != null)
            {
                return Convert.ToInt32(dgvShiftRequests.CurrentRow.Cells["ShiftID"].Value);
            }

            return -1;
        }
        private void StyleGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 40;

            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.Black;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            grid.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid.GridColor = Color.Silver;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ClearSelection();

            foreach (DataGridViewRow row in dgvShiftRequests.Rows)
            {
                if (row.Cells["Status"].Value != DBNull.Value && row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();

                    if (status == "Pending")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                    else if (status == "Rejected")
                    {
                        row.DefaultCellStyle.BackColor = Color.IndianRed;
                    }
                    else if (status == "Completed")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                }
            }
        }

        private void cmbFilterDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPendingShiftRequests();
        }

        private void cmbServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPendingShiftRequests();
        }

        private void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            string dateFilter = cmbDateSort.SelectedItem?.ToString() ?? "Latest First";
            string statusFilter = cmbStatusFilter.SelectedItem?.ToString() ?? "All";
            string customers = txtCustomerSearch.Text.Trim();

            LoadPendingShiftRequests(dateFilter, statusFilter, customers);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to reject this shift?", "Confirm Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                UpdateShiftStatus("Rejected");
            }
        }

        /////////////////////////////////////////////////////// End Of Shift Requests Section ////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////// Manage Admin /////////////////////////////////////////////////////////////////

        private void LoadAdmins()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT AdminID, Username, Email, Role FROM Admins";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvAdmins.DataSource = dt;
                StyleAdminGrid(dgvAdmins);
            }
        }

        private void btnAddAdmin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAdminUsername.Text) ||
        string.IsNullOrWhiteSpace(txtAdminEmail.Text) ||
        string.IsNullOrWhiteSpace(txtAdminPassword.Text) ||
        cmbAdminRole.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Admins (Username, Email, Password, Role) VALUES (@username, @email, @password, @role)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", txtAdminUsername.Text);
                cmd.Parameters.AddWithValue("@email", txtAdminEmail.Text);
                cmd.Parameters.AddWithValue("@password", txtAdminPassword.Text);
                cmd.Parameters.AddWithValue("@role", cmbAdminRole.SelectedItem.ToString());
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("✅ Admin added successfully.");
            LoadAdmins();
            ClearAdminFields();
        }

        private void btnUpdateAdmin_Click(object sender, EventArgs e)
        {

            if (dgvAdmins.CurrentRow == null)
            {
                MessageBox.Show("Select an admin to update.");
                return;
            }

            int adminId = Convert.ToInt32(dgvAdmins.CurrentRow.Cells["AdminID"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Admins SET Username=@username, Email=@email, Password=@password, Role=@role WHERE AdminID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", txtAdminUsername.Text);
                cmd.Parameters.AddWithValue("@email", txtAdminEmail.Text);
                cmd.Parameters.AddWithValue("@password", txtAdminPassword.Text);
                cmd.Parameters.AddWithValue("@role", cmbAdminRole.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@id", adminId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("✅ Admin updated successfully.");
            LoadAdmins();
            ClearAdminFields();
        }

        private void ClearAdminFields()
        {
            txtAdminUsername.Text = "";
            txtAdminEmail.Text = "";
            txtAdminPassword.Text = "";
            cmbAdminRole.SelectedIndex = -1;
        }

        private void dgvAdmins_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtAdminUsername.Text = dgvAdmins.Rows[e.RowIndex].Cells["Username"].Value.ToString();
                txtAdminEmail.Text = dgvAdmins.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                cmbAdminRole.SelectedItem = dgvAdmins.Rows[e.RowIndex].Cells["Role"].Value.ToString();
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtChangeadminemail.Text) ||
                string.IsNullOrWhiteSpace(txtCurrentPassword.Text) ||
                string.IsNullOrWhiteSpace(txtNewPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("Please fill in all password fields.");
                return;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("New passwords do not match.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM Admins WHERE Email = @Email AND Password = @OldPass";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", txtChangeadminemail.Text);
                checkCmd.Parameters.AddWithValue("@OldPass", txtCurrentPassword.Text);

                int exists = (int)checkCmd.ExecuteScalar();

                if (exists == 0)
                {
                    MessageBox.Show("Current password is incorrect.");
                    return;
                }

                string updateQuery = "UPDATE Admins SET Password = @NewPass WHERE Email = @Email";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@NewPass", txtNewPassword.Text);
                updateCmd.Parameters.AddWithValue("@Email", txtChangeadminemail.Text);
                updateCmd.ExecuteNonQuery();
            }

            MessageBox.Show("✅ Password updated successfully.");
            ClearPasswordFields();
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearPasswordFields();

        }

        private void ClearPasswordFields()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtChangeadminemail.Text = "";
            txtConfirmPassword.Text = "";
        }
        private void StyleAdminGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.GridColor = Color.LightGray;
            grid.RowTemplate.Height = 30;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.Black;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 201); 
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            grid.ClearSelection();
        }

        /////////////////////////////////////////////////////// End Of Manage Admin //////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////// Transport Units Section ///////////////////////////////////////////////////////////
        
        private void TransportNav_Click(object sender, EventArgs e)
        {
            btnVehicleInfo.BackColor = Color.Navy;
            btnAssignShift.BackColor = Color.Navy;
            btnAddVehicle.BackColor = Color.Navy;
            btnDrivers.BackColor = Color.Navy;
            btnAddDriver.BackColor = Color.Navy;

            panelVehicleInfo.Visible = false;
            panelAssignShift.Visible = false;
            panelAddVehicles.Visible = false;
            panelDrivers.Visible = false;
            panelAddDrivers.Visible = false;

            if (sender == btnVehicleInfo)
            {
                btnVehicleInfo.BackColor = Color.RoyalBlue;
                panelVehicleInfo.Visible = true;
                panelVehicleInfo.BringToFront();
                LoadVehicleInfo();
            }
            else if (sender == btnAssignShift)
            {
                btnAssignShift.BackColor = Color.RoyalBlue;
                panelAssignShift.Visible = true;
                panelAssignShift.BringToFront();
            }
            else if (sender == btnAddVehicle)
            {
                btnAddVehicle.BackColor = Color.RoyalBlue;
                panelAddVehicles.Visible = true;
                panelAddVehicles.BringToFront();
            }
            else if (sender == btnDrivers)
            {
                btnDrivers.BackColor = Color.RoyalBlue;
                panelDrivers.Visible = true;
                panelDrivers.BringToFront();
            }
            else if (sender == btnAddDriver)
            {
                btnAddDriver.BackColor = Color.RoyalBlue;
                panelAddDrivers.Visible = true;
                panelAddDrivers.BringToFront();
            }
        }

        private void SidebarButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Panel hoveredPanel)
            {
                hoveredPanel.BackColor = Color.RoyalBlue;
            }
        }

        private void SidebarButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                if (panel.BackColor != Color.RoyalBlue)
                    panel.BackColor = Color.Navy;
            }
        }

        private void pbClose_MouseClick(object sender, MouseEventArgs e)
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

        private void btnClearSearch_MouseEnter(object sender, EventArgs e)
        {
            btnClearSearch.BackColor = Color.LightGray;

        }

        private void btnClearSearch_MouseLeave(object sender, EventArgs e)
        {
            btnClearSearch.BackColor = Color.Transparent;
        }

        private void pbback_Click(object sender, EventArgs e)
        {
            frmAdminLogin adminLogin = new frmAdminLogin();
            adminLogin.Show();
            this.Hide();
        }

        private void label78_Click(object sender, EventArgs e)
        {
            panelAssignShift.BringToFront();
        }

        private void label76_MouseEnter(object sender, EventArgs e)
        {
            btnResetAdminFields.Font = new Font(btnResetAdminFields.Font.FontFamily, btnResetAdminFields.Font.Size, FontStyle.Underline | FontStyle.Bold);
        }

        private void btnResetAdminFields_Click(object sender, EventArgs e)
        {
            txtAdminUsername.Clear();
            txtAdminEmail.Clear();
            txtAdminPassword.Clear();
            cmbAdminRole.SelectedIndex = -1;
        }

        private void btnResetAdminFields_MouseLeave(object sender, EventArgs e)
        {
            btnResetAdminFields.Font = new Font(btnResetAdminFields.Font.FontFamily, btnResetAdminFields.Font.Size, FontStyle.Bold | FontStyle.Italic);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (dgvDrivers.CurrentRow == null)
            {
                MessageBox.Show("Please select a driver to delete.");
                return;
            }

            int driverId = Convert.ToInt32(dgvDrivers.CurrentRow.Cells["DriverID"].Value);

            DialogResult confirm = MessageBox.Show("Are you sure to delete this driver?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Drivers WHERE DriverID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", driverId);
                    cmd.ExecuteNonQuery();
                }

                LoadDrivers();
            }
        }
        private void ResetDriverForm()
        {
            txtDriverName.Clear();
            txtDriverPhone.Clear();
            txtdriverid.Clear();
        }
       
        private void LoadVehicleInfo()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
            SELECT 
                T.VehicleID,
                T.UnitName,
                T.VehicleNumber,
                T.VehicleType,
                T.Capacity,
                T.Status,
                ISNULL(D.FirstName + ' ' + D.LastName, 'Not Assigned') AS AssignedDriver
            FROM TransportUnits T
            LEFT JOIN VehicleAssignments VA ON T.VehicleID = VA.VehicleID
            LEFT JOIN Drivers D ON VA.DriverID = D.DriverID
            WHERE T.Status = 'Active';";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvVehicles.DataSource = dt;

                dgvVehicles.Columns["UnitName"].HeaderText = "Vehicle Name";
                dgvVehicles.Columns["VehicleNumber"].HeaderText = "Number";
                dgvVehicles.Columns["VehicleType"].HeaderText = "Type";
                dgvVehicles.Columns["Capacity"].HeaderText = "Capacity (kg)";
                dgvVehicles.Columns["Status"].HeaderText = "Status";
                dgvVehicles.Columns["AssignedDriver"].HeaderText = "Assigned Driver";

                dgvVehicles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvVehicles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvVehicles.ReadOnly = true;
            
              StyleVehicleGrid(); 
            }
        }
        private void StyleVehicleGrid()
        {
            dgvVehicles.BorderStyle = BorderStyle.None;
            dgvVehicles.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVehicles.RowHeadersVisible = false;
            dgvVehicles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehicles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvVehicles.DefaultCellStyle.BackColor = Color.White;
            dgvVehicles.DefaultCellStyle.ForeColor = Color.Black;
            dgvVehicles.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvVehicles.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvVehicles.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvVehicles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVehicles.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }


        private void ClearVehicleFields()
        {
            txtUnitName.Text = "";
            txtVehicleNumber.Text = "";
            txtCapacity.Text = "";
            cmbVehicleType.SelectedIndex = 0;
        }

        private void btnCancelAddVehicle_Click(object sender, EventArgs e)
        {
            ClearVehicleFields();
        }

        private void btnRefreshVehicles_Click(object sender, EventArgs e)
        {
            LoadVehicleInfo();
        }

        private void panelNavTransportUnits_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSearchVehicles_TextChanged(object sender, EventArgs e)
        {

            string filter = txtSearchVehicles.Text.Trim().ToLower();

            if (dgvVehicles.DataSource is DataTable dt)
            {
                dt.DefaultView.RowFilter = $@"
            CONVERT(UnitName, System.String) LIKE '%{filter}%'
            OR CONVERT(VehicleNumber, System.String) LIKE '%{filter}%'
            OR CONVERT(VehicleType, System.String) LIKE '%{filter}%'
            OR CONVERT(Status, System.String) LIKE '%{filter}%'";
            }
        }

        private void btnAddVehicles_Click(object sender, EventArgs e)
        {
            string unitName = txtUnitName.Text.Trim();
            string vehicleNumber = txtVehicleNumber.Text.Trim();
            string vehicleType = cmbVehicleType.SelectedItem?.ToString();
            string capacity = txtCapacity.Text.Trim();

            if (string.IsNullOrEmpty(unitName) || string.IsNullOrEmpty(vehicleNumber) ||
                string.IsNullOrEmpty(vehicleType) || string.IsNullOrEmpty(capacity))
            {
                MessageBox.Show("❌ Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO TransportUnits (UnitName, VehicleNumber, VehicleType, Capacity, Status) " +
                               "VALUES (@name, @number, @type, @capacity, 'Active')";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", unitName);
                cmd.Parameters.AddWithValue("@number", vehicleNumber);
                cmd.Parameters.AddWithValue("@type", vehicleType);
                cmd.Parameters.AddWithValue("@capacity", capacity);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            LoadVehicleInfo();  

           
            ClearVehicleFields();
            MessageBox.Show("✅ Vehicle added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvVehicles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvVehicles_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvVehicles.Rows[e.RowIndex];
                string vehicleId = row.Cells["VehicleID"].Value?.ToString();

                lblV_UnitName.Text = row.Cells["UnitName"].Value?.ToString();
                lblV_Number.Text = row.Cells["VehicleNumber"].Value?.ToString();
                lblV_Type.Text = row.Cells["VehicleType"].Value?.ToString();
                lblV_Capacity.Text = row.Cells["Capacity"].Value?.ToString();
                lblV_Status.Text = row.Cells["Status"].Value?.ToString();

                HighlightVehicleStatus(lblV_Status.Text);

                LoadVehicleShiftAndDriverDetails(vehicleId);
            }
        }
        private void HighlightVehicleStatus(string status)
        {
            status = status?.Trim().ToLower();

            switch (status)
            {
                case "active":
                    lblV_Status.BackColor = Color.LightGreen;
                    lblV_Status.ForeColor = Color.Black;
                    break;

                case "inactive":
                    lblV_Status.BackColor = Color.IndianRed;
                    lblV_Status.ForeColor = Color.White;
                    break;

                case "maintenance":
                    lblV_Status.BackColor = Color.Gold;
                    lblV_Status.ForeColor = Color.Black;
                    break;

                default:
                    lblV_Status.BackColor = Color.Gray;
                    lblV_Status.ForeColor = Color.White;
                    break;
            }
        }

        private void LoadVehicleShiftAndDriverDetails(string vehicleId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT TOP 1 
                ShiftID, 
                PickupCity AS Location
            FROM Shifts 
            WHERE AssignedVehicleID = @vehicleId 
            ORDER BY ShiftDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@vehicleId", vehicleId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblV_ShiftID.Text = reader["ShiftID"].ToString();
                        lblV_Location.Text = reader["Location"].ToString();
                    }
                    else
                    {
                        lblV_ShiftID.Text = "None";
                        lblV_Location.Text = "Unknown";
                    }
                }

                conn.Close();
            }

            lblV_AssignedDriver.Text = GetAssignedDriver(vehicleId);
        }

        private string GetAssignedDriver(string vehicleId)
        {
            return "Not Available"; 
        }

        private void cmbSelectDriver_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private int selectedShiftId = -1;

        private void LoadPendingShiftsForAssignment()
        {
            string query = @"
        SELECT 
            S.ShiftID,
            C.FirstName + ' ' + C.LastName AS Customer,
            S.ShiftDate,
            S.PickupCity,
            S.DeliveryCity,
            S.Status
        FROM Shifts S
        INNER JOIN Customers C ON S.CustomerID = C.CustomerID
        WHERE S.Status = 'Pending'
        ORDER BY S.ShiftDate ASC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                dgvAssignShiftPending.DataSource = dt;
                StyleGrid(dgvAssignShiftPending); 
            }
        }

        private void LoadAssignShiftResources()
        {
            cmbSelectVehicle.Items.Clear();
            cmbSelectDriver.Items.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmdVehicles = new SqlCommand(
                    "SELECT VehicleID, UnitName FROM TransportUnits WHERE Status = 'Active'", conn);
                SqlDataReader rdrVehicles = cmdVehicles.ExecuteReader();
                while (rdrVehicles.Read())
                {
                    cmbSelectVehicle.Items.Add(new ComboBoxItem
                    {
                        Text = rdrVehicles["UnitName"].ToString(),
                        Value = rdrVehicles["VehicleID"]
                    });
                }
                rdrVehicles.Close();

                SqlCommand cmdDrivers = new SqlCommand(
                    "SELECT DriverID, FirstName + ' ' + LastName AS FullName FROM Drivers WHERE Status = 'Active'", conn);
                SqlDataReader rdrDrivers = cmdDrivers.ExecuteReader();
                while (rdrDrivers.Read())
                {
                    cmbSelectDriver.Items.Add(new ComboBoxItem
                    {
                        Text = rdrDrivers["FullName"].ToString(),
                        Value = rdrDrivers["DriverID"]
                    });
                }
                rdrDrivers.Close();

                SqlDataAdapter adapter = new SqlDataAdapter(@"
            SELECT ShiftID, PickupCity, DeliveryCity, ServiceType, Status
            FROM Shifts
            WHERE Status = 'Pending'
            ORDER BY ShiftID DESC", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvAssignShiftPending.DataSource = dt;
            }

            if (cmbSelectVehicle.Items.Count > 0) cmbSelectVehicle.SelectedIndex = 0;
            if (cmbSelectDriver.Items.Count > 0) cmbSelectDriver.SelectedIndex = 0;
        }
        private void btnAssignShifts_Click(object sender, EventArgs e)
        {

            if (cmbSelectVehicle.SelectedItem is ComboBoxItem selectedVehicle &&
                cmbSelectDriver.SelectedItem is ComboBoxItem selectedDriver &&
                dgvAssignShiftPending.CurrentRow != null)
            {
                int shiftId = Convert.ToInt32(dgvAssignShiftPending.CurrentRow.Cells["ShiftID"].Value);
                int vehicleId = Convert.ToInt32(selectedVehicle.Value);
                int driverId = Convert.ToInt32(selectedDriver.Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"
                UPDATE Shifts
                SET AssignedVehicleID = @VehicleID,
                    AssignedDriverID = @DriverID,
                    Status = 'Ongoing'
                WHERE ShiftID = @ShiftID", conn);
                    cmd.Parameters.AddWithValue("@VehicleID", vehicleId);
                    cmd.Parameters.AddWithValue("@DriverID", driverId);
                    cmd.Parameters.AddWithValue("@ShiftID", shiftId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Shift successfully assigned to the selected vehicle and driver!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadAssignShiftResources(); 
            }
            else
            {
                MessageBox.Show("Please select both a vehicle, a driver, and a shift to assign.",
                    "Incomplete Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvAssignShiftPending_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedShiftId = Convert.ToInt32(dgvAssignShiftPending.Rows[e.RowIndex].Cells["ShiftID"].Value);
                lblSelectedShift.Text = "Selected Shift ID: " + selectedShiftId;
            }
        }
        private void btnAssignCancel_Click(object sender, EventArgs e)
        {
            ResetAssignShiftFields();
        }

        private void ResetAssignShiftFields()
        {
            cmbSelectVehicle.SelectedIndex = -1;
            cmbSelectDriver.SelectedIndex = -1;
            txtAssignNotes.Clear();
            lblSelectedShift.Text = "No shift selected";
            selectedShiftId = -1;
        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString() => Text;
        }


        private void LoadDrivers()
        {
            string query = @"SELECT DriverID, FirstName, ContactNo, Email, Status FROM Drivers ORDER BY FirstName ASC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                dgvDrivers.DataSource = dt;
                StyleDriversGrid(dgvDrivers);
            }
        }

        private void txtSearchDriver_TextChanged(object sender, EventArgs e)
        {

            string search = txtSearchDriver.Text.Trim();

            if (string.IsNullOrEmpty(search))
            {
                LoadDrivers();
                return;
            }

            string query = @"
        SELECT DriverID, FirstName, ContactNo, Email, Status
        FROM Drivers
        WHERE FirstName LIKE @search OR ContactNo LIKE @search
        ORDER BY FirstName ASC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                dgvDrivers.DataSource = dt;
                StyleDriversGrid(dgvDrivers);
            }
        }

        private void dgvDrivers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDrivers.Rows[e.RowIndex];
                txtDriverName.Text = row.Cells["FirstName"].Value.ToString();
                txtDriverPhone.Text = row.Cells["ContactNo"].Value.ToString();
                txtdriverid.Text = row.Cells["DriverID"].Value.ToString();
            }
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            if (dgvDrivers.CurrentRow == null)
            {
                MessageBox.Show("Please select a driver to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int driverId = Convert.ToInt32(dgvDrivers.CurrentRow.Cells["DriverID"].Value);
            string name = txtDriverName.Text.Trim();
            string phone = txtDriverPhone.Text.Trim();
            string email = txtDriverEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please fill all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            UPDATE Drivers 
            SET FirstName = @name, ContactNo = @phone, Email = @email
            WHERE DriverID = @id", conn);
                cmd.Parameters.AddWithValue("@id", driverId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("✅ Driver details updated.");
            ResetDriverForm();
            LoadDrivers();
        }
        private void StyleDriversGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Color.White;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToResizeRows = false;
            grid.ReadOnly = true;

            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            grid.DefaultCellStyle.ForeColor = Color.Black;
            grid.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Padding = new Padding(5);

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGreen;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid.ColumnHeadersHeight = 45;
            grid.RowTemplate.Height = 40;
            grid.GridColor = Color.LightGray;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ClearSelection();
        }

        private void btnResetDriver_Click(object sender, EventArgs e)
        {
            txtSearchDriver.Text = string.Empty;
            LoadDrivers();
            txtSearchDriver.Focus();
        }

        private void LoadReportTypes()
        {
            cmbReportType.Items.AddRange(new string[]
            {
                "All Customers",
                "Driver Status - Active",
                "Driver Status - Inactive",
                "All Shifts",
                "Pending Shifts",
                "Completed Shifts",
                "Transport Units Summary",
                "Assigned Vehicles",
                "Shift Overview"
            });
            cmbReportType.SelectedIndex = 0;
        }


        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string selectedReport = cmbReportType.SelectedItem.ToString();
            string query = "";

            switch (selectedReport)
            {
                case "All Customers":
                    query = "SELECT CustomerID, FirstName, LastName, Email, Username, CreatedAt FROM Customers";
                    break;
                case "Driver Status - Active":
                    query = "SELECT * FROM Drivers WHERE Status = 'Active'";
                    break;
                case "Driver Status - Inactive":
                    query = "SELECT * FROM Drivers WHERE Status = 'Inactive'";
                    break;
                case "All Shifts":
                    query = "SELECT * FROM Shifts";
                    break;
                case "Pending Shifts":
                    query = "SELECT * FROM Shifts WHERE Status = 'Pending'";
                    break;
                case "Completed Shifts":
                    query = "SELECT * FROM Shifts WHERE Status = 'Completed'";
                    break;
                case "Transport Units Summary":
                    query = "SELECT VehicleID, UnitName, VehicleNumber, VehicleType, Capacity, Status FROM TransportUnits";
                    break;
                case "Assigned Vehicles":
                    query = @"SELECT va.AssignmentID, d.FirstName + ' ' + d.LastName AS DriverName, 
                                tu.UnitName, tu.VehicleNumber, va.AssignedDate 
                                FROM VehicleAssignments va 
                                JOIN Drivers d ON va.DriverID = d.DriverID 
                                JOIN TransportUnits tu ON va.VehicleID = tu.VehicleID";
                    break;
                case "Shift Overview":
                    query = @"SELECT s.ShiftID, c.FirstName + ' ' + c.LastName AS CustomerName, 
                                s.ShiftDate, s.ServiceType, s.PickupCity, s.DeliveryCity, s.Status 
                                FROM Shifts s 
                                JOIN Customers c ON s.CustomerID = c.CustomerID";
                    break;
                default:
                    MessageBox.Show("Select a valid report type.");
                    return;
            }

            LoadReportData(query);
        }

        private void LoadReportData(string query)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvReport.DataSource = dt;
                    lblSummary.Text = "Total Records: " + dt.Rows.Count;
                    StyleDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            if (dgvReport == null || dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"EShift_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                string fullPath = Path.Combine(desktopPath, fileName);

                using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8))
                {
                    var headers = dgvReport.Columns.Cast<DataGridViewColumn>()
                                    .Select(col => col.HeaderText);
                    sw.WriteLine(string.Join(",", headers));

                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            var cells = row.Cells.Cast<DataGridViewCell>()
                                          .Select(cell => cell.Value?.ToString() ?? "");
                            sw.WriteLine(string.Join(",", cells));
                        }
                    }
                }

                DialogResult result = MessageBox.Show(
                    $"✅ Report exported successfully to your Desktop!\n\nFile: {fileName}\n\nWould you like to open the file location?",
                    "Export Complete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{fullPath}\"");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAddDrivers_Click(object sender, EventArgs e)
        {

            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string contactNo = txtContactNo.Text.Trim();
            string nic = txtNIC.Text.Trim();
            string email = txtEmail.Text.Trim();
            DateTime dob = dtpDOB.Value;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(contactNo) || string.IsNullOrWhiteSpace(nic) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Drivers 
            (FirstName, LastName, ContactNo, NIC, Email, DOB, Status)
            VALUES 
            (@FirstName, @LastName, @ContactNo, @NIC, @Email, @DOB, 'Active')", conn);

                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@ContactNo", contactNo);
                cmd.Parameters.AddWithValue("@NIC", nic);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@DOB", dob);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("✅ Driver added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetDriverForms();
            LoadDrivers();
        }

        private void btnCancelDriver_Click(object sender, EventArgs e)
        {
            ResetDriverForms();
        }

        private void ResetDriverForms()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtContactNo.Clear();
            txtNIC.Clear();
            txtEmail.Clear();
            dtpDOB.Value = DateTime.Today;
        }
        private void StyleDataGridView()
        {
            dgvReport.EnableHeadersVisualStyles = false;

            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersHeight = 40;

            dgvReport.DefaultCellStyle.BackColor = Color.White;
            dgvReport.DefaultCellStyle.ForeColor = Color.Black;
            dgvReport.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvReport.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvReport.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvReport.RowTemplate.Height = 30;

            dgvReport.GridColor = Color.LightGray;
            dgvReport.BorderStyle = BorderStyle.Fixed3D;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.AutoGenerateColumns = true;

            dgvReport.ReadOnly = true;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                frmAdminLogin Form = new frmAdminLogin();
                Form.Show();

                this.Hide();
            }
        }

        private void panelAddDrivers_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (dgvVehicles.CurrentRow == null)
            {
                MessageBox.Show("Please select a vehicle to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int vehicleId = Convert.ToInt32(dgvVehicles.CurrentRow.Cells["VehicleID"].Value);
            string unitName = dgvVehicles.CurrentRow.Cells["UnitName"].Value.ToString();

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to deactivate the vehicle '{unitName}'?",
                "Confirm Action",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE TransportUnits SET Status = 'Inactive' WHERE VehicleID = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", vehicleId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Vehicle deactivated successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadVehicleInfo();
            }
        }

        private void btnClearSearchVehicle_Click(object sender, EventArgs e)
        {
            txtSearchVehicles.Text = "";   
            LoadVehicleInfo();            
            txtSearchVehicles.Focus();
        }
    }
}
