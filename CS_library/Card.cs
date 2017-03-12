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
    public partial class Card : Form
    {
        public Card()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.SelectedIndex = -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(Form1.ConnectionString);
            conn.Open();
            
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入卡号");
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("请输入读者姓名");
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("请输入读者单位");
                return;
            }
            if(comboBox1 .SelectedIndex==-1)
            {
                MessageBox.Show("请选择读者类别");
                return;
            }
            char type = comboBox1.Text[0];
            SqlCommand check = new SqlCommand("select * from card where cno='" + textBox2.Text + "'", conn);
            SqlDataReader dr = check.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("The cardnum already exist!");
                dr.Close();
                conn.Close();
                return;
            }
            dr.Close();
            SqlCommand add = new SqlCommand("insert into card values('" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + comboBox1.Text + "' )", conn);
            try
            {
                add.ExecuteNonQuery();
                MessageBox.Show("借书证插入成功！");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("插入借书证失败，请检查输入数据结构！");
            } 
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(Form1.ConnectionString);
            conn.Open();
            SqlCommand check = new SqlCommand("select * from card where cno='" + textBox1.Text + "'", conn);
            SqlDataReader dr = check.ExecuteReader();
            if (!dr.Read())
            {
                MessageBox.Show("要删除的卡号" + textBox1.Text + "不存在！请重新输入！");
                dr.Close();
                conn.Close();
                return;
            }
            else
            {
                dr.Close();
                check = new SqlCommand("select * from borrow where cno='" + textBox1.Text + "' and return_date is null", conn);
                 dr = check.ExecuteReader();
                 if (dr.Read())
                 {
                     MessageBox.Show("要删除的卡号" + textBox1.Text + "有未还的书籍！请重新输入！");
                     dr.Close();
                     conn.Close();
                     return;
                 }
                 else
                 {
                     dr.Close();
                     SqlCommand delete = new SqlCommand("delete from borrow where cno='" + textBox1.Text + "' and return_date is not null", conn);
                     delete.ExecuteNonQuery();
                     delete = new SqlCommand("delete from card where cno='" + textBox1.Text + "'", conn);
                     delete.ExecuteNonQuery();
                     MessageBox.Show("借书证" + textBox1.Text + "已经成功删除!");
                     conn.Close();
                     return;
                 }
            }

            
        }
    }
}
