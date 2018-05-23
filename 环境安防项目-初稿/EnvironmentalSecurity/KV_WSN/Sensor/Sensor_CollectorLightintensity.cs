using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 光照强度（采集器）
    /// </summary>
    public class Sensor_CollectorLightintensity:SensorBase
    {
        public Sensor_CollectorLightintensity(byte type, byte addr, byte[] data) : base(type, addr, data)
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
                str = (this.Data[0] * 256 + this.Data[1]).ToString() + "lux";
            }
            return "光照强度：" + str;
        }
       
    }
}
