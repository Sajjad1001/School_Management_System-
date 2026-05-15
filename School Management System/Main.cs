using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnStudent_Click(object sender, EventArgs e)
        {
            Student student = new Student();
            student.Show();
        }

        private void btnteacher_Click(object sender, EventArgs e)
        {
            Teacher teacher = new Teacher();
            teacher.Show();
        }

        private void btnsection_Click(object sender, EventArgs e)
        {
            Section section = new Section();
            section.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void btndashboard_Click(object sender, EventArgs e)
        {
            Reports reports = new Reports();
            reports.Show();
            this.Hide();
        }

        private void btnsubject_Click(object sender, EventArgs e)
        {
            ExamResult examResult= new ExamResult();
            examResult.Show();
            this.Hide();
        }
    }
}
