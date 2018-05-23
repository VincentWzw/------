using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 智能插座
    /// </summary>
    public class Sensor_Socket:SensorBase
    {
        public Sensor_Socket(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            string state = "";
            if (this.Data[0] == 0x01)
            {
                state = "开";
            }
            else
            {
                state = "关";
            }
            return "状态：" + Converts.ByteToString(this.Data[0]) + "  " + state;
        }
     
    }
}
