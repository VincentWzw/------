using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 继电器
    /// </summary>
    public class Sensor_Relays:SensorBase
    {
        public Sensor_Relays(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            string state = "";
            for (int i = 0; i < 4; i++)
            {
                state = state + this.Data[i].ToString() + "  ";
            }
            return "状态：" + state;
        }
     
    }
}
