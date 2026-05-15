using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Section : Form
    {
        string constr = @"Data Source=DESKTOP-QBJEF9U;Initial Catalog=SchoolProject;Integrated Security=True";

        int selectedId = 0;

        public Section()
        {
            InitializeComponent();
        }

        // ================= LOAD FORM =================
        private void Section_Load(object sender, EventArgs e)
        {
            cmbsection.Items.AddRange(new string[] { "A", "B", "C", "D" });

            LoadTeachers();
            LoadClass();
        }

        // ================= LOAD TEACHERS =================
        void LoadTeachers()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT TeacherID, TeacherName FROM Teacher", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbteacher.DataSource = dt;
                cmbteacher.DisplayMember = "TeacherName";
                cmbteacher.ValueMember = "TeacherID";
                cmbteacher.SelectedIndex = -1;
            }
        }

        // ================= LOAD CLASS =================
        void LoadClass()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_Class_GetAll", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        // ================= ADD =================
        private void btn_add_Click(object sender, EventArgs e)
        {
            if (txtclassname.Text.Trim() == "" || cmbsection.SelectedIndex == -1 || cmbteacher.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            int teacherId;
            if (!int.TryParse(cmbteacher.SelectedValue?.ToString(), out teacherId))
            {
                MessageBox.Show("Invalid Teacher Selection!");
                return;
            }

            int totalStudents = 0;
            int.TryParse(txttotalstudents.Text, out totalStudents);

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("sp_Class_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClassName", txtclassname.Text.Trim());
                cmd.Parameters.AddWithValue("@Section", cmbsection.Text);
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                cmd.Parameters.AddWithValue("@TotalStudents", totalStudents);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Class Added Successfully");
            LoadClass();
            ClearFields();
        }

        // ================= DISPLAY =================
        private void btn_display_Click(object sender, EventArgs e)
        {
            LoadClass();
        }

        // ================= UPDATE =================
        private void btn_update_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select a record first!");
                return;
            }

            int teacherId;
            if (!int.TryParse(cmbteacher.SelectedValue?.ToString(), out teacherId))
            {
                MessageBox.Show("Invalid Teacher Selection!");
                return;
            }

            int totalStudents = 0;
            int.TryParse(txttotalstudents.Text, out totalStudents);

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("sp_Class_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClassID", selectedId);
                cmd.Parameters.AddWithValue("@ClassName", txtclassname.Text.Trim());
                cmd.Parameters.AddWithValue("@Section", cmbsection.Text);
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                cmd.Parameters.AddWithValue("@TotalStudents", totalStudents);

                con.Open();
                int result = cmd.ExecuteNonQuery();

                MessageBox.Show(result > 0 ? "Updated Successfully" : "Update Failed");
            }

            LoadClass();
            ClearFields();
        }

        // ================= DELETE =================
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select a record first!");
                return;
            }

            DialogResult dr = MessageBox.Show(
                "Are you sure you want to delete?",
                "Confirm",
                MessageBoxButtons.YesNo
            );

            if (dr == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("sp_Class_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ClassID", selectedId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Deleted Successfully");
                LoadClass();
                ClearFields();
            }
        }

        // ================= GRID CLICK =================
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            selectedId = Convert.ToInt32(row.Cells["ClassID"].Value);

            txtclassname.Text = row.Cells["ClassName"].Value?.ToString();
            cmbsection.Text = row.Cells["Section"].Value?.ToString();
            txttotalstudents.Text = row.Cells["TotalStudents"].Value?.ToString();

            if (row.Cells["TeacherID"].Value != null && row.Cells["TeacherID"].Value != DBNull.Value)
            {
                cmbteacher.SelectedValue = Convert.ToInt32(row.Cells["TeacherID"].Value);
            }
        }

        // ================= CLEAR =================
        private void btn_clear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        void ClearFields()
        {
            txtclassname.Clear();
            txttotalstudents.Clear();

            cmbsection.SelectedIndex = -1;
            cmbteacher.SelectedIndex = -1;

            selectedId = 0;
        }

        // ================= BACK BUTTON =================
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Main main1 = new Main();    
            main1.Show();
            this.Hide();
        }
    }
}