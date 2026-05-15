using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Teacher : Form
    {
        string constr = @"Data Source=DESKTOP-QBJEF9U;Initial Catalog=SchoolProject;Integrated Security=True";

        int selectedId = 0;

        public Teacher()
        {
            InitializeComponent();
        }

        // ================= LOAD =================
        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_Teacher_GetAll", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void Teacher_Load(object sender, EventArgs e)
        {
            LoadData();

            // OPTIONAL: fill gender combobox
            if (txtgender.Items.Count == 0)
            {
                txtgender.Items.Add("Male");
                txtgender.Items.Add("Female");
            }
        }

        // ================= INSERT =================
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (txtTeacherName.Text == "" ||
                txtsubject.Text == "" ||
                txtphone.Text == "" ||
                txtgender.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("sp_Teacher_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TeacherName", txtTeacherName.Text);
                cmd.Parameters.AddWithValue("@Gender", txtgender.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Subject", txtsubject.Text);
                cmd.Parameters.AddWithValue("@Phone", txtphone.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Teacher Added Successfully");
            ClearFields();
            LoadData();
        }
        // ================= UPDATE =================
        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Please select a teacher first!");
                return;
            }

            if (txtgender.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Gender!");
                return;
            }

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("sp_Teacher_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TeacherID", selectedId);
                cmd.Parameters.AddWithValue("@TeacherName", txtTeacherName.Text);
                cmd.Parameters.AddWithValue("@Gender", txtgender.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Subject", txtsubject.Text);
                cmd.Parameters.AddWithValue("@Phone", txtphone.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Updated Successfully");
            ClearFields();
            LoadData();
        }

        // ================= DELETE =================
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Please select a teacher first!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("sp_Teacher_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TeacherID", selectedId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Deleted Successfully");
                ClearFields();
                LoadData();
            }
        }

        // ================= GRID CLICK =================
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            selectedId = Convert.ToInt32(row.Cells["TeacherID"].Value);

            txtTeacherName.Text = row.Cells["TeacherName"].Value.ToString();
            txtsubject.Text = row.Cells["Subject"].Value.ToString();
            txtphone.Text = row.Cells["Phone"].Value.ToString();

           
            txtgender.SelectedItem = row.Cells["Gender"].Value.ToString();
        }

       
        private void btn_display_Click(object sender, EventArgs e)
        {
            LoadData();
        }

       
        void ClearFields()
        {
            txtTeacherName.Clear();
            txtsubject.Clear();
            txtphone.Clear();

            txtgender.SelectedIndex = -1;  
            selectedId = 0;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            txtTeacherName.Clear();
            txtsubject.Clear();
            txtphone.Clear();

            txtgender.SelectedIndex = -1;
            selectedId = 0;
        }
    }
}