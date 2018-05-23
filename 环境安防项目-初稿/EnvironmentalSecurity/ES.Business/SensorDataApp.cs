using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ES.Domain;
using ES.Utility;

namespace ES.Business
{
    /// <summary>
    /// 传感器数据信息数据库操作类
    /// </summary>
   public class SensorDataApp
    {
        /// <summary>
        /// 传感器数据集合
        /// </summary>
        /// <returns></returns>
        public static List<SensorData> GetList()
        {
            return new ES_DB().SensorData.ToList();
        }

        /// <summary>
        /// 传感器对应数据
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        public static List<SensorData> GetSensorDataList(Sensor sensor)
        {
            return GetList().Where(p => p.SensorID == sensor.ID).ToList();
        }

        /// <summary>
        /// 根据时间查询传感器数据
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public static List<SensorData> GetSensorDataList(DateTime dtStart, DateTime dtEnd)
        {

            return GetList().Where(p =>p.Time>=dtStart.Date&&p.Time<=dtEnd.Date).ToList();

        }

        /// <summary>
        /// 新增传感器数据
        /// </summary>
        /// <param name="sensorData"></param>
        /// <returns></returns>
        public static OperationResult Insert(SensorData sensorData)
        {
            try
            {
                ES_DB db = new ES_DB();                
                db.SensorData.Add(sensorData);
                db.SaveChanges();
                return new OperationResult(OperationResultType.Success);
            }
            catch (Exception err)
            {
                return new OperationResult(OperationResultType.Error, err.Message);

            }
        }

        /// <summary>
        /// 删除传感器数据
        /// </summary>
        /// <param name="sensorData"></param>
        /// <returns></returns>
        public static OperationResult Delete(SensorData sensorData)
        {
            try
            {
                ES_DB db = new ES_DB();
                SensorData sensorCore = db.SensorData.Find(sensorData.ID);               
                db.SensorData.Remove(sensorCore);
                db.SaveChanges();
                return new OperationResult(OperationResultType.Success);
            }
            catch (Exception err)
            {
                return new OperationResult(OperationResultType.Error, err.Message);
            }
        }

    }
}
