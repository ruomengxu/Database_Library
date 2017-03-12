using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO;

namespace CS_library
{
    public partial class Form1 : Form
    { 
        public static string ConnectionString = @"Data Source=XIN-PC;Initial Catalog=master;Integrated Security=True";
        public  static string m_AdminId="";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             m_AdminId = textBox1.Text;
            string m_Password = textBox2.Text;
            if (CheckAdminID(m_AdminId, m_Password))
            {
                textBox1.Text = "";
                textBox2.Text = "";
                AdminMain adm = new AdminMain();
                adm.ShowDialog();
            }
            else
            {
                m_AdminId = "";
                textBox2.Text = "";
                MessageBox.Show("管理员ID或密码输入错误，请重新输入！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reader reader = new Reader();
            reader.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public static bool CheckAdminID(string adminID, string password)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select password from adminitrator where id='" + adminID + "'", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (password == reader[0].ToString())
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            else
            {
                conn.Close();
                return false;
            }
        }
    }
}
