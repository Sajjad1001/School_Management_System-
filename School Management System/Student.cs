using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Student : Form
    {
        string constr = @"Data Source=DESKTOP-QBJEF9U;Initial Catalog=SchoolProject;Integrated Security=True";
        int selectedId = 0;

        public Student()
        {
            InitializeComponent();
        }

        // ================= LOAD DATA =================
        void LoadData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlDataAdapter da = new SqlDataAdapter("sp_Student_GetAll", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        // ================= LOAD FORM =================
        private void Student_Load(object sender, EventArgs e)
        {
            LoadData();

            txtGender.Items.Clear();
            txtGender.Items.AddRange(new string[] { "Male", "Female" });

            cmbcategory.Items.Clear();
            cmbcategory.Items.AddRange(new string[] { "BSCS", "BSSE", "BSAI" });
        }

        // ================= INSERT =================
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "" || txtGender.SelectedIndex == -1
                || cmbcategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("sp_Student_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentName", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@DOB", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@Gender", txtGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Category", cmbcategory.SelectedItem.ToString());

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Student Added Successfully");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // ================= UPDATE =================
        private void btn_update_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select student first!");
                return;
            }

            if (txtName.Text.Trim() == "" || txtGender.SelectedIndex == -1
                || cmbcategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("sp_Student_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentID", selectedId);
                    cmd.Parameters.AddWithValue("@StudentName", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@DOB", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@Gender", txtGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Category", cmbcategory.SelectedItem.ToString());

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Updated Successfully");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // ================= DELETE =================
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select student first!");
                return;
            }

            DialogResult dr = MessageBox.Show(
                "Are you sure you want to delete?",
                "Confirm",
                MessageBoxButtons.YesNo
            );

            if (dr == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        SqlCommand cmd = new SqlCommand("sp_Student_Delete", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StudentID", selectedId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Deleted Successfully");
                    LoadData();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // ================= GRID CLICK =================
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridView1.Rows[e.RowIndex];

            selectedId = Convert.ToInt32(row.Cells["StudentID"].Value);
            txtName.Text = row.Cells["StudentName"].Value?.ToString();
            txtPhone.Text = row.Cells["Phone"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            txtGender.SelectedItem = row.Cells["Gender"].Value?.ToString();
            cmbcategory.SelectedItem = row.Cells["Category"].Value?.ToString();

            if (row.Cells["DOB"].Value != null && row.Cells["DOB"].Value != DBNull.Value)
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["DOB"].Value);
        }

        // ================= CLEAR =================
        void ClearFields()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtGender.SelectedIndex = -1;
            cmbcategory.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Today;
            selectedId = 0;
        }

        // ================= BACK BUTTON =================
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }
    }
}