using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using KV_WSN.Sensor;

namespace KV_WSN
{
    public class GateWay
    {
        /// <summary>
        /// Socket
        /// </summary>
        private Socket socket;

        /// <summary>
        /// 接收数据线程
        /// </summary>
        private Thread tReceiveData;

        /// <summary>
        /// 存数据的集合
        /// </summary>
        public static List<SensorBase> sensorList = new List<SensorBase>();

        /// <summary>
        /// 缓存区数据 （静态） 
        /// </summary>
        public static string receiveData = "";

        /// <summary>
        /// 原始数据
        /// </summary>
        public string rawData = "";

        /// <summary>
        /// 发送缓存区
        /// </summary>
        public string sendData = "";

        ParseData parseData = new ParseData(sensorList, true);

        /// <summary>
        /// 连接网关
        /// </summary>
        /// <param name="ipAddr">网关IP</param>
        /// <param name="ipPort">网关端口</param>
        /// <returns>连接状态</returns>
        public bool Connect(string ipAddr, int ipPort)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(ipAddr);  //获取网关IP
                IPEndPoint ipe = new IPEndPoint(ip, ipPort);  //获取网关IP及端口

                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  //实例化Socket

                LingerOption lingerOption = new LingerOption(false, 0);  //设置Socket通信关闭时如果还有信息不接收
                socket.LingerState = lingerOption;

                this.socket.Connect(ipe);  //连接网关

                tReceiveData = new Thread(new ThreadStart(ReceiveData));
                tReceiveData.IsBackground = true;
                tReceiveData.Start();

                Thread.Sleep(10);

                parseData.StartParseData();

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveData()
        {
            while (true)
            {

                Thread.Sleep(1);  //线程停止1sms

                try
                {
                    Byte[] buffer = new Byte[1024];
                    Int32 count = this.socket.Receive(buffer);  //Socket接收数据

                    if (buffer[0] == 0xB5 && count % 9 == 0)  //判断数据下标为0的数据是否为0xB5及长度为9的倍数
                    {
                        Byte[] data = new Byte[count];
                        Array.Copy(buffer, 0, data, 0, data.Length);  //复制数据
                        receiveData = receiveData + Converts.BytesToString(data);  //将数据转化为String类型放置接收缓存区
                        this.rawData = this.rawData + receiveData;
                    }
                }
                catch (Exception )
                {
                   
                }
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="addr">地址</param>
        /// <param name="data">数据</param>
        /// <returns>状态</returns>
        public bool SendData(byte type, byte addr, byte[] data)
        {
            try
            {
                byte[] buffer = new byte[] { 0xA5, type, addr, data[0], data[1], data[2], data[3], data[4], 0x5A };
                socket.Send(buffer);
                sendData = sendData + Converts.BytesToString(buffer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DisConnect()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);  //禁止发送接收
                socket.Close();  //关闭Socket通信
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Ping()
        {
            SendData(0x00, 0x00, new byte[5] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
        }
    }
}
