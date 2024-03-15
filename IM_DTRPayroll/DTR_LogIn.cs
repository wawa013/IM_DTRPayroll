using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IM_DTRPayroll
{
    public partial class DTR_LogIn : Form
    {
        private MySqlConnection connection;
        //Josh na DB, MAIN
        //private const string connectionString = "server=localhost;port=3306;username=root;password=masellones;database=db_finalproject";

        //Jireh na db
        private const string connectionString = "server=localhost;Port=3306;Database=db_finalproject;Uid=root;Pwd=;";
        DataTable dt;
        MySqlDataAdapter da;

        public DTR_LogIn()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            txtbx_User.Text = "";
            txtbx_Pass.Text = "";
            this.Close(); 
        }

        private void lblUserID_Click(object sender, EventArgs e)
        {

        }

        private void btn_DTR_Click(object sender, EventArgs e)
        {
            if (txtbx_Pass.Text == "" || txtbx_User.Text == "")
            {
                MessageBox.Show("Please enter your EMPLOYEE ID and Password to time in / out.");
            }
            else
            {
                string empID = txtbx_User.Text;
                string password = txtbx_Pass.Text;
                int payrollPeriodID = GetCurrentPayrollPeriodID();

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        DataTable dt = new DataTable();

                        // Check if employee exists
                        string query = "SELECT * FROM user WHERE Employee_ID = @Employee_ID";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Employee_ID", empID);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            // Check if employee has already clocked in and out for the day
                            query = "SELECT * FROM dtr WHERE Employee_ID = @Employee_ID AND Date = @Date AND isClocked_In = 'Clocked In' AND isClocked_Out = 'Clocked Out'";
                            cmd = new MySqlCommand(query, connection);
                            cmd.Parameters.AddWithValue("@Employee_ID", empID);
                            cmd.Parameters.AddWithValue("@Date", label_Date.Text);
                            dt.Clear();
                            da.SelectCommand = cmd;
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("You have already Clocked In and Out for the day.");
                            }
                            else
                            {
                                // Check if employee has already clocked in
                                query = "SELECT * FROM dtr WHERE Employee_ID = @Employee_ID AND Date = @Date AND isClocked_In = 'Clocked In'";
                                cmd = new MySqlCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Employee_ID", empID);
                                cmd.Parameters.AddWithValue("@Date", label_Date.Text);
                                dt.Clear();
                                da.SelectCommand = cmd;
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    // Update database for clocking out
                                    string clockOutTime = DateTime.Now.ToString("HH:mm:ss");
                                    TimeSpan timeDiff = TimeSpan.Parse(clockOutTime) - TimeSpan.Parse(dt.Rows[0]["Clock_In"].ToString());
                                    double totalHours = timeDiff.TotalHours;
                                    double overtimeHours = Math.Max(totalHours - 9, 0);

                                    cmd = new MySqlCommand("UPDATE dtr SET Clock_Out = @Clock_Out, isClocked_Out = 'Clocked Out', Total_Hours = @Total_Hours, Overtime_Hours = @Overtime_Hours WHERE Employee_ID = @Employee_ID AND Date = @Date", connection);
                                    cmd.Parameters.AddWithValue("@Clock_Out", clockOutTime);
                                    cmd.Parameters.AddWithValue("@Total_Hours", totalHours);
                                    cmd.Parameters.AddWithValue("@Overtime_Hours", overtimeHours);
                                    cmd.Parameters.AddWithValue("@Employee_ID", empID);
                                    cmd.Parameters.AddWithValue("@Date", label_Date.Text);
                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("Clock Out Success");
                                }
                                else
                                {
                                    // Insert clock in record
                                    cmd = new MySqlCommand("INSERT INTO dtr (Employee_ID, PayrollPeriod_ID, Date, Clock_In, isClocked_In) VALUES (@Employee_ID, @PayrollPeriod_ID, @Date, @Clock_In, 'Clocked In')", connection);
                                    cmd.Parameters.AddWithValue("@Employee_ID", empID);
                                    cmd.Parameters.AddWithValue("@PayrollPeriod_ID", payrollPeriodID);
                                    cmd.Parameters.AddWithValue("@Date", label_Date.Text);
                                    cmd.Parameters.AddWithValue("@Clock_In", DateTime.Now.ToString("HH:mm:ss"));
                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("Clock In Success");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Employee not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private int GetCurrentPayrollPeriodID()
        {
            int payrollPeriodID = -1; // Default value for error handling

            // Query to fetch the payroll period based on the current date
            string query = "SELECT PayrollPeriod_ID FROM payroll_period " +
                           "WHERE start_date <= CURRENT_DATE() AND end_date >= CURRENT_DATE()";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        payrollPeriodID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving current payroll period: " + ex.Message);
            }

            return payrollPeriodID;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_Date.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            label_Time.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
