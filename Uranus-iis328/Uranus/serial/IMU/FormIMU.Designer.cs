namespace Uranus.DialogsAndWindows
{
    partial class FormIMU
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelData = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(363, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(316, 51);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "帧率";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "接收速率";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelData);
            this.groupBox1.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 51);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据";
            // 
            // labelData
            // 
            this.labelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelData.Location = new System.Drawing.Point(3, 15);
            this.labelData.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelData.Name = "labelData";
            this.labelData.Size = new System.Drawing.Size(341, 33);
            this.labelData.TabIndex = 0;
            this.labelData.Text = "请连接设备";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(10, 67);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(669, 377);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "加速度计";
            this.groupBox3.UseCompatibleTextRendering = true;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter_1);
            // 
            // FormIMU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 449);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormIMU";
            this.Text = "IMU";
            this.Load += new System.EventHandler(this.FormIMU_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelData;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}