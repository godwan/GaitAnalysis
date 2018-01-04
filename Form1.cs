using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace EcgChart
{
	public partial class Form1 : Form
	{
		SerialPort _sPort;
		string defPort;
		string openButtonText;
		string closeButtonText;
		string logPath;
		List<double> allValues = new List<double>();
		LogFile log; 
		MyChart type1Chart1;
        MyChart type1Chart2;

        MyChart type2Chart1;
        MyChart type2Chart2;

        MyChart type3Chart1;
        MyChart type3Chart2;

        MyChart type4Chart1;
        MyChart type4Chart2;

        DspEngine dspEngine;
        ComPort myPort;

        static int counter1 = 0;
        static int counter2 = 0;
        static int counter3 = 0;
        static int counter4 = 0;
        static int step = 1;  //修改采样率
        ///// <summary>
        ///// 加速度表1数据
        ///// </summary>
        //private List<SerialDataEntity> _type1Chart1Datas = new List<SerialDataEntity>();
        ///// <summary>
        ///// 角速度表1数据
        ///// </summary>
        //private List<SerialDataEntity> _type2Chart1Datas = new List<SerialDataEntity>();
        ///// <summary>
        ///// 角度表1数据
        ///// </summary>
        //private List<SerialDataEntity> _type3Chart1Datas = new List<SerialDataEntity>();
        ///// <summary>
        ///// 加速度表2数据
        ///// </summary>
        //private List<SerialDataEntity> _type1Chart2Datas = new List<SerialDataEntity>();
        ///// <summary>
        ///// 角速度表2数据
        ///// </summary>
        //private List<SerialDataEntity> _type2Chart2Datas = new List<SerialDataEntity>();
        ///// <summary>
        ///// 角度表2数据
        ///// </summary>
        //private List<SerialDataEntity> _type3Chart2Datas = new List<SerialDataEntity>();

        public Form1()
		{
			InitializeComponent();
			defPort = "COM2";
			openButtonText = "Open";
			closeButtonText = "Close";
			logPath = @".\log.txt";
			myPort = new ComPort();
			initAll();
		}
		
		void button1_Click(object sender, EventArgs e)
		{
			listBox1.DataSource=null;
			if(_sPort.IsOpen)
			{
				_sPort.Close();
				listBox1.Items.Add("Port closed: " + _sPort.PortName);
				log.saveData(allValues);
				button1.Text=openButtonText;
			}
			else
			{
				listBox1.Items.Clear();
				try
				{
					_sPort.Open();
					
					listBox1.Items.Add("Port opened: " + _sPort.PortName);
					button1.Text=closeButtonText;
				}
				catch
				{
					listBox1.Items.Add("Port unavaliable: " + _sPort.PortName);
				}	
			}
		}

		void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
            try
            {
                if (!_sPort.IsOpen) return;
                int bytes = _sPort.BytesToRead;
                byte[] buffer = new byte[bytes];
                _sPort.Read(buffer, 0, bytes);

                BeginInvoke(new SetTextDeleg(si_DataReceived),
                                 new object[] { buffer });
            }
            catch(Exception ex)
            {

            }
			
		}

		public delegate void SetTextDeleg(byte[] text);
		public static int counter = 0;
		public void si_DataReceived(byte[] data)
		{
			List<double> values = new List<double>();
            
			foreach(SerialDataEntity v in IncomingData.parseBytes(data))
			{
                //counter++;
                //values.Add(v.getX());
                //myChart.AddPoint(v.getX());
                //double vv = dspEngine.bcgFilter(v.getX());
                //myChart2.AddPoint(vv);
                //textBox1.Text = counter.ToString();
                //var x2 = dspEngine.bcgFilter(v.getX());
                //var y2 = dspEngine.bcgFilter(v.getY());
                //var z2 = dspEngine.bcgFilter(v.getZ());
                this.lbTemp.Text = v.getTem().ToString();

                //if ((int)v.getNum() == 1) {
                //    type1Chart1.AddPoint(v.getX(), 0);
                //    type1Chart1.AddPoint(v.getY(), 1);
                //    type1Chart1.AddPoint(v.getZ(), 2);
                //}



                switch ((int)v.getNum())
                {
                    case 1:
                        //加速度
                        var x2 = dspEngine.bcgFilter(v.getX());
                        var y2 = dspEngine.bcgFilter(v.getY());
                        var z2 = dspEngine.bcgFilter(v.getZ());

                        //values.Add(v.getX());
                        //values.Add(v.getY());
                        //values.Add(v.getZ());



                        if (++counter1== step) {
                            counter1 = 0;
                            type1Chart1.AddPoint(v.getX(), 0);
                            type1Chart1.AddPoint(v.getY(), 1);
                            type1Chart1.AddPoint(v.getZ(), 2);

                            type1Chart2.AddPoint(x2, 0);
                            type1Chart2.AddPoint(y2, 1);
                            type1Chart2.AddPoint(z2, 2);
                        }

                        

                        break;
                    case 2:
                        //角速度
                        if (++counter2 == step)
                        {
                            counter2 = 0;
                            type2Chart1.AddPoint(v.getX(), 0);
                            type2Chart1.AddPoint(v.getY(), 1);
                            type2Chart1.AddPoint(v.getZ(), 2);

                        }

                        //values.Add(v.getX());
                        //values.Add(v.getY());
                        //values.Add(v.getZ());

                        //type2Chart2.AddPoint(x2, 0);
                        //type2Chart2.AddPoint(y2, 1);
                        //type2Chart2.AddPoint(z2, 2);
                        break;
                    case 3:
                        //角度
                        if (++counter3 == step)
                        {
                            counter3 = 0;
                            type3Chart1.AddPoint(v.getX(), 0);
                            type3Chart1.AddPoint(v.getY(), 1);
                            type3Chart1.AddPoint(v.getZ(), 2);
                        }

                        //values.Add(v.getX());
                        //values.Add(v.getY());
                        //values.Add(v.getZ());
                        //values.Add(v.getTem());

                        //type3Chart2.AddPoint(x2, 0);
                        //type3Chart2.AddPoint(y2, 1);
                        //type3Chart2.AddPoint(z2, 2);
                        break;
                    case 4:
                        //磁场
                        if (++counter4 == step)
                        {
                            counter4 = 0;
                            type4Chart1.AddPoint(v.getX(), 0);
                            type4Chart1.AddPoint(v.getY(), 1);
                            type4Chart1.AddPoint(v.getZ(), 2);
                        }

                        //values.Add(v.getX());
                        //values.Add(v.getY());
                        //values.Add(v.getZ());
                        //values.Add(v.getTem());

                        //type4Chart2.AddPoint(x2, 0);
                        //type4Chart2.AddPoint(y2, 1);
                        //type4Chart2.AddPoint(z2, 2);
                        break;
                    default:
                        break;
                            
                }

            }
            //allValues.AddRange(values);
        }

		void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			_sPort.Close();
		}
		void initComboPorts()
		{
			string[] ports = ComPort.GetPorts();
			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(ports);
			comboBox1.Text = defPort;
		}
		SerialPort initPort(string portName)
		{
            int baud = 9600;// 115200;//57600;
			try {
				SerialPort port = new SerialPort(portName,baud);
				port.DataReceived += sp_DataReceived;
				return port;
			}
			catch
			{
				return null;
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(_sPort.IsOpen) {
				_sPort.Close(); button1.Text=openButtonText;
			}
			_sPort = initPort(comboBox1.Text);
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			initAll();
		}
		void initAll()
		{
            allValues.Clear();
			button1.Text=openButtonText;
			_sPort = initPort(defPort);
			initComboPorts();
			listBox1.DataSource=null;	
			listBox1.Items.Clear();
			log = new LogFile(logPath);
			type1Chart1 = new MyChart(zedGraphControl1);
            type1Chart2 = new MyChart(zedGraphControl2);

            type2Chart1 = new MyChart(zedGraphControl4);
            type2Chart2 = new MyChart(zedGraphControl3);

            type3Chart1 = new MyChart(zedGraphControl6);
            type3Chart2 = new MyChart(zedGraphControl5);

            type4Chart1 = new MyChart(zedGraphControl7);
            type4Chart2 = new MyChart(zedGraphControl8);

            dspEngine = new DspEngine();
            counter = 0;
        }
		
		void fileOpenClick(object sender, EventArgs e)
		{
			string fileName=FileTools.GetOpenFileName();
			if(fileName==null) return; 
			List<double>v = FileTools.LoadListFromFile(fileName);
			listBox1.DataSource = v;
			type1Chart1.DrawGraph(v,null,false);
            type1Chart2.DrawGraph(v, null, false);
        }
        
        void AddFromFileClick(object sender, EventArgs e)
        {
			string fileName=FileTools.GetOpenFileName();
			if(fileName==null) return; 
			List<double>v = FileTools.LoadListFromFile(fileName);
			listBox1.DataSource = v;
			string f = FileTools.GetFileName(fileName);
			type1Chart1.DrawGraph(v,f,true);
        }
        
        void SaveFileClick(object sender, EventArgs e)
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;

            string xx = currentTime.Year.ToString()+"_" + currentTime.Month.ToString() + "_"+ currentTime.Day.ToString() + "_" + currentTime.Hour.ToString() + "_" + currentTime.Minute.ToString() + "_" + currentTime.Second.ToString() + "_" + currentTime.Millisecond.ToString()+".txt";
           // string xx = currentTime.ToString();
            string fileName = "C:\\Users\\asus\\Desktop\\" + xx;
           // string fileName=FileTools.GetSaveFileName();
		    log.saveAsData(fileName,allValues);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void zedGraphControl2_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
