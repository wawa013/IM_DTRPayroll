using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static IM_DTRPayroll.RegNewEmp;

namespace IM_DTRPayroll
{
    public partial class RegNewEmp : Form
    {
        private MySqlConnection connection;
        //Josh na DB, MAIN
        //private const string connectionString = "server=localhost;port=3306;username=root;password=masellones;database=db_finalproject";

        //Jireh na db
        private const string connectionString = "server=localhost;Port=3306;Database=db_im_finalproj;Uid=root;Pwd=;";

        int val = 1000;
        
        public RegNewEmp()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);

            txtbx_User.TextChanged += UpdateEmployeeIdAndUsername;
            textBox1.TextChanged += UpdateEmployeeIdAndUsername;
            textBox2.TextChanged += UpdateEmployeeIdAndUsername;
        }

        private void RegNewEmp_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                MessageBox.Show("Database connection successful!");
                PopulatePosComboBox();
                PopulateSchedComboBox();
                PopulateComboBox("SELECT Gender FROM gender", comboBox_Gender);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }

        public class Position
        {
            public int PositionID { get; set; }
            public string PositionTitle { get; set; }

            public Position(int positionID, string positionTitle)
            {
                PositionID = positionID;
                PositionTitle = positionTitle;
            }

            public override string ToString()
            {
                return PositionTitle; // Display PositionTitle in ComboBox
            }
        }

        public class Department
        {
            public int DepartmentID { get; set; }
            public string DepartmentTitle { get; set; }

            public Department(int departmentID, string departmentTitle)
            {
                DepartmentID = departmentID;
                DepartmentTitle = departmentTitle;
            }

            public override string ToString()
            {
                return DepartmentTitle; 
            }
        }

        public class Schedule
        {
            public int ScheduleID { get; set; }
            public string ScheduleType { get; set; }

            public Schedule(int scheduleID, string scheduleType)
            {
                ScheduleID = scheduleID;
                ScheduleType = scheduleType;
            }

            public override string ToString()
            {
                return ScheduleType; 
            }
        }

        private void PopulatePosComboBox()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Position_ID, Position_Title FROM position";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        comboBox_Pos.Items.Clear();

                        while (reader.Read())
                        {
                            int positionID = reader.GetInt32("Position_ID");
                            string positionTitle = reader.GetString("Position_Title");

                            Position position = new Position(positionID, positionTitle);
                            comboBox_Pos.Items.Add(position);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void PopulateSchedComboBox()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Schedule_ID, Schedule_Type FROM schedule";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        comboBox_Sched.Items.Clear();

                        while (reader.Read())
                        {
                            int scheduleID = reader.GetInt32("Schedule_ID");
                            string scheduleTitle = reader.GetString("Schedule_Type");

                            Schedule schedule = new Schedule(scheduleID, scheduleTitle);
                            comboBox_Sched.Items.Add(schedule);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void PopulateComboBox(string query, ComboBox comboBox)
        {

            // Create a connection object
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a command object
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Execute the command and retrieve data
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Clear existing items
                        comboBox.Items.Clear();

                        // Loop through the data and add to combo box
                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader.GetString(0));
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }




        private void UpdateEmployeeIdAndUsername(object sender, EventArgs e)
        {
            

            GetNextEmployeeId();

            string firstName = txtbx_User.Text.Trim();
            string middleName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(middleName) && !string.IsNullOrEmpty(lastName))
            {
                string username = char.ToUpper(firstName[0]) + firstName.Substring(1).ToLower() + " " +
                                  char.ToUpper(middleName[0]) + middleName.Substring(1).ToLower() + " " +
                                  char.ToUpper(lastName[0]) + lastName.Substring(1).ToLower();
                label_User_concat.Text = username;
            }
            else
            {
                label_User_concat.Text = "Invalid Name";
            }
        }

        private void GetNextEmployeeId()
        {
            MySqlCommand cmd = new MySqlCommand("Select count(*) from employee_id", connection);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            i++;
            int valLbl = val + i;
            label_EmpID_Num.Text = valLbl.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_DTR_Click(object sender, EventArgs e)
        {
            string firstName = txtbx_User.Text.Trim();
            string middleName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();
            string username = label_User_concat.Text;
            string password = textBox4.Text.Trim(); //wala pa na tarung
            string type = "employee";
            string phoneNumber = textBox3.Text.Trim();
            string gender = comboBox_Gender.SelectedItem?.ToString();
            DateTime hireDate = dateTimePicker1.Value;
            DateTime birthDate = dateTimePicker2.Value;

            // Retrieve selected position
            Position selectedPosition = (Position)comboBox_Pos.SelectedItem;
            int selectedPositionID = selectedPosition.PositionID;

            Schedule selectedSchedule = (Schedule)comboBox_Sched.SelectedItem;
            int selectedScheduleID = selectedSchedule.ScheduleID;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(middleName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            string insertQuery = "INSERT INTO user (Employee_ID, Username, Password, Type) " +
                                 "VALUES (@Employee_ID, @Username, @Password, @Type)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@Employee_ID", label_EmpID_Num.Text);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Type", type);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User record created successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to create user record.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating user record: " + ex.Message);
            }

            string insertEmployeeQuery = "INSERT INTO employee_id (Employee_ID, First_Name, Middle_Name, Last_Name, Gender, Phone_Number, Hire_Date, Birth_Date, Position_ID, Schedule_ID) " +
                                         "VALUES (@Employee_ID, @First_Name, @Middle_Name, @Last_Name, @Gender, @Phone_Number, @Hire_Date, @Birth_Date, @Position_ID, @Schedule_ID)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(insertEmployeeQuery, conn);
                    cmd.Parameters.AddWithValue("@Employee_ID", label_EmpID_Num.Text);
                    cmd.Parameters.AddWithValue("@First_Name", firstName);
                    cmd.Parameters.AddWithValue("@Middle_Name", middleName);
                    cmd.Parameters.AddWithValue("@Last_Name", lastName);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Phone_Number", phoneNumber);
                    cmd.Parameters.AddWithValue("@Hire_Date", hireDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Birth_Date", birthDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Position_ID", selectedPositionID); // Insert selected position ID
                    cmd.Parameters.AddWithValue("@Schedule_ID", selectedScheduleID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee record created successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to create employee record.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating employee record: " + ex.Message);
            }
        }


        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_Pos_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
