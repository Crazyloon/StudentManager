using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace StudentManager
{
    public partial class MainMenu : Form
    {
        private const string mainMenu = "frmBAS_StudentManager";
        private DatabaseConnection dbc = new DatabaseConnection();
        private dsStudentManager datStudents = null;
        private ListViewItemComparer lvComparer;

        public MainMenu()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            // Plug Database Data into comboboxes 
            // This was coded before learning about databinding and could use a more elegant solution.
            // ----------------------------------------------------------------------------------------
            // populate combo boxes on pnlAddStudentDetails with BAS_Students database look up tables
            PopulateComboBoxFromTable("--Select One--", "BAS_StatusLookup", "StatusID", "StatusType", cboStudentType, false);
            PopulateComboBoxFromTable("--Select One--", "EduBackgroundLookup", "EduID", "EduBackground", cboExperience, false);
            PopulateComboBoxFromTable("--Select One--", "ReferralTypeLookup", "ReferralID", "ReferralType", cboInquiryType, false);
            // pnlExtraDetails
            PopulateComboBoxWithKeyValuesFromTable("--Select Title--", "JobTitleLookup", "JobTitleID", "JobTitle", cboJobTitle);
            //PopulateComboBoxFromTable("--Select Status--", "EmploymentStatusLookUp", "EmploymentStatusID", "EmploymentStatus", cboEmploymentStatus, false);
            PopulateComboBoxWithKeyValuesFromTable("--Select Status--", "EmploymentStatusLookUp", "EmploymentStatusID", "EmploymentStatus", cboEmploymentStatus);
            PopulateComboBoxWithKeyValuesFromTable("--Select College--", "Colleges", "CollegeID", "CollegeName", cboColleges);
            PopulateComboBoxWithKeyValuesFromTable("--Select Employer--", "Employers", "EmployerID", "EmployerName", cboEmployer);
            PopulateComboBoxWithKeyValuesFromTable("--Select Course Type--", "CourseTypeLookUp", "CourseTypeID", "CourseType", cboCourseType);
            PopulateComboBoxWithKeyValuesFromTable("--Select Status--", "EnrollmentStatusLookUp", "EnrollmentStatusID", "EnrollmentStatus", cboCourseEnrollment_EnrollmentStatus);
            PopulateComboBoxWithKeyValuesFromTable("--Select Course--", "Courses", "CourseID", "CourseNumber", cboCourseEnrollment_CourseNumber);

            PopulateComboBoxWithKeyValuesFromTable("--Select Credit Section--", "CreditSectionLookUp", "CreditSectionID", "CreditSection", cboCourseCreditSection);

            cboLastYearAttended.Items.Add("--Last Year Attended--");
            cboLastYearAttended.SelectedIndex = 0;
            for (int i = 0; i < 70; i++)
            {
                cboLastYearAttended.Items.Add((DateTime.Now.Year - i).ToString());
            }
            // ----------------------------------------------------------------------------------------

            // Load student selection ComboBoxes - triggers an event that clears the combo boxes above
            cboStudentLookup.Items.Add("New Student");
            LoadStudentsComboBox(cboStudentLookup);
            cboStudentLookup2.Items.Add("Select Student");
            LoadStudentsComboBox(cboStudentLookup2);

            // set the student listview to be have a customer sorter
            lvComparer = new ListViewItemComparer();
            lsvStudentView.ListViewItemSorter = lvComparer;

            // get the Students table in a dataset
            datStudents = dbc.GetAllStudentsInDataSet();

            // Preload Students View with that dataset          
            LoadStudentView(datStudents);

            // Preload Courses View
            LoadCourseView();
            pnlViewCourses.Visible = false;

            lsvCourseView.ListViewItemSorter = lvComparer;
            lsvEmploymentStatus.ListViewItemSorter = lvComparer;
            lsvEnrollmentData.ListViewItemSorter = lvComparer;
            lsvPreviousColleges.ListViewItemSorter = lvComparer;
        }

        #region Add/Modify Student Tab
        // Panels
        #region Student Details Panel
        // METHODS
        private void SetUpCurrentStudentDetails(int studentID)
        {
            // Clear any previous details
            ClearAddStudentDetailForm();
            // get the details for the current student
            SqlDataReader sdr = dbc.GetMostRecentStudentDetails(studentID);
            while (sdr.Read())
            {
                // process comboboxes
                cboExperience.SelectedIndex = (int)sdr["EduBackground"];
                cboInquiryType.SelectedIndex = (int)sdr["ReferralType"];
                cboStudentType.SelectedIndex = (int)sdr["BAS_Status"];
                // process checkboxes
                chkAttendedInfoSession.Checked = (bool)sdr["AttendedInfoSession"];
                chkRunningStart.Checked = (bool)sdr["RunningStartParticipant"];
                // process notes rtb
                rtbNotes.Text = sdr["Notes"].ToString(); // if 'Notes' is null, 'rtbNotes.Text' will just be empty

                // process radio button
                int preferredContactMethod = (int)sdr["PreferredContactMethod"];
                switch (preferredContactMethod)
                {
                    case 1:
                        radPhone.Checked = true;
                        break;
                    case 2:
                        radEmail.Checked = true;
                        break;
                    case 3:
                        radMail.Checked = true;
                        break;
                    case 4:
                        radText.Checked = true;
                        break;
                    default:
                        MessageBox.Show("An error occured while trying to process the students preferred contact method");
                        break;
                }
            }
            sdr.Close();
        }
        private void PopulateComboBoxFromTable(string initialDisplayText, string tableName, string valueFieldName, string keyFieldName, ComboBox cbo, bool showID)
        {
            SqlDataReader sdr = dbc.GetAllRowsFromTable(tableName);
            cbo.Items.Clear();
            cbo.Items.Add(initialDisplayText);
            if (showID)
            {
                while (sdr.Read())
                {
                    cbo.Items.Add(sdr[valueFieldName].ToString() + " " + sdr[keyFieldName].ToString());
                }
            }
            else
            {
                while (sdr.Read())
                {
                    cbo.Items.Add(sdr[keyFieldName].ToString());
                }
            }

            sdr.Close();
            cbo.SelectedIndex = 0;
        }
        private void PopulateComboBoxWithKeyValuesFromTable(string initialDisplayText, string tableName, string valueFieldName, string keyFieldName, ComboBox cbo)
        {
            Dictionary<string, int> comboboxSource = new Dictionary<string, int>();
            SqlDataReader sdr = dbc.GetAllRowsFromTable(tableName);

            comboboxSource.Add(initialDisplayText, -1);
            while (sdr.Read())
            {
                comboboxSource.Add(sdr[keyFieldName].ToString(), int.Parse(sdr[valueFieldName].ToString()));
            }
            cbo.DataSource = new BindingSource(comboboxSource, null);
            cbo.DisplayMember = "Key";
            cbo.ValueMember = "Value";


            sdr.Close();
            cbo.SelectedIndex = 0;
        }
        private void PopulateComboBoxWithKeyValuesFromCoursesAndCreditSectionLookUp(string initialDisplayText, string valueFieldName, ComboBox cbo, params string[] keyFieldNames)
        {
            Dictionary<string, int> comboboxSource = new Dictionary<string, int>();
            SqlDataReader sdr = dbc.GetCoursesCreditSection();

            StringBuilder sb = new StringBuilder();
            int[] padding = new int[keyFieldNames.Length];
            padding[0] = 15;
            comboboxSource.Add(initialDisplayText, -1);
            while (sdr.Read())
            {
                for (int i = 0; i < keyFieldNames.Length; i++)
                {
                    sb.Append(sdr[keyFieldNames[i]].ToString().PadRight(padding[i]));
                }
                comboboxSource.Add(sb.ToString(), int.Parse(sdr[valueFieldName].ToString()));
                sb.Clear();
            }
            cbo.DataSource = new BindingSource(comboboxSource, null);
            cbo.DisplayMember = "Key";
            cbo.ValueMember = "Value";


            sdr.Close();
            cbo.SelectedIndex = 0;
        }
        private void ClearAddStudentDetailForm()
        {
            if (cboExperience.Items.Count > 0)
                cboExperience.SelectedIndex = 0;
            if (cboInquiryType.Items.Count > 0)
                cboInquiryType.SelectedIndex = 0;
            if (cboStudentType.Items.Count > 0)
                cboStudentType.SelectedIndex = 0;

            chkAttendedInfoSession.Checked = false;
            chkRunningStart.Checked = false;
            rtbNotes.Clear();
            radPhone.Checked = false;
            radEmail.Checked = false;
            radMail.Checked = false;
            radText.Checked = false;
        }

        // EVENTS
        private void btnAddModifyDetails_Click(object sender, EventArgs e)
        {
            // Read all field data into variables
            if (cboExperience.SelectedIndex != 0 && cboInquiryType.SelectedIndex != 0 && cboStudentType.SelectedIndex != 0)
            {
                // the selected index for the combo box relates to the id field in the database.
                // if items are ADDED to the database selectedIndex will still be valid.
                // if items are REMOVED from the database it is advised that this code change.
                // fill the combo boxes with Dictionary<string, int> and use a binding source to populate the display and value
                // then use SelectedValue instead of SelectedIndex.
                int StatusID = cboStudentType.SelectedIndex;
                int EduID = cboExperience.SelectedIndex;
                int referralID = cboInquiryType.SelectedIndex;
                string notes = rtbNotes.Text;
                if (notes == string.Empty)
                {
                    notes = null;
                }
                bool runningStart = chkRunningStart.Checked;
                bool attendedInfosession = chkAttendedInfoSession.Checked;

                int contactTypeID = 0;
                if (radPhone.Checked)
                    contactTypeID = 1;
                else if (radEmail.Checked)
                    contactTypeID = 2;
                else if (radMail.Checked)
                    contactTypeID = 3;
                else if (radText.Checked)
                    contactTypeID = 4;

                if (contactTypeID != 0)
                {
                    int studentID = int.Parse(txtCurrentStudentID.Text);
                    dbc.AddStudentDetail(studentID, EduID, referralID, contactTypeID, attendedInfosession, runningStart, StatusID, notes);
                    MessageBox.Show("Success");
                    ClearAddStudentDetailForm();

                    // Get the current student whose information is being added/modified
                    string selectedStudent = (string)cboStudentLookup.SelectedItem;
                    string firstName, lastName;
                    int.TryParse(selectedStudent.Substring(0, selectedStudent.IndexOf(" ")), out studentID);
                    int currentStudent = dbc.GetLastStudent(studentID, out firstName, out lastName);

                    // set up next screen's visible data
                    txtID_ExtraDetails.Text = studentID.ToString();
                    txtFirstName_ExtraDetails.Text = firstName;
                    txtLastName_ExtraDetails.Text = lastName;

                    // show next screen
                    pnlAddStudentDetails.Hide();
                    pnlAdditionalDetails.Show();

                    // disable 'next' button
                    btnAddStudent_Next.Enabled = false;
                    btnAddStudent_Previous.Enabled = true;

                    // set panel number
                    lblAddStudent_CurrentPanel.Text = "3";
                }
                else
                    MessageBox.Show("Unable to add detail, first select a preferred method of contact for this student.", "Contact method missing");
            }
        }
        #endregion
        #region Additional Details Panel
        private dsStudentManager studentDetails = null;
        private System.Data.DataTable dtPreviousColleges = null;
        private System.Data.DataTable dtEmploymentDetails = null;

        // METHODS (All Panels)
        private bool isItemInComboBox(string itemName, ComboBox cbo)
        {
            foreach (KeyValuePair<string, int> item in cbo.Items)
            {
                if (item.Key == itemName)
                {
                    return true;
                }
            }
            return false;
        }
        // Attended Colleges GroupBox

        // Employment Status GroupBox

        // Tech Interests GroupBox
        private bool DisplayTechInterests(int studentID)
        {
            // get the data reader with tech interests for the current student
            try
            {
                // get the student's tech interests if they're availible
                studentDetails = dbc.GetStudentsTechInterests(studentID);
                if (studentDetails.TechInterests.Rows.Count > 0)
                {
                    bool dbDev = studentDetails.TechInterests[0].DatabaseDevelopment;
                    bool swDev = studentDetails.TechInterests[0].SoftwareDevelopment;
                    bool webDev = studentDetails.TechInterests[0].WebDevelopment;
                    bool mobileDev = studentDetails.TechInterests[0].MobileDevelopment;
                    bool dbAnalysis = studentDetails.TechInterests[0].DatabaseAnalysis;
                    bool swAnalysis = studentDetails.TechInterests[0].SoftwareAnalysis;
                    bool interfaceDesign = studentDetails.TechInterests[0].InterfaceDesign;

                    // set the check boxes to the students preference
                    chkDatabaseDevelopment.Checked = dbDev;
                    chkSoftwareDevelopment.Checked = swDev;
                    chkWebDevelopment.Checked = webDev;
                    chkMobileDevelopment.Checked = mobileDev;
                    chkDatabaseAnalysis.Checked = dbAnalysis;
                    chkSoftwareAnalysis.Checked = swAnalysis;
                    chkInterfaceDesign.Checked = interfaceDesign;
                    return true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while trying to retrieve this student's tech interets");
            }
            return false;
        }
        private void ClearTechInterests()
        {
            chkDatabaseDevelopment.Checked = false;
            chkSoftwareDevelopment.Checked = false;
            chkWebDevelopment.Checked = false;
            chkMobileDevelopment.Checked = false;
            chkDatabaseAnalysis.Checked = false;
            chkSoftwareAnalysis.Checked = false;
            chkInterfaceDesign.Checked = false;
        }
        private bool DisplayPreviousColleges(int studentID)
        {
            // fill the data reader with previous colleges for the current student
            try
            {
                // get the student's previous colleges if they're availible
                studentDetails = dbc.GetStudentsPreviousColleges(studentID);

                dtPreviousColleges = studentDetails.Tables[studentDetails.Tables.Count - 1];
                if (dtPreviousColleges.Rows.Count > 0)
                {
                    lsvPreviousColleges.Items.Clear();
                    ListViewItem lvi = null;
                    // add each row to the listview
                    for (int i = 0; i < dtPreviousColleges.Rows.Count; i++)
                    {
                        lvi = new ListViewItem(dtPreviousColleges.Rows[i]["CollegeName"].ToString());
                        lvi.SubItems.Add(dtPreviousColleges.Rows[i]["CurrentlyAttending"].ToString());
                        lvi.SubItems.Add(((int)dtPreviousColleges.Rows[i]["LastYearAttended"]).ToString());
                        lvi.SubItems.Add(((double)dtPreviousColleges.Rows[i]["GPA"]).ToString());

                        lsvPreviousColleges.Items.Add(lvi);
                    }
                    return true;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("An error occurred while trying to retrieve this student's previously attended colleges.");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Exception has occurred at 'DisplayPreviousColleges'\n" + ex.GetType().ToString() + "\n\n" + ex.Message);
                return false;
            }
            return false;
        }
        private void ClearPreviousColleges()
        {
            lsvPreviousColleges.Items.Clear();
            cboColleges.SelectedIndex = 0;
            chkCurrentlyAttending.Checked = false;
            cboLastYearAttended.SelectedIndex = 0;
            nudGPA.Value = 2.5m;
        }
        private bool DisplayEmploymentStatus(int studentID)
        {
            // fill the data reader with employment data for the current student
            try
            {
                // get the student's employers if they're availible
                studentDetails = dbc.GetStudentsEmploymentStatus(studentID);

                dtEmploymentDetails = studentDetails.Tables[studentDetails.Tables.Count - 1];
                lsvEmploymentStatus.Items.Clear();
                if (dtEmploymentDetails.Rows.Count > 0)
                {

                    ListViewItem lvi = null;
                    // add each row to the listview
                    for (int i = 0; i < dtEmploymentDetails.Rows.Count; i++)
                    {
                        lvi = new ListViewItem(dtEmploymentDetails.Rows[i]["EmploymentStatusID"].ToString());
                        lvi.SubItems.Add(dtEmploymentDetails.Rows[i]["EmployerName"].ToString());
                        lvi.SubItems.Add(dtEmploymentDetails.Rows[i]["EmploymentStatus"].ToString());
                        lvi.SubItems.Add(dtEmploymentDetails.Rows[i]["JobTitle"].ToString());
                        lvi.SubItems.Add(((DateTime)dtEmploymentDetails.Rows[i]["StatusChangedDate"]).ToShortDateString());


                        lsvEmploymentStatus.Items.Add(lvi);
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while trying to retreive this student's employment detials.");
            }
            return false;
        }
        private void ClearEmploymentStatus()
        {
            lsvEmploymentStatus.Items.Clear();
        }
        private void ClearEmploymnetStatusSelections()
        {
            cboEmployer.SelectedIndex = 0;
            cboEmploymentStatus.SelectedIndex = 0;
            cboJobTitle.SelectedIndex = 0;
        }

        // EVENTS (All Panels)
        private void pnlAdditionalDetails_VisibleChanged(object sender, EventArgs e)
        {
            if (pnlAdditionalDetails.Visible == true)
            {
                // prepare the dataset for new data
                if (studentDetails != null)
                    studentDetails.Clear();

                // get current student
                int currentStudentID = int.Parse(txtID_ExtraDetails.Text);

                // clear any previously displayed data
                ClearTechInterests();
                ClearPreviousColleges();
                ClearEmploymentStatus();

                // show tech interests, show employers, show previous colleges
                // these 3 methods use the dataset 'studentDetails'
                DisplayTechInterests(currentStudentID);
                DisplayPreviousColleges(currentStudentID);
                DisplayEmploymentStatus(currentStudentID);
            }
        }

        // EVENTS Attended Colleges GroupBox
        private void btnAddCollege_Click(object sender, EventArgs e)
        {
            // get new college's name
            string collegeName = txtAddCollege.Text.Trim();
            txtAddCollege.Clear();
            // check the DataSource to make sure that college does not already exist
            if (collegeName != string.Empty)
            {
                if (!isItemInComboBox(collegeName, cboColleges))
                {
                    // insert that college into the database.
                    dbc.AddCollege(collegeName);

                    // update combobox with new college
                    PopulateComboBoxWithKeyValuesFromTable("--Select College--", "Colleges", "CollegeID", "CollegeName", cboColleges);
                }
                else
                    MessageBox.Show("The college you are trying to add already exists.");
            }
        }
        private void btnRemoveCollege_Click(object sender, EventArgs e)
        {
            // get the selected college's ID and...
            int collegeID = (int)cboColleges.SelectedValue;
            string collegeName = ((KeyValuePair<string, int>)cboColleges.SelectedItem).Key;

            // remove it from the database.
            DialogResult dr = MessageBox.Show("You are about to remove " + collegeName + " from the database, this can only be done, through this interface, if no records exist which reference the college you wish to remove.\n\nThis function is mostly meant to remove unused colleges and those added by mistake", "Remove college?", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                bool isCollegeRemoved = dbc.RemoveCollege(collegeID);
                if (!isCollegeRemoved)
                {
                    MessageBox.Show("Unable to remove " + collegeName + " because one or more records reference it in the database.\n\nContact your database administrator for further assistance.");
                }
            }

            // update the combo box to reflect the changes in the database
            PopulateComboBoxWithKeyValuesFromTable("--Select College--", "Colleges", "CollegeID", "CollegeName", cboColleges);
        }
        private void btnAddModifyAttendedCollege_Click(object sender, EventArgs e)
        {
            // get users attended college info
            int studentID = int.Parse(txtID_ExtraDetails.Text);

            //string selectedCollege = (string)cboColleges.SelectedItem;
            int selectedCollegeID;
            //int.TryParse(selectedCollege.Substring(0, selectedCollege.IndexOf(" ")), out selectedCollegeID);
            selectedCollegeID = (int)cboColleges.SelectedValue;

            bool currentlyAttending = chkCurrentlyAttending.Checked;

            int lastYearAttended;
            int.TryParse(cboLastYearAttended.SelectedItem.ToString(), out lastYearAttended);
            double gpa = (double)nudGPA.Value;

            // Update existing record or add a new one
            // double click list view to populate the user inputs
            // when adding/updating check the college ID for that StudentID, if the PK exists, update, else add
            if (cboColleges.SelectedIndex > 0 && lastYearAttended > 0)
            {
                if (dbc.AttendedCollege_PrimaryKeyExists(studentID, selectedCollegeID))
                {
                    dbc.UpdateAttendedCollege(selectedCollegeID, studentID, currentlyAttending, lastYearAttended, gpa);
                    DisplayPreviousColleges(studentID);
                }
                else
                {
                    dbc.AddAttendedCollege(selectedCollegeID, studentID, currentlyAttending, lastYearAttended, gpa);
                    DisplayPreviousColleges(studentID);
                }
            }
        }
        private void lsvPreviousColleges_DoubleClick(object sender, EventArgs e)
        {
            // on double click, populate user controls with data from the list view
            int selectedIndex = lsvPreviousColleges.SelectedIndices[0];
            string collegeName = lsvPreviousColleges.Items[selectedIndex].Text;
            bool currentlyAttending = Convert.ToBoolean(lsvPreviousColleges.Items[selectedIndex].SubItems[1].Text);
            int lastYearAttended = int.Parse(lsvPreviousColleges.Items[selectedIndex].SubItems[2].Text);
            double GPA = double.Parse(lsvPreviousColleges.Items[selectedIndex].SubItems[3].Text);

            // first index is a string telling the user how to use the combo box.
            // Set the index to select to 1 and skip over the first item in the combo box.
            int index = 1;
            foreach (KeyValuePair<string, int> item in cboColleges.Items)
            {
                if (item.Key == "--Select College--")
                {
                    continue;
                }
                string collegeReal = item.Key;
                if (collegeReal == collegeName)
                {
                    cboColleges.SelectedIndex = index;
                    break;
                }
                index++;
            }

            // set the checkbox checked property
            chkCurrentlyAttending.Checked = currentlyAttending;

            // first index is a string telling the user how to use the combo box.
            // Set the index to select to 1 and skip over the first item in the combo box.
            index = 0;
            int selectedYear = 0;
            foreach (object year in cboLastYearAttended.Items)
            {
                if (int.TryParse(year.ToString(), out selectedYear))
                {
                    if (lastYearAttended == selectedYear)
                    {
                        cboLastYearAttended.SelectedIndex = index;
                        break;
                    }
                }
                index++;
            }

            // set the value of the selected GPA
            nudGPA.Value = (decimal)GPA;
        }

        // EVENTS Employment Status GroupBox
        private void btnAddModify_EmploymentStatus_Click(object sender, EventArgs e)
        {
            if (cboEmployer.SelectedIndex > 0 && cboEmploymentStatus.SelectedIndex > 0 && cboJobTitle.SelectedIndex > 0)
            {
                // get student id
                int studentID = int.Parse(txtID_ExtraDetails.Text);
                // get employer id
                int selectedEmployerID = (int)cboEmployer.SelectedValue;
                // get employment status
                int employmentStatus = (int)cboEmploymentStatus.SelectedIndex;
                // get job title
                int jobTitle = (int)cboJobTitle.SelectedValue;

                // add the new record
                dbc.AddEmploymentStatus(selectedEmployerID, studentID, employmentStatus, jobTitle, DateTime.Now);
                // update interface
                ClearEmploymnetStatusSelections();
                DisplayEmploymentStatus(studentID);
            }

        }
        private void btnDelete_EmploymentStatus_Click(object sender, EventArgs e)
        {
            // get the EmploymentStatusID from the selected index of the list view
            int index = lsvEmploymentStatus.SelectedIndices[0];
            int id = int.Parse(lsvEmploymentStatus.Items[index].Text);
            // delete the record in EmploymentStatus with that matching ID

            dbc.DeleteEmploymentStatus(id);
            DisplayEmploymentStatus(int.Parse(txtID_ExtraDetails.Text));
        }
        private void btnAddEmployer_Click(object sender, EventArgs e)
        {
            // get employer name and clear text box
            string employerName = txtAddEmployer.Text.Trim();
            txtAddEmployer.Clear();

            // check DataSource to make sure the employer does not already exist.
            if (employerName != string.Empty)
            {
                if (!isItemInComboBox(employerName, cboEmployer))
                {
                    dbc.AddEmployer(employerName);

                    PopulateComboBoxWithKeyValuesFromTable("--Select Employer--", "Employers", "EmployerID", "EmployerName", cboEmployer);
                }
                else
                    MessageBox.Show("The employer you are trying to add already exists.");
            }

        }
        private void btnRemoveEmployer_Click(object sender, EventArgs e)
        {
            // get the selected employers's ID and...
            int employerID = (int)cboEmployer.SelectedValue;
            string employerName = ((KeyValuePair<string, int>)cboEmployer.SelectedItem).Key;

            // remove it from the database if the user is sure they want it removed.
            DialogResult dr = MessageBox.Show("You are about to remove " + employerName + " from the database, this can only be done, through this interface, if no records exist which reference the employer you wish to remove.\n\nThis function is mostly meant to remove unused employers and those added by mistake", "Remove employer?", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                bool isEmployerRemoved = dbc.RemoveEmployer(employerID);
                if (!isEmployerRemoved)
                {
                    MessageBox.Show("Unable to remove " + employerName + " because one or more records reference it in the database.\n\nContact your database administrator for further assistance.");
                }
                else // update the combo box to reflect the changes in the database
                    PopulateComboBoxWithKeyValuesFromTable("--Select Employer--", "Employers", "EmployerID", "EmployerName", cboEmployer);
            }

        }
        private void btnAddJobTitle_Click(object sender, EventArgs e)
        {
            string jobTitle = txtAddJobTitle.Text.Trim();
            txtAddJobTitle.Clear();

            if (jobTitle != string.Empty)
            {
                if (!isItemInComboBox(jobTitle, cboJobTitle))
                {
                    dbc.AddJobTitle(jobTitle);
                    PopulateComboBoxWithKeyValuesFromTable("--Select Title--", "JobTitleLookup", "JobTitleID", "JobTitle", cboJobTitle);
                }
                else
                {
                    MessageBox.Show("The title you are trying to add already exists.");
                }
            }
        }
        private void btnRemoveJobTitle_Click(object sender, EventArgs e)
        {
            // get selected JobTitleID and JobTitle
            int jobTitleID = (int)cboJobTitle.SelectedValue;
            string jobTitle = ((KeyValuePair<string, int>)cboJobTitle.SelectedItem).Key;

            // remove it from the database if the user is sure they want it removed.
            DialogResult dr = MessageBox.Show("You are about to remove " + jobTitle + " from the database, this can only be done through this interface if no records exist which reference the job title you wish to remove.\n\nThis function is mostly meant to remove unused job titles and those added by mistake", "Remove job title?", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                bool isJobTitleRemoved = dbc.RemoveJobTitle(jobTitleID);
                if (!isJobTitleRemoved)
                {
                    MessageBox.Show("Unable to remove " + jobTitle + " because one or more records reference it in the database.\n\nContact your database administrator for further assistance.");
                }
                else // update the combo box to reflect the changes in the database
                    PopulateComboBoxWithKeyValuesFromTable("--Select Title--", "JobTitleLookup", "JobTitleID", "JobTitle", cboJobTitle);
            }
        }

        // EVENTS Tech Interests GroupBox     
        private void btnAddModify_TechInterests_Click(object sender, EventArgs e)
        {
            // get current student
            int currentStudentID = int.Parse(txtID_ExtraDetails.Text);

            // read tech interests for current student
            bool dbDev = chkDatabaseDevelopment.Checked;
            bool swDev = chkSoftwareDevelopment.Checked;
            bool webDev = chkWebDevelopment.Checked;
            bool mobileDev = chkMobileDevelopment.Checked;
            bool dbAnalysis = chkDatabaseAnalysis.Checked;
            bool swAnalysis = chkSoftwareAnalysis.Checked;
            bool interfaceDesign = chkInterfaceDesign.Checked;

            // get the data reader with tech interests for the current student
            try
            {
                // get the student's tech interests if they're availible
                studentDetails = dbc.GetStudentsTechInterests(currentStudentID);
                if (studentDetails.TechInterests.Rows.Count == 1)
                {
                    // if a row exist for this student, update that row
                    studentDetails.TechInterests[0].DatabaseDevelopment = dbDev;
                    studentDetails.TechInterests[0].SoftwareDevelopment = swDev;
                    studentDetails.TechInterests[0].WebDevelopment = webDev;
                    studentDetails.TechInterests[0].MobileDevelopment = mobileDev;
                    studentDetails.TechInterests[0].DatabaseAnalysis = dbAnalysis;
                    studentDetails.TechInterests[0].SoftwareAnalysis = swAnalysis;
                    studentDetails.TechInterests[0].InterfaceDesign = interfaceDesign;

                    // commit changes to database.
                    dbc.techInterestsAdapter.Update(studentDetails.TechInterests);
                }
                else
                {
                    // create a new row for this student.
                    DataRow drRow = studentDetails.TechInterests.NewRow();

                    // set it's values
                    drRow[0] = currentStudentID;
                    drRow[1] = dbDev;
                    drRow[2] = swDev;
                    drRow[3] = webDev;
                    drRow[4] = mobileDev;
                    drRow[5] = dbAnalysis;
                    drRow[6] = swAnalysis;
                    drRow[7] = interfaceDesign;

                    // apply them to the dataset
                    studentDetails.TechInterests.Rows.Add(drRow);

                    // commit those changes to the database
                    dbc.techInterestsAdapter.Update(studentDetails.TechInterests);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region Add Student Panel
        // METHODS
        private void LoadStudentsComboBox(ComboBox cbo)
        {
            SqlDataReader sdr;
            try
            {
                sdr = dbc.GetAllStudents("LastName", false);
                string firstName, lastName;
                int studentID;
                while (sdr.Read())
                {
                    //SELECT FirstName, LastName, BirthDate, Gender, Address, City, State, Zip, PersonalEmail, StudentEmail, RTCStudentID, Phone, Notes, StudentDocumentsLocation
                    studentID = (int)sdr["StudentID"];
                    firstName = (string)sdr["FirstName"];
                    lastName = (string)sdr["LastName"];
                    cbo.Items.Add(studentID.ToString().PadRight(4) + " " + firstName + " " + lastName);
                }
                sdr.Close();
                cbo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nException Type: " + ex.GetType());
            }
        }
        private void ClearAddStudentForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            dtpBirthDate.Value = DateTime.Now;
            radFemale.Checked = false;
            radMale.Checked = false;
            txtAddress.Clear();
            txtCity.Clear();
            cboState.Text = "";
            txtZip.Clear();
            txtPrimaryEmail.Clear();
            txtRTCEmail.Clear();
            txtRTCSID.Clear();
            txtPhoneNo.Clear();
            rtbStudentNotes.Clear();
            txtAddStudent_FolderPath.Clear();
        }
        private void PopulateStudentInfo(SqlDataReader sdr)
        {
            try
            {
                while (sdr.Read())
                {
                    //SELECT FirstName, LastName, BirthDate, Gender, Address, City, State, Zip, PersonalEmail, StudentEmail, RTCStudentID, Phone, Notes
                    txtFirstName.Text = (string)sdr["FirstName"];
                    txtLastName.Text = (string)sdr["LastName"];

                    if (sdr["BirthDate"] != DBNull.Value)
                    {
                        dtpBirthDate.Value = (DateTime)sdr["BirthDate"];
                    }
                    else
                        dtpBirthDate.Value = DateTime.Now;


                    string gender = sdr["Gender"].ToString();
                    if (gender == "False")
                        radMale.Checked = true;
                    else
                        radFemale.Checked = true;

                    if (sdr["Address"] != DBNull.Value)
                    {
                        txtAddress.Text = (string)sdr["Address"];
                    }
                    else
                        txtAddress.Clear();

                    if (sdr["City"] != DBNull.Value)
                    {
                        txtCity.Text = (string)sdr["City"];
                    }
                    else
                        txtCity.Clear();

                    if (cboState.Items.Contains(sdr["State"]))
                    {
                        cboState.SelectedIndex = cboState.Items.IndexOf(sdr["State"]);
                    }
                    else
                        cboState.SelectedIndex = 0;

                    if (sdr["Zip"] != DBNull.Value)
                    {
                        txtZip.Text = (string)sdr["Zip"];
                    }
                    else
                        txtZip.Clear();

                    txtPrimaryEmail.Text = (string)sdr["PersonalEmail"];

                    if (sdr["StudentEmail"] != DBNull.Value)
                    {
                        txtRTCEmail.Text = (string)sdr["StudentEmail"];
                    }
                    else
                        txtRTCEmail.Clear();

                    if (sdr["RTCStudentID"] != DBNull.Value)
                    {
                        txtRTCSID.Text = (string)sdr["RTCStudentID"];
                    }
                    else
                        txtRTCSID.Clear();

                    if (sdr["Phone"] != DBNull.Value)
                    {
                        txtPhoneNo.Text = (string)sdr["Phone"];
                    }
                    else
                        txtPhoneNo.Clear();


                    if (sdr["Notes"] != DBNull.Value)
                    {
                        rtbStudentNotes.Text = (string)sdr["Notes"];
                    }
                    else
                        rtbStudentNotes.Clear();

                    if (sdr["StudentDocumentsLocation"] != DBNull.Value)
                    {
                        txtAddStudent_FolderPath.Text = (string)sdr["StudentDocumentsLocation"];
                    }
                    else
                        txtAddStudent_FolderPath.Clear();
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.GetType());
            }
        }
        private bool UpdateExistingStudent(int studentID)
        {
            string fName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower());

            if (fName == string.Empty)
            {
                MessageBox.Show("Must Enter a First Name");
                txtFirstName.BackColor = Color.Red;
                return false;
            }


            string lName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text.Trim().ToLower());

            if (lName.Length > 2)
            {
                if (lName[1] == '\'')
                {
                    Char.ToUpper(lName[2]);
                }
            }
            else if (lName == string.Empty)
            {
                MessageBox.Show("Must Enter a Last Name");
                txtLastName.BackColor = Color.Red;
                return false;
            }


            DateTime birthDate = dtpBirthDate.Value;
            if (birthDate.Date == DateTime.Now.Date)
            {
                DialogResult dr = MessageBox.Show("Is this student's birthdate unknown?", "Invalid Birthdate", MessageBoxButtons.YesNo);
                if (dr != DialogResult.Yes)
                {
                    return false;
                }
            }

            bool gender = false;
            if (radMale.Checked == true || radFemale.Checked == true)
            {
                if (radMale.Checked == true)
                    gender = false;
                else if (radFemale.Checked == true)
                    gender = true;
            }
            else
            {
                MessageBox.Show("Gender Required.");
                return false;
            }

            string address = txtAddress.Text.Trim().ToUpper();
            //if (address.Length < 5)
            //{
            //    MessageBox.Show("The address you entered is invalid.\n\nIf you believe this message is an error, contact your Database Administrator.");
            //    txtAddress.BackColor = Color.Red;
            //    return false;
            //}

            //Propercase city
            string city = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtCity.Text.Trim().ToLower());
            if (city.Length > 0)
            {
                Char.ToUpper(city[0]);
            }
            //if (city.Length < 3)
            //{
            //    MessageBox.Show("City name too short.");
            //    txtCity.BackColor = Color.Red;
            //    return false;
            //}

            string state;
            if (cboState.SelectedIndex <= 0)
            {
                state = "NULL";
            }
            state = (string)cboState.SelectedItem;

            int zip = 0;
            if (txtZip.Text != String.Empty)
            {
                if (!int.TryParse(txtZip.Text, out zip))
                {
                    MessageBox.Show("Verify the zip code is numeric.");
                    txtZip.BackColor = Color.Red;
                    return false;
                }
                else if (txtZip.Text.Length != 5)
                {
                    MessageBox.Show("Check that the zip code entere is 5 digits long.");
                    txtZip.BackColor = Color.Red;
                    return false;
                }
            }

            string email = txtPrimaryEmail.Text.Trim().ToLower();
            if (email == string.Empty)
            {
                MessageBox.Show("Primary email required");
                txtPrimaryEmail.BackColor = Color.Red;
                return false;
            }
            // verify format with regular expression
            string rtcEmail = txtRTCEmail.Text.Trim().ToLower();
            // verify format with regular expression

            string rtcSID = txtRTCSID.Text;
            if (rtcSID != string.Empty)
            {
                if (rtcSID.Length != 9)
                {
                    MessageBox.Show("The RTC Student ID is 9 digits.");
                    txtRTCSID.BackColor = Color.Red;
                    return false;
                }
                // check SID is numeric
                double testSID;
                if (!double.TryParse(rtcSID, out testSID))
                {
                    MessageBox.Show("RTC Student ID must be digits only.");
                    txtRTCSID.BackColor = Color.Red;
                    return false;
                }
            }

            string phoneNo = txtPhoneNo.Text;
            long testPhone;
            if (phoneNo != string.Empty)
            {
                if (!long.TryParse(phoneNo, out testPhone))
                {
                    MessageBox.Show("Phone number must be 10 digits");
                    txtPhoneNo.BackColor = Color.Red;
                    return false;
                }
                if (phoneNo.Length != 10)
                {
                    MessageBox.Show("Please verify the phone number is correct\nDo not include any symbols, only numbers.");
                    txtPhoneNo.BackColor = Color.Red;
                    return false;
                }
            }

            string notes = rtbStudentNotes.Text;
            string folderPath = txtAddStudent_FolderPath.Text;
            if (folderPath == string.Empty)
            {
                DialogResult dr = MessageBox.Show("This student does not have a path to their documents. Are you sure you want to update this student anyway?", "No path provided", MessageBoxButtons.YesNo);
                if (dr != DialogResult.Yes)
                {
                    return false;
                }
            }

            return dbc.UpdateStudent(studentID, rtcSID, fName, lName, phoneNo, rtcEmail, email, address, city, state, zip.ToString(), birthDate, gender, notes, folderPath);
        }
        private bool AddNewStudent()
        {
            string fName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower());

            if (fName == string.Empty)
            {
                MessageBox.Show("Must Enter a First Name");
                txtFirstName.BackColor = Color.Red;
                return false;
            }


            string lName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text.Trim().ToLower());

            if (lName.Length > 2)
            {
                if (lName[1] == '\'')
                {
                    Char.ToUpper(lName[2]);
                }
            }
            else if (lName == string.Empty)
            {
                MessageBox.Show("Must Enter a Last Name");
                txtLastName.BackColor = Color.Red;
                return false;
            }


            DateTime birthDate = dtpBirthDate.Value;
            if (birthDate.Date == DateTime.Now.Date)
            {
                DialogResult dr = MessageBox.Show("Is this student's birthdate unknown?", "Invalid Birthdate", MessageBoxButtons.YesNoCancel);
                if (dr != DialogResult.Yes)
                {
                    return false;
                }
            }

            bool gender = false;
            if (radMale.Checked == true || radFemale.Checked == true)
            {
                if (radMale.Checked == true)
                    gender = false;
                else if (radFemale.Checked == true)
                    gender = true;
            }
            else
            {
                MessageBox.Show("Gender Required.");
                return false;
            }

            string address = txtAddress.Text.Trim().ToUpper();
            //if (address.Length < 5)
            //{
            //    MessageBox.Show("The address you entered is invalid.\n\nIf you believe this message is an error, contact your Database Administrator.");
            //    txtAddress.BackColor = Color.Red;
            //    return false;
            //}

            //Propercase city
            string city = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtCity.Text.Trim().ToLower());
            if (city.Length > 0)
            {
                Char.ToUpper(city[0]);
            }
            //if (city.Length < 3)
            //{
            //    MessageBox.Show("City name too short.");
            //    txtCity.BackColor = Color.Red;
            //    return false;
            //}

            string state;
            if (cboState.SelectedIndex <= 0)
            {
                state = "NULL";
            }
            state = (string)cboState.SelectedItem;

            int zip = 0;
            if (txtZip.Text != String.Empty)
            {
                if (!int.TryParse(txtZip.Text, out zip))
                {
                    MessageBox.Show("Verify the zip code is numeric.");
                    txtZip.BackColor = Color.Red;
                    return false;
                }
                else if (txtZip.Text.Length != 5)
                {
                    MessageBox.Show("Check that the zip code entere is 5 digits long.");
                    txtZip.BackColor = Color.Red;
                    return false;
                }
            }

            string email = txtPrimaryEmail.Text.Trim().ToLower();
            if (email == string.Empty)
            {
                MessageBox.Show("Primary email required");
                txtPrimaryEmail.BackColor = Color.Red;
                return false;
            }
            // verify format with regular expression
            string rtcEmail = txtRTCEmail.Text.Trim().ToLower();
            // verify format with regular expression

            string rtcSID = txtRTCSID.Text;
            if (rtcSID != string.Empty)
            {
                if (rtcSID.Length != 9)
                {
                    MessageBox.Show("The RTC Student ID is 9 digits.");
                    txtRTCSID.BackColor = Color.Red;
                    return false;
                }
                // check SID is numeric
                double testSID;
                if (!double.TryParse(rtcSID, out testSID))
                {
                    MessageBox.Show("RTC Student ID must be digits only.");
                    txtRTCSID.BackColor = Color.Red;
                    return false;
                }
            }

            string phoneNo = txtPhoneNo.Text;
            long testPhone;
            if (phoneNo != string.Empty)
            {
                if (!long.TryParse(phoneNo, out testPhone))
                {
                    MessageBox.Show("Please verify the phone number is correct\nDo not include any symbols, only numbers.");
                    txtPhoneNo.BackColor = Color.Red;
                    return false;
                }
                if (phoneNo.Length != 10)
                {
                    MessageBox.Show("Phone number must be 10 digits");
                    txtPhoneNo.BackColor = Color.Red;
                    return false;
                }
            }

            string notes = rtbStudentNotes.Text;
            string folderPath = txtAddStudent_FolderPath.Text;
            if (folderPath == string.Empty)
            {
                DialogResult dr = MessageBox.Show("This student does not have a path to their documents. Are you sure you want to update this student anyway?", "No path provided", MessageBoxButtons.YesNo);
                if (dr != DialogResult.Yes)
                {
                    return false;
                }
            }

            return dbc.AddStudent(rtcSID, fName, lName, phoneNo, rtcEmail, email, address, city, state, zip.ToString(), birthDate, gender, notes, folderPath);
        }
        // EVENTS
        private void btnGetStudentInfoFilePath_Click(object sender, EventArgs e)
        {
            string folderPath = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                folderPath = fbd.SelectedPath;
            }
            txtAddStudent_FolderPath.Text = folderPath;
        }
        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            string selectedStudent = (string)cboStudentLookup.SelectedItem;
            int studentID;
            int.TryParse(selectedStudent.Substring(0, selectedStudent.IndexOf(" ")), out studentID);
            string firstName, lastName;
            if (studentID == 0)
            {
                if (AddNewStudent())
                {
                    // clear student lookup items and repopulate
                    // comboboxes with updated info
                    cboStudentLookup.Items.Clear();
                    cboStudentLookup2.Items.Clear();

                    cboStudentLookup.Items.Add("New Student");
                    LoadStudentsComboBox(cboStudentLookup);
                    cboStudentLookup2.Items.Add("Select Student");
                    LoadStudentsComboBox(cboStudentLookup2);

                    MessageBox.Show("Success");
                    // GO TO NEXT SCREEN
                    // get student ID from whomever was just added
                    // to display their info on the next screen

                    int currentStudent = dbc.GetLastStudent(studentID, out firstName, out lastName);
                    if (currentStudent != -1)
                    {
                        cboStudentLookup.SelectedIndex = cboStudentLookup.Items.Count - 1;

                        // Add Student was successful, change screens
                        pnlAddStudent.Hide();
                        pnlAddStudentDetails.Show();

                        // set up next screen's visible data
                        txtCurrentStudentID.Text = currentStudent.ToString();
                        txtFirstName_Details.Text = firstName;
                        txtLastName_Details.Text = lastName;

                        //enable previous button and next
                        btnAddStudent_Previous.Enabled = true;
                        btnAddStudent_Next.Enabled = true;

                        // set page number
                        lblAddStudent_CurrentPanel.Text = "2";
                    }
                    else
                        MessageBox.Show("There was an error handling your request.");
                }
            }
            else
            {
                // update student with ID studentID
                UpdateExistingStudent(studentID);
                int currentStudent = dbc.GetLastStudent(studentID, out firstName, out lastName);

                if (currentStudent != -1)
                {
                    // Add Student was successful, change screens
                    pnlAddStudent.Hide();
                    pnlAddStudentDetails.Show();

                    //enable previous button and next
                    btnAddStudent_Previous.Enabled = true;
                    btnAddStudent_Next.Enabled = true;

                    // set page number
                    lblAddStudent_CurrentPanel.Text = "2";

                    // set up next screen's visible data
                    txtCurrentStudentID.Text = studentID.ToString();
                    txtFirstName_Details.Text = firstName;
                    txtLastName_Details.Text = lastName;

                    // prepare next screen with cureent students info
                    SetUpCurrentStudentDetails(studentID);
                }
                else
                    MessageBox.Show("There was an error handling your request.");
            }

        }
        private void cboStudentLookup_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the datareader of the selected student
            // studentID is the first character in the combo box
            string selectedStudent = (string)cboStudentLookup.SelectedItem;
            int studentID;
            int.TryParse(selectedStudent.Substring(0, selectedStudent.IndexOf(" ")), out studentID);
            if (studentID != 0)
            {
                SqlDataReader sdr = dbc.GetStudentByID(studentID);
                // populate students data
                PopulateStudentInfo(sdr);
                sdr.Close();
                btnAddStudent_Next.Enabled = true;
            }
            else
            {
                ClearAddStudentForm();
                ClearAddStudentDetailForm();
                ClearAddAdditionalDetailsForm();
                btnAddStudent_Next.Enabled = false;
            }
        }

        private void ClearAddAdditionalDetailsForm()
        {
            ClearTechInterests();
            ClearPreviousColleges();
            ClearEmploymentStatus();
        }
        private void txtRTCSID_TextChanged(object sender, EventArgs e)
        {
            if (txtRTCSID.Text.Contains("."))
            {
                int index = txtRTCSID.Text.IndexOf('.');
                txtRTCSID.Text = txtRTCSID.Text.Remove(index, 1);
                txtRTCSID.SelectionStart = txtRTCSID.Text.Length;
            }
        }
        #endregion
        // Methods

        // Events
        private void btnAddStudent_Next_Click(object sender, EventArgs e)
        {
            if (cboStudentLookup.SelectedIndex != 0)
            {
                // get current student
                string selectedStudent = (string)cboStudentLookup.SelectedItem;
                string firstName, lastName;
                int studentID;
                int.TryParse(selectedStudent.Substring(0, selectedStudent.IndexOf(" ")), out studentID);
                int currentStudent = dbc.GetLastStudent(studentID, out firstName, out lastName);

                if (pnlAddStudent.Visible == true)
                {
                    // set up next screen's visible data
                    txtCurrentStudentID.Text = studentID.ToString();
                    txtFirstName_Details.Text = firstName;
                    txtLastName_Details.Text = lastName;
                    SetUpCurrentStudentDetails(studentID);

                    // Change screens
                    pnlAddStudent.Hide();
                    pnlAddStudentDetails.Show();

                    //enable previous button and next
                    btnAddStudent_Previous.Enabled = true;
                    btnAddStudent_Next.Enabled = true;

                    // set page number
                    lblAddStudent_CurrentPanel.Text = "2";
                }
                else if (pnlAddStudentDetails.Visible == true)
                {
                    // set up next screen's visible data
                    txtID_ExtraDetails.Text = studentID.ToString();
                    txtFirstName_ExtraDetails.Text = firstName;
                    txtLastName_ExtraDetails.Text = lastName;

                    // Change screens
                    pnlAddStudentDetails.Hide();
                    pnlAdditionalDetails.Show();

                    // disable next button
                    btnAddStudent_Next.Enabled = false;
                    btnAddStudent_Previous.Enabled = true;

                    // set page number
                    lblAddStudent_CurrentPanel.Text = "3";
                }
            }
        }
        private void btnAddStudent_Previous_Click(object sender, EventArgs e)
        {
            if (cboStudentLookup.SelectedIndex != 0)
            {
                // get current student
                string selectedStudent = (string)cboStudentLookup.SelectedItem;
                string firstName, lastName;
                int studentID;
                int.TryParse(selectedStudent.Substring(0, selectedStudent.IndexOf(" ")), out studentID);
                int currentStudent = dbc.GetLastStudent(studentID, out firstName, out lastName);

                if (pnlAddStudentDetails.Visible == true)
                {
                    // Change screens
                    pnlAddStudentDetails.Hide();
                    pnlAddStudent.Show();

                    //disable previous button enable next
                    btnAddStudent_Previous.Enabled = false;
                    btnAddStudent_Next.Enabled = true;

                    // set page number
                    lblAddStudent_CurrentPanel.Text = "1";
                }
                else if (pnlAdditionalDetails.Visible == true)
                {
                    SetUpCurrentStudentDetails(studentID);

                    // Change screens
                    pnlAdditionalDetails.Hide();
                    pnlAddStudentDetails.Show();

                    // set up next screen's visible data
                    txtID_ExtraDetails.Text = studentID.ToString();
                    txtFirstName_ExtraDetails.Text = firstName;
                    txtLastName_ExtraDetails.Text = lastName;

                    // enable both buttons
                    btnAddStudent_Previous.Enabled = true;
                    btnAddStudent_Next.Enabled = true;

                    // set page number
                    lblAddStudent_CurrentPanel.Text = "2";
                }
            }
        }
        #endregion

        #region Add Course Tab
        // METHODS
        private void LoadCourseView()
        {
            lsvCourseView.Items.Clear();
            ListViewItem lvi = null;
            try
            {
                // get the sql reader for all courses
                SqlDataReader sdr = dbc.GetAllCourses();
                // push the data into a listviewitem

                while (sdr.Read())
                {
                    lvi = new ListViewItem(((int)sdr["CourseID"]).ToString());
                    lvi.SubItems.Add((string)sdr["CourseNumber"]);
                    lvi.SubItems.Add((string)sdr["CourseItemNumber"]);
                    lvi.SubItems.Add((string)sdr["CreditSection"]);
                    lvi.SubItems.Add((string)sdr["CourseName"]);
                    lvi.SubItems.Add(sdr["CourseDescription"].ToString());
                    lvi.SubItems.Add(((int)sdr["Credits"]).ToString());
                    lvi.SubItems.Add(((bool)sdr["RequiredForBASAdmission"]).ToString());
                    lvi.SubItems.Add(((bool)sdr["RequiredForBASCompletion"]).ToString());

                    // display the listviewitem to the list view
                    lsvCourseView.Items.Add(lvi);
                }
                sdr.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Type: " + ex.GetType() + "\nError Message:" + ex.Message);
            }
        }
        // EVENTS
        private void btnAddCourse_Click(object sender, EventArgs e)
        {
            //public bool AddCourse(string courseID, string courseName, string courseDescription, int credits, bool requiredBASAdmission, bool requiredBASCompletion)
            string courseNumber = txtCourseNumber.Text.Trim();
            string itemNumber = txtItemNumber.Text.Trim();
            string courseName = txtCourseName.Text.Trim();
            int credits = (int)nudCredits.Value;
            string description = rtbCourseDescription.Text.Trim();
            int type = (int)cboCourseType.SelectedValue;
            int section = (int)cboCourseCreditSection.SelectedValue;
            bool requiredForAdmission = chkBASAdmission.Checked;
            bool requiredForCompletion = chkBASCompletion.Checked;

            if (txtCourseID.Text == string.Empty)
            {
                if (type > 0 && section > 0 && courseNumber != string.Empty && courseName != string.Empty && credits >= 0 && itemNumber != string.Empty)
                {
                    if (dbc.AddCourse(courseNumber, itemNumber, courseName, type, description, credits, requiredForAdmission, requiredForCompletion, section))
                    {
                        MessageBox.Show("Success");
                    }
                    else
                        MessageBox.Show("Cannot add a Course with a duplicate Course Number or Item Number.");
                }
                else
                {
                    MessageBox.Show("Missing data, please check the inputs and try again.");
                }
            }
            else
            {
                int courseID = int.Parse(txtCourseID.Text);
                DialogResult dr = MessageBox.Show("You are about to modify an existing course!\nAre you sure you want to save these changes?", "Save Changes?", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    if (dbc.ModifyCourse(courseID, courseNumber, itemNumber, courseName, type, description, credits, requiredForAdmission, requiredForCompletion, section))
                    {
                        MessageBox.Show("Success");
                    }
                    else
                        MessageBox.Show("Failure");
                }
            }

            // update the course view
            LoadCourseView();
            // clear the add course form


        }
        private void btnViewCourses_Click(object sender, EventArgs e)
        {
            if (pnlAddCourse.Visible)
            {
                pnlAddCourse.Hide();
                pnlViewCourses.Show();
            }
            else if (pnlViewCourses.Visible)
            {
                pnlViewCourses.Hide();
                pnlAddCourse.Show();
            }
        }
        private void chkBASAdmission_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBASAdmission.Checked)
            {
                chkBASCompletion.Checked = true;
            }
        }
        private void lsvCourseView_DoubleClick(object sender, EventArgs e)
        {
            if (lsvCourseView.SelectedIndices.Count != 0)
            {
                // get the id for the course they selected
                int courseID = int.Parse(lsvCourseView.Items[lsvCourseView.SelectedIndices[0]].Text);
                // run a query on that ID
                SqlDataReader sdr = dbc.GetCourseData(courseID);
                // swap panels from viewCourse to addCourse
                pnlViewCourses.Hide();
                pnlAddCourse.Show();
                // populate Course Data to allow for modification
                while (sdr.Read())
                {
                    cboCourseType.SelectedIndex = ((int)sdr["CourseTypeID"]);
                    cboCourseCreditSection.SelectedIndex = ((int)sdr["CreditSection"]);
                    txtItemNumber.Text = (string)sdr["CourseItemNumber"];
                    txtCourseID.Text = ((int)sdr["CourseID"]).ToString();
                    txtCourseNumber.Text = (string)sdr["CourseNumber"];
                    txtCourseName.Text = (string)sdr["CourseName"];
                    rtbCourseDescription.Text = sdr["CourseDescription"].ToString();
                    nudCredits.Value = (decimal)(int)sdr["Credits"];
                    chkBASAdmission.Checked = (bool)sdr["RequiredForBASAdmission"];
                    chkBASCompletion.Checked = (bool)sdr["RequiredForBASCompletion"];
                }
                sdr.Close();
            }
        }
        private void btnClearAddCourse_Click(object sender, EventArgs e)
        {
            cboCourseCreditSection.SelectedIndex = 0;
            cboCourseType.SelectedIndex = 0;
            txtItemNumber.Clear();
            txtCourseID.Clear();
            txtCourseNumber.Clear();
            txtCourseName.Clear();
            rtbCourseDescription.Clear();
            txtItemNumber.Clear();
            nudCredits.Value = 7;
            chkBASAdmission.Checked = false;
            chkBASCompletion.Checked = false;
        }
        private void btnDeleteCourse_Click(object sender, EventArgs e)
        {
            if (lsvCourseView.SelectedIndices.Count != 0)
            {
                // get selected index
                int selectedCourse = lsvCourseView.SelectedIndices[0];
                // get the course id for selected course
                int courseID = int.Parse(lsvCourseView.Items[selectedCourse].Text);
                // delete from courses where course id matches
                DialogResult dr = MessageBox.Show("Are you sure you want to permanently remove this course?", "Permanently Delete?", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    int retval = dbc.RemoveCourse(courseID);
                    if (retval > 0)
                    {
                        LoadCourseView();
                    }
                    else
                        MessageBox.Show("Unable to remove the selected course because one or more students have an associated record.\n\nContact your database administrator for more details.");

                }
            }
        }
        #endregion

        #region View Students Tab
        // METHODS
        private void DisplayStudentsListView(SqlDataReader sdr)
        {
            lsvStudentView.Items.Clear();
            ListViewItem lvi;
            try
            {
                while (sdr.Read())
                {
                    lvi = new ListViewItem(sdr["StudentID"].ToString());

                    if (sdr["RTCStudentID"] != DBNull.Value)
                        lvi.SubItems.Add(sdr["RTCStudentID"].ToString());
                    else
                        lvi.SubItems.Add("");

                    lvi.SubItems.Add((string)sdr["FirstName"]);
                    lvi.SubItems.Add((string)sdr["LastName"]);

                    if (sdr["Phone"] != DBNull.Value)
                        lvi.SubItems.Add(sdr["Phone"].ToString());
                    else
                        lvi.SubItems.Add("");

                    if (sdr["StudentEmail"] != DBNull.Value)
                        lvi.SubItems.Add((string)sdr["StudentEmail"]);
                    else
                        lvi.SubItems.Add("");


                    lvi.SubItems.Add((string)sdr["PersonalEmail"]);

                    if (sdr["Address"] != DBNull.Value)
                        lvi.SubItems.Add((string)sdr["Address"]);
                    else
                        lvi.SubItems.Add("");

                    if (sdr["City"] != DBNull.Value)
                        lvi.SubItems.Add((string)sdr["City"]);
                    else
                        lvi.SubItems.Add("");

                    if (sdr["State"] != DBNull.Value)
                        lvi.SubItems.Add((string)sdr["State"]);
                    else
                        lvi.SubItems.Add("");

                    if (sdr["Zip"] != DBNull.Value)
                        lvi.SubItems.Add((string)sdr["Zip"]);
                    else
                        lvi.SubItems.Add("");

                    if (sdr["BirthDate"] != DBNull.Value)
                        lvi.SubItems.Add(((DateTime)sdr["BirthDate"]).ToShortDateString());
                    else
                        lvi.SubItems.Add("");


                    string gender = sdr["Gender"].ToString();
                    if (gender == "False")
                        gender = "Male";
                    else
                        gender = "Female";
                    lvi.SubItems.Add(gender);

                    if (sdr["Notes"] != DBNull.Value)
                        lvi.SubItems.Add((string)sdr["Notes"]);
                    else
                        lvi.SubItems.Add("");


                    if (sdr["StudentDocumentsLocation"] != DBNull.Value)
                        lvi.SubItems.Add((string)sdr["StudentDocumentsLocation"]);
                    else
                        lvi.SubItems.Add("");

                    lsvStudentView.Items.Add(lvi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.GetType());
            }

        }
        private void LoadStudentView()
        {
            try
            {
                SqlDataReader sdr;
                sdr = dbc.GetAllStudents("StudentID", false);
                DisplayStudentsListView(sdr);
                sdr.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Error loading students");
            }
        }
        private void LoadStudentView(dsStudentManager dsStudents)
        {
            lsvStudentView.Items.Clear();
            ListViewItem lvi = null;
            for (int i = 0; i < dsStudents.Students.Rows.Count; i++)
            {
                // added in order...
                // 1
                lvi = new ListViewItem(dsStudents.Students[i].StudentID.ToString());
                // 2
                if (dsStudents.Students[i].IsRTCStudentIDNull())
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(dsStudents.Students[i].RTCStudentID.ToString());
                // 3
                lvi.SubItems.Add(dsStudents.Students[i].FirstName);
                // 4
                lvi.SubItems.Add(dsStudents.Students[i].LastName);
                // 5
                if (dsStudents.Students[i].IsPhoneNull())
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(dsStudents.Students[i].Phone);
                // 6
                if (dsStudents.Students[i].IsStudentEmailNull())
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(dsStudents.Students[i].StudentEmail);
                // 7
                lvi.SubItems.Add(dsStudents.Students[i].PersonalEmail);
                // 8 
                if (dsStudents.Students[i].IsAddressNull())
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(dsStudents.Students[i].Address);

                // 9
                if (dsStudents.Students[i].IsCityNull())
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(dsStudents.Students[i].City);

                // 10
                if (dsStudents.Students[i].IsStateNull())
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(dsStudents.Students[i].State);

                // 11
                if (dsStudents.Students[i].IsZipNull())
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(dsStudents.Students[i].Zip);

                // 12
                if (dsStudents.Students[i].IsBirthDateNull())
                    lvi.SubItems.Add("");
                else
                {
                    DateTime bdate = (DateTime)dsStudents.Students[i].BirthDate;
                    lvi.SubItems.Add(bdate.ToShortDateString());
                }
                // 13
                if (dsStudents.Students[i].Gender)
                    lvi.SubItems.Add("Female");
                else
                    lvi.SubItems.Add("Male");

                // 14
                if (dsStudents.Students[i].IsNotesNull())
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(dsStudents.Students[i].Notes);

                // 15
                if (dsStudents.Students[i].IsStudentDocumentsLocationNull())
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(dsStudents.Students[i].StudentDocumentsLocation);

                lsvStudentView.Items.Add(lvi);
            }
        }

        private void DisplayStudentsListViewForStudentsWhoHaveNotFinished_AASCoursesForYear1(int requiredCouresCount)
        {
            // this query selects students who have not completed the required courses for the Application Developer certificate (CreditSection 1).
            var query =
                from student in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                where !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                        join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                        where ce.EnrollmentStatus == 3 && c.CreditSection == 1
                        group s2 by s2.StudentID into s3
                        where s3.Count() >= requiredCouresCount
                        select s3.Key).Contains<int>(student.StudentID)
                select student;

            LoadStudentView(query);
        }
        private void DisplayStudentsListViewForStudentsWhoHaveNotFinished_AASCoursesForYear2(int requiredCourseCount)
        {
            // this query selects students who have not completed the required courses for the Application Developer certificate (CreditSection 2).
            var query =
                from student in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                where !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                        join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                        where ce.EnrollmentStatus == 3 && c.CreditSection == 2
                        group s2 by s2.StudentID into s3
                        where s3.Count() >= requiredCourseCount
                        select s3.Key).Contains<int>(student.StudentID)
                select student;

            LoadStudentView(query);
        }
        private void DisplayStudentsListViewForStudentsWhoHaveNotFinished_GenEdCoursesForAASDegree(int requiredCourses, int requiredEnglishCourses)
        {
            // this query selects students who have not completed the required Gen Ed courses for the Application Developer certificate (CreditSection 3 - 3 coures required).
            //                                                                                                                          (CreditSection 4 - 1 of 2 courses required).    
            var query =
                from student in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                where !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                        join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                        where ce.EnrollmentStatus == 3 && c.CreditSection == 3
                        group s2 by s2.StudentID into s3
                        where s3.Count() >= requiredCourses
                        select s3.Key).Contains<int>(student.StudentID)
                &&
                !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                  join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                  join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                  where ce.EnrollmentStatus == 3 && c.CreditSection == 4
                  group s2 by s2.StudentID into s3
                  where s3.Count() >= requiredEnglishCourses
                  select s3.Key).Contains<int>(student.StudentID)
                select student;

            LoadStudentView(query);
        }
        private void DisplayStudentsListViewForStudentsWhoHaveNotFinished_GenEdCoursesForAASTDegree(int requiredMathCourses, int requiredSocialScienceCourses, int requiredHumanitiesCourses)
        {
            var query =
                from student in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                where !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                        join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                        where ce.EnrollmentStatus == 3 && c.CreditSection == 5
                        group s2 by s2.StudentID into s3
                        where s3.Count() >= requiredMathCourses
                        select s3.Key).Contains<int>(student.StudentID)
                &&
                !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                  join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                  join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                  where ce.EnrollmentStatus == 3 && c.CreditSection == 6
                  group s2 by s2.StudentID into s3
                  where s3.Count() >= requiredSocialScienceCourses
                  select s3.Key).Contains<int>(student.StudentID)
                &&
                !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                  join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                  join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                  where ce.EnrollmentStatus == 3 && c.CreditSection == 7
                  group s2 by s2.StudentID into s3
                  where s3.Count() >= requiredHumanitiesCourses
                  select s3.Key).Contains<int>(student.StudentID)
                select student;

            LoadStudentView(query);
        }
        private void DisplayStudentsListViewForStudentsWhoHaveNotFinished_BASCoursesForYear1(int requiredCourseCount)
        {
            var query =
                from student in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                where !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                        join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                        where ce.EnrollmentStatus == 3 && c.CreditSection == 8
                        group s2 by s2.StudentID into s3
                        where s3.Count() >= requiredCourseCount
                        select s3.Key).Contains<int>(student.StudentID)
                select student;

            LoadStudentView(query);
        }
        private void DisplayStudentsListViewForStudentsWhoHaveNotFinished_BASCoursesForYear2(int requiredCourseCount)
        {
            var query =
                from student in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                where !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                        join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                        where ce.EnrollmentStatus == 3 && (c.CreditSection == 9 || c.CreditSection == 10)
                        group s2 by s2.StudentID into s3
                        where s3.Count() >= requiredCourseCount
                        select s3.Key).Contains<int>(student.StudentID)
                select student;

            LoadStudentView(query);
        }
        private void DisplayStudentsListViewForStudentsWhoHaveNotFinished_GenEdCoursesForBASDegree(int requiredCourseCount)
        {
            var query =
                from student in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                where !(from s2 in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        join ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>() on student.StudentID equals ce.StudentID
                        join c in datStudents.Courses.AsEnumerable<dsStudentManager.CoursesRow>() on ce.CourseID equals c.CourseID
                        where ce.EnrollmentStatus == 3 && c.CreditSection == 11
                        group s2 by s2.StudentID into s3
                        where s3.Count() >= requiredCourseCount
                        select s3.Key).Contains<int>(student.StudentID)
                select student;

            LoadStudentView(query);
        }

        private void LoadStudentView(EnumerableRowCollection<StudentManager.dsStudentManager.StudentsRow> linq_query)
        {
            lsvStudentView.Items.Clear();
            ListViewItem lvi = null;
            foreach (var student in linq_query)
            {
                lvi = new ListViewItem(student.StudentID.ToString());
                lvi.SubItems.Add(student.RTCStudentID.ToString());
                lvi.SubItems.Add(student.FirstName.ToString());
                lvi.SubItems.Add(student.LastName.ToString());
                lvi.SubItems.Add(student.Phone.ToString());
                lvi.SubItems.Add(student.StudentEmail.ToString());
                lvi.SubItems.Add(student.PersonalEmail.ToString());
                lvi.SubItems.Add(student.Address.ToString());
                lvi.SubItems.Add(student.City.ToString());
                lvi.SubItems.Add(student.State.ToString());
                lvi.SubItems.Add(student.Zip.ToString());
                lvi.SubItems.Add(student.BirthDate.ToString());
                if (student.Gender)
                    lvi.SubItems.Add("Female");
                else
                    lvi.SubItems.Add("Male");
                lvi.SubItems.Add(student.Notes.ToString());
                lvi.SubItems.Add(student.StudentDocumentsLocation.ToString());

                lsvStudentView.Items.Add(lvi);
            }
        }
        // EVENTS
        private void cboStudentViewQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            // hide all query panels and only show the one needed in the chosen case
            pnlDynamicQuery01.Hide();
            pnlDynamicQuery02.Hide();
            pnlDynamicQuery03.Hide();
            // ensure that the student view is rest to the proper heigth
            lsvStudentView.Height = 435;

            switch (cboStudentViewQuery.SelectedIndex)
            {
                case 0:
                    // has not completed given course
                    pnlDynamicQuery01.Show();
                    break;
                case 1:
                    // graduated
                    pnlDynamicQuery02.Show();
                    break;
                case 2:
                    DisplayStudentsListViewForStudentsWhoHaveNotFinished_AASCoursesForYear1(12);
                    break;
                case 3:
                    DisplayStudentsListViewForStudentsWhoHaveNotFinished_AASCoursesForYear2(10);
                    break;
                case 4:
                    DisplayStudentsListViewForStudentsWhoHaveNotFinished_GenEdCoursesForAASDegree(3, 1);
                    break;
                case 5:
                    DisplayStudentsListViewForStudentsWhoHaveNotFinished_GenEdCoursesForAASTDegree(1, 1, 1);
                    break;
                case 6:
                    DisplayStudentsListViewForStudentsWhoHaveNotFinished_BASCoursesForYear1(6);
                    break;
                case 7:
                    DisplayStudentsListViewForStudentsWhoHaveNotFinished_BASCoursesForYear2(6);
                    break;
                case 8:
                    DisplayStudentsListViewForStudentsWhoHaveNotFinished_GenEdCoursesForBASDegree(6);
                    break;
                case 9:
                    // have notes field containing:
                    pnlDynamicQuery03.Show();
                    break;
                default:
                    break;
            }
        }
        private void btnViewAllStudents_Click(object sender, EventArgs e)
        {
            cboStudentViewQuery.SelectedIndex = -1;
            lsvStudentView.Height = 435;
            lsvStudentView.Show();
            LoadStudentView(datStudents);
        }

        private void cboDynamicQuery01CourseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDynamicQuery01CourseList.SelectedIndex != 0)
            {
                // get the selected course id and display 
                // students who have not completed the course
                int courseID = int.Parse(cboDynamicQuery01CourseList.SelectedValue.ToString());

                // query selects all students from the students table where their id is not in the sub query
                // that selects StudentID from course enrollment where course id is the given course id and enrollment status is 'completed'
                // the result is a list of students who have not completed the given course.

                var query =
                    from student in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                    where !(from ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>()
                            where ce.CourseID == courseID && ce.EnrollmentStatus == 3
                            select ce.Field<int>("StudentID")).Contains<int>(student.StudentID)
                    select student;

                LoadStudentView(query);
            }
        }
        private void radYear_CheckedChanged(object sender, EventArgs e)
        {
            if (radSingleYear.Checked)
            {
                cboYearEnd.Enabled = false;
            }
            else if (radBetweenYears.Checked)
            {
                cboYearEnd.Enabled = true;
            }
        }
        private void btnViewGraduates_Click(object sender, EventArgs e)
        {
            if (radBetweenYears.Checked && cboYearEnd.SelectedIndex != -1 && cboYearStart.SelectedIndex != -1)
            {
                // read both years and parse them.
                int yearStart = int.Parse(cboYearStart.SelectedItem.ToString());
                int yearEnd = int.Parse(cboYearEnd.SelectedItem.ToString());

                DisplayStudentsListViewForStudentsWhoHaveGraduatedBetweenYears(yearStart, yearEnd);
            }
            else if (radSingleYear.Checked && cboYearStart.SelectedIndex != -1)
            {
                int yearStart = int.Parse(cboYearStart.SelectedItem.ToString());
                DisplayStudentsListViewForStudentsWhoHaveGraduatedBetweenYears(yearStart, yearStart);
            }
        }

        private void DisplayStudentsListViewForStudentsWhoHaveGraduatedBetweenYears(int yearStart, int yearEnd)
        {
            if (yearStart > yearEnd)
            {
                int temp = yearStart;
                yearStart = yearEnd;
                yearEnd = temp;
            }
            try
            {
                SqlDataReader sdr = dbc.ReturnBASGraduates(yearStart, yearEnd);
                DisplayStudentsListView(sdr);
                sdr.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Course Enrollment Tab
        // METHODS
        private void ResetEnrollmentControls()
        {
            txtCourseEnrollment_DetailID.Clear();
            txtCourseEnrollment_CourseName.Clear();
            cboCourseEnrollment_CourseNumber.SelectedIndex = 0;
            cboCourseEnrollment_CourseNumber.Enabled = true;
            cboCourseEnrollment_EnrollmentStatus.SelectedIndex = 0;
            dtpCourseEnrollment_EnrollmentDate.Value = DateTime.Now;
            rtbCourseEnrollment_Notes.Clear();
            chkCourseEnrollment_ConditionalAdmission.Checked = false;
            chkCourseEnrollment_EquivalentCompletion.Checked = false;
        }
        private void DisplayCourseEnrollmentDetails(int studentID)
        {
            string fName, lName;
            // studentID is the first characters before the first space in the combo box

            if (studentID != 0)
            {
                studentID = dbc.GetLastStudent(studentID, out fName, out lName);
                lblStudentIDCourseData.Text = "ID: " + studentID;
                lblStudentCourseData.Text = "Student: " + fName + " " + lName;

                // Get course enrollment data for selected student
                SqlDataReader sdr = dbc.GetStudentEnrollmentData(studentID);
                lsvEnrollmentData.Items.Clear();
                ListViewItem lvi = null;
                while (sdr.Read())
                {
                    lvi = new ListViewItem(sdr["EnrollmentID"].ToString());
                    lvi.SubItems.Add((string)sdr["CourseNumber"]);
                    lvi.SubItems.Add((string)sdr["CourseName"]);
                    lvi.SubItems.Add((string)sdr["CourseItemNumber"]);
                    lvi.SubItems.Add((string)sdr["EnrollmentStatus"]);
                    lvi.SubItems.Add(((DateTime)sdr["EnrollmentDate"]).ToShortDateString());
                    if (sdr["CompletionDate"] != DBNull.Value)
                        lvi.SubItems.Add(((DateTime)sdr["CompletionDate"]).ToShortDateString());
                    else
                        lvi.SubItems.Add("");

                    lvi.SubItems.Add(((bool)sdr["ConditionalAdmission"]).ToString());
                    lvi.SubItems.Add(((bool)sdr["TransferEquivalent"]).ToString());
                    lvi.SubItems.Add(sdr["Notes"].ToString());


                    lsvEnrollmentData.Items.Add(lvi);
                }
                sdr.Close();
            }
            else
            {
                lsvEnrollmentData.Items.Clear();
                lblStudentIDCourseData.Text = "ID: ";
                lblStudentCourseData.Text = "Student: ";
            }
        }
        // EVENTS
        private void cboCourseEnrollment_EnrollmentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCourseEnrollment_EnrollmentStatus.SelectedIndex == 3)
            {
                dtpCourseEnrollment_CompletionDate.Enabled = true;
            }
            else
                dtpCourseEnrollment_CompletionDate.Enabled = false;
        }
        private void btnCourseEnrollment_ClearForm_Click(object sender, EventArgs e)
        {
            ResetEnrollmentControls();
        }
        private void lsvEnrollmentData_DoubleClick(object sender, EventArgs e)
        {
            // get the id of the enrollment info selected
            string selectedItemText = lsvEnrollmentData.Items[lsvEnrollmentData.SelectedIndices[0]].Text;
            int detailID = int.Parse(selectedItemText);
            // pull enrollment data for that ID
            string courseNumber = lsvEnrollmentData.SelectedItems[0].SubItems[1].Text;

            string name = lsvEnrollmentData.SelectedItems[0].SubItems[3].Text;
            string status = lsvEnrollmentData.SelectedItems[0].SubItems[4].Text;
            string enrollmentDate = lsvEnrollmentData.SelectedItems[0].SubItems[5].Text;
            string completionDate = lsvEnrollmentData.SelectedItems[0].SubItems[6].Text;
            string conditional = lsvEnrollmentData.SelectedItems[0].SubItems[7].Text;
            string equivalent = lsvEnrollmentData.SelectedItems[0].SubItems[8].Text;
            string notes = lsvEnrollmentData.SelectedItems[0].SubItems[9].Text;

            // populate enrollment data fields
            txtCourseEnrollment_DetailID.Text = detailID.ToString();
            txtCourseEnrollment_CourseName.Text = name;
            cboCourseEnrollment_CourseNumber.SelectedIndex = cboCourseEnrollment_CourseNumber.FindStringExact(courseNumber);
            cboCourseEnrollment_EnrollmentStatus.SelectedIndex = cboCourseEnrollment_EnrollmentStatus.FindStringExact(status);
            DateTime date = DateTime.Parse(enrollmentDate);
            if (completionDate != string.Empty)
            {
                DateTime dateCompleted = DateTime.Parse(completionDate);
                dtpCourseEnrollment_CompletionDate.Value = dateCompleted;
                dtpCourseEnrollment_CompletionDate.Enabled = true;
            }
            dtpCourseEnrollment_EnrollmentDate.Value = date;
            chkCourseEnrollment_ConditionalAdmission.Checked = Convert.ToBoolean(conditional);
            chkCourseEnrollment_EquivalentCompletion.Checked = Convert.ToBoolean(equivalent);
            rtbCourseEnrollment_Notes.Text = notes;

            // temporarily disable course number combo box
            cboCourseEnrollment_CourseNumber.Enabled = false;
            // hide enrollment data listview and show the enrollment data screen
            pnlCourseEnrollment.Show();
            pnlViewStudentEnrollment.Hide();
        }
        private void cboCourseEnrollment_CourseNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCourseEnrollment_CourseNumber.SelectedIndex > 0)
            {
                int courseID = (int)cboCourseEnrollment_CourseNumber.SelectedValue;

                SqlDataReader sdr = dbc.GetCourseData(courseID);
                while (sdr.Read())
                {
                    txtCourseEnrollment_CourseName.Text = (string)sdr["CourseName"];
                }
                sdr.Close();
            }
        }
        private void btnAddModify_CourseEnrollment_Click(object sender, EventArgs e)
        {
            // if a student is selected
            if (cboStudentLookup2.SelectedIndex > 0)
            {
                // get student ID
                string selectedItem = (string)cboStudentLookup2.SelectedItem;
                int studentID = int.Parse(selectedItem.Substring(0, selectedItem.IndexOf(" ")));
                // get Enrollment details
                int courseID = (int)cboCourseEnrollment_CourseNumber.SelectedValue;
                DateTime enrollmentDate = dtpCourseEnrollment_EnrollmentDate.Value;
                DateTime? completionDate = null;
                if (dtpCourseEnrollment_CompletionDate.Enabled == true)
                {
                    completionDate = dtpCourseEnrollment_CompletionDate.Value;
                }
                bool bAreDatesOkay = true;
                if (completionDate != null)
                {
                    int dateTest = DateTime.Compare((DateTime)completionDate, enrollmentDate);
                    if (dateTest != 1)
                    {
                        bAreDatesOkay = false;
                    }
                }

                int enrollmentStatus = cboCourseEnrollment_EnrollmentStatus.SelectedIndex;
                bool conditionalAdmission = chkCourseEnrollment_ConditionalAdmission.Checked;
                bool equivalentCompletion = chkCourseEnrollment_EquivalentCompletion.Checked;
                string notes = rtbCourseEnrollment_Notes.Text.Trim();


                if (enrollmentStatus > 0 && courseID > 0)
                {
                    if (bAreDatesOkay)
                    {
                        // insert a new record
                        if (txtCourseEnrollment_DetailID.Text == string.Empty)
                        {
                            // check if the student enrolled in this course or has completed it
                            if (dbc.TimesCompletedOrEnrolled(studentID, courseID) == 0)
                            {
                                dbc.AddCourseEnrollmentData(studentID, courseID, enrollmentStatus, enrollmentDate, completionDate, conditionalAdmission, equivalentCompletion, notes);
                                MessageBox.Show("Success");
                            }
                            else
                            {
                                MessageBox.Show("Unable to enroll student in duplicate course, or a course that they have completed");
                            }
                        }
                        else // update an existing
                        {
                            int enrollmentID = int.Parse(txtCourseEnrollment_DetailID.Text);
                            dbc.UpdateCourseEnrollmentData(studentID, enrollmentID, enrollmentStatus, enrollmentDate, completionDate, conditionalAdmission, equivalentCompletion, notes);
                            MessageBox.Show("Success");
                        }
                        // update listview to reflect DB Changes.
                        DisplayCourseEnrollmentDetails(studentID);
                    }
                    else
                        MessageBox.Show("Completion Date must occur after Enrollment Date");
                }
                else
                    MessageBox.Show("Ensure an Enrollment Status and CourseID have been selected");
            }

        }
        private void btnViewEnrollment_Click(object sender, EventArgs e)
        {
            if (pnlCourseEnrollment.Visible)
            {
                pnlCourseEnrollment.Hide();
                pnlViewStudentEnrollment.Show();
            }
            else if (pnlViewStudentEnrollment.Visible)
            {
                pnlViewStudentEnrollment.Hide();
                pnlCourseEnrollment.Show();
            }
        }
        private void cboStudentLookup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStudent = (string)cboStudentLookup2.SelectedItem;
            int studentID;
            int.TryParse(selectedStudent.Substring(0, selectedStudent.IndexOf(" ")), out studentID);
            // show students course enrollment info
            DisplayCourseEnrollmentDetails(studentID);
            // clear entry fields
            ResetEnrollmentControls();
        }

        #endregion

        // Load data for DynamicQuery Boxes
        bool tabIndex2_HasLoaded = false;
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    // View Students Tab is selected
                    if (!tabIndex2_HasLoaded)
                    {
                        // logic for loading DynamicQuery Panels with their details

                        // pnlDynamicQuery01 -> Fill Course List ComboBox
                        PopulateComboBoxWithKeyValuesFromCoursesAndCreditSectionLookUp("Course Number".PadRight(15) + "Course Name", "CourseID", cboDynamicQuery01CourseList, "CourseNumber", "CourseName");

                        // pnlDynamicQuery02 -> Fill Date ComboBoxes
                        for (int i = DateTime.Now.Year; i >= 2010; i--)
                        {
                            cboYearStart.Items.Add(i.ToString());
                            cboYearEnd.Items.Add(i.ToString());
                        }
                        tabIndex2_HasLoaded = true;

                        // pnlDynamicQuery03 -> Fill possible tables to search notes fields
                        Dictionary<string, string> comboboxSource = new Dictionary<string, string>();
                        comboboxSource.Add("--Search Table--", "");
                        comboboxSource.Add("Course Enrollment", "CourseEnrollment");
                        comboboxSource.Add("Student Details", "StudentDetails");
                        comboboxSource.Add("Students", "Students");

                        cboDynamicQuery03_TableChoice.DataSource = new BindingSource(comboboxSource, null);
                        cboDynamicQuery03_TableChoice.DisplayMember = "Key";
                        cboDynamicQuery03_TableChoice.ValueMember = "Value";
                        cboDynamicQuery03_TableChoice.SelectedIndex = 0;
                    }

                    // get the Students table in a dataset
                    datStudents = dbc.GetAllStudentsInDataSet();
                    // Preload Students View with that dataset            
                    LoadStudentView(datStudents);

                    break;
                case 4:
                    break;
                default:
                    break;
            }
        }

        // Sorts the listview sequentially in the reverse order from which it currently sits
        private void anyListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvComparer.SortColumn)
            {
                if (lvComparer.Order == System.Windows.Forms.SortOrder.Ascending)
                    lvComparer.Order = System.Windows.Forms.SortOrder.Descending;
                else
                    lvComparer.Order = System.Windows.Forms.SortOrder.Ascending;
            }
            else
            {
                lvComparer.SortColumn = e.Column;
                lvComparer.Order = System.Windows.Forms.SortOrder.Descending;
            }
            ListView lv = sender as ListView;

            if (lv.Name == lsvStudentView.Name)
                lsvStudentView.Sort();
            else if (lv.Name == lsvCourseView.Name)
                lsvCourseView.Sort();
            else if (lv.Name == lsvPreviousColleges.Name)
                lsvPreviousColleges.Sort();
            else if (lv.Name == lsvEnrollmentData.Name)
                lsvEnrollmentData.Sort();
            else if (lv.Name == lsvEmploymentStatus.Name)
                lsvEmploymentStatus.Sort();


        }

        /// Associate any TextBox with this event to change it's back color from red to white when the user enters the textbox.
        private void restoreTextBoxBackgroundColor_Enter(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox s = sender as System.Windows.Forms.TextBox;
            if (s.BackColor == Color.Red)
            {
                s.BackColor = Color.White;
            }
        }

        // Utility Methods
        private void ExportListViewToExcel(ListView lsv)
        {
            Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
            xla.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;
            int i = 2, j = 1;
            for (int k = 0; k < lsv.Columns.Count; k++)
            {
                ws.Cells[1, k + 1] = lsv.Columns[k].Text;
            }
            foreach (ListViewItem comp in lsv.Items)
            {
                ws.Cells[i, j] = comp.Text.ToString();

                foreach (ListViewItem.ListViewSubItem drv in comp.SubItems)
                {
                    ws.Cells[i, j] = drv.Text.ToString();
                    j++;
                }
                j = 1;
                i++;
            }
        }

        // nested class to sort by column in a list view
        public class ListViewItemComparer : IComparer
        {
            private int _ColumnToSort;
            private System.Windows.Forms.SortOrder OrderOfSort;
            private CaseInsensitiveComparer ObjectCompare;

            public int SortColumn
            {
                get { return _ColumnToSort; }
                set { _ColumnToSort = value; }
            }

            public System.Windows.Forms.SortOrder Order
            {
                get { return OrderOfSort; }
                set { OrderOfSort = value; }
            }

            public ListViewItemComparer()
            {
                _ColumnToSort = 0;
                OrderOfSort = System.Windows.Forms.SortOrder.None;
                ObjectCompare = new CaseInsensitiveComparer();
            }

            public ListViewItemComparer(int sortColumn, System.Windows.Forms.SortOrder sortOrder)
            {
                _ColumnToSort = sortColumn;
                OrderOfSort = sortOrder;
                ObjectCompare = new CaseInsensitiveComparer();
            }

            int IComparer.Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listViewX, listViewY;

                listViewX = (ListViewItem)x;
                listViewY = (ListViewItem)y;

                // try to parse a date from the data
                DateTime firstDate;
                DateTime secondDate;
                bool parseSuccess = DateTime.TryParse(listViewX.SubItems[_ColumnToSort].Text, out firstDate);
                bool parseSuccess2 = DateTime.TryParse(listViewY.SubItems[_ColumnToSort].Text, out secondDate);

                // try to parse a number from the data
                double firstNum;
                double secondNum;
                bool isParseFirstNumSuccess = double.TryParse(listViewX.SubItems[_ColumnToSort].Text, out firstNum);
                bool isParseSecondNumSuccess = double.TryParse(listViewY.SubItems[_ColumnToSort].Text, out secondNum);


                if (parseSuccess && parseSuccess2)
                {
                    compareResult = DateTime.Compare(firstDate, secondDate) * -1;
                }
                else if (isParseFirstNumSuccess && isParseSecondNumSuccess)
                {
                    if (firstNum > secondNum)
                        compareResult = 1;
                    else if (secondNum > firstNum)
                        compareResult = -1;
                    else
                        compareResult = 0;
                }
                else
                {
                    compareResult = ObjectCompare.Compare(listViewX.SubItems[_ColumnToSort].Text, listViewY.SubItems[_ColumnToSort].Text);
                }

                if (OrderOfSort == System.Windows.Forms.SortOrder.Ascending)
                {
                    return (-compareResult);
                }
                else if (OrderOfSort == System.Windows.Forms.SortOrder.Descending)
                {
                    return compareResult;
                }
                else
                    return 0;
            }
        }

        // exit the application
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // show application about form
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // pop up notification about product authors

            frmAbout abt = new StudentManager.frmAbout();
            abt.Show();

        }


        // FIND A HOME FOR THESE:
        private void btnFindNotesText_Click(object sender, EventArgs e)
        {
            int index = cboDynamicQuery03_TableChoice.SelectedIndex;
            string searchText = txtDynamicQuery03_Notes.Text.ToLower();

            // restore original layout and hide non-essential list views


            // which table is selected?
            switch (index)
            {
                case 0:
                    break;
                case 1: // course enrollment
                    var qry_NotesInCourseEnrollmentTable =
                        from stu in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        where (from ce in datStudents.CourseEnrollment.AsEnumerable<dsStudentManager.CourseEnrollmentRow>()
                               where !ce.IsNotesNull() && ce.Notes.ToLower().Contains(searchText)
                               select ce.StudentID).Contains<int>(stu.StudentID)
                        select stu;

                    LoadStudentView(qry_NotesInCourseEnrollmentTable);
                    // shrink the listview and add another to display
                    // a list view of course enrollment


                    break;
                case 2: // student details
                    var qry_NotesInStudentDetailsTable =
                        from stu in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        where (from sd in datStudents.StudentDetails.AsEnumerable<dsStudentManager.StudentDetailsRow>()
                               where !sd.IsNotesNull() && sd.Notes.ToLower().Contains(searchText)
                               select sd.StudentID).Contains<int>(stu.StudentID)
                        select stu;

                    LoadStudentView(qry_NotesInStudentDetailsTable);
                    // shrink the listview and add another to display
                    // a list view of student details


                    break;
                case 3: // students
                    var qry_NotesInStudentsTable =
                        from tbl in datStudents.Students.AsEnumerable<dsStudentManager.StudentsRow>()
                        where tbl.Notes.ToLower().Contains(searchText)
                        select tbl;

                    LoadStudentView(qry_NotesInStudentsTable);

                    break;
            }
        }

        private void lsvStudentView_DoubleClick(object sender, EventArgs e)
        {
            // pull selected student id
            int index = lsvStudentView.SelectedIndices[0];
            int studentID = int.Parse(lsvStudentView.Items[index].Text);

            if (cboDynamicQuery03_TableChoice.SelectedIndex == 2)
            {
                lsvStudentDetailsView.Items.Clear();
                // get the student details for the selected student
                Dictionary<string, object> paramsArr = new Dictionary<string, object>();
                paramsArr.Add("@StudentID", studentID);
                SqlDataReader sdr = dbc.ReturnRowsFromStoredProcedure("GetStudentDetails", paramsArr);
                ListViewItem lvi = null;
                while (sdr.Read())
                {
                    // load student details listview
                    lvi = new ListViewItem((string)sdr["EduBackground"]);
                    lvi.SubItems.Add((string)sdr["ReferralType"]);
                    lvi.SubItems.Add((string)sdr["ContactMethod"]);
                    lvi.SubItems.Add(((bool)sdr["AttendedInfoSession"]).ToString());
                    lvi.SubItems.Add(((bool)sdr["RunningStartParticipant"]).ToString());
                    lvi.SubItems.Add((string)sdr["StatusType"]);
                    lvi.SubItems.Add(((DateTime)sdr["LastUpdated"]).ToShortDateString());
                    lvi.SubItems.Add(sdr["Notes"].ToString());

                    lsvStudentDetailsView.Items.Add(lvi);
                }
                sdr.Close();
            }
            else if (cboDynamicQuery03_TableChoice.SelectedIndex == 1)
            {
                lsvEnrollmentDetailsView.Items.Clear();
                // get the student details for the selected student
                Dictionary<string, object> paramsArr = new Dictionary<string, object>();
                paramsArr.Add("@StudentID", studentID);
                SqlDataReader sdr = dbc.ReturnRowsFromStoredProcedure("GetStudentEnrollmentData", paramsArr);
                ListViewItem lvi = null;
                while (sdr.Read())
                {
                    lvi = new ListViewItem(sdr["EnrollmentID"].ToString());
                    lvi.SubItems.Add((string)sdr["CourseNumber"]);
                    lvi.SubItems.Add((string)sdr["CourseName"]);
                    lvi.SubItems.Add((string)sdr["CourseItemNumber"]);
                    lvi.SubItems.Add((string)sdr["EnrollmentStatus"]);
                    lvi.SubItems.Add(((DateTime)sdr["EnrollmentDate"]).ToShortDateString());

                    if (sdr["CompletionDate"] != DBNull.Value)
                        lvi.SubItems.Add(((DateTime)sdr["CompletionDate"]).ToShortDateString());
                    else
                        lvi.SubItems.Add("");

                    lvi.SubItems.Add(((bool)sdr["ConditionalAdmission"]).ToString());
                    lvi.SubItems.Add(((bool)sdr["TransferEquivalent"]).ToString());
                    lvi.SubItems.Add(sdr["Notes"].ToString());

                    lsvEnrollmentDetailsView.Items.Add(lvi);
                }
                sdr.Close();
            }
        }

        private void cboDynamicQuery03_TableChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            // set View Students interface to the default display
            lsvEnrollmentDetailsView.Hide();
            lsvStudentDetailsView.Hide();
            lsvStudentView.Height = 435;

            // change the interface layout based on the tables the user wants to query
            switch (cboDynamicQuery03_TableChoice.SelectedIndex)
            {
                case 0: // cbo display message selected
                    break;
                case 1: // EnrollmentStatus table selected
                    lsvStudentView.Height = 300;
                    lsvEnrollmentDetailsView.Show();
                    break;
                case 2: // StudentDetails table selected
                    lsvStudentView.Height = 300;
                    lsvStudentDetailsView.Show();
                    break;
                case 3: // Students table selected (keep defaults)
                    break;
                default:
                    break;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = item.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    ListView sourceControl = (ListView)owner.SourceControl;
                    ExportListViewToExcel(sourceControl);
                }
            }
        }

        private void btnQuickAdd_AddStudent_Click(object sender, EventArgs e)
        {
            string fname, lname, email;
            bool gender;

            fname = txtQuickAdd_FName.Text.Trim();
            lname = txtQuickAdd_LName.Text.Trim();
            email = txtQuickAdd_Email.Text.Trim();
            if (fname != string.Empty && lname != string.Empty && email != string.Empty)
            {
                if (radQuickAdd_Male.Checked || radQuickAdd_Female.Checked)
                {
                    gender = radQuickAdd_Female.Checked; // if female is checked gender is true (female) else it is false (male)

                    dbc.AddStudent(null, fname, lname, null, null, email, null, null, null, "0", DateTime.Now.Date, gender, null, null);

                    btnQuickAdd_AddStudent.BackColor = Color.Lime;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Interval = 3000;
                    t.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                    t.AutoReset = false;
                    t.Enabled = true;

                    // clear inputs
                    txtQuickAdd_Email.Clear();
                    txtQuickAdd_FName.Clear();
                    txtQuickAdd_LName.Clear();
                    radQuickAdd_Female.Checked = false;
                    radQuickAdd_Male.Checked = false;
                }
                else
                    MessageBox.Show("Select a gender to add the student.");
            }
            else
                MessageBox.Show("Adding a new student requires that you provide their first name, last name, email and gender.");
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            btnQuickAdd_AddStudent.BackColor = SystemColors.Control;
        }
    }
}
