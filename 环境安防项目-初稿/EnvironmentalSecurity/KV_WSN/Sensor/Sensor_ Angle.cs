using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 角度传感器
    /// </summary>
   public class Sensor__Angle:SensorBase
    {
        public Sensor__Angle(byte type, byte addr, byte[] data) : base(type, addr, data)
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
                str = (this.Data[2] * 256 + this.Data[3]).ToString();
            }
            return "角度：" + str + "度";
        }
        
    }
}
