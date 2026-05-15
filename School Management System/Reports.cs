using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Reports : Form
    {
        string constr = @"Data Source=DESKTOP-QBJEF9U;Initial Catalog=SchoolProject;Integrated Security=True";

        public Reports()
        {
            InitializeComponent();
        }

        private void Reports_Load(object sender, EventArgs e)
        {
           
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ================= STUDENT REPORT =================
        private void button1_Click(object sender, EventArgs e)
        {
            LoadData("sp_Student_GetAll");
        }

        // ================= TEACHER REPORT =================
        private void button2_Click(object sender, EventArgs e)
        {
            LoadData("sp_Teacher_GetAll");
        }

        // ================= RESULT REPORT =================
        private void button3_Click(object sender, EventArgs e)
        {
            LoadData("sp_Result_GetAll_Details");
        }

        // ================= COMMON FUNCTION =================
        void LoadData(string procedureName)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlDataAdapter da = new SqlDataAdapter(procedureName, con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                da.Fill(dt);

                // 🔥 VERY IMPORTANT (refresh structure)
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();

                dataGridView1.DataSource = dt;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Main main   = new Main();
            main.Show();
            this.Hide();
        }
    }
}