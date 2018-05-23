using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// PM2.5
    /// </summary>
    public class Sensor_PM:SensorBase
    {
        public Sensor_PM(byte type, byte addr, byte[] data) : base(type, addr, data)
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
                str = (this.Data[0] * 256 + this.Data[1]).ToString();
            }
            return "PM2.5：" + str + "ug/m3";
        }
       
    }
}
