using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace eShiftManagementSystem
{
    public partial class frmCustomerDashboard : Form
    {
        string connectionString = @"Server=DESKTOP-L03EVJF\SQLEXPRESS;Database=E-ShiftDB;Trusted_Connection=True;";

        public frmCustomerDashboard()
        {
            InitializeComponent();
            LoadProvinces(); // ✅ Load provinces here
            toolTip1.SetToolTip(pbClose, "Back");
            HighlightTab(PanelRequestShift); // Default selection
            ShowContainer(panelrequestnewshiftcontainer); // Show initial content panel

        }

        public string LoggedInCustomerName { get; set; }

        private void frmCustomerDashboard_Load(object sender, EventArgs e)
        {
            ///////////////////////////REQUEST SHIFT//////////////////////
            LoadCustomerName();

            // Attach click events
            PanelRequestShift.Click += Panel_Click;
            panelOngoingJobsTab.Click += Panel_Click;
            panelJobHistoryTab.Click += Panel_Click;

            lblrequestnewshift.Click += Panel_Click;
            lblmyongoingjobs.Click += Panel_Click;
            lbljobhistory.Click += Panel_Click;

            // Set welcome text if name is passed
            if (!string.IsNullOrEmpty(LoggedInCustomerName))
            {
                lblwelcome.Text = $"Welcome, {LoggedInCustomerName}";
            }

            // Set default selected tab to Ongoing Jobs
            HighlightTab(PanelRequestShift);
            ShowContainer(panelrequestnewshiftcontainer);

            //event for combo boxes
            AttachComboBoxEvents();


            // Enable auto-suggest (user typing triggers match suggestions)
            cmbPickupCity.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbPickupCity.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbDeliveryCity.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbDeliveryCity.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbPickupProvince.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbPickupProvince.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbDeliveryProvince.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbDeliveryProvince.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbPickupDistrict.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbPickupDistrict.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbDeliveryDistrict.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbDeliveryDistrict.AutoCompleteSource = AutoCompleteSource.ListItems;

            //Make the pphone number formatting by adding the countrycode first
            txtPickupPhone.Text = "+94 ";
            txtRecipientPhone.Text = "+94 ";

            txtPickupPhone.TextChanged += (s, args) => FormatPhoneNumber(txtPickupPhone);
            txtRecipientPhone.TextChanged += (s, args) => FormatPhoneNumber(txtRecipientPhone);

            txtPickupPhone.KeyDown += txtPhone_KeyDown;
            txtRecipientPhone.KeyDown += txtPhone_KeyDown;


            lblwelcome.Text = "WELCOME, " + LoggedInCustomerName;
            // LoggedInCustomerID will be used for submitting shifts etc.

            if (!string.IsNullOrEmpty(LoggedInCustomerEmail))
            {
                lblCustomerEmail.Text = MaskEmail(LoggedInCustomerEmail);
            }

            LoadCustomerDetails();
            //////////////////////////////ONGOING JOBS////////////////////////

        }


        ////////////////////////////////////////////////// REQUEST SHIFT ///////////////////////////////////////////////////////

        //Loading the customer name beside the welcome logo
        private void LoadCustomerName()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT FirstName FROM Customers WHERE CustomerID = @CustomerID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", LoggedInCustomerID);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            lblwelcome.Text = "Welcome, " + result.ToString(); // Label to show name
                        }
                        else
                        {
                            lblwelcome.Text = "Welcome!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading name: " + ex.Message);
                }
            }
        }

        private void AttachComboBoxEvents()
        {
            cmbPickupProvince.SelectedIndexChanged += cmbPickupProvince_SelectedIndexChanged;
            cmbPickupDistrict.SelectedIndexChanged += cmbPickupDistrict_SelectedIndexChanged;
            cmbDeliveryProvince.SelectedIndexChanged += cmbDeliveryProvince_SelectedIndexChanged;
            cmbDeliveryDistrict.SelectedIndexChanged += cmbDeliveryDistrict_SelectedIndexChanged;
        }


        // Handles all panel clicks
        private void Panel_Click(object sender, EventArgs e)
        {
            if (sender == panelOngoingJobsTab || sender == lblmyongoingjobs)
            {
                HighlightTab(panelOngoingJobsTab);
                ShowContainer(PanleOngoingJobsContainer);
                LoadOngoingJobsToGrid(); // ← loads fresh data
            }

            else if (sender == PanelRequestShift || sender == lblrequestnewshift)
            {
                HighlightTab(PanelRequestShift);
                ShowContainer(panelrequestnewshiftcontainer);
            }

            else if (sender == panelJobHistoryTab || sender == lbljobhistory)
            {
                HighlightTab(panelJobHistoryTab);
                ShowContainer(paneljobhistorycontainer);
                LoadJobHistoryToGrid();
            }
        }

        // Highlights the selected panel and resets others
        private void HighlightTab(Panel selectedPanel)
        {
            PanelRequestShift.BackColor = Color.FromArgb(150, 182, 242);
            panelOngoingJobsTab.BackColor = Color.FromArgb(74, 232, 128);
            panelJobHistoryTab.BackColor = Color.FromArgb(241, 157, 151);

            Panelcontainerborder.BackColor = Color.Transparent;

            lblrequestnewshift.ForeColor = Color.White;
            lblmyongoingjobs.ForeColor = Color.White;
            lbljobhistory.ForeColor = Color.White;


            if (selectedPanel == panelOngoingJobsTab)
            {
                panelOngoingJobsTab.BackColor = Color.FromArgb(52, 179, 83);
                Panelcontainerborder.BackColor = Color.FromArgb(52, 179, 83);
                lblmyongoingjobs.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                lblrequestnewshift.Font = new Font("Segoe UI", 15.2F, FontStyle.Bold);
                lbljobhistory.Font = new Font("Segoe UI", 15.2F, FontStyle.Bold);
            }
            else if (selectedPanel == PanelRequestShift)
            {
                PanelRequestShift.BackColor = Color.FromArgb(51, 153, 255);
                Panelcontainerborder.BackColor = Color.FromArgb(51, 153, 255);
                lblrequestnewshift.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                lblmyongoingjobs.Font = new Font("Segoe UI", 15.2F, FontStyle.Bold);
                lbljobhistory.Font = new Font("Segoe UI", 15.2F, FontStyle.Bold);

            }
            else if (selectedPanel == panelJobHistoryTab)
            {
                panelJobHistoryTab.BackColor = Color.FromArgb(235, 70, 70);
                Panelcontainerborder.BackColor = Color.FromArgb(235, 70, 70);
                lbljobhistory.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                lblmyongoingjobs.Font = new Font("Segoe UI", 15.2F, FontStyle.Bold);
                lblrequestnewshift.Font = new Font("Segoe UI", 15.2F, FontStyle.Bold);
            }
        }

        // Show selected container and hide others
        private void ShowContainer(Panel visibleContainer)
        {

            PanleOngoingJobsContainer.Visible = false;
            panelrequestnewshiftcontainer.Visible = false;
            paneljobhistorycontainer.Visible = false;

            visibleContainer.Visible = true;
            visibleContainer.BringToFront();
        }

        //close button
        private void pbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //close button hover effect
        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;
        }

        // Dictionary to map province to list of districts
        private Dictionary<string, List<string>> provinceDistrictMap = new Dictionary<string, List<string>>()
{
            { "Central", new List<string> { "Kandy", "Matale", "Nuwara Eliya" } },
            { "Eastern", new List<string> { "Ampara", "Batticaloa", "Trincomalee" } },
            { "Northern", new List<string> { "Jaffna", "Kilinochchi", "Mullaitivu", "Mannar", "Vavuniya" } },
            { "North Central", new List<string> { "Anuradhapura", "Polonnaruwa" } },
            { "North Western", new List<string> { "Kurunegala", "Puttalam" } },
            { "Sabaragamuwa", new List<string> { "Kegalle", "Ratnapura" } },
            { "Southern", new List<string> { "Galle", "Matara", "Hambantota" } },
            { "Uva", new List<string> { "Badulla", "Monaragala" } },
            { "Western", new List<string> { "Colombo", "Gampaha", "Kalutara" } }
        };

        //Dictionary to map districts to list of cities+
        Dictionary<string, List<string>> districtCityMap = new Dictionary<string, List<string>>()
        {
            { "Colombo", new List<string>{ "Biyagama", "Colombo", "Dehiwala", "Homagama", "Katunayake", "Kelaniya", "Kottawa", "Maharagama", "Padukka", "Pettah", "Piliyandala", "Seeduwa", "Wattala" } },
            { "Gampaha", new List<string>{ "Gampaha", "Kadawatha", "Kandana", "Kelaniya", "Mirigama", "Negombo", "Seeduwa", "Weliweriya" } },
            { "Kalutara", new List<string>{ "Agalawatta", "Beruwala", "Bulathsinhala", "Kalutara", "Panadura", "Wadduwa" } },
            { "Kandy", new List<string>{ "Gampola", "Kandy", "Wattegama" } },
            { "Matale", new List<string>{ "Dambulla", "Elahera", "Matale", "Nalanda" } },
            { "Nuwara Eliya", new List<string>{ "Hatton", "Maskeliya", "Nawalapitiya", "Nuwara Eliya", "Talawakelle", "Welimada" } },
            { "Kurunegala", new List<string>{ "Alawwa", "Dambadeniya", "Kuliyapitiya", "Kurunegala", "Melsiripura", "Pannala", "Polgahawela" } },
            { "Puttalam", new List<string>{ "Anamaduwa", "Chilaw", "Kalpitiya", "Nawagaththegama", "Puttalam" } },
            { "Galle", new List<string>{ "Ahungalla", "Ambalangoda", "Elpitiya", "Galle", "Hikkaduwa" } },
            { "Matara", new List<string>{ "Deniyaya", "Dikwella", "Matara", "Mirissa", "Pitigala", "Weligama" } },
            { "Hambantota", new List<string>{ "Hambantota", "Kataragama", "Lunugamwehera", "Tangalle", "Tissamaharama" } },
            { "Badulla", new List<string>{ "Badulla", "Bandarawela", "Ella", "Haputale", "Madulsima" } },
            { "Monaragala", new List<string>{ "Monaragala" } },
            { "Ratnapura", new List<string>{ "Balangoda", "Embilipitiya", "Kuruwita", "Panamure", "Ratnapura" } },
            { "Kegalle", new List<string>{ "Aranayaka", "Kegalle", "Mawanella", "Yatiyanthota" } },
            { "Anuradhapura", new List<string>{ "Anuradhapura", "Kekirawa", "Padaviya", "Thalawa", "Thambuttegama" } },
            { "Polonnaruwa", new List<string>{ "Elahera", "Polonnaruwa" } },
            { "Trincomalee", new List<string>{ "Muthurajawela", "Trincomalee" } },
            { "Batticaloa", new List<string>{ "Batticaloa" } },
            { "Ampara", new List<string>{ "Ampara", "Kalmunai" } },
            { "Jaffna", new List<string>{ "Jaffna", "Point Pedro", "Valvettithurai" } },
            { "Vavuniya", new List<string>{ "Vavuniya" } },
            { "Mannar", new List<string>{ "Mannar" } },
            { "Kilinochchi", new List<string>{ "Kilinochchi" } },
            { "Mullaitivu", new List<string>{ "Mullaitivu" } }
        };

        public int LoggedInCustomerID { get; set; }

        public string LoggedInCustomerEmail { get; set; }

        //Make the email hide part of it for secure
        private string MaskEmail(string email)
        {
            // johndoe@gmail.com → joh***@gmail.com
            int atIndex = email.IndexOf('@');
            if (atIndex <= 1)
                return email;

            string namePart = email.Substring(0, atIndex);
            string domainPart = email.Substring(atIndex);

            int visibleChars = Math.Min(3, namePart.Length - 1);
            string maskedName = namePart.Substring(0, visibleChars) + new string('*', namePart.Length - visibleChars);

            return maskedName + domainPart;
        }

        //Submit button
        private void btnregister_Click(object sender, EventArgs e)
        {

            // Validation
            if (string.IsNullOrWhiteSpace(txtPickupName.Text))
            {
                MessageBox.Show("Pickup Name is required."); return;
            }
            if (string.IsNullOrWhiteSpace(txtPickupPhone.Text))
            {
                MessageBox.Show("Pickup Phone is required."); return;
            }
            if (string.IsNullOrWhiteSpace(txtRecipientName.Text))
            {
                MessageBox.Show("Recipient Name is required."); return;
            }
            if (string.IsNullOrWhiteSpace(txtRecipientPhone.Text))
            {
                MessageBox.Show("Recipient Phone is required."); return;
            }
            if (string.IsNullOrWhiteSpace(txtPackageDesc.Text))
            {
                MessageBox.Show("Package Description is required."); return;
            }
            if (string.IsNullOrWhiteSpace(txtPackageWeight.Text))
            {
                MessageBox.Show("Package Weight is required."); return;
            }
            if (string.IsNullOrWhiteSpace(txtPickupAddress.Text))
            {
                MessageBox.Show("Pickup Address is required."); return;
            }
            if (string.IsNullOrWhiteSpace(txtDeliveryAddress.Text))
            {
                MessageBox.Show("Delivery Address is required."); return;
            }
            if (cmbServiceType.SelectedIndex == -1)
            {
                MessageBox.Show("Service Type must be selected."); return;
            }
            if (cmbPickupProvince.SelectedIndex == -1)
            {
                MessageBox.Show("Pickup Province must be selected."); return;
            }
            if (cmbPickupDistrict.SelectedIndex == -1)
            {
                MessageBox.Show("Pickup District must be selected."); return;
            }
            if (cmbPickupCity.SelectedIndex == -1)
            {
                MessageBox.Show("Pickup City must be selected."); return;
            }
            if (cmbDeliveryProvince.SelectedIndex == -1)
            {
                MessageBox.Show("Delivery Province must be selected."); return;
            }
            if (cmbDeliveryDistrict.SelectedIndex == -1)
            {
                MessageBox.Show("Delivery District must be selected."); return;
            }


            // Build pickup and delivery addresses
            string pickupLocation = $"{txtPickupAddress.Text}, {cmbPickupCity.Text}, {cmbPickupDistrict.Text}, {cmbPickupProvince.Text}";
            string deliveryLocation = $"{txtDeliveryAddress.Text}, {cmbDeliveryCity.Text}, {cmbDeliveryDistrict.Text}, {cmbDeliveryProvince.Text}";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                INSERT INTO Shifts 
                (CustomerID, ShiftDate, ShiftStart, ShiftEnd, Status, ServiceType, PickupContactName, PickupContactPhone, PackageDescription, ApproxWeightKg, PickupProvince, PickupDistrict, PickupCity, PickupAddress, RecipientName, RecipientPhone, DeliveryProvince, DeliveryDistrict, DeliveryCity, DeliveryAddress)
                VALUES 
                (@CustomerID, @ShiftDate, NULL, NULL, @Status, @ServiceType,
                 @PickupContactName, @PickupContactPhone, @PackageDescription, @ApproxWeightKg,
                 @PickupProvince, @PickupDistrict, @PickupCity, @PickupAddress,
                 @RecipientName, @RecipientPhone,
                 @DeliveryProvince, @DeliveryDistrict, @DeliveryCity, @DeliveryAddress)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@CustomerID", LoggedInCustomerID);
                        cmd.Parameters.AddWithValue("@ShiftDate", dtpPickupDate.Value.Date);
                        cmd.Parameters.AddWithValue("@Status", "Pending");
                        cmd.Parameters.AddWithValue("@ServiceType", cmbServiceType.Text);

                        cmd.Parameters.AddWithValue("@PickupContactName", txtPickupName.Text);
                        cmd.Parameters.AddWithValue("@PickupContactPhone", txtPickupPhone.Text);
                        cmd.Parameters.AddWithValue("@PackageDescription", txtPackageDesc.Text);
                        cmd.Parameters.AddWithValue("@ApproxWeightKg", txtPackageWeight.Text);

                        cmd.Parameters.AddWithValue("@PickupProvince", cmbPickupProvince.Text);
                        cmd.Parameters.AddWithValue("@PickupDistrict", cmbPickupDistrict.Text);
                        cmd.Parameters.AddWithValue("@PickupCity", cmbPickupCity.Text);
                        cmd.Parameters.AddWithValue("@PickupAddress", txtPickupAddress.Text);

                        cmd.Parameters.AddWithValue("@RecipientName", txtRecipientName.Text);
                        cmd.Parameters.AddWithValue("@RecipientPhone", txtRecipientPhone.Text);

                        cmd.Parameters.AddWithValue("@DeliveryProvince", cmbDeliveryProvince.Text);
                        cmd.Parameters.AddWithValue("@DeliveryDistrict", cmbDeliveryDistrict.Text);
                        cmd.Parameters.AddWithValue("@DeliveryCity", cmbDeliveryCity.Text);
                        cmd.Parameters.AddWithValue("@DeliveryAddress", txtDeliveryAddress.Text);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Shift request submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearRequestForm();
                        }
                        else
                        {
                            MessageBox.Show("Failed to submit shift request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // clear fields
        private void ClearRequestForm()
        {
            cmbServiceType.SelectedIndex = -1;
            cmbServiceType.Text = "";

            dtpPickupDate.Value = DateTime.Now;

            txtPickupName.Clear();
            txtPickupPhone.Clear();
            txtRecipientName.Clear();
            txtRecipientPhone.Clear();
            txtPackageDesc.Clear();
            txtPackageWeight.Clear();

            cmbPickupProvince.SelectedIndex = -1;
            cmbPickupProvince.Text = "";
            cmbPickupDistrict.Items.Clear();
            cmbPickupDistrict.Text = "";
            cmbPickupCity.Items.Clear();
            cmbPickupCity.Text = "";
            txtPickupAddress.Clear();

            cmbDeliveryProvince.SelectedIndex = -1;
            cmbDeliveryProvince.Text = "";
            cmbDeliveryDistrict.Items.Clear();
            cmbDeliveryDistrict.Text = "";
            cmbDeliveryCity.Items.Clear();
            cmbDeliveryCity.Text = "";
            txtDeliveryAddress.Clear();
        }

        //clear fields button
        private void btnClearFields_Click(object sender, EventArgs e)
        {
            // Clear ComboBoxes
            cmbServiceType.SelectedIndex = -1;
            cmbServiceType.Text = "";

            cmbPickupProvince.SelectedIndex = -1;
            cmbPickupProvince.Text = "";

            cmbPickupDistrict.SelectedIndex = -1;
            cmbPickupDistrict.Text = "";

            cmbPickupCity.SelectedIndex = -1;
            cmbPickupCity.Text = "";

            cmbDeliveryProvince.SelectedIndex = -1;
            cmbDeliveryProvince.Text = "";

            cmbDeliveryDistrict.SelectedIndex = -1;
            cmbDeliveryDistrict.Text = "";

            cmbDeliveryCity.SelectedIndex = -1;
            cmbDeliveryCity.Text = "";


            // Clear TextBoxes
            txtPickupName.Clear();
            txtPickupPhone.Clear();
            txtPackageDesc.Clear();
            txtPackageWeight.Clear();
            txtPickupAddress.Clear();
            txtRecipientName.Clear();
            txtRecipientPhone.Clear();
            txtDeliveryAddress.Clear();

            // Reset Date Picker
            dtpPickupDate.Value = DateTime.Now;
        }

        // When user selects a pickup province, load relevant districts into the Pickup District combo box
        private void cmbPickupProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPickupDistrict.Items.Clear();
            if (cmbPickupProvince.SelectedItem != null && provinceDistrictMap.TryGetValue(cmbPickupProvince.SelectedItem.ToString(), out var districts))
            {
                cmbPickupDistrict.Items.AddRange(districts.ToArray());
            }
        }

        // When user selects a delivery province, load relevant districts into the Delivery District combo box
        private void cmbDeliveryProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDeliveryDistrict.Items.Clear();
            if (cmbDeliveryProvince.SelectedItem != null && provinceDistrictMap.TryGetValue(cmbDeliveryProvince.SelectedItem.ToString(), out var districts))
            {
                cmbDeliveryDistrict.Items.AddRange(districts.ToArray());
            }
        }

        // When a district is selected for pickup, load matching cities into the pickup city combo box
        private void cmbPickupDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPickupCity.Items.Clear();
            if (cmbPickupDistrict.SelectedItem != null && districtCityMap.TryGetValue(cmbPickupDistrict.SelectedItem.ToString(), out var cities))
            {
                cmbPickupCity.Items.AddRange(cities.ToArray());
            }
        }

        // When delivery district changes, load related cities into delivery city combo box
        private void cmbDeliveryDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDeliveryCity.Items.Clear();
            if (cmbDeliveryDistrict.SelectedItem != null && districtCityMap.TryGetValue(cmbDeliveryDistrict.SelectedItem.ToString(), out var cities))
            {
                cmbDeliveryCity.Items.AddRange(cities.ToArray());
            }
        }

        private void LoadProvinces()
        {
            var provinces = provinceDistrictMap.Keys.ToList();

            cmbPickupProvince.Items.Clear();
            cmbPickupProvince.Items.AddRange(provinces.ToArray());
            cmbPickupProvince.SelectedIndex = -1;

            cmbDeliveryProvince.Items.Clear();
            cmbDeliveryProvince.Items.AddRange(provinces.ToArray());
            cmbDeliveryProvince.SelectedIndex = -1;
        }

        // Format phone numbers as "+94 XX XXX XXXX" and limit to 9 digits
        private void FormatPhoneNumber(TextBox txt)
        {
            string input = txt.Text.Replace("+94", "").Replace(" ", "").Trim();

            // Remove non-digit characters
            input = new string(input.Where(char.IsDigit).ToArray());

            if (input.Length > 9)
                input = input.Substring(0, 9); // Limit to 9 digits

            string formatted = "+94 ";
            if (input.Length > 0)
                formatted += input.Substring(0, Math.Min(2, input.Length));
            if (input.Length > 2)
                formatted += " " + input.Substring(2, Math.Min(3, input.Length - 2));
            if (input.Length > 5)
                formatted += " " + input.Substring(5);

            txt.Text = formatted;
            txt.SelectionStart = txt.Text.Length; // Keep cursor at end
        }

        // Prevent user from deleting "+94 " prefix in phone number fields
        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Prevent deleting prefix
            if ((e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete) && txt.SelectionStart <= 4)
            {
                e.SuppressKeyPress = true;
            }
        }

        // Handles logout confirmation and redirects user to login form
        private void lblLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                frmcustomerlogin loginForm = new frmcustomerlogin();
                loginForm.Show();
                this.Hide();
            }
        }

        //hovering over logout label
        private void lblLogout_MouseEnter(object sender, EventArgs e)
        {
            lblLogout.Font = new Font(lblLogout.Font.FontFamily, lblLogout.Font.Size, FontStyle.Underline | FontStyle.Bold);
        }

        private void lblLogout_MouseLeave(object sender, EventArgs e)
        {
            lblLogout.Font = new Font(lblLogout.Font.FontFamily, lblLogout.Font.Size, FontStyle.Bold | FontStyle.Italic);
        }

        // Load customer profile details from database into form fields
        private void LoadCustomerDetails()
        {
            string connectionString = @"Data Source=DESKTOP-L03EVJF\SQLEXPRESS;Initial Catalog=E-ShiftDB;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT FirstName, LastName, Email, Username FROM Customers WHERE CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", LoggedInCustomerID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtprofilefirstname.Text = reader["FirstName"].ToString();
                        txtprofilelastname.Text = reader["LastName"].ToString();
                        txtprofileemail.Text = reader["Email"].ToString();
                        txtprofileusername.Text = reader["Username"].ToString();

                        LoggedInCustomerEmail = reader["Email"].ToString();
                        lblCustomerEmail.Text = MaskEmail(LoggedInCustomerEmail);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customer details: " + ex.Message);
                }
            }
        }

        // Update customer profile information in the database
        private void btnupdate_Click(object sender, EventArgs e)
        {

            string newFirstName = txtprofilefirstname.Text.Trim();
            string newLastName = txtprofilelastname.Text.Trim();
            string newEmail = txtprofileemail.Text.Trim();
            string newUsername = txtprofileusername.Text.Trim();

            string connectionString = @"Data Source=DESKTOP-L03EVJF\SQLEXPRESS;Initial Catalog=E-ShiftDB;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE Customers 
                             SET FirstName = @FirstName, LastName = @LastName, 
                                 Email = @Email, Username = @Username 
                             WHERE CustomerID = @CustomerID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FirstName", newFirstName);
                    cmd.Parameters.AddWithValue("@LastName", newLastName);
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@Username", newUsername);
                    cmd.Parameters.AddWithValue("@CustomerID", LoggedInCustomerID);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Profile updated successfully!");
                        LoggedInCustomerEmail = newEmail;
                        lblCustomerEmail.Text = MaskEmail(newEmail);
                    }
                    else
                    {
                        MessageBox.Show("Update failed.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Toggle visibility of the profile panel when profile picture is clicked
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panelprofile.Visible = !panelprofile.Visible;
            panelprofile.BringToFront();

        }

        // Toggle profile panel when profile icon is clicked
        private void panelprofile_MouseClick(object sender, MouseEventArgs e)
        {
            pbprofile.BackColor = Color.LightGray;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            panelprofile.Visible = false;
        }

        //////////////////////////////////////////////////END OF REQUEST SHIFT///////////////////////////////////////////////////

        /////////////////////////////////////////////////////ONGOING JOBS///////////////////////////////////////////////////////

        private void dgvOngoingJobs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string status = dgvOngoingJobs.Rows[e.RowIndex].Cells["Status"].Value?.ToString();
            int shiftId = Convert.ToInt32(dgvOngoingJobs.Rows[e.RowIndex].Cells["Job ID"].Value);

            // ✅ ACTION BUTTON
            if (e.ColumnIndex == dgvOngoingJobs.Columns["Action"].Index)
            {
                if (status == "Pending")
                {
                    MessageBox.Show("You cannot complete a pending job. Wait until it's marked as Ongoing.");
                    return;
                }

                DialogResult result = MessageBox.Show("Mark this shift as Completed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MarkShiftAsCompleted(shiftId);
                }
            }

            // ✅ CANCEL BUTTON
            else if (e.ColumnIndex == dgvOngoingJobs.Columns["Cancel"].Index)
            {
                if (status != "Pending")
                {
                    MessageBox.Show("Only pending jobs can be cancelled.");
                    return;
                }

                DialogResult confirm = MessageBox.Show("Are you sure you want to cancel this job?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    CancelShift(shiftId);
                }
            }
        }

        private void MarkShiftAsCompleted(int shiftId)
        {
            string query = "UPDATE Shifts SET Status = 'Completed' WHERE ShiftID = @ShiftID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ShiftID", shiftId);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Shift marked as completed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOngoingJobsToGrid(); // Refresh table
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadOngoingJobsToGrid()
        {
            string query = @"
        SELECT 
            ShiftID AS [Job ID], 
            ShiftDate AS [Date], 
            PickupCity AS [From], 
            DeliveryCity AS [To], 
            PackageDescription AS [Description], 
            Status
        FROM Shifts
        WHERE CustomerID = @CustomerID AND Status IN ('Pending', 'Ongoing')
        ORDER BY ShiftDate DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", LoggedInCustomerID);

                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvOngoingJobs.DataSource = null;
                    dgvOngoingJobs.Columns.Clear();
                    dgvOngoingJobs.DataSource = dt;

                    // Re-add the buttons AFTER binding
                    AddActionAndCancelButtons();

                    // Force each row height manually (the key fix!)
                    foreach (DataGridViewRow row in dgvOngoingJobs.Rows)
                    {
                        row.Height = 40;
                    }

                    dgvOngoingJobs.RowTemplate.Height = 40;
                    dgvOngoingJobs.RowHeadersVisible = false;

                    dgvOngoingJobs.DefaultCellStyle.Font = new Font("Segoe UI", 12);
                    dgvOngoingJobs.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
                    dgvOngoingJobs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvOngoingJobs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    StyleOngoingJobsGrid();
                    ApplyRowColorsByStatus();

                    dgvOngoingJobs.DataBindingComplete += dgvOngoingJobs_DataBindingComplete;
                    dgvOngoingJobs.ClearSelection(); // prevent row auto-selection
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading ongoing jobs: " + ex.Message);
                }
            }
        }

        private void AddActionAndCancelButtons()
        {
            DataGridViewButtonColumn completeButton = new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Mark As",
                Text = "✅ Complete",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            };

            DataGridViewButtonColumn cancelButton = new DataGridViewButtonColumn
            {
                Name = "Cancel",
                HeaderText = "Cancel",
                Text = "🗑 Cancel",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            };

            dgvOngoingJobs.Columns.Add(completeButton);
            dgvOngoingJobs.Columns.Add(cancelButton);
        }

        private void dgvOngoingJobs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvOngoingJobs.Rows)
            {
                row.Height = 40;
            }
        }

        private void StyleOngoingJobsGrid()
        {
            dgvOngoingJobs.EnableHeadersVisualStyles = false;
            dgvOngoingJobs.BorderStyle = BorderStyle.None;
            dgvOngoingJobs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvOngoingJobs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOngoingJobs.GridColor = Color.LightGray;

            // Header
            dgvOngoingJobs.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvOngoingJobs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOngoingJobs.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // Cells
            dgvOngoingJobs.DefaultCellStyle.BackColor = Color.White;
            dgvOngoingJobs.DefaultCellStyle.ForeColor = Color.Black;
            dgvOngoingJobs.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgvOngoingJobs.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvOngoingJobs.DefaultCellStyle.Font = new Font("Segoe UI", 12F);

            // Row Height
            dgvOngoingJobs.RowTemplate.Height = 40; // 🚀 Set fixed row height here

            // Color buttons
            foreach (DataGridViewRow row in dgvOngoingJobs.Rows)
            {
                var status = row.Cells["Status"].Value?.ToString();

                if (status == "Pending")
                {
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                }
                else if (status == "Ongoing")
                {
                    row.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                }
                else if (status == "Completed")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if (status == "Cancelled")
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                }

                // Buttons style
                DataGridViewCell cancelCell = row.Cells["Cancel"];
                DataGridViewCell actionCell = row.Cells["Action"];

                if (cancelCell is DataGridViewButtonCell)
                {
                    cancelCell.Style.BackColor = Color.IndianRed;
                    cancelCell.Style.ForeColor = Color.White;
                    cancelCell.Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }

                if (actionCell is DataGridViewButtonCell)
                {
                    actionCell.Style.BackColor = Color.MediumSeaGreen;
                    actionCell.Style.ForeColor = Color.White;
                    actionCell.Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
            }

            dgvOngoingJobs.ClearSelection(); 
        }

        private void ApplyRowColorsByStatus()
        {
            foreach (DataGridViewRow row in dgvOngoingJobs.Rows)
            {
                string status = row.Cells["Status"].Value?.ToString();

                if (status == "Completed")
                {
                    row.DefaultCellStyle.BackColor = Color.Honeydew; 
                    row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                }
                else if(status == "Ongoing")
                {
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                    row.DefaultCellStyle.ForeColor = Color.DarkBlue;
                }
                if (status == "Pending")
                {
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                    row.DefaultCellStyle.ForeColor = Color.DarkGoldenrod;
                }
            }
        }

        private void CancelShift(int shiftId)
        {
            string query = "UPDATE Shifts SET Status = 'Cancelled' WHERE ShiftID = @ShiftID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ShiftID", shiftId);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Shift cancelled successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOngoingJobsToGrid(); // Refresh grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error cancelling shift: " + ex.Message);
                }
            }
        }

        private void dgvJobHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //////////////////////////////////////////END OF ONGOING JOBS///////////////////////////////////////////////////////

        //////////////////////////////////////////JOB HISTORY///////////////////////////////////////////////////////

        private void LoadJobHistoryToGrid()
        {
            string query = @"
            SELECT 
                ShiftID AS [Job ID], 
                ShiftDate AS [Date], 
                PickupCity AS [From], 
                DeliveryCity AS [To], 
                PackageDescription AS [Description], 
                Status
            FROM Shifts
            WHERE CustomerID = @CustomerID AND Status IN ('Completed', 'Cancelled')
            ORDER BY ShiftDate DESC";


            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", LoggedInCustomerID);

                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvJobHistory.DataSource = dt;

                    StyleJobHistoryGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading job history: " + ex.Message);
                }
            }
        }
        private void StyleJobHistoryGrid()
        {
            dgvJobHistory.BorderStyle = BorderStyle.None;
            dgvJobHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvJobHistory.RowHeadersVisible = false;
            dgvJobHistory.RowTemplate.Height = 40;

            dgvJobHistory.DefaultCellStyle.BackColor = Color.White;
            dgvJobHistory.DefaultCellStyle.ForeColor = Color.Black;
            dgvJobHistory.DefaultCellStyle.Font = new Font("Segoe UI", 13F);
            dgvJobHistory.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgvJobHistory.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvJobHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvJobHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray;
            dgvJobHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvJobHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            dgvJobHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            ApplyRowColorsJobHistory();
            dgvJobHistory.GridColor = Color.Silver;
            dgvJobHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvJobHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJobHistory.MultiSelect = false;
            dgvJobHistory.ClearSelection();
        }

        private void ApplyRowColorsJobHistory()
        {
            foreach (DataGridViewRow row in dgvJobHistory.Rows)
            {
                string status = row.Cells["Status"].Value?.ToString();

                if (status == "Completed")
                {
                    row.DefaultCellStyle.BackColor = Color.Honeydew;
                    row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                }
                else if (status == "Cancelled")
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                    row.DefaultCellStyle.ForeColor = Color.IndianRed;
                }
            }

        }
    }
}