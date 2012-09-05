using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormQuery : Form
    {
        public FormQuery()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Owner;
            Filter f = new Filter();
            f.keywords = "";
            f.fx = tbfx.Text;
            f.cc = tbcc.Text;
            f.zy = tbzy.Text;
            f.sd = tbsd.Text;
            f.ed = tbed.Text;
            f.a = comboBox1.SelectedItem.ToString();
            f1.Bind(f);
            Close();
        }
    }
}
