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
    public partial class FormCreate : Form
    {
        public FormCreate()
        {
            InitializeComponent();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (tb_pc.Text != "")
            {
                TextBox si = tb_pc;
                //sheet pc duplicate            
                string sqlStr = "SELECT count(*) FROM sheets WHERE pc = '" + si.Text + "'";
                int scount = Int16.Parse(DB.excuteScalar(sqlStr));
                
                if (scount > 0)
                {
                    //open this
                    sqlStr = "UPDATE config SET pc = '" + si.Text + "'";
                    DB.excuteSql(sqlStr);
                    //start recruit
                }
                else
                {
                    //insert sheet
                    sqlStr = "INSERT INTO sheets(pc) VALUES('" + si.Text + "')";
                    DB.excuteSql(sqlStr);
                    //open this
                    sqlStr = "UPDATE config SET pc = '" + si.Text + "'";
                    DB.excuteSql(sqlStr);
                    //start recruit
                }
                FormNew f = (FormNew)Owner;
                Form1 f1 = (Form1)f.Owner;
                f1.pc = si.Text;
                f1.startRecruit();
                f.Close();
                Close();
            }
        }
    }
}
