using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using KV_WSN.Sensor;
using KV_WSN;

namespace KV_WSN
{
    public class Coordinator
    {
        /// <summary>
        /// 接收数据缓存区
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

        /// <summary>
        /// 串口
        /// </summary>
        private SerialPort serialPort;

        /// <summary>
        /// 接收数据线程
        /// </summary>
        private Thread tReceiveData;

        /// <summary>
        /// 数据集合
        /// </summary>
        public static List<SensorBase> sensorList = new List<SensorBase>();

        ParseData parseData = new ParseData(sensorList, false);

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="port">串口号</param>
        /// <returns>状态</returns>
        public bool OpenSerialPort(string port)
        {
            try
            {
                serialPort = new SerialPort(port, 9600);
                serialPort.ReadTimeout = 500;  //设置写入超时
                serialPort.WriteTimeout = 500;  //设置读取超时
                serialPort.Open();  //打开串号

               
                serialPort.DataReceived += SerialPort_DataReceived;
                Thread.Sleep(10);

            

             

                return true;
            }
            catch
            {
                return false;
            }
        }
    
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int count = this.serialPort.BytesToRead;  //数据的长度
                Byte[] buffer = new Byte[count];
                this.serialPort.Read(buffer, 0, buffer.Length);  //串口接收数据

                if (buffer[0] == 0xB5 && count % 9 == 0)  //判断数据下标为0的数据是否为0xB5及长度为9的倍数
                {
                    Byte[] data = new Byte[count];
                    Array.Copy(buffer, 0, data, 0, data.Length);  //复制数据
                    receiveData = receiveData + Converts.BytesToString(data);  //将数据转化为String类型放置接收缓存区;                                            
                    this.rawData = this.rawData + receiveData;
                    Thread.Sleep(10);
                    parseData.StartParseData();
                }

            }
            catch (Exception )
            {
              
            }
           
           
        }

     

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="addr">地址</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public bool SendData(byte type, byte addr, byte[] data)
        {
            try
            {
                byte[] buffer = new byte[] { 0xA5, type, addr, data[0], data[1], data[2], data[3], data[4], 0x5A };
                serialPort.Write(buffer, 0, buffer.Length);
                sendData = sendData + Converts.BytesToString(buffer);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 协调器发送数据
        /// </summary>
        /// <returns></returns>
        public bool SendData(byte[] buffer)
        {
            try
            {
                serialPort.Write(buffer, 0, buffer.Length);
                sendData = sendData + Converts.BytesToString(buffer);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns>状态</returns>
        public bool CloseSerialPort()
        {
            try
            {
                serialPort.Close();  //关闭串号
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
