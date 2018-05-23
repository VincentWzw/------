using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 门磁传感器
    /// </summary>
    public class Sensor_Magnetic:SensorBase
    {
        public Sensor_Magnetic(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            string state = "";
            if (this.Data[1] == 0x01)
            {
                state = "布防";
            }
            else
            {
                state = "撤防";
            }
            if (this.Data[0] == 0x01)
            {
                state = state + "，开";
            }
            else
            {
                state = state + "，关";
            }
            return "状态：" + state;
        }
       
    }
}
