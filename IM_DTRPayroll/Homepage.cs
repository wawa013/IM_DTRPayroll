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
    public partial class HOMEPAGE : Form
    {
        public HOMEPAGE()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            lblTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lblDate_Click(object sender, EventArgs e)
        {

        }

        private void btn_DTR_Click(object sender, EventArgs e)
        {
            //this.Hide();
            DTR_LogIn DTR_log = new DTR_LogIn();
            DTR_log.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegNewEmp reg = new RegNewEmp();
            reg.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Admin_LogIn adlog = new Admin_LogIn();
            adlog.ShowDialog();
        }

        private void btn_DTR_Click_1(object sender, EventArgs e)
        {
            DTR_LogIn dtrlog = new DTR_LogIn();
            dtrlog.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Overtime_ReqForm overtime_form = new Overtime_ReqForm();
            overtime_form.ShowDialog();
        }
    }
}
