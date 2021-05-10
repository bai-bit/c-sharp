using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Diagnostics;
//using System.Timers.Timer;

using System.IO;
using System.Reflection;
using System.Globalization;

namespace socket_test
{
    public partial class Form1 : Form
    {
        //保存节点的id号
        int[] node_id = new int[16] ;
        //保存数据
        string[] temData = new string[16] { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A" };
        int[] node_online_flag = new int[15] ;
        //开启定时器，单位为毫秒
        int time = 0;
        System.Timers.Timer t = new System.Timers.Timer(20000);
        //追加到文件中
        static FileStream fs_csv,fs_txt;
        static StreamWriter sw_csv,sw_txt;

        Socket socketWatch;
        Socket socketclient;

        Thread th;
        bool listen_flag = true;
        bool thread_flag = true;

        int node_num = 14;
        bool time_flag = false;
        bool node_num_flag = false;
        bool save_file_flag = false;
        bool socket_connect_flag = false;

        public Form1()
        {
            InitializeComponent();
            this.textBox4.ReadOnly = true;
            this.textBox1.Text = new System.Net.IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address).ToString();
            
            textBox2.Text = "1883";
                       
           // this.textBox2.ReadOnly = true;
            this.groupBox1.Enabled = false;

            t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；

            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //读取输入框中的内容
            //判断输入的内容是否正确

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //读取输入框中的内容
            //判断输入的内容是否正确
            //textBox2.Text = "1883";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //读取输入框的内容
            //绑定ip地址，端口号
            //创建等待连接对列
            //设置为被动监听
            //等待连接
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip;

            if (button1.Text == "监听")
            {
                listen_flag = true;
               
                this.textBox1.ReadOnly = true;
                this.button1.Enabled = false;
                
                ip = IPAddress.Parse(textBox1.Text);
                
                //this.textBox1.Text = ip.ToString();
                //if (IsPortOccupedFun2(int.Parse(textBox2.Text)) == false)
                //{

                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(textBox2.Text));
                
                socketWatch.Bind(point);

                ShowMsg("listen success! Waiting for device connection.....");
               
                socketWatch.Listen(10);

                th = new Thread(listen);

                th.IsBackground = true;
                th.Start(socketWatch);
            }
            else
            {
                listen_flag = false;
                button1.Text = "监听";
                ShowMsg("Stop listing....");
                textBox5.AppendText("当前没有在连接中的设备" + "\r\n");
                t.Enabled = false;
                this.groupBox1.Enabled = false;

            }
        }
        #region//判断IP地址是否被绑定
        /// <summary>
        /// 判断当前绑定的ip地址是否已经被绑定
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        internal static Boolean IsPortOccupedFun2(Int32 port)
        {
            Boolean result = false;
            try
            {
                System.Net.NetworkInformation.IPGlobalProperties iproperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
                System.Net.IPEndPoint[] ipEndPoints = iproperties.GetActiveTcpListeners();
                foreach (var item in ipEndPoints)
                {
                    if (item.Port == port)
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region// 监听，等待接收来自集中器的消息
        //bool time_out_flag = false;
        //int timeout = 0;
        /// <summary>
        /// 监听，等待接收来自集中器的消息
        /// </summary>
        /// <param name="o"></param>
        void listen(object o)
        {
            Socket socketwatch = o as Socket;
            //int frame_count = 0;
            
            while(listen_flag)
            {
                 socketclient = socketwatch.Accept();

                ShowMsg(socketclient.RemoteEndPoint.ToString() + ":" + "connect success");
                textBox5.AppendText("在线：" + socketclient.RemoteEndPoint.ToString() + "\r\n");
                thread_flag = true;
                
                this.button1.Enabled = true;
                button1.Text = "停止";
                button2.Text = "暂停";
                this.textBox4.ReadOnly = false;

                this.groupBox1.Enabled = true;

                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                while (thread_flag && listen_flag)
                {
                    #region
                    //button1.Text = frame_count++.ToString ();
                    //if (time_out_flag)
                    //{


                    //    if (int.Parse(textBox4.Text) < 1200)
                    //    {
                    //        timeout = int.Parse(textBox4.Text) * 10;
                    //    }
                    //    else
                    //        timeout = int.Parse(textBox4.Text) * 2;
                    //}

                    //try
                    //{
                    //    if (timeout > 0)
                    //        socketclient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout * 1000);
                    //    else
                    //        socketclient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 60000);
                    //}
                    //catch
                    //{

                    //}
                    #endregion

                    byte[] buf = new byte[512];
                    int unixtime = 0;

                    try
                    {
                        int r = socketclient.Receive(buf);
                        socket_connect_flag = true;
                        if (r == 0)
                            break;
                    }
                    catch (SocketException e)
                    {

                        if (e.ErrorCode == 10060)
                        {
                            ShowMsg(socketclient.RemoteEndPoint.ToString() + ":" + "connect error");
                            textBox5.AppendText("掉线：" + socketclient.RemoteEndPoint.ToString() + "\r\n");
                            thread_flag = false;
                            t.Enabled = false;
                            this.button1.Enabled = false;
                            socketclient.Close();

                            break;
                        }
                    }
                    catch
                    {

                    }

                    string str = ToHexString(buf);
                    //拆包处理信息
                    //关心的元素：buf[2:5]   buf[6:9]   buf[12]   buf[18:21]   buf[22]   buf[25;26]   buf[27:32]
                    //int data = Enco/*ding.UTF8.get(buf, 0, r);*/
                    unixtime |= buf[21];
                    unixtime |= buf[20] << 8;
                    unixtime |= buf[19] << 16;
                    unixtime |= buf[18] << 24;
                    time = unixtime;
                    //ShowDate(unixtime);
                    int lora_node_id = 0;

                    lora_node_id |= buf[9];
                    lora_node_id |= buf[8] << 8;
                    //lora_node_id |= buf[7] << 16;
                    //lora_node_id |= buf[6] << 24;

                    int j;
                    for (j = 0; j < 15; j++)
                    {
                        if (lora_node_id == node_id[j])
                        {
                            //解析数据
                            break;
                        }
                    }

                    //处理信息，打印消息
                    //直接显示节点的实际id号，显示后三位id号，十六进制显示
                    if (15 == j)
                    {
                        //新加入的节点，判断为0的元素，在此位置加入新节点id号，解析数据
                        for (j = 0; j < 15; j++)
                            if (node_id[j] == 0)
                            {
                                node_id[j] = lora_node_id;
                                break;
                            }
                    }
                    #region
                    //将id号按照从大到小进行排列
                    //if (j == 15)
                    //{
                    //    node_id[14] = lora_node_id;
                    //    int temp = 0;
                    //    //排序，分配编号
                    //    for (int m = 0; m < 14; m++)
                    //        for (int n = m + 1; n < 15; n++)
                    //            if (node_id[m] < node_id[n])
                    //            {
                    //                temp = node_id[m];
                    //                node_id[m] = node_id[n];
                    //                node_id[n] = temp;
                    //            }
                    //}
                    #endregion

                    for (j = 0; j < 15; j++)
                    {
                        if (lora_node_id == node_id[j])
                            break;
                    }

                    if (buf[22] == 0x01)
                    {
                        //掉线，对应的节点显示N/A
                        //查找当前节点对应的数组元素
                        //然后将对应的元素置为N/A
                        node_online_flag[j]++;
                        if (node_online_flag[j] == 2)
                        {
                            temData[j] = "N/A";
                        }
                    }
                    else
                    {
                        try
                        {
                            node_online_flag[j] = 0;
                            int datanum = (buf[25] << 8) | buf[26];
                            int[] data = new int[256];
                            if (data.Length < datanum)
                                datanum = data.Length;
                            for (int i = 0; i < datanum; i++)
                                data[i] = buf[27 + i];
                            float temperature = ((float)(data[0] | data[1] << 8)) / 1000;
                            //temData[j] = temperature;
                            temData[j] = Convert.ToDecimal(temperature).ToString("f3");
                        }
                        catch
                        {

                        }
                    }
                }
            }
            //断开连接，关闭套接字，回收资源
            socketclient.Close();
            socketwatch.Close();
            socketwatch.Dispose();
            
        }

        #endregion

        /// <summary>
        /// 向终端显示温度信息
        /// </summary>
        /// <param name="temperature"></param>
        void showtemperature(float temperature)
        {
            //显示模板
            //创建一个数组，数组中保存温度信息。
            //时间采用刷新时间内，最后一次更新的时间戳
            //time + "," + temperature[0] + "," + temperature[1] + "," + ...
            this.textBox6.Enabled = false;
            //检测socket是否在连接状态
            if (socket_connect_flag)
                socket_connect_flag = false;
            else
            {
                //Environment.Exit(0);

            }

            if (save_file_flag)
            {
                sw_csv.Write(DateTime.Now.ToString("F") + "," + temData[0] + "," + temData[1] + "," + temData[2] + "," + temData[3] + "," + temData[4] + "," + temData[5] + "," + temData[6] + "," + temData[7] + "," + temData[8] + "," + temData[9] + "," + temData[10] + "," + temData[11] + "," + temData[12] + "," + temData[13] + "," + temData[14] + "," + "\r\n");
                sw_csv.Flush();
            }
            fs_txt = new FileStream("tsic506.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            sw_txt = new StreamWriter(fs_txt, Encoding.Default);
            sw_txt.Write(DateTime.Now.ToString("F") + ","  + Convert.ToString(node_id[0],16) + temData[0] + "," + Convert.ToString(node_id[1], 16) + temData[1] + "," + Convert.ToString(node_id[2], 16) + temData[2] + "," + Convert.ToString(node_id[3], 16) + temData[3] + "," + Convert.ToString(node_id[4], 16) + temData[4] + "," + Convert.ToString(node_id[5], 16) + temData[5] + "," + Convert.ToString(node_id[6], 16) + temData[6] + "," + Convert.ToString(node_id[7], 16) + temData[7] + "," + Convert.ToString(node_id[8], 16) + temData[8] + "," + Convert.ToString(node_id[9], 16) + temData[9] + "," + Convert.ToString(node_id[10], 16) + temData[10] + "," + Convert.ToString(node_id[11], 16) + temData[11] + "," + Convert.ToString(node_id[12], 16) + temData[12] + "," + Convert.ToString(node_id[13], 16) + temData[13] + "," + Convert.ToString(node_id[14], 16) + temData[14] + "," + "\r\n");
            sw_txt.Flush();
            sw_txt.Close();
            fs_txt.Close();
            if (time_flag)
            {
                ShowDate(time);
            }
            try
            {
                for (int i = 0; i < node_num; i++)
                {
                    if (temData[i] != "N/A")
                    {
                        if (node_num_flag)
                            textBox6.AppendText("节点" + Convert.ToString(node_id[i], 16) + "：");
                        textBox6.AppendText(temData[i] + "," + "  ");
                       // temData[i] = "N/A";
                    }
                }
                
                textBox6.AppendText("\r\n");
                //textBox6.AppendText("," + temData[0] + "," + temData[1] + "," + temData[2] + "," + temData[3] + "," + temData[4] + "," + temData[5] + "," + temData[6] + "," + temData[7] + "," + temData[8] + "," + temData[9] + "," + temData[10] + "," + temData[11] + "," + temData[12] + "," + temData[13] + "," + temData[14] + "," + temData[15] + "\r\n");
                //textBox6.AppendText(node_id[0].ToString() + "   " + node_id[1].ToString() + "  " + node_id[2].ToString() + "  " + node_id[3].ToString() + "   " + node_id[4].ToString() + " " + node_id[5].ToString() + "  " + node_id[6].ToString() + "  " + node_id[7].ToString() + "     " + node_id[8].ToString() + "  " + node_id[9].ToString() + "      " + node_id[10].ToString() + "   " + node_id[11].ToString() + "   " + node_id[12].ToString() + "     " + node_id[13].ToString() + "     " + node_id[14].ToString() + "\r\n");

                this.textBox6.Enabled = true;
            }
            catch
            {

            }
        }
        /// <summary>
        /// 将unix时间戳转换成时间格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToDateTime(int time)
        {
            if (time < 0)
                throw new ArgumentOutOfRangeException("time is out of range");

            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(time);
        }
        /// <summary>
        /// 将byte转换成string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            showtemperature(123);
        }
        /// <summary>
        /// 向终端显示数据
        /// </summary>
        /// <param name="str"></param>
        void ShowMsg(string str)
        {
            textBox3.Text = ("  " + str + "\r\n");
        }

        void ShowDate(int unixtime)
        {
            DateTime datetime;
            datetime = UnixTimeToDateTime(unixtime);
            //textBox5.Text = (datetime.Year.ToString() + "/" + datetime.Month.ToString() + "/" + datetime.Day.ToString() + "/" + "\r\n");

            try
            {
                textBox6.AppendText(DateTime.Now.ToString("F") + "," + "  ");
            }
            catch
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
         
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!open_file_flag)
            {
                sw_csv.Close();
                fs_csv.Close();
            }
        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            int iMax = 864000;//首先设置上限值
            if (textBox4.Text != null && textBox4.Text != "")
            {
                if (int.Parse(textBox4.Text) > iMax)
                {
                    textBox4.Text = (iMax - 1).ToString();
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (button2.Text == "暂停")
            {
                button2.Text = "继续";
                t.Enabled = false;
                //执行关闭定时器
            }
            else
            {
                button2.Text = "暂停";
                t.Enabled = true;
                //执行开启定时器
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //设置定时器，显示数量
            //获取信息,从新设置socket的超时,将标志位置true
            if (textBox4.Text != null && textBox4.Text != "" && int.Parse(textBox4.Text) > 0)
            {
                //time_out_flag = true;

                t.Interval = int.Parse(textBox4.Text) * 1000;
            }
            if(textBox7.Text != null && textBox7.Text != "" && int.Parse (textBox7.Text ) > 0)
            {
                node_num = int.Parse (textBox7.Text);
            }
        }
        

        private void button5_Click(object sender, EventArgs e)
        {
            //清空textBox6
            textBox6.Text = "";
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //时间标志位
            if (checkBox1.Checked)
                time_flag = true;
            else
                time_flag = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //编号标志位
            if (checkBox2.Checked)
                node_num_flag = true;
            else
                node_num_flag = false;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            int iMax = 15;//首先设置上限值
            if (textBox4.Text != null && textBox4.Text != "")
            {
                if (int.Parse(textBox4.Text) > iMax)
                {
                    textBox4.Text = (iMax - 1).ToString();
                }
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //打开保存数据的文件
            //判断当前目录下有没有存在目标文件，
            //如果没有，提示用户，目标文件不存在，是否重新创建数据文件
            //如果存在，用只读方式打开
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "打开文件";
            ofd.Filter = "excel|*.csv|text|*.txt|all file|*,*";
            ofd.InitialDirectory = Application.StartupPath;

            DialogResult status = ofd.ShowDialog();
            if(status == DialogResult.OK )
            {
                Process process1 = new Process();
                process1.StartInfo.FileName = ofd.FileName;
                process1.StartInfo.Arguments = "";
                process1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process1.Start();
            }

        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //将打印到终端的信息保存到文件
            //文件名由用户来指定
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "保存文件";
            sfd.Filter = "text|*.txt|excel|*.csv|all file|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs1 = new FileStream(sfd.FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs1, Encoding.Default);
                sw.Write(textBox6.Text);
                sw.Close();
                fs1.Close();

            }                
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        bool open_file_flag = true;
        private void button6_Click(object sender, EventArgs e)
        {
            //将数据按照固定的模板保存到文件中
            //用一个标志来实现、
            if(button6.Text == "导入文件")
            {
                //先判断
                if (open_file_flag)
                {
                    open_file_flag = false;
                    fs_csv = new FileStream("tsic506.csv", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    sw_csv = new StreamWriter(fs_csv, Encoding.Default);
                    sw_csv.Write("时间" + "," + "节点1" + "," + "节点2" + "," + "节点3" + "," + "节点4" + "," + "节点5" + "," + "节点6" + "," + "节点7" + "," + "节点8" + "," + "节点9" + "," + "节点10" + "," + "节点11" + "," + "节点12" + "," + "节点13" + "," + "节点14" + "," + "节点15" + "\r\n");
                    sw_csv.Flush();
                }

                save_file_flag = true;
                button6.Text = "取消导入";
            }   
            else
            {
                save_file_flag = false;
                button6.Text = "导入文件";
            }
        }
    }
}
