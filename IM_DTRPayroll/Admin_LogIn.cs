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
    public partial class Admin_LogIn : Form
    {
        String UserTrue = "admin", PassTrue = "1234";
        public Admin_LogIn()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_DTR_Click(object sender, EventArgs e)
        {
            String UserInput, PassInput;
            UserInput = txtbx_AdminID.Text;
            PassInput = txtbx_AdminPass.Text;
            //check if user input is same as set login credentials
            if (UserInput == UserTrue && PassInput == PassTrue)
            {
                this.Close();
                AdminPageMenu ad = new AdminPageMenu();
                ad.ShowDialog();
            }
            else
            {
                MessageBox.Show("Incorrect Username or Password!");
                //reset textfields after incorrect input
                txtbx_AdminID.Text = "";
                txtbx_AdminPass.Text = "";
            }
        }
    }
}
