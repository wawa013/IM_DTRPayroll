using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IM_DTRPayroll
{
    public partial class AdminPageMenu : Form
    {
        private MySqlConnection connection;
        //Josh na DB, MAIN
        //private const string connectionString = "server=localhost;port=3306;username=root;password=masellones;database=db_finalproject";

        //Jireh na db
        private const string connectionString = "server=localhost;Port=3306;Database=db_finalproject;Uid=root;Pwd=;";
        MySqlDataAdapter da;
        DataTable dt;

        public AdminPageMenu()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DeductionsForm dedform = new DeductionsForm();
            dedform.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OvertimeApproval overtimeApprove = new OvertimeApproval();
            overtimeApprove.ShowDialog();
        }

        private void lblDate_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    CreatePayrollPeriod();
                    InsertEmployeePayrollRecords(connection);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void CreatePayrollPeriod()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Get the current date
                    DateTime currentDate = DateTime.Today;

                    // Calculate the start date as the 1st day of the current month
                    DateTime startDate = new DateTime(currentDate.Year, currentDate.Month, 1);

                    // Calculate the end date as 15 days after the start date
                    DateTime endDate = startDate.AddDays(14);

                    // Create a new payroll period with auto-incremented PayrollPeriod_ID
                    string insertQuery = "INSERT INTO payroll_period (Start_Date, End_Date) VALUES (@Start_Date, @End_Date)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@Start_Date", startDate);
                    insertCmd.Parameters.AddWithValue("@End_Date", endDate);

                    startDate = endDate;

                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("New payroll period created successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to create new payroll period.");
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void InsertEmployeePayrollRecords(MySqlConnection connection)
        {
            try
            {
                // Retrieve all existing employee IDs
                List<int> employeeIDs = new List<int>();
                string query = "SELECT Employee_ID FROM employee_id";
                MySqlCommand command = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employeeIDs.Add(reader.GetInt32("Employee_ID"));
                    }
                }

                // Get the payroll period ID of the newly created payroll period
                int payrollPeriodID;
                query = "SELECT MAX(PayrollPeriod_ID) FROM payroll_period";
                command = new MySqlCommand(query, connection);
                payrollPeriodID = Convert.ToInt32(command.ExecuteScalar());

                // Insert records into employee_payroll table for each employee
                foreach (int employeeID in employeeIDs)
                {
                    string insertQuery = "INSERT INTO employee_payroll (Employee_ID, PayrollPeriod_ID) VALUES (@Employee_ID, @PayrollPeriod_ID)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@Employee_ID", employeeID);
                    insertCmd.Parameters.AddWithValue("@PayrollPeriod_ID", payrollPeriodID);
                    insertCmd.ExecuteNonQuery();
                }
                MessageBox.Show("Successfully created Employee Payroll IDs");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting employee payroll records: " + ex.Message);
            }
        }

        private void btn_DTR_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM employee_payroll";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    dt = new DataTable();
                    da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM employee_payroll";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    dt = new DataTable();
                    da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ButtonsLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}




