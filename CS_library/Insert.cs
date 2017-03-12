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
    public partial class Insert : Form
    {
        public Insert()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bno.Text = "";
            category.Text = "";
            title.Text = "";
            press.Text="";
            publish_year.Text="";
            author.Text="";
            price.Text="";
            total.Text = "";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bno.Text == "")
            {
                MessageBox.Show("请输入图书编号");
                return;
            }
            if (total.Text == "")
            {
                MessageBox.Show("请输入图书入库量");
                return;
            }
            SqlConnection conn = new SqlConnection(Form1.ConnectionString);
            conn.Open();
            SqlCommand check = new SqlCommand("select  total,stock from book where bno='" + bno.Text + "'", conn);
            SqlDataReader dr = check.ExecuteReader();
            if (dr.Read())//图书馆中已经有要入库的图书记录
            {
                int lastTotal = int.Parse(total.Text);
                int lastStock = int.Parse(total.Text);
                lastTotal += int.Parse(dr[0].ToString());
                lastStock += int.Parse(dr[1].ToString());
                dr.Close();
                SqlCommand update = new SqlCommand("update book set total=" + lastTotal.ToString() + ",stock=" + lastStock.ToString() + "   where bno='" + bno.Text + "'", conn);
                try
                {
                    update.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    MessageBox.Show("插入图书失败，请检查输入数据结构！");
                } 
                MessageBox.Show("图书" + bno.Text + "库存量添加成功");
                conn.Close();
                return;
            }
            else//图书馆中没有要入库的图书记录
            {
                if (category.Text == "")
                {
                    MessageBox.Show("请输入图书类别");
                    return;
                }
                if (title.Text == "")
                {
                    MessageBox.Show("请输入图书名称");
                    return;
                }
                if (press.Text == "")
                {
                    MessageBox.Show("请输入图书出版社");
                    return;
                } 
                if (publish_year.Text == "")
                {
                    MessageBox.Show("请输入图书出版年份");
                    return;
                } 
                if (author.Text == "")
                {
                    MessageBox.Show("请输入图书作者");
                    return;
                }
                if (price.Text == "")
                {
                    MessageBox.Show("请输入图书价格");
                    return;
                }
                dr.Close();
                SqlCommand insert = new SqlCommand("insert into book values('" + bno.Text + "','" + category.Text + "','" + title.Text + "','" + press.Text + "','" + publish_year.Text + "','" + author.Text + "','" + price.Text + "','" + total.Text + "','" + total.Text + "')", conn);
                try
                {
                    insert.ExecuteNonQuery();
                    MessageBox.Show("图书" + bno.Text + "新入库成功");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("插入图书失败，请检查输入数据结构！");
                } 
                conn.Close();
                return;
            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(Form1.ConnectionString);
            conn.Open();
            string[] s;
            s=richTextBox1.Text.Split('\n');
            string bno="";
            string insertnum="";
            string[] ss;
            bool issuccess = true;
            for (int i = 0; i < s.Length; i++)
            {
                //获取书号和入库数,判断是否是新书
                
                ss = s[i].Split(',');
                bno = ss[0];
                bno=bno .Replace ("(","");
                bno = bno.Replace("'", "");
                bno = bno.Trim();
                insertnum = ss[7];
                insertnum = insertnum.Replace(")", "");
                insertnum = insertnum.Replace(";", "");
                insertnum = insertnum.Trim();
                //MessageBox.Show(insertnum);
                SqlCommand check = new SqlCommand("select * from book where bno='"+bno +"'", conn);
                SqlDataReader dr = check.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    string ins = s[i];
                    ins = ins.Replace(")", "");
                    ins = ins.Replace(";", "");
                    ins = ins.Trim();
                    ins = ins + "," + insertnum + ");";
                    SqlCommand insert = new SqlCommand("insert into book values" + ins +"",conn );
                    try
                    {
                        insert.ExecuteNonQuery();
                        
                    }
                    catch (SqlException ex)
                    {
                        issuccess = false;
                        MessageBox.Show("插入图书失败，请检查输入数据结构！");
                        
                    } 
                }
                else
                {
                    dr.Close();
                    SqlCommand update = new SqlCommand("update book set total=total+" + insertnum + ",stock=stock+" + insertnum + "   where bno=" + bno +"", conn);
                    update.ExecuteNonQuery();
                }
           
            }
            conn.Close();
            if (issuccess) MessageBox.Show("图书批量入库成功");
            return;
        }
    }
}
