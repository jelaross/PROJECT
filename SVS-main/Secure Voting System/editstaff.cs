﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Secure_Voting_System
{
    public partial class editstaff : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SQL_CONNECTION"]);
        public editstaff()
        {
            InitializeComponent();
            fillgrid(dataGridView1);
            fillgrid(dataGridView2);
            fillgrid(dataGridView3);
            fillcombo();
            
        }

        public void fillgrid(DataGridView js)
        {

            con.Open();
            string str = "select * from staff";
            DataSet ds = new DataSet();
            SqlDataAdapter sqlda = new SqlDataAdapter(str, con);
            sqlda.Fill(ds);
            js.DataSource = ds.Tables[0].DefaultView;
            con.Close();
        }

        private void viewstaff_Load(object sender, EventArgs e)
        {

        }
        public void fillcombo()
        {
            string query = "select sid from staff";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0].ToString());
                comboBox2.Items.Add(dr[0].ToString());
            }
            con.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "select  * from staff where sid='" + comboBox1.Text + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox1.Text = dr[1].ToString();
               
                textBox3.Text = dr[3].ToString();
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                new FloatingNotification(this, "Please select Sid", "#d33235");
                return;
            }

            con.Open();
            SqlTransaction transaction = con.BeginTransaction();

            SqlCommand command1 = new SqlCommand("DELETE FROM login_details where lid=(SELECT lid FROM staff where sid=@sid)", con, transaction);
            command1.Parameters.AddWithValue("@sid", comboBox2.Text);
            command1.ExecuteNonQuery();

            SqlCommand command2 = new SqlCommand("DELETE FROM staff where sid=@sid", con, transaction);
            command2.Parameters.AddWithValue("@sid", comboBox2.Text);
            command2.ExecuteNonQuery();

            transaction.Commit();
            con.Close();

            fillgrid(dataGridView1);
            fillgrid(dataGridView2);
            fillgrid(dataGridView3);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == ""){
                new FloatingNotification(this, "Please select Sid", "#d33235");
                return;
            }
            string query = "update staff set name='" + textBox1.Text + "', dob='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',gender='" + textBox3.Text + "' where sid='" + comboBox1.Text + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            if (cmd.ExecuteNonQuery() > 0)
            {

                textBox1.Text = "";
                
                textBox3.Text = "";
                comboBox1.Text = "";
                new FloatingNotification(this, "Staff updated", "#41a45d");

            }
           
            con.Close();
            fillgrid(dataGridView1);
            fillgrid(dataGridView2);
            fillgrid(dataGridView3);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string query = "select  * from staff where sid='" + comboBox1.Text + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox1.Text = dr[1].ToString();
                // textBox2.Text = ;
                textBox3.Text = dr[3].ToString();

                DateTime parsedDate;
                bool success = DateTime.TryParse(dr[2].ToString(), out parsedDate);

                if (success)
                {
                    // Set the DateTimePicker's value
                    dateTimePicker1.Value = parsedDate;
                }
                else
                {
                    MessageBox.Show("Invalid date format");
                }
            }
            con.Close();
        }

        
    }
}
