using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 烟雾传感器
    /// </summary>
    public class Sensor_Smoke:SensorBase
    {
        public Sensor_Smoke(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            string state = "";
            if (this.Data[0] == 0x01)
            {
                state = "有烟";
            }
            else
            {
                state = "无烟";
            }
            return "状态：" + Converts.ByteToString(this.Data[0]) + "  " + state;
        }
      
    }
}
