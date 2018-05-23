using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 拉力压力传感器
    /// </summary>
    public class Sensor_Tension:SensorBase
    {
        public Sensor_Tension(byte type, byte addr, byte[] data) : base(type, addr, data)
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
            string state = "";
            if (this.Data[0] == 0xFF && this.Data[1] == 0xFF && str != "无数据") 
            {
                state = "-";
                str = (((byte)~this.Data[2]) * 256 + ((byte)~this.Data[3])).ToString();
            }
            return "拉力：" + state + str + "克";
        }
    
    }
}
