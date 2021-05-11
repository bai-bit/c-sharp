using System;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

using Uranus.Data;
using Uranus.DialogsAndWindows;
namespace Uranus.DialogsAndWindows
{
    public partial class FormIMUConfig : BaseForm
    {
        public delegate bool SendDataHandler(byte[] buffer, int index, int count);
        public event SendDataHandler OnDataSend;
        private Queue TextQueue = new Queue();

        IMUData imuData;
        private System.Windows.Forms.Timer TextUpdateTimer = new System.Windows.Forms.Timer();
        bool openconfig_flag = false;

        public FormIMUConfig()
        {
            InitializeComponent();
        }

        public void PutRawData(byte[] buffer)
        {
            if (this.Visible == true)
            {
                foreach (byte b in buffer)
                {
                    TextQueue.Enqueue(((char)b).ToString());
                }
            }
        }

        public void PutPacket(object sender, IMUData imuData)
        {
            this.imuData = imuData;
        }

        private void IMUConfig_Load(object sender, EventArgs e)
        {
            bool ret;
            ret = SendATCmd("AT+EOUT=0");
            Thread.Sleep(10);

            textBoxTerminal.Clear();
            TextQueue.Clear();

            TextUpdateTimer.Interval = 20;
            TextUpdateTimer.Tick += new EventHandler(TextUpdateTimer_Tick);
            TextUpdateTimer.Start();

            //在打开配置窗口的时候，发送这个指令，获取配置信息。
            //利用一个标志位，不让这些信息显示出来。只填充对应的输入框。
            openconfig_flag = true;
            SendATCmd("AT+INFO");
        }

