using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// CO2浓度（采集器）
    /// </summary>
    public class Sensor_CollectorCO2:SensorBase
    {
        public Sensor_CollectorCO2(byte type, byte addr, byte[] data) : base(type, addr, data)
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
                str = (this.Data[0] * 256 + this.Data[1]).ToString() + "ppm";
            }
            return "CO2浓度：" + str;
        }
      
    }
}
