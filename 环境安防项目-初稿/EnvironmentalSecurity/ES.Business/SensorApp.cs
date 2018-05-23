using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using ES.Domain;

using ES.Utility;

namespace ES.Business
{
    /// <summary>
    /// 传感器数据库操作类
    /// </summary>
   public class SensorApp
    {
       
        /// <summary>
        /// 传感器集合
        /// </summary>
        /// <returns></returns>
        public static List<Sensor> GetList()
        {
           return new ES_DB().Sensor.OrderBy(p => p.Type).ToList();
        }

        /// <summary>
        /// 对应协调器的传感器集合
        /// </summary>
        /// <param name="coor"></param>
        /// <returns></returns>
        public static List<Sensor> GetCoorList(Coordinator coor)
        {
            return GetList().Where(p =>p.CoordinatorID == coor.ID).OrderBy(p=>p.Type).ToList();
        }

        /// <summary>
        /// 对应传感器的数据集合
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        public static List<Sensor> GetSensorDataList(Sensor sensor)
        {
            return GetList().Where(p =>p.Type==sensor.Type&&p.Address==sensor.Address).OrderBy(p=>p.Type).ToList();
        }

        /// <summary>
        /// 继电器集合
        /// </summary>
        /// <returns></returns>
        public static List<Sensor> GetRelayList()
        {
            return GetList().Where(p => p.Type == 0x20).ToList();
        }

        /// <summary>
        /// 新增传感器
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        public static OperationResult Insert(Sensor sensor)
        {
            try
            {
                ES_DB db =new ES_DB();
                if (db.Sensor.Count(p => p.Name == sensor.Name) > 0)
                {
                    return new OperationResult(OperationResultType.ValidError, "已存在此名称的传感器！");
                }
                db.Sensor.Add(sensor);
                db.SaveChanges();
                return new OperationResult(OperationResultType.Success);
            }
            catch (Exception err)
            {
                return new OperationResult(OperationResultType.Error, err.Message);
            }
        }

        /// <summary>
        /// 编辑传感器
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        public static OperationResult Update(Sensor sensor)
        {
            try
            {
                ES_DB db = new ES_DB();
                Sensor sensorCore = db.Sensor.First(p =>p.ID == sensor.ID);
                if (sensorCore.Name!=sensor.Name)
                {
                    if (db.Sensor.Count(p => p.Name == sensor.Name) > 0)
                    {
                        return new OperationResult(OperationResultType.ValidError, "已存在此名称的传感器！");
                    }
                }
                db.Entry(sensorCore).CurrentValues.SetValues(sensor);
                db.SaveChanges();
                return new OperationResult(OperationResultType.Success);
            }
            catch (Exception err)
            {
                return new OperationResult(OperationResultType.Error, err.Message);
            }
        }

        /// <summary>
        /// 删除传感器
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        public static OperationResult Delete(Sensor sensor)
        {
            try
            {
                ES_DB db = new ES_DB();

                Sensor sensorCore = db.Sensor.Find(sensor.ID);
                if (sensorCore.SensorData.Count > 0)
                {
                    return new OperationResult(OperationResultType.ValidError, "传感器有数据无法删除！");
                }
               
                db.Sensor.Remove(sensorCore);
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
