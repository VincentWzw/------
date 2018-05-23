using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 红外对射光栅
    /// </summary>
    public class Sensor_Grating:SensorBase
    {
        public Sensor_Grating(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            return "状态：" + Converts.ByteToString(this.Data[0]);
        }
       
    }
}
