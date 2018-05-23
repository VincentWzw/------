using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 温湿度传感器
    /// </summary>
    public class Sensor_TH:SensorBase
    {
        public Sensor_TH(byte type, byte addr, byte[] data):base(type,addr,data)
        {
     
        }

        public override string GetData()
        {
            string str = "";
            if (this.DataStr == "FFFFFFFFFF")
            {
                str = "无数据";
            }
            else
            {
                str = this.Data[1].ToString() + "℃  " + "湿度：" + this.Data[3].ToString();
            }
            return "温度：" + str + "%";
        }
       
    }
}
