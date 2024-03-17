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
    public partial class OvertimeApproval : Form
    {
        private MySqlConnection connection;
        //Josh na DB, MAIN
        //private const string connectionString = "server=localhost;port=3306;username=root;password=masellones;database=db_finalproject";

        //Jireh na db
        private const string connectionString = "server=localhost;Port=3306;Database=db_im_finalproj;Uid=root;Pwd=;";
        DataTable dt;
        MySqlDataAdapter da;
        public OvertimeApproval()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OvertimeApproval_Load(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM overtime";
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM overtime";
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

        private void btn_DTR_Click(object sender, EventArgs e)
        {
            // Retrieve the EmpPayroll_ID from the textbox
            int empPayrollID;
            if (!int.TryParse(txtbx_User.Text, out empPayrollID))
            {
                MessageBox.Show("Invalid EmpPayroll_ID format. Please enter a valid integer.");
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Construct the SQL query to select the record based on EmpPayroll_ID
                    string query = "SELECT * FROM overtime WHERE EmpPayroll_ID = @EmpPayrollID";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@EmpPayrollID", empPayrollID);

                    // Execute the query to retrieve the record
                    dt = new DataTable();
                    da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);

                    // Check if any records are found
                    if (dt.Rows.Count > 0)
                    {
                        // Update the status of the record to 'Approved'
                        string updateQuery = "UPDATE overtime SET Overtime_Status = 'Approved' WHERE EmpPayroll_ID = @EmpPayrollID";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                        updateCmd.Parameters.AddWithValue("@EmpPayrollID", empPayrollID);
                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Overtime request approved successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to approve overtime request.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No overtime request found for the provided EmpPayroll_ID.");
                    }

                    // Update the DataGridView with the updated data
                    dataGridView1.DataSource = dt;

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}

