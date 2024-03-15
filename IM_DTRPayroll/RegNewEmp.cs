using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace IM_DTRPayroll
{
    public partial class RegNewEmp : Form
    {
        private MySqlConnection connection;
        private const string connectionString = "server=localhost;port=3307;username=root;password=masellones;database=db_finalproject";

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }

        private void UpdateEmployeeIdAndUsername(object sender, EventArgs e)
        {
            int employeeId = GetNextEmployeeId();
            label15.Text = employeeId.ToString();

            string firstName = txtbx_User.Text.Trim();
            string middleName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(middleName) && !string.IsNullOrEmpty(lastName))
            {
                string username = char.ToUpper(firstName[0]) + firstName.Substring(1).ToLower() +
                                  char.ToUpper(middleName[0]) + middleName.Substring(1).ToLower() +
                                  char.ToUpper(lastName[0]) + lastName.Substring(1).ToLower();
                label21.Text = username;
            }
            else
            {
                label21.Text = "Invalid Name";
            }
        }


        private int GetNextEmployeeId()
        {
            string query = "SELECT MAX(Employee_ID) FROM employee_id";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            int nextEmployeeId = 0;

            try
            {
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    nextEmployeeId = Convert.ToInt32(result) + 1;
                }
                else
                {
                    nextEmployeeId = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving next Employee ID: " + ex.Message);
            }

            return nextEmployeeId;
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
            string username = label21.Text;
            string password = "defaultpassword"; //wala pa na tarung
            string type = "employee";
            string phoneNumber = textBox3.Text.Trim();
            string gender = comboBox4.SelectedItem?.ToString();
            DateTime hireDate = dateTimePicker1.Value;
            DateTime birthDate = dateTimePicker2.Value;

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
                    cmd.Parameters.AddWithValue("@Employee_ID", label15.Text);
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

            string insertEmployeeQuery = "INSERT INTO employee_id (Employee_ID, Username, Password, Type, Phone_Number, Gender, Hire_Date, Birth_Date) " +
                                         "VALUES (@Employee_ID, @Username, @Password, @Type, @Phone_Number, @Gender, @Hire_Date, @Birth_Date)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(insertEmployeeQuery, conn);
                    cmd.Parameters.AddWithValue("@Employee_ID", label15.Text);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Phone_Number", phoneNumber);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Hire_Date", hireDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Birth_Date", birthDate.ToString("yyyy-MM-dd"));

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
            comboBox4.Items.Clear();
            comboBox4.Items.Add("Male");
            comboBox4.Items.Add("Female");
            comboBox4.Items.Add("Other");
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
