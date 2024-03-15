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
    public partial class Overtime_ReqForm : Form
    {
        private MySqlConnection connection;
        //Josh na DB, MAIN
        //private const string connectionString = "server=localhost;port=3306;username=root;password=masellones;database=db_finalproject";

        //Jireh na db
        private const string connectionString = "server=localhost;Port=3306;Database=db_finalproject;Uid=root;Pwd=;";

        public Overtime_ReqForm()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void Overtime_ReqForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_DTR_Click(object sender, EventArgs e)
        {
            int empPayrollID = Convert.ToInt32(txtbx_EmpID.Text);
            decimal reqHours = decimal.Parse(txtbx_ReqOT.Text);
            string status = "Pending";
            DateTime reqDate = dateTimePicker1.Value;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Construct the SQL query
                    string query = "INSERT INTO overtime (EmpPayroll_ID, Date, Overtime_Hours_Requested, Overtime_Status) " +
                                   "VALUES (@EmployeePayroll_ID, @Request_Date, @Requested_Hours, @status)";

                    // Create and execute the command
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeePayroll_ID", empPayrollID);
                        command.Parameters.AddWithValue("@Request_Date", reqDate);
                        command.Parameters.AddWithValue("@Requested_Hours", reqHours);
                        command.Parameters.AddWithValue("@status", status);
                        

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Overtime request submitted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to submit overtime request.");
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
}