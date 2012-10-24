using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
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

            string sqlStr = "SELECT distinct(zy) FROM zd_zb";
            OleDbDataReader dr = DB.dataReader(sqlStr);
            cbbkzy.Items.Clear();
            cbbkzy.Items.Add(new ComboboxItem("——请选择——", ""));
            while (dr.Read())
            {
                cbbkzy.Items.Add(new ComboboxItem(dr["zy"].ToString(), dr["zy"].ToString()));
            }
            dr.Close();
            cbbkzy.SelectedIndex = 0;


            sqlStr = "SELECT distinct(fx) FROM zd_zb";

            dr = DB.dataReader(sqlStr);
            cbbkfx.Items.Clear();
            cbbkfx.Items.Add(new ComboboxItem("——请选择——", ""));
            while (dr.Read())
            {
                cbbkfx.Items.Add(new ComboboxItem(dr["fx"].ToString(), dr["fx"].ToString()));
            }
            dr.Close();
            cbbkfx.SelectedIndex = 0;

            sqlStr = "SELECT distinct(cc) FROM zd_zb";

            dr = DB.dataReader(sqlStr);
            cbbkcc.Items.Clear();
            cbbkcc.Items.Add(new ComboboxItem("——请选择——", ""));
            while (dr.Read())
            {
                cbbkcc.Items.Add(new ComboboxItem(dr["cc"].ToString(), dr["cc"].ToString()));
            }
            dr.Close();
            cbbkcc.SelectedIndex = 0;

            comboBox1.SelectedIndex = 0;
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

            string bkzy = "";
            string bkfx = "";
            string bkcc = "";

            if ((ComboboxItem)cbbkzy.SelectedItem != null)
            {
                bkzy = ((ComboboxItem)cbbkzy.SelectedItem).Value.ToString();
            }
            if ((ComboboxItem)cbbkfx.SelectedItem != null)
            {
                bkfx = ((ComboboxItem)cbbkfx.SelectedItem).Value.ToString();
            }
            if ((ComboboxItem)cbbkcc.SelectedItem != null)
            {
                bkcc = ((ComboboxItem)cbbkcc.SelectedItem).Value.ToString();
            }

            f.fx = bkfx;
            f.cc = bkcc;
            f.zy = bkzy;
            f.sd = tbsd.Text;
            f.ed = tbed.Text;
            f.a = comboBox1.SelectedItem.ToString();
            f1.Bind(f);
            Close();
        }
    }
}
