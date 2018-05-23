using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ES.Domain;
using ES.Utility;

namespace ES.Business
{
   public class MessageApp
    {


        /// <summary>
        /// 操作信息集合
        /// </summary>
        /// <returns></returns>
        public static List<Message> GetList()
        {
            return new ES_DB().MessageSet.ToList();
        }

        /// <summary>
        /// 根据时间查询操作信息
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public static List<Message> GetMessageList(DateTime dtStart, DateTime dtEnd)
        {

            return GetList().Where(p => p.Time >= dtStart.Date && p.Time <= dtEnd.Date).ToList();

        }

        /// <summary>
        /// 新增操作信息
        /// </summary>
        /// <param name="coordinator"></param>
        /// <returns></returns>
        public static OperationResult Insert(Message message)
        {
            try
            {
                ES_DB db = new ES_DB();               
                db.MessageSet.Add(message);
                db.SaveChanges();
                return new OperationResult(OperationResultType.Success);
            }
            catch (Exception err)
            {
                return new OperationResult(OperationResultType.Error, err.Message);
            }
        }

        /// <summary>
        /// 删除操作信息
        /// </summary>
        /// <param name="coordinator"></param>
        /// <returns></returns>
        public static OperationResult Delete(Message message)
        {
            try
            {
                ES_DB db = new ES_DB();
                //找到集合中对应实体类
                Message messageCore = db.MessageSet.Find(message.Id);
               
                db.MessageSet.Remove(messageCore);
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
