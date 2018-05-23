using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 电动窗帘
    /// </summary>
    public class Sensor_Curtain:SensorBase
    {
        public Sensor_Curtain(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            string state = "";
            if (this.Data[0] == 0x01)
            {
                state = "开";
            }
            else if (this.Data[0] == 0x02)
            {
                state = "关";
            }
            else
            {
                state = "停";
            }
            return "状态：" + Converts.ByteToString(this.Data[0]) + "  " + state;
        }
       
    }
}
