using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ES.Domain;

namespace ES.Business
{
    /// <summary>
    /// 历史记录显示类
    /// </summary>
    public class ShowRefer
    {

        private string name;
        private DateTime time;
        private string data;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        /// <summary>
        /// 显示协调器传感器集合
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<ShowRefer> ShowReferCoorList(Coordinator coordinator)
        {

            ObservableCollection<ShowRefer> observableCollection = new ObservableCollection<ShowRefer>();

            foreach (Sensor sensor in GetCoorList(coordinator))
            {
                foreach (SensorData sensorData in GetSensorDatasList(sensor))
                {
                    ShowRefer showRefer = new ShowRefer();
                    showRefer.Name = sensor.Name;
                    showRefer.Time = (DateTime) sensorData.Time;
                    showRefer.Data = sensorData.Data;
                    observableCollection.Add(showRefer);
                }                                            
            }
           
            return observableCollection;
        }

        /// <summary>
        /// 根据时间查询传感器数据
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public static List<ShowRefer> ShowCoorTimeList(Coordinator coordinator, DateTime dtStart, DateTime dtEnd)
        {

            return ShowReferCoorList(coordinator).Where(p => p.Time >= dtStart.Date && p.Time <= dtEnd.Date).OrderByDescending(p=>p.Time).ToList();

        }

        /// <summary>
        /// 显示对应传感器集合
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<ShowRefer> ShowReferSensorList(Sensor sensor)
        {

            ObservableCollection<ShowRefer> observableCollection = new ObservableCollection<ShowRefer>();
            
                foreach (SensorData sensorData in GetSensorDatasList(sensor))
                {
                    ShowRefer showRefer = new ShowRefer();
                    showRefer.Name = sensor.Name;
                    showRefer.Time = (DateTime) sensorData.Time;
                    showRefer.Data = sensorData.Data;
                    observableCollection.Add(showRefer);
                }
         

            return observableCollection;
        }

      


        /// <summary>
        /// 对应协调器的传感器集合
        /// </summary>
        /// <param name="coor"></param>
        /// <returns></returns>
        public static List<Sensor> GetCoorList(Coordinator coor)
        {
            return GetList().Where(p => p.CoordinatorID == coor.ID).OrderBy(p => p.Type).ToList();
        }

        /// <summary>
        /// 传感器集合
        /// </summary>
        /// <returns></returns>
        public static List<Sensor> GetList()
        {
            return new ES_DB().Sensor.OrderBy(p => p.Type).ToList();
        }

        /// <summary>
        /// 传感器数据集合
        /// </summary>
        /// <returns></returns>
        public static List<SensorData> GetSensorDataList()
        {
            return new ES_DB().SensorData.OrderBy(p => p.Sensor.Type).ThenByDescending(p => p.Time).ToList();
        }

        /// <summary>
        /// 传感器对应数据
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        public static List<SensorData> GetSensorDatasList(Sensor sensor)
        {
            return GetSensorDataList().Where(p => p.SensorID == sensor.ID).ToList();
        }
      
        /// <summary>
        /// 根据时间查询传感器数据
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public static List<ShowRefer> ShowSensorDataTimeList(Sensor sensor, DateTime dtStart, DateTime dtEnd)
        {

            return ShowReferSensorList(sensor).Where(p => p.Time >= dtStart.Date && p.Time <= dtEnd.Date).OrderByDescending(p => p.Time).ToList();

        }

    }
}
