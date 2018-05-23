
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ES.Domain;



namespace ES.Business
{
   /// <summary>
   /// 传感器类型转换类
   /// </summary>
   public class SensorTypeApp
    {
     

        public static List<EnumSensorType> GetList()
        {
            List<EnumSensorType> list = new List<EnumSensorType>();
            foreach (EnumSensorType type in Enum.GetValues(typeof(EnumSensorType)))
            {
               
                list.Add(type);
            }
            return list;
        }

    }
}
