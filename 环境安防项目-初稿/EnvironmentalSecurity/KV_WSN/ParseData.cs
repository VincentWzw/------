using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KV_WSN.Sensor;

namespace KV_WSN
{
    public class ParseData
    {
        private string receiveData = "";

        private Thread tParseData;

        public List<SensorBase> sensorList;   

        private bool isGateWay = false;

        /// <summary>
        /// 解析数据
        /// </summary>

        public ParseData(List<SensorBase> sensorList, bool isGateWay)
        {
            this.isGateWay = isGateWay;
            this.sensorList = sensorList;
        }

        /// <summary>
        /// 开始解析数据
        /// </summary>
        public void StartParseData()
        {
            tParseData = new Thread(new ThreadStart(ParseReceiveData));
            tParseData.IsBackground = true;
            tParseData.Start();
        }
        /// <summary>
        /// 解析接收数据
        /// </summary>
        private void ParseReceiveData()
        {
            //while (true)
            //{
                Thread.Sleep(1); 

                if (isGateWay)
                {
                    receiveData = GateWay.receiveData;
                    GateWay.receiveData = "";
                }
                else
                {
                    receiveData = Coordinator.receiveData;
                    Coordinator.receiveData = "";
                }

                while (this.receiveData != "")
                {
                    Thread.Sleep(1);

                    Byte[] data = new Byte[9];
                    try
                    {
                        data = Converts.StringToBytes(receiveData.Substring(0, 18));
                        receiveData = receiveData.Remove(0, 18);
                    }
                    catch { }

                    if (data[0] == 0xB5 && data.Length == 9)
                    {
                        SensorBase sameSensor = sensorList.FirstOrDefault(p => p.Type.Equals(data[1]) && p.Addr.Equals(data[2]));
                        if (sameSensor != null)
                        {
                            sameSensor.Data = new byte[5] { data[3], data[4], data[5], data[6], data[7] };
                        }
                        else
                        {
                            sensorList.Add(GetType(data));
                           
                        }
                    }
                }
            //}
        }

        /// <summary>
        /// 获取传感器类型
        /// </summary>
        /// <param name="data">类型</param>
        /// <returns>传感器的类型</returns>
        private SensorBase GetType(byte[] data)
        {
            byte[] cachedata=new byte[]{ data[3], data[4], data[5], data[6], data[7] };
            SensorBase sensor = null;
            switch (data[1])
            {
                case (byte)EnumType.数码管:
                    sensor = new Sensor_LED(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.继电器:
                    sensor = new Sensor_Relays(data[1], data[2], cachedata );
                    return sensor;
                case (byte)EnumType.温湿度传感器:
                    sensor = new Sensor_TH(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.烟雾传感器:
                    sensor = new Sensor_Smoke(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.红外传感器:
                    sensor = new Sensor_IT(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.震动传感器:
                    sensor = new Sensor_Shake(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.红外对射光栅:
                    sensor = new Sensor_Grating(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.智能插座:
                    sensor = new Sensor_Socket(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.ZigBee信号转发器:
                    sensor = new Sensor_315MTransponder(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.门磁报警器:
                    sensor = new Sensor_Magnetic(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.电动窗帘:
                    sensor = new Sensor_Curtain(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.空气温湿度:
                    sensor = new Sensor_CollectorATH(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.土壤温湿度:
                    sensor = new Sensor_CollectorSTH(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.二氧化碳浓度:
                    sensor = new Sensor_CollectorCO2(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.光照强度:
                    sensor = new Sensor_CollectorLightintensity(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.光强:
                    sensor = new Sensor_CollectorLightintensity(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.激光测距传感器:
                    sensor = new Sensor_LaserRanging(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.转速传感器:
                    sensor = new Sensor_Revolution(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.大气压力传感器:
                    sensor = new Sensor_BarometricPressure(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.PM25:
                    sensor = new Sensor_PM(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.角度传感器:
                    sensor = new Sensor__Angle(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.位移传感器:
                    sensor = new Sensor__Displacement(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.扭矩传感器:
                    sensor = new Sensor_Torque(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.拉力压力传感器:
                    sensor = new Sensor_Tension(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.超声波液位传感器:
                    sensor = new Sensor_UltrasonicLiquidLevel(data[1], data[2], cachedata);
                    return sensor;
                case (byte)EnumType.热偶传感器:
                    sensor = new Sensor_Thermocouple(data[1], data[2], cachedata);
                    return sensor;
                default:
                    sensor=new Sensor_Unknown(data[1], data[2], cachedata);
                    return sensor;
            }
        }


    }
}
