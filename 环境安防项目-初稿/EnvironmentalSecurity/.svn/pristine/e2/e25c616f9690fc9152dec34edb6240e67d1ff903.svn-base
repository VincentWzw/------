using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 空气温湿度（采集器）
    /// </summary>
    public class Sensor_CollectorATH:SensorBase
    {
        public Sensor_CollectorATH(byte type, byte addr, byte[] data) : base(type, addr, data)
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
                str = ((this.Data[0] * 256 + this.Data[1]) / 100).ToString() + "." + ((this.Data[0] * 256 + this.Data[1]) % 100) + "℃" + 
                    "  空气湿度：" + ((this.Data[2] * 256 + this.Data[3]) / 100).ToString() + "." + ((this.Data[2] * 256 + this.Data[3]) % 100);
            }
            return "空气温度：" + str + "%";
        }
       
    }
}
