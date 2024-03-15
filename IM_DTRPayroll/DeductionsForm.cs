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
using static IM_DTRPayroll.RegNewEmp;

namespace IM_DTRPayroll
{
    public partial class DeductionsForm : Form
    {
        private MySqlConnection connection;
        //Josh na DB, MAIN
        //private const string connectionString = "server=localhost;port=3306;username=root;password=masellones;database=db_finalproject";

        //Jireh na db
        private const string connectionString = "server=localhost;Port=3306;Database=db_finalproject;Uid=root;Pwd=;";

        public DeductionsForm()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
        }

        private void DeductionsForm_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                MessageBox.Show("Database connection successful!");
                PopulateDedTypeComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }

        }
/*
        public class PayrollPeriod
        {
            public int PayrollPeriodID { get; set; }
            public string StartDate { get; set; }

            public PayrollPeriod(int payrollperiodID, string startDate)
            {
                PayrollPeriodID = payrollperiodID;
                StartDate = startDate;
            }

            public override string ToString()
            {
                return StartDate;
            }
        }
*/
        public class DeductionType
        {
            public int DeductionTypeID { get; set; }
            public string DeductionTypeName { get; set; }

            public DeductionType(int deductionTypeID, string deductionTypeName)
            {
                DeductionTypeID = deductionTypeID;
                DeductionTypeName = deductionTypeName;
            }

            public override string ToString()
            {
                return DeductionTypeName;
            }
        }

      /*  private void PopulatePayrollPeriodComboBox()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT PayrollPeriod_ID, Start_Date FROM payroll_period";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        comboBox_PayPeriod.Items.Clear();

                        while (reader.Read())
                        {
                            int payrollPeriodID = reader.GetInt32("PayrollPeriod_ID");
                            DateTime startDate = reader.GetDateTime("Start_Date");

                            // Convert DateTime to string representation with desired format
                            string startDateString = startDate.ToString("yyyy-MM-dd");

                            PayrollPeriod payperiod = new PayrollPeriod(payrollPeriodID, startDateString);
                            comboBox_PayPeriod.Items.Add(payperiod);
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
      */
        private void PopulateDedTypeComboBox()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Deduction_Type_ID, Deduction_Type_Name FROM other_deductions";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        comboBox_DedType.Items.Clear();

                        while (reader.Read())
                        {
                            int deductionTypeID = reader.GetInt32("Deduction_Type_ID");
                            string deductionTypeName = reader.GetString("Deduction_Type_Name");

                            DeductionType dedtype = new DeductionType(deductionTypeID, deductionTypeName);
                            comboBox_DedType.Items.Add(dedtype);
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void btn_DTR_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_PayPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_DedType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
