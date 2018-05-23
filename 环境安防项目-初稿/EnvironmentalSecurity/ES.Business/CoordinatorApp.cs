using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ES.Domain;
using ES.Utility;

namespace ES.Business
{
   public class CoordinatorApp
    {
        /// <summary>
        /// 协调器集合
        /// </summary>
        /// <returns></returns>
        public static List<Coordinator> GetList()
        {
            return new ES_DB().Coordinator.ToList();
        }

      
        /// <summary>
        /// 串口号是否冲突
        /// </summary>
        /// <param name="COM"></param>
        /// <returns></returns>
        public static OperationResult VerifyCOM(string COM)
        {
            try
            {
                ES_DB db = new ES_DB();
                if (db.Coordinator.Count(p => p.COM == COM) > 0)
                {
                    return new OperationResult(OperationResultType.ValidError, "串口号冲突！");
                }
                return new OperationResult(OperationResultType.Success);
            }
            catch (Exception err)
            {
                return new OperationResult(OperationResultType.Error, err.Message);
            }
        }

        /// <summary>
        /// 新增协调器
        /// </summary>
        /// <param name="coordinator"></param>
        /// <returns></returns>
        public static OperationResult Insert(Coordinator coordinator)
        {
            try
            {
                ES_DB db = new ES_DB();
             
                if (db.Coordinator.Count(p => p.Name== coordinator.Name)>0)
                {
                    return new OperationResult(OperationResultType.ValidError, "已存在此名称的协调器！");
                }
                db.Coordinator.Add(coordinator);          
                db.SaveChanges();
                return new OperationResult(OperationResultType.Success);
            }
            catch (Exception err)
            {
                return new OperationResult(OperationResultType.Error, err.Message);
            }
        }

        /// <summary>
        /// 编辑协调器
        /// </summary>
        /// <param name="coordinator"></param>
        /// <returns></returns>
        public static OperationResult Update(Coordinator coordinator)
        {
            try
            {
                ES_DB db = new ES_DB();
                Coordinator coordinatorCore = db.Coordinator.First(p => p.ID == coordinator.ID);
                if (coordinatorCore.Name!=coordinator.Name)
                {
                    if (db.Coordinator.Count(p => p.Name == coordinator.Name) > 0)
                    {
                        return new OperationResult(OperationResultType.ValidError, "已存在此名称的协调器！");
                    }
                }
                
                db.Entry(coordinatorCore).CurrentValues.SetValues(coordinator);
                db.SaveChanges();
                return new OperationResult(OperationResultType.Success);
            }
            catch (Exception err)
            {
                return new OperationResult(OperationResultType.Error, err.Message);
            }
        }
        
        /// <summary>
        /// 删除协调器
        /// </summary>
        /// <param name="coordinator"></param>
        /// <returns></returns>
        public static OperationResult Delete(Coordinator coordinator)
        {
            try
            {
                ES_DB db = new ES_DB();
                //找到集合中对应实体类
                Coordinator coorCore = db.Coordinator.Find(coordinator.ID);
                if (coorCore.Sensor.Count > 0)
                {
                    return new OperationResult(OperationResultType.ValidError, "此设备下有传感器！不允许删除！");
                }
                db.Coordinator.Remove(coorCore);
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

