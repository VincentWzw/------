using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 热偶传感器
    /// </summary>
    public class Sensor_Thermocouple:SensorBase
    {
        public Sensor_Thermocouple(byte type, byte addr, byte[] data) : base(type, addr, data)
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
                byte[] bs = new byte[] { this.Data[0], this.Data[1], this.Data[2], this.Data[3] };
                int a = (int)Convert.ToInt64(Converts.BytesToString(bs), 16);
                str = (a / 100).ToString() + "." + (a % 100);
            }
            return "温度：" + str + "℃";
        }
    }
}
