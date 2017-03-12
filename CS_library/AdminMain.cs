using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_library
{
    public partial class AdminMain : Form
    {
        public AdminMain()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            BorrowReturn borrowandreturn = new BorrowReturn();
            borrowandreturn.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Insert ins = new Insert();
            ins.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Card cardgovern = new Card();
            cardgovern.ShowDialog();
        }
    }
}
