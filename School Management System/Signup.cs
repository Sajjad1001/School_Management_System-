using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Signup : Form
    {
        string constr = @"Data Source=DESKTOP-QBJEF9U;Initial Catalog=SchoolProject;Integrated Security=True";
        public Signup()
        {
            InitializeComponent();
        }

        private void Signup_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
       
            // 1. Empty field validation
            if (txtusername.Text == "" || txtemail.Text == "" || txtphonenbr.Text == "" || txtpassword.Text == "" || txtcpassword.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            // 2. Confirm Password check
            if (txtpassword.Text != txtcpassword.Text)
            {
                MessageBox.Show("Password and Confirm Password do not match");
                return;
            }

            // 3. Optional: Password length check
            if (txtpassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters");
                return;
            }

            try
            {
                // 4. Call Stored Procedure
                SqlConnection con=new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("sp_signup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", txtusername.Text);
                cmd.Parameters.AddWithValue("@Email", txtemail.Text);
                cmd.Parameters.AddWithValue("@PhoneNbr", txtphonenbr.Text);
                cmd.Parameters.AddWithValue("@Role",comboBox1.Text); 
                cmd.Parameters.AddWithValue("@Password", txtpassword.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Signup Successful");

               
                txtusername.Clear();
                txtemail.Clear();
                txtphonenbr.Clear();
                txtpassword.Clear();
                txtcpassword.Clear();
                comboBox1.Items.Clear();


                Form1 form1 = new Form1();
                form1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }
    }
    }