        private bool SendATCmd(string cmd)
        {
            textBoxCmd.Text = cmd;
            cmd += "\r\n";
            byte[] data = System.Text.Encoding.ASCII.GetBytes(cmd);

            // 在提示是否更改波特率
            if (cmd.Contains("BAUD"))
            {
                if (MessageBox.Show(cmd, "确认更改波特率!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
                {
                    return false;
                }
            }

            return OnDataSend(data, 0, data.Length);
        }



        void TextUpdateTimer_Tick(object sender, EventArgs e)
        {
            string Text = "";
            Queue mySyncdQ = Queue.Synchronized(TextQueue);
            int count = mySyncdQ.Count;
            for (int i = 0; i < count; i++)
            {
                Text += mySyncdQ.Dequeue();
            }

            //进行判断接收的数据
            isnumeric(Text);
            //清除显示窗口
            TextQueue.Clear();
            
            //加标志位，在配置窗口刚打开时，和读取配置时候，不要显示信息
            if(!openconfig_flag)
                textBoxTerminal.AppendText(Text);
            else
                openconfig_flag = false;

        }

        public bool isnumeric(string str)
        {
            char[] ch = new char[str.Length];
            ch = str.ToCharArray();
            int num = 0;
            int count = 0;

            if (str.IndexOf("ACC THRE -X:") > 0)
                if ((num = str.IndexOf("ACC THRE -X:")) > 0)
                {
                    textBox1.Text = "-";
                    for (int i = num; i < str.Length; i++)
                    {
                        if (ch[i] >= 48 && ch[i] <= 57)
                        {
                            count = i;
                            textBox1.Text += ch[i];
                        }
                        if (count != 0 && count != i)
                        {
                            count = 0;
                            i = str.Length;
                        }
                    }
                }

            if ((num = str.IndexOf("ACC THRE  X:")) > 0)
            {
                textBox7.Text = "";
                for (int i = num; i < str.Length; i++)
                {
                    if (ch[i] >= 48 && ch[i] <= 57)
                    {
                        count = i;
                        textBox7.Text += ch[i];
                    }
                    if (count != 0 && count != i)
                    {
                        count = 0;
                        i = str.Length;
                    }
                }
            }

            if ((num = str.IndexOf("ACC THRE -Y:")) > 0)
            {
                textBox6.Text = "-";
                for (int i = num; i < str.Length; i++)
                {
                    if (ch[i] >= 48 && ch[i] <= 57)
                    {
                        count = i;
                        textBox6.Text += ch[i];
                    }
                    if (count != 0 && count != i)
                    {
                        count = 0;
                        i = str.Length;
                    }
                }
            }

            if ((num = str.IndexOf("ACC THRE  Y:")) > 0)
            {
                textBox5.Text = "";
                for (int i = num; i < str.Length; i++)
                {
                    if (ch[i] >= 48 && ch[i] <= 57)
                    {
                        count = i;
                        textBox5.Text += ch[i];
                    }
                    if (count != 0 && count != i)
                    {
                        count = 0;
                        i = str.Length;
                    }
                }
            }

            if ((num = str.IndexOf("ACC THRE -Z:")) > 0)
            {
                textBox4.Text = "-";
                for (int i = num; i < str.Length; i++)
                {
                    if (ch[i] >= 48 && ch[i] <= 57)
                    {
                        count = i;
                        textBox4.Text += ch[i];
                    }
                    if (count != 0 && count != i)
                    {
                        count = 0;
                        i = str.Length;
                    }
                }
            }

            if ((num = str.IndexOf("ACC THRE  Z:")) > 0)
            {
                textBox3.Text = "";
                for (int i = num; i < str.Length; i++)
                {
                    if (ch[i] >= 48 && ch[i] <= 57)
                    {
                        count = i;
                        textBox3.Text += ch[i];
                    }
                    if (count != 0 && count != i)
                    {
                        count = 0;
                        i = str.Length;
                    }
                }
            }

            if ((num = str.IndexOf(" HALT VALUE:")) > 0)
            {
                textBox2.Text = "";
                for (int i = num; i < str.Length; i++)
                {
                    if (ch[i] >= 48 && ch[i] <= 57)
                    {
                        count = i;
                        textBox2.Text += ch[i];
                    }
                    if (count != 0 && count != i)
                    {
                        count = 0;
                        i = str.Length;
                    }
                }
            }


            if ((num = str.IndexOf(" HALT DELAY:")) > 0)
            {
                textBox8.Text = "";
                for (int i = num; i < str.Length; i++)
                {
                    if (ch[i] >= 48 && ch[i] <= 57)
                    {
                        count = i;
                        textBox8.Text += ch[i];
                    }
                    if (count != 0 && count != i)
                    {
                        count = 0;
                        i = str.Length;
                    }
                }
            }

            if ((num = str.IndexOf(" WARN DELAY:")) > 0)
            {
                textBox9.Text = "";
                for (int i = num; i < str.Length; i++)
                {
                    if (ch[i] >= 48 && ch[i] <= 57)
                    {
                        count = i;
                        textBox9.Text += ch[i];
                    }
                    if (count != 0 && count != i)
                    {
                        count = 0;
                        i = str.Length;
                    }
                }
            }

            if ((num = str.IndexOf(" TIMER DELAY:")) > 0)
            {
                textBox10.Text = "";
                for (int i = num; i < str.Length; i++)
                {
                    if (ch[i] >= 48 && ch[i] <= 57)
                    {
                        count = i;
                        textBox10.Text += ch[i];
                    }
                    if (count != 0 && count != i)
                    {
                        count = 0;
                        i = str.Length;
                    }
                }
            }

            return true;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            textBoxTerminal.Clear();
        }

        private void buttonRST_Click(object sender, EventArgs e)
        {
            SendATCmd("AT+RST");
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendATCmd(textBoxCmd.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendATCmd("AT+EOUT=1");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendATCmd("AT+EOUT=0");
        }

         private void FormIMUConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            SendATCmd("AT+EOUT=1");

            /* fix the bug that when entering this windows, sometime, will cause hardfault */
            TextUpdateTimer.Dispose();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //低通滤波 50Hz
            SendATCmd("AT+LOW_P=50");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //低通滤波 60Hz
            SendATCmd("AT+LOW_P=60");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //低通滤波 70Hz
            SendATCmd("AT+LOW_P=70");
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            //低通滤波 140Hz
            SendATCmd("AT+LOW_P=140");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //高通20Hz
            SendATCmd("AT+HIGH_P=20");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //高通30Hz
            SendATCmd("AT+HIGH_P=30");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //校准信息
            SendATCmd("AT+ACC_CAL");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //判断输入的数字
            //int n = int.Parse(textBox1.Text);
            //if (n < 0)
            //    textBox1.Text = n.ToString();
        }

        //写入配置
        //将1到9的控件输入内容进行识别，读取，然后通过特定的命令将这些数值写入到模块中
        private void button8_Click(object sender, EventArgs e)
        {
            double n = 0;
            //判断输入框的内容，不能为空，否则就退出该函数
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "")
            {
                MessageBox.Show("不能为空");
                return;
            }

            try
            {
                //判断输入的内容是否为数字。
                //如果是数字，判断是否符合取值范围 value range
                //只要有一个不符合，就提示用户，然后退出函数。

                //-x   (0, -8000)单位是mg
                n = double.Parse(textBox1.Text);
                if(n > 0 ||  n  <= -8000)
                {
                    MessageBox.Show("-X 取值范围（0，-8000)");
                    return;
                }
                
                //x      (0, 8000)单位是mg
                n = double.Parse(textBox7.Text);
                if (n < 0 || n >= 8000)
                {
                    MessageBox.Show("X 取值范围（0，8000)");
                    return;
                }

                // -Y     (0, -8000)单位是mg
                n = double.Parse(textBox6.Text);
                if (n > 0 || n <= -8000)
                {
                    MessageBox.Show("-Y 取值范围（0，-8000)");
                    return;
                }

                // Y       (0, 8000)单位是mg
                n = double.Parse(textBox5.Text);
                if (n < 0 || n >= 8000)
                {
                    MessageBox.Show("Y 取值范围（0，8000)");
                    return;
                }

                // -Z      (0, -8000)单位是mg
                n = double.Parse(textBox4.Text);
                if (n > 0 || n <= -8000)
                {
                    MessageBox.Show("-Z 取值范围（0，-8000)");
                    return;
                }

                // Z       (0, 8000)单位是mg
                n = double.Parse(textBox3.Text);
                if (n < 0 || n >= 8000)
                {
                    MessageBox.Show("Z 取值范围（0，8000)");
                    return;
                }

                //H        (0, 8000)且小于xyz的数值  单位是mg
                n = double.Parse(textBox2.Text);
                if (n < 0 || n >= 8000 )
                {
                    MessageBox.Show("H 取值范围（0，8000)");
                    return;
                }

                //HD       停机报警（0, 5000)  单位是毫秒
                n = double.Parse(textBox8.Text);
                if (n < 0 || n >= 5000)
                {
                    MessageBox.Show("HD 取值范围（0，5000)");
                    return;
                }

                //WD        高频报警（0，5000）单位是毫秒
                n = double.Parse(textBox9.Text);
                if (n < 0 || n >= 5000)
                {
                    MessageBox.Show("WD 取值范围（0，5000)");
                    return;
                }

                //TD        停机状态检测（0， 600）单位是秒
                n = double.Parse(textBox10.Text);
                if (n < 0 || n >= 600)
                {
                    MessageBox.Show("HD 取值范围（0，600)");
                    return;
                }


            }
            catch (FormatException)
            {
                MessageBox.Show("写入的设置内容包含了非法字符，请重新输入！");
            }

            //判断无错误之后，重新读取，发送指令
            SendATCmd("AT+ACC_TH=X," + double.Parse(textBox1.Text));
            Thread.Sleep(100);
            SendATCmd("AT+ACC_TH=X," + double.Parse(textBox7.Text));
            Thread.Sleep(100);

            SendATCmd("AT+ACC_TH=Y," + double.Parse(textBox6.Text));
            Thread.Sleep(100);
            SendATCmd("AT+ACC_TH=Y," + double.Parse(textBox5.Text));
            Thread.Sleep(100);

            SendATCmd("AT+ACC_TH=Z," + double.Parse(textBox4.Text));
            Thread.Sleep(100);
            SendATCmd("AT+ACC_TH=Z," + double.Parse(textBox3.Text));
            Thread.Sleep(100);

            SendATCmd("AT+ACC_TH=H," + double.Parse(textBox2.Text));
            Thread.Sleep(100);

            SendATCmd("AT+DELAY=H," + double.Parse(textBox8.Text));
            Thread.Sleep(100);
            SendATCmd("AT+DELAY=W," + double.Parse(textBox9.Text));
            Thread.Sleep(100);
            SendATCmd("AT+DELAY=T," + double.Parse(textBox10.Text));
            Thread.Sleep(100);
        }

        //读取阈值配置
        private void button9_Click(object sender, EventArgs e)
        {
            SendATCmd("AT+INFO");
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //int n = int.Parse(textBox7.Text);
            //if (n < 0)
            //    textBox1.Text = "ok";
        }
    }

}
