using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN.Sensor
{
    /// <summary>
    /// 大气压力传感器
    /// </summary>
   public class Sensor_BarometricPressure:SensorBase
    {
        public Sensor_BarometricPressure(byte type, byte addr, byte[] data) : base(type, addr, data)
        {

        }

        public override string GetData()
        {
            string str = "";
            if (this.Data[0] == 0xFF && this.Data[1] == 0xFF) 
            {
                str = "无数据";
            }
            else
            {
                str = (int.Parse((this.Data[0] * 256 + this.Data[1]).ToString()) / 10).ToString();
            }
            return "大气压：" + str + "Kpa";
        }
      

    }
}
