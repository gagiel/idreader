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
    public partial class FormPlan : Form
    {
        public FormPlan()
        {
            InitializeComponent();
        }

        string xwdz = "";
        int editPlan = 0;

        private void FormPlan_Load(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Owner;
            xwdz = f1.jxzd;
            BindPlan();

        }

        public void BindPlan()
        {
            string sqlStr = "SELECT * FROM zd_zb WHERE xwzd = '" + xwdz + "'";
            DataTable dt = DB.dataTable(sqlStr);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "专业";
            dataGridView1.Columns[3].HeaderText = "方向";
            dataGridView1.Columns[4].HeaderText = "层次";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            editPlan = 0;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlStr = "";
            OleDbCommand cmd = new OleDbCommand();
            if (editPlan == 0)
            {
                //insert
                sqlStr = "INSERT INTO zd_zb(xwzd, zy, fx, cc) VALUES(@xwzd, @zy, @fx, @cc)";
                cmd.CommandText = sqlStr;
                cmd.Parameters.AddWithValue("@xwzd", xwdz);
                cmd.Parameters.AddWithValue("@zy", textBox1.Text);
                cmd.Parameters.AddWithValue("@fx", textBox2.Text);
                cmd.Parameters.AddWithValue("@cc", textBox3.Text);
                DB.excuteSql(cmd);
            }
            else
            {
                //update
                sqlStr = "UPDATE zd_zb SET xwzd=@xwzd, zy=@zy, fx=@fx, cc=@cc WHERE id=@id";
                cmd.CommandText = sqlStr;
                cmd.Parameters.AddWithValue("@xwzd", xwdz);
                cmd.Parameters.AddWithValue("@zy", textBox1.Text);
                cmd.Parameters.AddWithValue("@fx", textBox2.Text);
                cmd.Parameters.AddWithValue("@cc", textBox3.Text);
                cmd.Parameters.AddWithValue("@id", editPlan);
                DB.excuteSql(cmd);
            }

            BindPlan();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            editPlan = 0;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            editPlan = Int16.Parse(dataGridView1.SelectedRows[0].Cells["id"].Value.ToString());
            textBox1.Text = dataGridView1.SelectedRows[0].Cells["zy"].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells["fx"].Value.ToString() ;
            textBox3.Text = dataGridView1.SelectedRows[0].Cells["cc"].Value.ToString();
            textBox1.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int deletePlan = Int16.Parse(dataGridView1.SelectedRows[0].Cells["id"].Value.ToString());
            //delete
            OleDbCommand cmd = new OleDbCommand();
            string sqlStr = "DELETE FROM zd_zb WHERE id=@id";
            cmd.CommandText = sqlStr;
            cmd.Parameters.AddWithValue("@id", deletePlan);
            DB.excuteSql(cmd);

            BindPlan();
        }
    }
}
