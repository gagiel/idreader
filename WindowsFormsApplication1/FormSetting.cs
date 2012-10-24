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
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            DataTable dt = DB.dataTable("SELECT distinct(xwzd) AS xwzd FROM zd_zb");
            cb_jxzd.DataSource = dt;
            cb_jxzd.DisplayMember = "xwzd";
            cb_jxzd.ValueMember = "xwzd";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string jxzd = "";
            try
            {
                jxzd = cb_jxzd.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                jxzd = cb_jxzd.Text;
            }
            string sqlStr = "UPDATE config SET jxzd = '" + jxzd + "', jxfdd = '" + tb_jxfdd.Text + "'";
            DB.excuteSql(sqlStr);
            Form1 f = (Form1)Owner;
            f.jxzd = jxzd;
            f.jxfdd = tb_jxfdd.Text;
            Close();
        }
    }
}
