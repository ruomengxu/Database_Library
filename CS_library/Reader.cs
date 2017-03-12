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
    public partial class Reader : Form
    {
        public Reader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string keyword = textBox1.Text;
            SqlConnection conn = new SqlConnection(Form1.ConnectionString);
            conn.Open();
            string SelectCategory="";
            string SortingCategory = "";
            switch (comboBox1.Text)
            {
                case "书名":
                    SelectCategory = "title";
                    break;
                case "类别":
                    SelectCategory = "category";
                    break;
                case "出版社":
                    SelectCategory = "press";
                    break;
                case "作者":
                    SelectCategory = "author";
                    break;
                case "书号":
                    SelectCategory = "bno";
                    break;
            }
            string searchCmd = "";
            if (radioButton1.Checked)
            {
                keyword += '%';
                keyword = '%' + keyword;
                searchCmd = "select * from book where " + SelectCategory + " like '" + @keyword + "'";
            }
            else
                searchCmd = "select * from book where " + SelectCategory + " = '" + keyword + "'";

            if (comboBox2.SelectedIndex != -1)
            {
                switch (comboBox2.Text)
                {
                    case "书名":
                        SortingCategory = "title";
                        break;
                    case "类别":
                        SortingCategory = "category";
                        break;
                    case "出版社":
                        SortingCategory = "press";
                        break;
                    case "作者":
                        SortingCategory = "author";
                        break;
                    case "书号":
                        SortingCategory = "bno";
                        break;
                    case "价格":
                        SortingCategory = "price";
                        break;
                    case "年份":
                        SortingCategory = "publish_year";
                        break;
                }
               searchCmd +=" order by " + SortingCategory + " ";
            }    
            SqlDataAdapter dataAdapter = new SqlDataAdapter(searchCmd, conn);
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
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = -1;
            radioButton1.Checked = false;

        }

        private void Reader_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double minvalue = double.Parse (textBox2.Text);
            double maxvalue = double.Parse (textBox3.Text);
            SqlConnection conn = new SqlConnection(Form1.ConnectionString);
            conn.Open();
            string SelectCategory = "";
            string SortingCategory = "";
            string searchCmd = "";
            switch (comboBox4.Text)
            {
                case "价格":
                    SelectCategory = "price";
                    break;
                case "年份":
                    SelectCategory = "publish_year";
                    break;
            }
             searchCmd = "select * from book where " + SelectCategory + " >" + minvalue + " and " + SelectCategory + " <" + maxvalue + "";
             if (comboBox3.SelectedIndex != -1)
             {
                 switch (comboBox3.Text)
                 {
                     case "书名":
                         SortingCategory = "title";
                         break;
                     case "类别":
                         SortingCategory = "category";
                         break;
                     case "出版社":
                         SortingCategory = "press";
                         break;
                     case "作者":
                         SortingCategory = "author";
                         break;
                     case "书号":
                         SortingCategory = "bno";
                         break;
                     case "价格":
                         SortingCategory = "price";
                         break;
                     case "年份":
                         SortingCategory = "publish_year";
                         break;
                 }
                 searchCmd += " order by " + SortingCategory + " ";
             }    
            SqlDataAdapter dataAdapter = new SqlDataAdapter(searchCmd, conn);
            SqlCommandBuilder commandbuilder = new SqlCommandBuilder(dataAdapter);
            DataTable table = new DataTable();
            dataAdapter.Fill(table);
            dataGridView1.DataSource = table;
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            dataGridView1.DataSource = null;
            comboBox4.SelectedIndex = 0;
            comboBox3.SelectedIndex = -1;
        }
    }
}
