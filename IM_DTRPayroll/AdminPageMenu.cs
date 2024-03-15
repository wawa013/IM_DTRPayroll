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
    }
}
