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
    public partial class BorrowReturn : Form
    {
        string m_cno="";
        public BorrowReturn()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
             m_cno = textBox1.Text;
            SqlConnection conn = new SqlConnection(Form1.ConnectionString);
            conn.Open();
            string searchCmd = "select * from borrow where cno='" + m_cno + "' and return_date is null";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(searchCmd , conn);
            SqlCommandBuilder commandbuilder = new SqlCommandBuilder(dataAdapter);
            DataTable table = new DataTable();
            dataAdapter.Fill(table);
            dataGridView1.DataSource = table;
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            dataGridView1.DataSource = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入借书证号");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入图书编号");
                return;
            }
            if (comboBox1 .SelectedIndex==-1)
            {
                MessageBox.Show("请选择结束或还书");
                return;
            }
            bool isBorrowOrReturn=true ;
            switch(comboBox1.Text )
            {
                case "借出":
                    isBorrowOrReturn =true ;
                    break;
                case "归还":
                    isBorrowOrReturn=false ;
                    break ;
            }
            SqlConnection conn = new SqlConnection(Form1.ConnectionString);
            conn.Open();
            SqlCommand check = new SqlCommand("select  * from borrow  where cno='" + textBox1.Text + "' and bno='" + textBox2.Text + "' and return_date is null", conn);
            SqlDataReader dr = check.ExecuteReader();
            if (isBorrowOrReturn)//借书模块
            {
                if (dr.Read())//检验是否已经借出
                {
                    dr.Close();
                    MessageBox.Show("此书已被您借出，不能重复外借！");
                    conn .Close();
                    return;
                }
                else
                {
                    dr.Close();
                    SqlCommand check2 = new SqlCommand("select stock from book  where  bno='" + textBox2.Text + "' ", conn);
                    SqlDataReader dr2= check2.ExecuteReader();
                    dr2.Read();
                    if (int.Parse(dr2[0].ToString()) >= 1)//检验图书是否有余量
                    {
                        dr2.Close();
                        SqlCommand delete = new SqlCommand("delete from borrow where cno='" + textBox1.Text + "' and bno='" + textBox2.Text + "'", conn);
                        delete.ExecuteNonQuery();
                        SqlCommand insert = new SqlCommand("insert into borrow values('"+textBox1.Text+"','"+textBox2.Text+"',convert(varchar(10),getdate(),120),'"+Form1.m_AdminId+"',null,null)", conn);
                        insert.ExecuteNonQuery();
                        SqlCommand update = new SqlCommand("update book set stock=stock-1 where bno='" + textBox2.Text + "'", conn);
                        update.ExecuteNonQuery();
                        MessageBox.Show("借书成功！");
                        conn.Close();
                        return;
                    }
                    else
                    {
                        dr2.Close();
                        MessageBox.Show("图书余量不足！");
                        conn.Close();
                        return;
                    }

                   
                }
            }
            else//还书模块
            {
                if (!dr.Read())
                {
                    dr.Close();
                    MessageBox.Show("没有相关的借书记录，请检查借书证号和图书编号！");
                    conn .Close();
                    return;
                }
                else
                {
                    dr.Close();
                    SqlCommand update = new SqlCommand("update borrow set return_date=convert(varchar(10),getdate(),120),return_id='" + Form1.m_AdminId + "' where cno='" + textBox1.Text + "' and bno='" + textBox2.Text + "'", conn);
                    update.ExecuteNonQuery();
                    SqlCommand update2 = new SqlCommand("update book set stock=stock+1 where bno='" + textBox2.Text + "'", conn);
                    update2.ExecuteNonQuery();
                    MessageBox.Show("还书成功！");
                    conn.Close();
                    return;
                }

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            comboBox1.SelectedIndex = -1;
        }
    }
}
