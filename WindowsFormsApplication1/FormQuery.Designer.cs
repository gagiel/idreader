namespace WindowsFormsApplication1
{
    partial class FormQuery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbfx = new System.Windows.Forms.TextBox();
            this.tbcc = new System.Windows.Forms.TextBox();
            this.tbzy = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tbsd = new System.Windows.Forms.DateTimePicker();
            this.tbed = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "专业";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "方向";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "层次";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "报名日期";
            // 
            // tbfx
            // 
            this.tbfx.Location = new System.Drawing.Point(71, 39);
            this.tbfx.Name = "tbfx";
            this.tbfx.Size = new System.Drawing.Size(201, 21);
            this.tbfx.TabIndex = 2;
            // 
            // tbcc
            // 
            this.tbcc.Location = new System.Drawing.Point(71, 66);
            this.tbcc.Name = "tbcc";
            this.tbcc.Size = new System.Drawing.Size(201, 21);
            this.tbcc.TabIndex = 3;
            // 
            // tbzy
            // 
            this.tbzy.Location = new System.Drawing.Point(71, 12);
            this.tbzy.Name = "tbzy";
            this.tbzy.Size = new System.Drawing.Size(201, 21);
            this.tbzy.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "至";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(71, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(152, 149);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbsd
            // 
            this.tbsd.CustomFormat = "yyyy-MM-dd";
            this.tbsd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tbsd.Location = new System.Drawing.Point(71, 93);
            this.tbsd.Name = "tbsd";
            this.tbsd.Size = new System.Drawing.Size(200, 21);
            this.tbsd.TabIndex = 4;
            // 
            // tbed
            // 
            this.tbed.CustomFormat = "yyyy-MM-dd";
            this.tbed.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tbed.Location = new System.Drawing.Point(71, 121);
            this.tbed.Name = "tbed";
            this.tbed.Size = new System.Drawing.Size(200, 21);
            this.tbed.TabIndex = 5;
            // 
            // FormQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 184);
            this.Controls.Add(this.tbed);
            this.Controls.Add(this.tbsd);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbzy);
            this.Controls.Add(this.tbcc);
            this.Controls.Add(this.tbfx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "高级查询";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbfx;
        private System.Windows.Forms.TextBox tbcc;
        private System.Windows.Forms.TextBox tbzy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DateTimePicker tbsd;
        private System.Windows.Forms.DateTimePicker tbed;

    }
}