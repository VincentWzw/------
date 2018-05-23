using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 震动传感器
    /// </summary>
    public class Sensor_Shake:SensorBase
    {
        public Sensor_Shake(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            string state = "";
            if (this.Data[0] == 0x01)
            {
                state = "有震动";
            }
            else
            {
                state = "无震动";
            }
            return "状态：" + Converts.ByteToString(this.Data[0]) + "  " + state;
        }
      
    }
}
