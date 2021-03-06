﻿using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib.Zip;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApplication1
{
    
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public string jxzd; //教学站点
        public string jxfdd; //教学辅导点
        public string pc; //批次
        public string xlsPath;//导入excel路径

        #region idcard
        [DllImport("termb.dll")]
        private static extern int InitComm(int port); //打开端口
        [DllImport("termb.dll")]
        private static extern int CloseComm(); //关闭端口
        [DllImport("termb.dll")]
        private static extern int Authenticate(); //卡认证
        [DllImport("termb.dll")]
        private static extern int Read_Content(int active); //读取卡
        [DllImport("termb.dll")]
        private static extern int Read_Content_Path(string cPath, int active);//读取卡

        public static ICard readCard()
        {
            CloseComm();
            int ic = 0;
            ic = InitComm(1001);
            if (ic != 1)
            {
                MessageBox.Show("没有检查到可用的读卡器");
                ic = InitComm(1002);
                MessageBox.Show("测试USB2中...");
            }
            if (ic != 1)
            {
                ic = InitComm(1003);
                MessageBox.Show("测试USB3中...");
            }
            if (ic != 1)
            {
                ic = InitComm(1004);
                MessageBox.Show("测试USB4中...");
            }
            if (ic != 1)
            {
                MessageBox.Show("没有连接可用的读卡器");
            }
            int li_ret = 0;
            li_ret = Authenticate();
            Read_Content(1);

            ICard icard = new ICard();
            try
            {
                FileStream fs1 = new FileStream("wz.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs1, Encoding.GetEncoding("UCS-2"));
                string result = sr.ReadLine();
                fs1.Close();
                sr.Close();
                icard.name = result.Substring(0, 15).Trim();
                icard.sex = result.Substring(15, 1).Trim();
                icard.ethnic = result.Substring(16, 2).Trim();
                icard.birthday = result.Substring(18, 8).Trim();
                icard.address = result.Substring(26, 35).Trim();
                icard.idcardno = result.Substring(61, 18).Trim();
                icard.authority = result.Substring(79, 15).Trim();
                icard.start_dateline = result.Substring(94, 8).Trim();
                icard.end_dateline = result.Substring(102, 8).Trim();
                Image i1 = Image.FromFile("zp.bmp");
                Image i2 = new Bitmap(i1);
                icard.img = i2;
                i1.Dispose();

                FileInfo file = new FileInfo(@"wz.txt");
                if (file.Exists)
                {
                    file.Delete(); //删除单个文件
                }
                file = new FileInfo(@"zp.bmp");
                if (file.Exists)
                {
                    file.Delete(); //删除单个文件
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return icard;

        }

        public class ICard
        {
            public string name;
            public string sex;
            public string ethnic;
            public string birthday;
            public string address;
            public string idcardno;
            public string authority;
            public string start_dateline;
            public string end_dateline;
            public Image img;

            public void saveImg(string jxzd, string pc, string sfzh)
            {
                saveJpg(this.img, jxzd, pc, sfzh);
            }
        }
        #endregion

        public void checkJpg()
        {
            string sql = "select sfzh from recruit";
            OleDbDataReader dr = DB.dataReader(sql);
            string str = Application.StartupPath;
            string exists = "";
            while (dr.Read())
            {
                FileInfo file = new FileInfo(str + "/" + jxzd + "/" + pc + "/" + dr["sfzh"].ToString() + ".jpg");
                if (file.Exists)
                {
                    exists += "'" + dr["sfzh"].ToString() + "',";
                }
            }
            dr.Close();
            if (exists.Length > 1)
            {
                sql = "UPDATE recruit SET avatar = '有' WHERE sfzh in (" + exists.Substring(0, exists.Length - 1) + ")";
                DB.excuteSql(sql);
                sql = "UPDATE recruit SET avatar = '' WHERE sfzh not in (" + exists.Substring(0, exists.Length - 1) + ")";
                DB.excuteSql(sql);
            }
        }

        public static void saveJpg(Image img, string jxzd, string pc, string sfzh)
        {
            Bitmap bmp = new Bitmap(img);
            img.Dispose();
            System.Drawing.Image image = bmp;
            System.Drawing.Image newImage = image.GetThumbnailImage(bmp.Width, bmp.Height, null, new IntPtr());
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImage(newImage, 0, 0, newImage.Width, newImage.Height); //将原图画到指定的图上
            g.Dispose();
            string str = Application.StartupPath;
            Directory.CreateDirectory(str + "/" + jxzd + "/" + pc);
            try
            {
                FileInfo file = new FileInfo(str + "/" + jxzd + "/" + pc + "/" + sfzh + ".jpg");
                if (file.Exists)
                {
                    file.Delete(); //删除单个文件
                }
                newImage.Save(str + "/" + jxzd + "/" + pc + "/" + sfzh + ".jpg", ImageFormat.Jpeg);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            FormRecruit fr = new FormRecruit();
            fr.Owner = this;
            fr.ShowDialog();
        }

        public void setting_dialog()
        {
            Form f = new FormSetting();
            f.Owner = this;
            f.ShowDialog();
        }

        private void setting_btn_Click(object sender, EventArgs e)
        {
            setting_dialog();
        }

        public void Bind(Filter f)
        {
            string sqlStr;
            sqlStr = "SELECT '" + DateTime.Today.Year.ToString() + "', recruit.bmh, recruit.xm, recruit.zkzh, recruit.jxzd, recruit.jxfdd, zd_xb.mc AS xb, zd_mz.mc AS mz,  " +
                "zd_zzmm.mc AS zzmm, recruit.csrq, recruit.sfzh, recruit.bysj, recruit.gzdw, recruit.byxx, recruit.byzdm, recruit.bkzy,  " +
                "recruit.bkfx, recruit.bkcc, zd_xxxs.mc AS xxxs, recruit.txdz, recruit.yzdm, recruit.lxdh, recruit.sjh, zd_sfjxs.mc AS sfjxs,  " +
                "recruit.dateline, recruit.id, recruit.pc, recruit.avatar " +
                "FROM      (((((recruit INNER JOIN " +
                "zd_mz ON recruit.mz = zd_mz.dm) INNER JOIN " +
                "zd_sfjxs ON recruit.sfjxs = zd_sfjxs.ID) INNER JOIN " +
                "zd_xb ON recruit.xb = zd_xb.dm) INNER JOIN " +
                "zd_xxxs ON recruit.xxxs = zd_xxxs.ID) INNER JOIN " +
                "zd_zzmm ON recruit.zzmm = zd_zzmm.ID) " +
                "WHERE recruit.pc = '" + pc + "' and recruit.jxzd = '" + jxzd + "'";

            if (f.keywords != null)
            {
                string[] keywords = f.keywords.Split(' ');
                foreach (string k in keywords)
                {
                    sqlStr += " AND (recruit.nf like '%" + k + "%' OR recruit.xm like '%" + k + "%'" +
                        " OR recruit.sfzh like '%" + k + "%' OR recruit.bmh like '%" + k + "%'" +
                        " OR recruit.bkzy like '%" + k + "%' OR recruit.bkfx like '%" + k + "%'" +
                        " OR recruit.sjh like '%" + k + "%' OR recruit.dateline like '%" + k + "%'" +
                        " OR recruit.bkcc like '%" + k + "%' OR zd_xxxs.mc like '%" + k + "%')";
                }
            }
            if (f.zy != null && f.zy != "")
            { 
                sqlStr += " AND recruit.bkzy like '%" + f.zy + "%'";
            }
            if (f.fx != null && f.fx != "")
            {
                sqlStr += " AND recruit.bkfx like '%" + f.fx + "%'";
            }
            if (f.cc != null && f.cc != "")
            {
                sqlStr += " AND recruit.bkcc like '%" + f.cc + "%'";
            }
            if (f.sd != null && f.sd != "")
            {
                sqlStr += " AND recruit.dateline >= #" + f.sd + "#";
            }
            if (f.ed != null && f.ed != "")
            {
                sqlStr += " AND recruit.dateline <= #" + f.ed + "#";
            }
            if (f.ed != null && f.ed != "")
            {
                sqlStr += " AND recruit.dateline <= #" + f.ed + "#";
            }
            if (f.a != null && f.a != "")
            {
                checkJpg();
                if (f.a == "有头像")
                {
                    sqlStr += " AND recruit.avatar = '有'";
                }
                else if (f.a == "无头像")
                {
                    sqlStr += " AND (recruit.avatar = '' OR recruit.avatar is null)";
                }
            }

            System.Data.DataTable dt = DB.dataTable(sqlStr);
            DataColumn Col = dt.Columns.Add("编号", System.Type.GetType("System.Int32"));
            Col.SetOrdinal(0);// to put the column in position 0;
            foreach(DataRow dr in dt.Rows)
            {
                dr[0] = dt.Rows.IndexOf(dr) + 1;
            }
            dgv_recruit.DataSource = dt;
            dgv_recruit.Columns[0].HeaderText = "编号";
            dgv_recruit.Columns[1].HeaderText = "年份*";
            dgv_recruit.Columns[2].HeaderText = "报名号";
            //dgv_recruit.Columns[2].Visible = false;
            dgv_recruit.Columns[3].HeaderText = "姓名*";
            dgv_recruit.Columns[4].HeaderText = "准考证号";
            //dgv_recruit.Columns[4].Visible = false;
            dgv_recruit.Columns[5].HeaderText = "教学站点*";
            //dgv_recruit.Columns[5].Visible = false;
            dgv_recruit.Columns[6].HeaderText = "教学辅导点";
            //dgv_recruit.Columns[6].Visible = false;
            dgv_recruit.Columns[7].HeaderText = "性别*";
            //dgv_recruit.Columns[7].Visible = false;
            dgv_recruit.Columns[8].HeaderText = "民族*";
            //dgv_recruit.Columns[8].Visible = false;
            dgv_recruit.Columns[9].HeaderText = "政治面貌*";
            //dgv_recruit.Columns[9].Visible = false;
            dgv_recruit.Columns[10].HeaderText = "出生日期(格式为：20010101)*";
            //dgv_recruit.Columns[10].Visible = false;
            dgv_recruit.Columns[11].HeaderText = "身份证号*";
            //dgv_recruit.Columns[11].Visible = false;
            dgv_recruit.Columns[12].HeaderText = "毕业时间(格式为：200101)";
            //dgv_recruit.Columns[12].Visible = false;
            dgv_recruit.Columns[13].HeaderText = "工作单位";
            //dgv_recruit.Columns[13].Visible = false;
            dgv_recruit.Columns[14].HeaderText = "毕业学校";
            //dgv_recruit.Columns[14].Visible = false;
            dgv_recruit.Columns[15].HeaderText = "毕业证代码";
            //dgv_recruit.Columns[15].Visible = false;
            dgv_recruit.Columns[16].HeaderText = "报考专业*";
            dgv_recruit.Columns[17].HeaderText = "报考方向*";
            dgv_recruit.Columns[18].HeaderText = "报考层次*";
            dgv_recruit.Columns[19].HeaderText = "学习形式*";
            dgv_recruit.Columns[20].HeaderText = "通信地址";
            //dgv_recruit.Columns[20].Visible = false;
            dgv_recruit.Columns[21].HeaderText = "邮政代码";
            //dgv_recruit.Columns[21].Visible = false;
            dgv_recruit.Columns[22].HeaderText = "联系电话";
            //dgv_recruit.Columns[22].Visible = false;
            dgv_recruit.Columns[23].HeaderText = "手机号";
            dgv_recruit.Columns[24].HeaderText = "是否进修生*";
            //dgv_recruit.Columns[24].Visible = false;
            dgv_recruit.Columns[25].HeaderText = "报名日期";
            dgv_recruit.Columns[26].Visible = false;
            dgv_recruit.Columns[27].Visible = false;
            dgv_recruit.Columns[28].HeaderText = "头像";

            dgv_recruit.AutoResizeColumns();
            dgv_recruit.AutoResizeColumnHeadersHeight();
        }

        public void initDb()
        {
            int cc = Int16.Parse(DB.excuteScalar("SELECT count(*) FROM config"));
            if (cc == 0)
            {
                DB.excuteSql("INSERT INTO config(jxzd) VALUES('')");
            }

            jxzd = DB.excuteScalar("SELECT jxzd FROM config WHERE id = 1");
            if (jxzd == "" || jxzd == "none")
            {
                setting_dialog();
            }
            else
            {
                return;
            }
            initDb();
        }

        public void closeRecruit()
        {
            btn_add.Enabled = false;
            btn_edit.Enabled = false;
            btn_del.Enabled = false;
            btn_search.Enabled = false;
            tb_query.Enabled = false;
            btn_export.Enabled = false;
            btn_export_pic.Enabled = false;
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;
        }

        public void startRecruit()
        {
            btn_add.Enabled = true;
            btn_edit.Enabled = true;
            btn_del.Enabled = true;
            btn_search.Enabled = true;
            tb_query.Enabled = true;
            btn_export.Enabled = true;
            btn_export_pic.Enabled = true;
            toolStripButton1.Enabled = true;
            toolStripButton2.Enabled = true;
            cb_avatar.SelectedIndex = 0;
            Bind(new Filter());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initDb();
            closeRecruit();
            Text = jxzd + "报名系统";
        }

        private void start_pc_Click(object sender, EventArgs e)
        {
            Form f = new FormNew();
            f.Owner = this;
            f.ShowDialog();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            Filter f = new Filter();
            f.keywords = tb_query.Text;
            if (cb_avatar.SelectedItem!=null)
            {
                f.a = cb_avatar.SelectedItem.ToString();
            }            
            Bind(f);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            try
            {
                int rid = Int16.Parse(dgv_recruit.SelectedRows[0].Cells["id"].Value.ToString());
                FormRecruit fr = new FormRecruit(rid);
                fr.Owner = this;
                fr.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("编辑出现错误，请先选择要编辑的行。" + ex.Message);
                return;
            }      
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            int rid = Int16.Parse(dgv_recruit.SelectedRows[0].Cells["id"].Value.ToString());
            string sqlStr = "DELETE FROM recruit WHERE id = " + rid.ToString();
            DB.excuteSql(sqlStr);

            Filter f = new Filter();
            f.keywords = tb_query.Text;
            Bind(f);
        }

        public void SaveAs(DataGridView gridView)
        {
            //导出到execl
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "导出Excel (*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CreatePrompt = true;
                saveFileDialog.Title = "导出文件保存路径";
                saveFileDialog.ShowDialog();
                string strName = saveFileDialog.FileName;
                if (strName.Length != 0)
                {
                    toolStripProgressBar1.Visible = true;
                    System.Reflection.Missing miss = System.Reflection.Missing.Value;
                    Excel.Application excel = new Excel.Application();
                    // Microsoft.Office.Interop.Excel.ApplicationClass excel = new Microsoft.Office.Interop.Excel.ApplicationClass();
                    excel.Workbooks.Add(true); ;
                    // excel.Visible = false;//若是true，则在导出的时候会显示EXcel界面。
                    if (excel == null)
                    {
                        MessageBox.Show("EXCEL无法启动！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Microsoft.Office.Interop.Excel.Workbooks books = (Microsoft.Office.Interop.Excel.Workbooks)excel.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook book = (Microsoft.Office.Interop.Excel.Workbook)(books.Add(miss));
                    Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)book.ActiveSheet;
                    sheet.Name = "Sheet1";

                    //表头
                    for (int i = 2; i < gridView.ColumnCount - 3; i++)
                    {
                        excel.Cells[1, i - 1] = gridView.Columns[i - 1].HeaderText.ToString();

                    }

                    //填充数据
                    for (int i = 0; i < gridView.RowCount; i++)
                    {
                        //j也是从1开始  原因如上  每个人需求不一样
                        for (int j = 1; j < gridView.ColumnCount - 4; j++)
                        {
                            if (gridView[j, i].Value.GetType() == typeof(string))
                            {
                                excel.Cells[i + 2, j] = "'" + gridView[j, i].Value.ToString();
                            }
                            else
                            {
                                excel.Cells[i + 2, j] = gridView[j, i].Value.ToString();
                            }


                        }

                        toolStripStatusLabel1.Text = "正在导出..." + i + "/" + gridView.RowCount;
                        toolStripProgressBar2.Value = (i * 100 / gridView.RowCount);
                    }

                    sheet.SaveAs(strName, miss, miss, miss, miss, miss, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, miss, miss, miss);
                    book.Close(false, miss, miss);
                    books.Close();
                    excel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                    GC.Collect();
                    toolStripStatusLabel1.Text = "就绪";
                    MessageBox.Show("数据已经成功导出!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    toolStripProgressBar2.Value = 0;

                    System.Diagnostics.Process.Start(strName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误提示");
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            SaveAs(dgv_recruit);
        }

        private void btn_export_pic_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "导出压缩包 (*.zip)|*.zip";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CreatePrompt = true;
                saveFileDialog.Title = "导出文件保存路径";
                saveFileDialog.ShowDialog();
                string strName = saveFileDialog.FileName;
                if (strName.Length != 0)
                {
                    FastZip zip = new FastZip();
                    if (Directory.Exists("tmp")) Directory.Delete(@"tmp", true);
                    if (!Directory.Exists("tmp")) Directory.CreateDirectory("tmp");
                    toolStripProgressBar1.Visible = true;

                    //填充数据
                    for (int i = 0; i < dgv_recruit.RowCount; i++)
                    {
                        try
                        {
                            File.Copy(Application.StartupPath + "/" + dgv_recruit.Rows[i].Cells["jxzd"].Value.ToString() + "/" + dgv_recruit.Rows[i].Cells["pc"].Value.ToString() + "/" + dgv_recruit.Rows[i].Cells["sfzh"].Value.ToString() + ".jpg", "tmp/" + dgv_recruit.Rows[i].Cells["sfzh"].Value.ToString() + ".jpg", true);
                        }
                        catch (Exception ex)
                        {
                        }
                        
                        toolStripProgressBar1.Value += 100 / dgv_recruit.RowCount;
                    }
                    FileStream fs = new System.IO.FileStream(strName, FileMode.Create);
                    zip.CreateZip(fs, "tmp", true, "", "");
                    MessageBox.Show("数据已经成功导出!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    toolStripProgressBar1.Value = 0;
                    if (Directory.Exists("tmp")) Directory.Delete(@"tmp", true);
                    fs.Close();
                    System.Diagnostics.Process.Start(strName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误提示");
                return;
            }
        }

        private void btn_zsjh_Click(object sender, EventArgs e)
        {
            Form f = new FormPlan();
            f.Owner = this;
            f.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form f = new FormQuery();
            f.Owner = this;
            f.ShowDialog();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择要导入的Excel";
            ofd.Filter = "Excel 97-2003(*.xls)|*.xls|Excel 2007(*.xlsx)|*.xlsx";
            ofd.ShowDialog();
            xlsPath = ofd.FileName;
            if (!File.Exists(ofd.FileName))
            {
                MessageBox.Show("Excel为空");
                return;
            }
            else
            {
                ImportData();
            }
        }
        public string get_string(object str)
        {
            try
            {
                return str.ToString().Trim();
            }
            catch
            {
                return "";
            }
        }
        public void ImportData()
        {
            Excel.Application ObjExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook;
            Excel.Worksheet ObjWorkSheet;
            ObjWorkBook = ObjExcel.Workbooks.Open(xlsPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1];
            try
            {

                for (int i = 2; i <= ObjWorkSheet.UsedRange.Rows.Count; i++)
                {
                    toolStripStatusLabel1.Text = "正在导入..." + i + "/" + ObjWorkSheet.UsedRange.Rows.Count;
                    string selectSql = "SELECT id FROM zd_zzmm WHERE mc = '" + get_string(((Excel.Range)ObjWorkSheet.Cells[i, 9]).Value2) + "'";
                    int zzmm = Int16.Parse(DB.excuteScalar(selectSql));
                    selectSql = "SELECT id FROM zd_xxxs WHERE mc = '" + get_string(((Excel.Range)ObjWorkSheet.Cells[i, 19]).Value2) + "'";
                    int xxxs = Int16.Parse(DB.excuteScalar(selectSql));
                    selectSql = "SELECT id FROM zd_sfjxs WHERE mc = '" + get_string(((Excel.Range)ObjWorkSheet.Cells[i, 24]).Value2) + "'";
                    int sfjxs = Int16.Parse(DB.excuteScalar(selectSql));
                    selectSql = "SELECT dm FROM zd_mz WHERE mc = '" + get_string(((Excel.Range)ObjWorkSheet.Cells[i, 8]).Value2) + "'";
                    int mz = Int16.Parse(DB.excuteScalar(selectSql));
                    selectSql = "SELECT dm FROM zd_xb WHERE mc = '" + get_string(((Excel.Range)ObjWorkSheet.Cells[i, 7]).Value2) + "'";
                    int xb = Int16.Parse(DB.excuteScalar(selectSql));

                    selectSql = "SELECT count(*) FROM recruit WHERE sfzh = '" + get_string(((Excel.Range)ObjWorkSheet.Cells[i, 11]).Value2) + "'";
                    int has = Int16.Parse(DB.excuteScalar(selectSql));
                    if (has == 0)
                    {
                        string sqlStr = "INSERT INTO recruit(nf,bmh,xm,zkzh,jxzd,jxfdd,xb,mz,zzmm,csrq,sfzh,bysj,gzdw,byxx," +
                           "byzdm,bkzy,bkfx,bkcc,xxxs,txdz,yzdm,lxdh,sjh,sfjxs,pc,dateline) VALUES(@nf,@bmh,@xm,@zkzh," +
                           "@jxzd,@jxfdd,@xb,@mz,@zzmm,@csrq,@sfzh,@bysj,@gzdw,@byxx,@byzdm,@bkzy,@bkfx,@bkcc,@xxxs," +
                           "@txdz,@yzdm,@lxdh,@sjh,@sfjxs,@pc,@dateline)";
                        OleDbCommand bat_cmd = new OleDbCommand();
                        bat_cmd.CommandText = sqlStr;
                        bat_cmd.Parameters.AddWithValue("@nf", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 1]).Value2));
                        bat_cmd.Parameters.AddWithValue("@bmh", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 2]).Value2));
                        bat_cmd.Parameters.AddWithValue("@xm", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 3]).Value2));
                        bat_cmd.Parameters.AddWithValue("@zkzh", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 4]).Value2));
                        bat_cmd.Parameters.AddWithValue("@jxzd", jxzd);
                        bat_cmd.Parameters.AddWithValue("@jxfdd", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 6]).Value2));
                        bat_cmd.Parameters.AddWithValue("@xb", xb);
                        bat_cmd.Parameters.AddWithValue("@mz", mz);
                        bat_cmd.Parameters.AddWithValue("@zzmm", zzmm);
                        bat_cmd.Parameters.AddWithValue("@csrq", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 10]).Value2));
                        bat_cmd.Parameters.AddWithValue("@sfzh", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 11]).Value2));
                        bat_cmd.Parameters.AddWithValue("@bysj", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 12]).Value2));
                        bat_cmd.Parameters.AddWithValue("@gzdw", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 13]).Value2));
                        bat_cmd.Parameters.AddWithValue("@byxx", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 14]).Value2));
                        bat_cmd.Parameters.AddWithValue("@byzdm", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 15]).Value2));
                        bat_cmd.Parameters.AddWithValue("@bkzy", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 16]).Value2));
                        bat_cmd.Parameters.AddWithValue("@bkfx", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 17]).Value2));
                        bat_cmd.Parameters.AddWithValue("@bkcc", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 18]).Value2));
                        bat_cmd.Parameters.AddWithValue("@xxxs", xxxs);
                        bat_cmd.Parameters.AddWithValue("@txdz", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 20]).Value2));
                        bat_cmd.Parameters.AddWithValue("@yzdm", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 21]).Value2));
                        bat_cmd.Parameters.AddWithValue("@lxdh", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 22]).Value2));
                        bat_cmd.Parameters.AddWithValue("@sjh", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 23]).Value2));
                        bat_cmd.Parameters.AddWithValue("@sfjxs", sfjxs);
                        bat_cmd.Parameters.AddWithValue("@pc", this.pc);
                        bat_cmd.Parameters.AddWithValue("@dateline", DateTime.Today);
                        DB.excuteSql(bat_cmd);

                        sqlStr = "SELECT count(*) FROM zd_zb WHERE xwzd = '" + jxzd + "' AND zy = '" + get_string(((Excel.Range)ObjWorkSheet.Cells[i, 16]).Value2) + "' AND fx ='" + get_string(((Excel.Range)ObjWorkSheet.Cells[i, 17]).Value2) + "' AND cc = '" + get_string(((Excel.Range)ObjWorkSheet.Cells[i, 18]).Value2) + "'";
                        if (DB.excuteScalar(sqlStr) == "0")
                        {
                            sqlStr = "INSERT INTO zd_zb(xwzd, zy, fx, cc) VALUES(@xwzd, @zy, @fx, @cc);";
                            OleDbCommand cmd = new OleDbCommand();
                            cmd.CommandText = sqlStr;
                            cmd.Parameters.AddWithValue("@xwzd", jxzd);
                            cmd.Parameters.AddWithValue("@zy", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 16]).Value2));
                            cmd.Parameters.AddWithValue("@fx", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 17]).Value2));
                            cmd.Parameters.AddWithValue("@cc", get_string(((Excel.Range)ObjWorkSheet.Cells[i, 18]).Value2));
                            DB.excuteSql(cmd); //处理数据
                        }
                    }
                    toolStripProgressBar2.Value = ((i * 100) / ObjWorkSheet.UsedRange.Rows.Count);
                    Application.DoEvents();

                }
                Bind(new Filter());
                MessageBox.Show("导入成功");
                toolStripProgressBar2.Value = 0;
                toolStripStatusLabel1.Text = "就绪";
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败，导入数据格式错误。" + ex.Message);
                return;
            }
            
            //string strCon;
            //strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + xlsPath + "';Extended Properties='Excel 12.0;HDR=YES'";
            //OleDbConnection con = new OleDbConnection(strCon);
            //DataSet ds = new DataSet();
            //try
            //{
            //    OleDbDataAdapter da = new OleDbDataAdapter("select * from [学生信息$]", con);
            //    da.Fill(ds);
            //}
            //catch (Exception ex)
            //{
            //    OleDbDataAdapter da = new OleDbDataAdapter("select * from [sheet1$]", con);
            //    da.Fill(ds);
            //}

            //try
            //{
            //    int row_count = 0;
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        row_count += 1;
            //        toolStripStatusLabel1.Text = "正在导入..." + row_count + "/" + ds.Tables[0].Rows.Count;
            //        string selectSql = "SELECT id FROM zd_zzmm WHERE mc = '" + dr[8].ToString() + "'";
            //        int zzmm = Int16.Parse(DB.excuteScalar(selectSql));
            //        selectSql = "SELECT id FROM zd_xxxs WHERE mc = '" + dr[18].ToString() + "'";
            //        int xxxs = Int16.Parse(DB.excuteScalar(selectSql));
            //        selectSql = "SELECT id FROM zd_sfjxs WHERE mc = '" + dr[23].ToString() + "'";
            //        int sfjxs = Int16.Parse(DB.excuteScalar(selectSql));
            //        selectSql = "SELECT dm FROM zd_mz WHERE mc = '" + dr[7].ToString() + "'";
            //        int mz = Int16.Parse(DB.excuteScalar(selectSql));
            //        selectSql = "SELECT dm FROM zd_xb WHERE mc = '" + dr[6].ToString() + "'";
            //        int xb = Int16.Parse(DB.excuteScalar(selectSql));

            //        selectSql = "SELECT count(*) FROM recruit WHERE sfzh = '" + dr[10].ToString() + "'";
            //        int has = Int16.Parse(DB.excuteScalar(selectSql));
            //        if (has == 0)
            //        {
            //            string sqlStr = "INSERT INTO recruit(nf,bmh,xm,zkzh,jxzd,jxfdd,xb,mz,zzmm,csrq,sfzh,bysj,gzdw,byxx," +
            //               "byzdm,bkzy,bkfx,bkcc,xxxs,txdz,yzdm,lxdh,sjh,sfjxs,pc,dateline) VALUES(@nf,@bmh,@xm,@zkzh," +
            //               "@jxzd,@jxfdd,@xb,@mz,@zzmm,@csrq,@sfzh,@bysj,@gzdw,@byxx,@byzdm,@bkzy,@bkfx,@bkcc,@xxxs," +
            //               "@txdz,@yzdm,@lxdh,@sjh,@sfjxs,@pc,@dateline)";
            //            OleDbCommand cmd = new OleDbCommand();
            //            cmd.CommandText = sqlStr;
            //            cmd.Parameters.AddWithValue("@nf", dr[0].ToString());
            //            cmd.Parameters.AddWithValue("@bmh", dr[1].ToString());
            //            cmd.Parameters.AddWithValue("@xm", dr[2].ToString());
            //            cmd.Parameters.AddWithValue("@zkzh", dr[3].ToString());
            //            cmd.Parameters.AddWithValue("@jxzd", jxzd);
            //            cmd.Parameters.AddWithValue("@jxfdd", dr[5].ToString());
            //            cmd.Parameters.AddWithValue("@xb", xb);
            //            cmd.Parameters.AddWithValue("@mz", mz);
            //            cmd.Parameters.AddWithValue("@zzmm", zzmm);
            //            cmd.Parameters.AddWithValue("@csrq", dr[9].ToString());
            //            cmd.Parameters.AddWithValue("@sfzh", dr[10].ToString());
            //            cmd.Parameters.AddWithValue("@bysj", dr[11].ToString());
            //            cmd.Parameters.AddWithValue("@gzdw", dr[12].ToString());
            //            cmd.Parameters.AddWithValue("@byxx", dr[13].ToString());
            //            cmd.Parameters.AddWithValue("@byzdm", dr[14].ToString());
            //            cmd.Parameters.AddWithValue("@bkzy", dr[15].ToString());
            //            cmd.Parameters.AddWithValue("@bkfx", dr[16].ToString());
            //            cmd.Parameters.AddWithValue("@bkcc", dr[17].ToString());
            //            cmd.Parameters.AddWithValue("@xxxs", xxxs);
            //            cmd.Parameters.AddWithValue("@txdz", dr[19].ToString());
            //            cmd.Parameters.AddWithValue("@yzdm", dr[20].ToString());
            //            cmd.Parameters.AddWithValue("@lxdh", dr[21].ToString());
            //            cmd.Parameters.AddWithValue("@sjh", dr[22].ToString());
            //            cmd.Parameters.AddWithValue("@sfjxs", sfjxs);
            //            cmd.Parameters.AddWithValue("@pc", this.pc);
            //            cmd.Parameters.AddWithValue("@dateline", DateTime.Today);
            //            DB.excuteSql(cmd); //处理数据

            //            sqlStr = "SELECT count(*) FROM zd_zb WHERE xwzd = '" + jxzd + "' AND zy = '" + dr[15].ToString() + "' AND fx ='" + dr[16].ToString() + "' AND cc = '" + dr[17].ToString() + "'";
            //            if(DB.excuteScalar(sqlStr) == "0"){
            //                sqlStr = "INSERT INTO zd_zb(xwzd, zy, fx, cc) VALUES(@xwzd, @zy, @fx, @cc);";
            //                cmd = new OleDbCommand();
            //                cmd.CommandText = sqlStr;
            //                cmd.Parameters.AddWithValue("@xwzd", jxzd);
            //                cmd.Parameters.AddWithValue("@zy", dr[15].ToString());
            //                cmd.Parameters.AddWithValue("@fx", dr[16].ToString());
            //                cmd.Parameters.AddWithValue("@cc", dr[17].ToString());
            //                DB.excuteSql(cmd); //处理数据
            //            }
            //        }
            //        toolStripProgressBar2.Value = ((row_count * 100) / ds.Tables[0].Rows.Count);
            //        Application.DoEvents(); 
            //    }
            //    Bind(new Filter());
            //    MessageBox.Show("导入成功");
            //    toolStripProgressBar2.Value = 0;
            //    toolStripStatusLabel1.Text = "就绪";

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("导入失败，导入数据格式错误。" + ex.Message);
            //    return;
            //}
        }

        private void dgv_recruit_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rid = Int16.Parse(dgv_recruit.SelectedRows[0].Cells["id"].Value.ToString());
            FormRecruit fr = new FormRecruit(rid);
            fr.Owner = this;
            fr.ShowDialog();
        }
    }
}
