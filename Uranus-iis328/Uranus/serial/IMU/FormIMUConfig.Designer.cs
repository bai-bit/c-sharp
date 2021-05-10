namespace Uranus.DialogsAndWindows
{
    partial class FormIMUConfig
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
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonRST = new System.Windows.Forms.Button();
            this.textBoxTerminal = new System.Windows.Forms.TextBox();
            this.textBoxCmd = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonINFO = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(548, 428);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(109, 24);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "退出配置模式";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonRST
            // 
            this.buttonRST.Location = new System.Drawing.Point(6, 46);
            this.buttonRST.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRST.Name = "buttonRST";
            this.buttonRST.Size = new System.Drawing.Size(100, 23);
            this.buttonRST.TabIndex = 2;
            this.buttonRST.Text = "复位";
            this.buttonRST.UseVisualStyleBackColor = true;
            this.buttonRST.Click += new System.EventHandler(this.buttonRST_Click);
            // 
            // textBoxTerminal
            // 
            this.textBoxTerminal.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxTerminal.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTerminal.Location = new System.Drawing.Point(6, 20);
            this.textBoxTerminal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxTerminal.Multiline = true;
            this.textBoxTerminal.Name = "textBoxTerminal";
            this.textBoxTerminal.ReadOnly = true;
            this.textBoxTerminal.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxTerminal.Size = new System.Drawing.Size(499, 220);
            this.textBoxTerminal.TabIndex = 3;
            this.textBoxTerminal.Text = "欢迎来到IMU配置界面";
            // 
            // textBoxCmd
            // 
            this.textBoxCmd.Location = new System.Drawing.Point(11, 20);
            this.textBoxCmd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxCmd.Name = "textBoxCmd";
            this.textBoxCmd.Size = new System.Drawing.Size(479, 21);
            this.textBoxCmd.TabIndex = 4;
            this.textBoxCmd.Text = "AT+?";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonClear);
            this.groupBox2.Controls.Add(this.textBoxTerminal);
            this.groupBox2.Location = new System.Drawing.Point(149, 12);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(508, 283);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "接收区";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(439, 256);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(50, 20);
            this.buttonClear.TabIndex = 9;
            this.buttonClear.Text = "清除";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.button5_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(439, 45);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(50, 20);
            this.buttonSend.TabIndex = 5;
            this.buttonSend.Text = "写入";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.buttonINFO);
            this.groupBox4.Controls.Add(this.buttonRST);
            this.groupBox4.Location = new System.Drawing.Point(6, 12);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(129, 415);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "快捷按钮";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 100);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "关闭数据输出";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 73);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "开启数据输出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonINFO
            // 
            this.buttonINFO.Location = new System.Drawing.Point(6, 20);
            this.buttonINFO.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonINFO.Name = "buttonINFO";
            this.buttonINFO.Size = new System.Drawing.Size(100, 23);
            this.buttonINFO.TabIndex = 3;
            this.buttonINFO.Text = "模块信息";
            this.buttonINFO.UseVisualStyleBackColor = true;
            this.buttonINFO.Click += new System.EventHandler(this.buttonINFO_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxCmd);
            this.groupBox5.Controls.Add(this.buttonSend);
            this.groupBox5.Location = new System.Drawing.Point(149, 300);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Size = new System.Drawing.Size(508, 70);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "发送AT指令";
            // 
            // FormIMUConfig
            // 
            this.AcceptButton = this.buttonSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 454);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormIMUConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIMUConfig_FormClosing);
            this.Load += new System.EventHandler(this.IMUConfig_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonRST;
        private System.Windows.Forms.TextBox textBoxTerminal;
        private System.Windows.Forms.TextBox textBoxCmd;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonINFO;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
    }
}