using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 转速传感器
    /// </summary>
    public class Sensor_Revolution:SensorBase
    {
        public Sensor_Revolution(byte type, byte addr, byte[] data) : base(type, addr, data)
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
            return "转速：" + str + "圈/分钟";
        }
       
    }
}
