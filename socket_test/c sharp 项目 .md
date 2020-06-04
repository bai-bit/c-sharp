# c sharp 项目

## 项目描述

​	软件名称：有人lora集中器服务器

​	实现的功能：

​	获取本机的ip地址，由于电脑上也许会有多个ip地址，这里我只取ip地址列表中的第一个ip地址，并显示在屏幕上，如果不是需要绑定的IP地址，可以手动修改。

​	端口号是固定的号码，然后点击监听，在程序上，绑定文本框中的ip地址和端口号。设置等待连接队列，被动监听状态，可以被客户端搜索到这个服务器。然后是等待连接，如果被客户端监听到，开始三次握手的过程，创建连接。连接成功后，在屏幕上打印信息。提示用户，是哪个客户机连接，端口号是多少，ip地址是多少。

​	然后在现实文本框中现实客户机发送的信息，在这里，是显示经过处理的信息。不断的将客户端发送过来的信息追加到文本框中。一共用十五个节点，每个节点每隔五秒发送一次温度信息，客户机在收到一个节点的温度信息后就立即发送给服务器。服务器收到来自客户机的信息后，进行拆包处理，把温度信息和节点编号打印到屏幕上。

​	可以将服务器接收到的信息保存到文件中，也可以选择将打印出来的信息保存到文件中。

​	下面是c#代码，我是第一次接触这个编程语言，看着教程做的。需要什么功能就直接去百度。对这个编程语言了解非常少，而且这个项目的代码也很烂。仅供我自己学习使用。

