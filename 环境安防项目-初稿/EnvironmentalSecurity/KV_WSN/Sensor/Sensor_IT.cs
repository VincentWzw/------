using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 红外人体传感器
    /// </summary>
    public class Sensor_IT:SensorBase
    {
        public Sensor_IT(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            string state = "";
            if (this.Data[0] == 0x01)
            {
                state = "有人";
            }
            else
            {
                state = "无人";
            }
            return "状态：" + Converts.ByteToString(this.Data[0]) + "  " + state;
        }
       
    }
}
