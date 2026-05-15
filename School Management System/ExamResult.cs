using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class ExamResult : Form
    {
        // Connection string
        string conStr = "Data Source=.;Initial Catalog=SchoolProject;Integrated Security=True";

        public ExamResult()
        {
            InitializeComponent();
        }

        // ================= LOAD FORM =================
        private void ExamResult_Load(object sender, EventArgs e)
        {
            LoadExamData();
        }

        // ================= LOAD DATA =================
        private void LoadExamData()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_Exam_GetAll", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                // Change column names
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["ExamID"].HeaderText = "ID";
                    dataGridView1.Columns["ExamName"].HeaderText = "Exam Name";
                    dataGridView1.Columns["Subject"].HeaderText = "Subject";
                    dataGridView1.Columns["ExamDate"].HeaderText = "Date";
                }
            }
        }

        // ================= ADD =================
        private void btn_addexam_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("sp_Exam_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ExamName", txtexamname.Text);
                cmd.Parameters.AddWithValue("@Subject", txtsubject.Text);
                cmd.Parameters.AddWithValue("@ExamDate", dateTimePicker1.Value);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Exam Added Successfully");

                ClearFields();
                LoadExamData();
            }
        }

        // ================= UPDATE =================
        private void btn_update_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int examID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ExamID"].Value);

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("sp_Exam_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ExamID", examID);
                    cmd.Parameters.AddWithValue("@ExamName", txtexamname.Text);
                    cmd.Parameters.AddWithValue("@Subject", txtsubject.Text);
                    cmd.Parameters.AddWithValue("@ExamDate", dateTimePicker1.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Exam Updated Successfully");

                    ClearFields();
                    LoadExamData();
                }
            }
        }

        // ================= DELETE =================
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int examID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ExamID"].Value);

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("sp_Exam_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ExamID", examID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Exam Deleted Successfully");

                    ClearFields();
                    LoadExamData();
                }
            }
        }

        // ================= GRID CLICK =================
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtexamname.Text = dataGridView1.CurrentRow.Cells["ExamName"].Value.ToString();
                txtsubject.Text = dataGridView1.CurrentRow.Cells["Subject"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["ExamDate"].Value);
            }
        }

        // ================= CLEAR =================
        private void ClearFields()
        {
            txtexamname.Clear();
            txtsubject.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }
    }
}