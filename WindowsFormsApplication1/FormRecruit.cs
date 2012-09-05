using System;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace WindowsFormsApplication1
{
    public partial class FormRecruit : Form
    {
        public FormRecruit()
        {
            InitializeComponent();
            rid = 0;
        }

        public FormRecruit(int id)
        {
            InitializeComponent();
            rid = id;
        }

        public string jxzd;
        public string jxfdd;
        public string pc;
        public string picPath;
        public int rid = 0;
        Form1 fowner = null;
        Form1.ICard icard = null;
        private bool rePic = false;

        public void clearCard()
        {
            tbxm.Text = "";
            cbxb.SelectedIndex = 0;
            cbmz.SelectedIndex = 0;
            tbsfzh.Text = "";
            tbcsrq.Text = "";
            tbtxdz.Text = "";
            tbsjh.Text = "";
            pb_cun.Image = null;
            btn_pic.Visible = true;
        }

        public void setCard()
        {
            icard = Form1.readCard();
            if (icard != null)
            {
                string selectSql = "SELECT * FROM recruit WHERE sfzh = '" + icard.idcardno + "'";
                OleDbDataReader dr = DB.dataReader(selectSql);
                if (dr.Read())
                {
                    rid = Int16.Parse(dr["id"].ToString());
                    dr.Close();
                    this.FormRecruit_Load(this, null);
                }
                else
                {
                    dr.Close();
                    tbxm.Text = icard.name;
                    cbxb.SelectedValue = icard.sex;
                    cbmz.SelectedValue = Int16.Parse(icard.ethnic).ToString();
                    tbsfzh.Text = icard.idcardno;
                    tbcsrq.Text = icard.birthday;
                    tbtxdz.Text = icard.address;
                    pb_cun.Image = icard.img;
                    btn_pic.Visible = false;
                }
            }
            else
            {
                clearCard();
                btn_pic.Visible = true;
            }
        }

        public void BindZy(string jxzd, string fx, string cc)
        {
            string sqlStr = "SELECT distinct(zy) FROM zd_zb WHERE xwzd = '" + jxzd + "'";  //zy
            if (fx != "") sqlStr += " AND fx = '" + fx + "'";
            if (cc != "") sqlStr += " AND cc = '" + cc + "'";

            OleDbDataReader dr = DB.dataReader(sqlStr);
            cbbkzy.Items.Clear();
            cbbkzy.Items.Add(new ComboboxItem("——请选择——", ""));
            while (dr.Read())
            {
                cbbkzy.Items.Add(new ComboboxItem(dr["zy"].ToString(), dr["zy"].ToString()));
            }
            dr.Close();
            cbbkzy.SelectedIndex = 0;
        }

        public void BindFx(string jxzd, string zy)
        {
            string sqlStr = "SELECT distinct(fx) FROM zd_zb WHERE xwzd = '" + jxzd + "'";  //zy
            if (zy != "") sqlStr += " AND zy = '" + zy + "'";

            OleDbDataReader dr = DB.dataReader(sqlStr);
            cbbkfx.Items.Clear();
            cbbkfx.Items.Add(new ComboboxItem("——请选择——", ""));
            while (dr.Read())
            {
                cbbkfx.Items.Add(new ComboboxItem(dr["fx"].ToString(), dr["fx"].ToString()));
            }
            dr.Close();
            cbbkfx.SelectedIndex = 0;
        }

        public void BindCc(string jxzd, string zy, string fx)
        {
            string sqlStr = "SELECT distinct(cc) FROM zd_zb WHERE xwzd = '" + jxzd + "'";  //zy
            if (zy != "") sqlStr += " AND zy = '" + zy + "'";
            if (fx != "") sqlStr += " AND fx = '" + fx + "'";

            OleDbDataReader dr = DB.dataReader(sqlStr);
            cbbkcc.Items.Clear();
            cbbkcc.Items.Add(new ComboboxItem("——请选择——", ""));
            while (dr.Read())
            {
                cbbkcc.Items.Add(new ComboboxItem(dr["cc"].ToString(), dr["cc"].ToString()));
            }
            dr.Close();
            cbbkcc.SelectedIndex = 0;
        }

        private void FormRecruit_Load(object sender, EventArgs e)
        {
            this.zd_sfjxsTableAdapter.Fill(this.dbDataSet.zd_sfjxs);
            this.zd_zzmmTableAdapter.Fill(this.dbDataSet.zd_zzmm);
            this.zd_xbTableAdapter.Fill(this.dbDataSet.zd_xb);
            this.zd_mzTableAdapter.Fill(this.dbDataSet.zd_mz);
            this.zd_xxxsTableAdapter.Fill(this.dbDataSet.zd_xxxs);

            if (fowner == null) fowner = (Form1)Owner;
            jxzd = fowner.jxzd;
            jxfdd = fowner.jxfdd;
            pc = fowner.pc;

            cbmz.SelectedIndex = 0;
            cbxb.SelectedIndex = 0;
            cbzzmm.SelectedIndex = 0;
            cbsfjxs.SelectedIndex = 1;
            tbjxfdd.Text = jxfdd;

            BindZy(jxzd, "", "");
            BindFx(jxzd, "");
            BindCc(jxzd, "", "");

            if (rid != 0)
            {
                string sqlStr = "SELECT * FROM recruit WHERE id = " + rid.ToString();
                OleDbDataReader dr = DB.dataReader(sqlStr);
                string bkzy, bkfx, bkcc;
                if (dr.Read())
                {
                    tbbmh.Text = dr["bmh"].ToString();
                    tbxm.Text = dr["xm"].ToString();
                    tbzkzh.Text = dr["zkzh"].ToString();
                    tbjxfdd.Text = dr["jxfdd"].ToString();
                    cbxb.SelectedValue = dr["xb"].ToString();
                    cbmz.SelectedValue = dr["mz"].ToString();
                    cbzzmm.SelectedValue = dr["zzmm"].ToString();
                    tbcsrq.Text = dr["csrq"].ToString();
                    tbsfzh.Text = dr["sfzh"].ToString();
                    tbbysj.Text = dr["bysj"].ToString();
                    tbgzdw.Text = dr["gzdw"].ToString();
                    tbbyxx.Text = dr["byxx"].ToString();
                    tbbyzdm.Text = dr["byzdm"].ToString();                    
                    cbxxxs.SelectedValue = dr["xxxs"].ToString();
                    tbtxdz.Text = dr["txdz"].ToString();
                    tbyzdm.Text = dr["yzdm"].ToString();
                    tblxdh.Text = dr["lxdh"].ToString();
                    tbsjh.Text = dr["sjh"].ToString();
                    cbsfjxs.SelectedValue = dr["sfjxs"].ToString();
                    
                    bkzy= dr["bkzy"].ToString();
                    bkfx = dr["bkfx"].ToString();
                    bkcc = dr["bkcc"].ToString();
                    
                    try
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(Application.StartupPath + "/" + jxzd + "/" + pc + "/" + dr["sfzh"].ToString() + ".jpg");
                        System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                        img.Dispose();
                        pb_cun.Image = bmp;
                    }
                    catch (Exception ex)
                    {
                    }
                    dr.Close();
                    foreach (ComboboxItem ci in cbbkzy.Items)
                    {
                        if (ci.Value.ToString() == bkzy)
                        {
                            cbbkzy.SelectedIndex = cbbkzy.Items.IndexOf(ci);
                        }
                    }
                    foreach (ComboboxItem ci in cbbkfx.Items)
                    {
                        if (ci.Value.ToString() == bkfx)
                        {
                            cbbkfx.SelectedIndex = cbbkfx.Items.IndexOf(ci);
                        }
                    }
                    foreach (ComboboxItem ci in cbbkcc.Items)
                    {
                        if (ci.Value.ToString() == bkcc)
                        {
                            cbbkcc.SelectedIndex = cbbkcc.Items.IndexOf(ci);
                        }
                    }
                }
                else
                {
                    dr.Close();
                    Close();
                }
            }

            this.cbbkzy.SelectedIndexChanged += new System.EventHandler(this.cbbkzy_SelectedIndexChanged);
            this.cbbkfx.SelectedIndexChanged += new System.EventHandler(this.cbbkfx_SelectedIndexChanged);
            this.cbbkcc.SelectedIndexChanged += new System.EventHandler(this.cbbkcc_SelectedIndexChanged);
            
        }

        private void save_Click(object sender, EventArgs e)
        {
            string nf = DateTime.Today.Year.ToString();
            string bmh = tbbmh.Text;
            string xm = tbxm.Text;
            string zkzh = tbzkzh.Text;
            string jxfdd = tbjxfdd.Text;
            string xb = cbxb.SelectedValue.ToString();
            string mz = cbmz.SelectedValue.ToString();
            int zzmm = Int16.Parse(cbzzmm.SelectedValue.ToString());
            string csrq = tbcsrq.Text;
            string sfzh = tbsfzh.Text;
            string bysj = tbbysj.Text;
            string gzdw = tbgzdw.Text;
            string byxx = tbbyxx.Text;
            string byzdm = tbbyzdm.Text;
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
            int xxxs = Int16.Parse(cbxxxs.SelectedValue.ToString());
            string txdz = tbtxdz.Text;
            string yzdm = tbyzdm.Text;
            string lxdh = tblxdh.Text;
            string sjh = tbsjh.Text;
            int sfjxs = Int16.Parse(cbsfjxs.SelectedValue.ToString());

            //校验身份证
            string sqlStr = "SELECT COUNT(*) FROM recruit WHERE sfzh = '" + sfzh + "' AND pc ='" + pc + "'";
            int rcount = Int16.Parse(DB.excuteScalar(sqlStr));
            if (rcount > 0 && rid == 0)
            {
                MessageBox.Show("身份证重复，此人在此批次中已经报名过");
            }
            else if (xm == "") MessageBox.Show("姓名不能为空");
            else if (mz == "") MessageBox.Show("民族不能为空");
            else if (sfzh == "") MessageBox.Show("身份证号不能为空");
            else if (xb == "") MessageBox.Show("性别不能为空");
            else if (zzmm == 0) MessageBox.Show("政治面貌不能为空");
            else if (bkzy == "") MessageBox.Show("报考专业不能为空");
            else if (bkcc == "") MessageBox.Show("报考层次不能为空");
            else if (xxxs == 0) MessageBox.Show("学习形式不能为空");
            else if (sfjxs == 0) MessageBox.Show("是否进修生不能为空");
            else if (sjh == "") MessageBox.Show("手机号码不能为空");
            else
            {
                //验证通过
                if (rid == 0)
                {
                    sqlStr = "INSERT INTO recruit(nf,bmh,xm,zkzh,jxzd,jxfdd,xb,mz,zzmm,csrq,sfzh,bysj,gzdw,byxx," +
                        "byzdm,bkzy,bkfx,bkcc,xxxs,txdz,yzdm,lxdh,sjh,sfjxs,pc,dateline) VALUES(@nf,@bmh,@xm,@zkzh," +
                        "@jxzd,@jxfdd,@xb,@mz,@zzmm,@csrq,@sfzh,@bysj,@gzdw,@byxx,@byzdm,@bkzy,@bkfx,@bkcc,@xxxs," +
                        "@txdz,@yzdm,@lxdh,@sjh,@sfjxs,@pc,@dateline)";
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = sqlStr;
                    cmd.Parameters.AddWithValue("@nf", nf);
                    cmd.Parameters.AddWithValue("@bmh", bmh);
                    cmd.Parameters.AddWithValue("@xm", xm);
                    cmd.Parameters.AddWithValue("@zkzh", zkzh);
                    cmd.Parameters.AddWithValue("@jxzd", jxzd);
                    cmd.Parameters.AddWithValue("@jxfdd", jxfdd);
                    cmd.Parameters.AddWithValue("@xb", xb);
                    cmd.Parameters.AddWithValue("@mz", mz);
                    cmd.Parameters.AddWithValue("@zzmm", zzmm);
                    cmd.Parameters.AddWithValue("@csrq", csrq);
                    cmd.Parameters.AddWithValue("@sfzh", sfzh);
                    cmd.Parameters.AddWithValue("@bysj", bysj);
                    cmd.Parameters.AddWithValue("@gzdw", gzdw);
                    cmd.Parameters.AddWithValue("@byxx", byxx);
                    cmd.Parameters.AddWithValue("@byzdm", byzdm);
                    cmd.Parameters.AddWithValue("@bkzy", bkzy);
                    cmd.Parameters.AddWithValue("@bkfx", bkfx);
                    cmd.Parameters.AddWithValue("@bkcc", bkcc);
                    cmd.Parameters.AddWithValue("@xxxs", xxxs);
                    cmd.Parameters.AddWithValue("@txdz", txdz);
                    cmd.Parameters.AddWithValue("@yzdm", yzdm);
                    cmd.Parameters.AddWithValue("@lxdh", lxdh);
                    cmd.Parameters.AddWithValue("@sjh", sjh);
                    cmd.Parameters.AddWithValue("@sfjxs", sfjxs);
                    cmd.Parameters.AddWithValue("@pc", pc);
                    cmd.Parameters.AddWithValue("@dateline", DateTime.Today);
                    DB.excuteSql(cmd); //处理数据
                    if (icard != null) icard.saveImg(jxzd, pc, sfzh);//身份证图片
                    else if (pb_cun.Image != null) Form1.saveJpg(pb_cun.Image, jxzd, pc, sfzh);//上传的图片
                    clearCard();
                    fowner.startRecruit();
                }
                else
                {
                    sqlStr = "UPDATE recruit SET nf = @nf, bmh = @bmh, xm = @xm, zkzh = @zkzh, jxzd = @jxzd"+
                        ", jxfdd = @jxfdd, xb = @xb, mz = @mz, zzmm = @zzmm, csrq = @csrq, sfzh = @sfzh, bysj = @bysj"+
                        ", gzdw = @gzdw, byxx = @byxx, byzdm = @byzdm, bkzy = @bkzy, bkfx = @bkfx, bkcc = @bkcc"+
                        ", xxxs = @xxxs, txdz = @txdz, yzdm = @yzdm, lxdh = @lxdh, sjh = @sjh, sfjxs = @sfjxs"+
                        ", pc = @pc, dateline = @dateline WHERE id = " + rid;
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = sqlStr;
                    cmd.Parameters.AddWithValue("@nf", nf);
                    cmd.Parameters.AddWithValue("@bmh", bmh);
                    cmd.Parameters.AddWithValue("@xm", xm);
                    cmd.Parameters.AddWithValue("@zkzh", zkzh);
                    cmd.Parameters.AddWithValue("@jxzd", jxzd);
                    cmd.Parameters.AddWithValue("@jxfdd", jxfdd);
                    cmd.Parameters.AddWithValue("@xb", xb);
                    cmd.Parameters.AddWithValue("@mz", mz);
                    cmd.Parameters.AddWithValue("@zzmm", zzmm);
                    cmd.Parameters.AddWithValue("@csrq", csrq);
                    cmd.Parameters.AddWithValue("@sfzh", sfzh);
                    cmd.Parameters.AddWithValue("@bysj", bysj);
                    cmd.Parameters.AddWithValue("@gzdw", gzdw);
                    cmd.Parameters.AddWithValue("@byxx", byxx);
                    cmd.Parameters.AddWithValue("@byzdm", byzdm);
                    cmd.Parameters.AddWithValue("@bkzy", bkzy);
                    cmd.Parameters.AddWithValue("@bkfx", bkfx);
                    cmd.Parameters.AddWithValue("@bkcc", bkcc);
                    cmd.Parameters.AddWithValue("@xxxs", xxxs);
                    cmd.Parameters.AddWithValue("@txdz", txdz);
                    cmd.Parameters.AddWithValue("@yzdm", yzdm);
                    cmd.Parameters.AddWithValue("@lxdh", lxdh);
                    cmd.Parameters.AddWithValue("@sjh", sjh);
                    cmd.Parameters.AddWithValue("@sfjxs", sfjxs);
                    cmd.Parameters.AddWithValue("@pc", pc);
                    cmd.Parameters.AddWithValue("@dateline", DateTime.Today);
                    DB.excuteSql(cmd); //处理数据
                    //if (icard != null) icard.saveImg(jxzd, pc, sfzh);//身份证图片
                    //else 
                    if (pb_cun.Image != null && rePic) Form1.saveJpg(pb_cun.Image, jxzd, pc, sfzh);//上传的图片
                    clearCard();
                    fowner.startRecruit();
                }
                rePic = false;
                MessageBox.Show("录入报名信息成功");
            }
        }

        private void cbbkzy_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem zy = (ComboboxItem)cbbkzy.SelectedItem;
            ComboboxItem fx = (ComboboxItem)cbbkfx.SelectedItem;
            ComboboxItem cc = (ComboboxItem)cbbkcc.SelectedItem;
            BindFx(jxzd, zy.Value.ToString());
            BindCc(jxzd, zy.Value.ToString(), "");
        }

        private void cbbkfx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem zy = (ComboboxItem)cbbkzy.SelectedItem;
            ComboboxItem fx = (ComboboxItem)cbbkfx.SelectedItem;
            ComboboxItem cc = (ComboboxItem)cbbkcc.SelectedItem;
            BindCc(jxzd, zy.Value.ToString(), fx.Value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setCard();
        }

        private void btn_pic_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择要上传的图片";
            ofd.Filter = "JPEG(*.jpg)|*.jpg";
            ofd.ShowDialog();
            picPath = ofd.FileName;
            if (!File.Exists(ofd.FileName))
            {
                MessageBox.Show("照片为空");
                return;
            }
            rePic = true;
            System.Drawing.Image img = System.Drawing.Image.FromFile(picPath);
            System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
            img.Dispose();
            pb_cun.Image = bmp;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbbkcc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
