using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Documents;
using ES.Domain;
using KV_WSN;
using KV_WSN.Sensor;
using Coordinator = ES.Domain.Coordinator;

namespace ES.Business
{
    /// <summary>
    /// 数据信息显示类
    /// </summary>
    public class ShowSensorData:INotifyPropertyChanged
    {
       

        private string name;
        private string address;
        private string time;
        private string data;
        private string state;

        public event PropertyChangedEventHandler PropertyChanged;
        

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (this.PropertyChanged!=null)
                {
                    this.PropertyChanged.Invoke(this,new PropertyChangedEventArgs("Name"));
                }
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Address"));
                }
            }
        }

        public string Data
        {
            get { return data; }
            set
            {
                data = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Data"));
                }
            }
        }

        public string State
        {
            get { return state; }
            set
            {
               
                state = value;
                
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("State"));
                }
            }
        }

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

       

         /// <summary>
         /// 显示传感器集合
         /// </summary>
         /// <returns></returns>
        public static ObservableCollection<ShowSensorData> ShowSensorDatasList(Coordinator coordinator)
        { 
            
            ObservableCollection<ShowSensorData> observableCollection=new ObservableCollection<ShowSensorData>();
          
            foreach (Sensor sensor in GetCoorList(coordinator))
            {
                ShowSensorData showSensor = new ShowSensorData();
                showSensor.Name = sensor.Name;
                showSensor.Address = Converts.ByteToString((byte) sensor.Address);
                showSensor.State = "未连接";
                observableCollection.Add(showSensor);
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
       
    }
}
