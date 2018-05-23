using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 数码管
    /// </summary>
    public class Sensor_LED:SensorBase
    {
        public Sensor_LED(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            string state = "";
            for (int i = 0; i < 4; i++) 
            {
                state = state + this.Data[i].ToString();
            }
            if (this.Data[4] == 0x01)
            {
                state = state + "，开";
            }
            else
            {
                state = state + "，关";
            }
            return "数值：" + state;
        }

       

    }
}