```c#
amespace socket_test
{
    public partial class Form1 : Form
    {
        //保存节点的id号
        int[] node_id = new int[16];
        //保存数据
        string[] temData = new string[16] { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A" };
        int[] node_online_flag = new int[15] ;
        //开启定时器，单位为毫秒
        int time = 0;
        System.Timers.Timer t = new System.Timers.Timer(5000); //开启定时器
        //追加到文件中，要想对文件进行操作，就必须申明下面两个变量
        //一个是用来操作文件流一个是用来向文件中写入数据的流
        static FileStream fs;
        static StreamWriter sw;

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
            this.textBox1.Text = new System.Net.IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address).ToString();//获取本机的ip地址

            textBox2.Text = "1883";
            this.textBox2.ReadOnly = true;  //使textBox2输入框的内容不能更改
            this.groupBox1.Enabled = false; //使groupBox1为不可操作状态

            t.Elapsed += new System.Timers.ElapsedEventHandler(theout); //到达时间的时候执行事件；

            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = "1883";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //读取输入框的内容
            //绑定ip地址，端口号
            //创建等待连接对列
            //设置为被动监听
            //等待连接
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//申请套接字
            IPAddress ip;

            if (button1.Text == "监听")
            {
                listen_flag = true;
               
                this.textBox1.ReadOnly = true;
                this.button1.Enabled = false;
                
                ip = IPAddress.Parse(textBox1.Text);
                
                //if (IsPortOccupedFun2(int.Parse(textBox2.Text)) == false)//这个函数是判断套接字是否被绑定，这里用不到它
  
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(textBox2.Text));
                
                socketWatch.Bind(point);//绑定端口号和IP地址

                ShowMsg("listen success! Waiting for device connection.....");
               
                socketWatch.Listen(10);//设置为被动监听状态，等待连接队列有十个

                th = new Thread(listen);//开启新的线程，这个线程是用来等待客户机的连接的

                th.IsBackground = true;//将新开的线程设置为后台线程，这样，在主线程结束时，后台线程就跟着结束了
                th.Start(socketWatch);//新线程开始执行
            }
            else
            {
                listen_flag = false;
                button1.Text = "监听";
                ShowMsg("Stop listing....");
                textBox5.AppendText("当前没有在连接中的设备" + "\r\n");//这个语句的功能就是将括号种的字符串追加到textBox中
                t.Enabled = false;
                this.groupBox1.Enabled = false;//关闭设置区域，禁止进行设置操作

            }
        }
        #region//判断IP地址是否被绑定####这个关键字是用来合并代码用的
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
        bool time_out_flag = false;
        int timeout = 0;
        /// <summary>
        /// 监听，等待接收来自集中器的消息
        /// </summary>
        /// <param name="o"></param>
        void listen(object o)
        {
            Socket socketwatch = o as Socket;//线程中传参数
            
            while(listen_flag)
            {
                 socketclient = socketwatch.Accept();//阻塞，等待连接

                ShowMsg(socketclient.RemoteEndPoint.ToString() + ":" + "connect success"); //打印消息，连接成功
                textBox5.AppendText("在线：" + socketclient.RemoteEndPoint.ToString() + "\r\n");
                thread_flag = true;
                
                this.button1.Enabled = true;
                button1.Text = "停止";
                button2.Text = "暂停";
                this.textBox4.ReadOnly = false;

                this.groupBox1.Enabled = true;//使能设置区域

                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；开启定时器
                while (thread_flag && listen_flag)
                {
                    if (time_out_flag)
                    {
                        if (int.Parse(textBox4.Text) < 1200)//设置超时时间
                        {
                            timeout = int.Parse(textBox4.Text) * 2;
                        }
                        else
                            timeout = int.Parse(textBox4.Text) * 2;
                    }

                    try
                    {
                        if (timeout > 0)
                            socketclient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout * 1000);
                        else
                            socketclient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 60000);
                    }
                    catch
                    {

                    }
                  
                    byte[] buf = new byte[512];
                    int unixtime = 0;

                    try
                    {
                        int r = socketclient.Receive(buf);//接受消息，阻塞态，在这里，如果等待的时间超过了上面设置的时间，就会放弃等待，返回一个错误，并提示用户，客户机掉线，
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
                    lora_node_id |= buf[7] << 16;
                    lora_node_id |= buf[6] << 24;

                    int j = 0;
                    for (j = 0; j < 15; j++)
                    {
                        if (lora_node_id == node_id[j])
                            break;
                    }

                    //处理信息，打印消息
                    if (j == 15)
                    {
                        node_id[14] = lora_node_id;
                        int temp = 0;
                        //排序，分配编号
                        for (int m = 0; m < 14; m++)
                            for (int n = m + 1; n < 15; n++)
                                if (node_id[m] < node_id[n])
                                {
                                    temp = node_id[m];
                                    node_id[m] = node_id[n];
                                    node_id[n] = temp;
                                }
                    }

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
                        if (node_online_flag[j] == 2)//节点掉线后，会返回两次到三次以上的掉线数据包，提示节点掉线
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
                            temData[j] = Convert.ToDecimal(temperature).ToString("f3");//将数据转换成字符串，并保存在数组中
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
            this.textBox6.Enabled = false;//这句话和最后一句话，是为了防止鼠标在选中显示的数据时，下一次打印时，会打乱显示的内容
            //检测socket是否在连接状态
            if (socket_connect_flag)
                socket_connect_flag = false;
            else
            {
                Environment.Exit(0);//退出程序，回收环境资源
            }
               
            if (save_file_flag)
            {
                sw.Write(DateTime.Now.ToString("F") + "," + temData[0] + "," + temData[1] + "," + temData[2] + "," + temData[3] + "," + temData[4] + "," + temData[5] + "," + temData[6] + "," + temData[7] + "," + temData[8] + "," + temData[9] + "," + temData[10] + "," + temData[11] + "," + temData[12] + "," + temData[13] + "," + temData[14] + "," + "\r\n");//将数据写入文件中
                sw.Flush();//强制刷新缓存区
            }

            if(time_flag)
            {
                ShowDate(time);//显示时间
            }
            try
            {
                for (int i = 0; i < node_num; i++)
                {
                    if (temData[i] != "N/A")
                    {
                        if (node_num_flag)
                            textBox6.AppendText("节点" + (i + 1) + "：");
                        textBox6.AppendText(temData[i] + "," + "  ");//打印节点数据，追加的方式
                        //temData[i] = "N/A";
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
            showtemperature(123);//这个函数就是用来打印数据的，定时器每5秒启动一次，每一次启动，就执行这个函数
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
                sw.Close();
                fs.Close();
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
                time_out_flag = true;

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
                    fs = new FileStream("tsic506.csv", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    sw = new StreamWriter(fs, Encoding.Default);
                    sw.Write("时间" + "," + "节点1" + "," + "节点2" + "," + "节点3" + "," + "节点4" + "," + "节点5" + "," + "节点6" + "," + "节点7" + "," + "节点8" + "," + "节点9" + "," + "节点10" + "," + "节点11" + "," + "节点12" + "," + "节点13" + "," + "节点14" + "," + "节点15" + "\r\n");
                    sw.Flush();
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

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
```
