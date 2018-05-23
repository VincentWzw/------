using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ES.Domain;

namespace ES.Business
{
    /// <summary>
    /// 主界面显示类
    /// </summary>
    public class ShowInterface: INotifyPropertyChanged
    {
        private string time;
        private string name;
        private string state;
        private string strike;

        public event PropertyChangedEventHandler PropertyChanged;


        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Time"));
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        public string State
        {
            get
            {               
                return state;
            }
            set
            {
                state = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("State"));
                }
            }
        }

        public string Strike
        {
            get
            {
               
                return strike;
            }
            set
            {
                strike = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Strike"));
                }
            }
        }


        /// <summary>
        /// 传感器集合
        /// </summary>
        /// <returns></returns>
        public static List<Sensor> GetList()
        {
            return new ES_DB().Sensor.Where(p => p.Type!=32).OrderBy(p=>p.CoordinatorID).ThenByDescending(p=>p.Type).ToList();
        }

        /// <summary>
        /// 主界面显示集合
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<ShowInterface> ShowInterfacesList(Coordinator coordinator)
        {
            ObservableCollection<ShowInterface> showInterfaceList=new ObservableCollection<ShowInterface>();
            foreach (Sensor sensor in GetCoorList(coordinator) )      
            {
                ShowInterface showInterface=new ShowInterface();
                showInterface.Name = sensor.Name;
                showInterface.Time = DateTime.Now.ToLongTimeString();
                showInterface.State = "未连接";
                showInterface.Strike = "未触发";
                showInterfaceList.Add(showInterface);
            }
            return showInterfaceList;
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
        /// 对应传感器的数据集合
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        public static List<Sensor> GetSensorDataList(Sensor sensor)
        {
            return GetList().Where(p => p.Type == sensor.Type && p.Address == sensor.Address).OrderBy(p => p.Type).ToList();
        }
    }
}
