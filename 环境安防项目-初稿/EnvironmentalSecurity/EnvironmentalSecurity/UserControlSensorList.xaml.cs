using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ES.Business;
using ES.Domain;
using ES.Utility;
using KV_WSN;
using KV_WSN.Sensor;

namespace EnvironmentalSecurity
{
    /// <summary>
    /// UserControlSensorList.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlSensorList : UserControl
    {

        /// <summary>
        /// 协调器接口
        /// </summary>
        KV_WSN.Coordinator kvCoordinator = new KV_WSN.Coordinator();

        /// <summary>
        /// 控制线程
        /// </summary>
        private bool threadPing = true;

        /// <summary>
        /// 传感器数据
        /// </summary>
        private SensorData _sensorData = new SensorData();

        /// <summary>
        /// 传感器
        /// </summary>
        private Sensor _sensor = new Sensor();

        /// <summary>
        /// 协调器
        /// </summary>
        private ES.Domain.Coordinator _coordinator;

        /// <summary>
        /// 显示数据集合
        /// </summary>
        private ObservableCollection<ShowSensorData> data;

        /// <summary>
        /// 协调器数据集合（协调器）
        /// </summary>
        private List<SensorBase> sensorList = new List<SensorBase>();

        public UserControlSensorList()
        {
            InitializeComponent();
            SetListCoor();

            lvSensorList.ItemsSource = data;

        }

        /// <summary>
        /// listbox变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstCoor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lstCoor.SelectedItem != null)
            {

                try
                {

                    _coordinator = GetListboxSelected();
                    if (_coordinator != null)
                    {
                        if (lblPort.Content.ToString()=="打开串口"&&lblPing.Content.ToString()=="Ping")
                        {

                            _sensor.CoordinatorID = _coordinator.ID;
                            data = ShowSensorData.ShowSensorDatasList(_coordinator);


                            lvSensorList.ItemsSource = data;

                            lblSensorCount.Content = "传感器数量：" + lvSensorList.Items.Count;
                        }
                        else
                        {
                            kvCoordinator.CloseSerialPort();
                            threadPing = false;

                            lblPort.Content = "打开串口";
                            lblPing.Content = "Ping";

                            _sensor.CoordinatorID = _coordinator.ID;
                            data = ShowSensorData.ShowSensorDatasList(_coordinator);


                            lvSensorList.ItemsSource = data;

                            lblSensorCount.Content = "传感器数量：" + lvSensorList.Items.Count;

                        }
                    }


                }
                catch (Exception)
                {

                }

            }




        }




        /// <summary>
        /// 新增协调器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInsertCoor_OnClick(object sender, RoutedEventArgs e)
        {
            ES.Domain.Coordinator coor = new ES.Domain.Coordinator();
            WindowCoorInfo windowCoorInfo = new WindowCoorInfo(coor, true);
            if (windowCoorInfo.ShowDialog() == true)
            {
                SetListCoor();

            }
        }

        /// <summary>
        /// 编辑协调器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>          
        private void BtnUpdateCoor_OnClick(object sender, RoutedEventArgs e)
        {
            _coordinator = GetListboxSelected();
            WindowCoorInfo windowCoorInfo = new WindowCoorInfo(_coordinator, false);
            if (windowCoorInfo.ShowDialog() == true)
            {
                SetListCoor();
            }
        }

        /// <summary>
        /// 删除协调器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void BtnDeleteCoor_OnClick(object sender, RoutedEventArgs e)
        {
            if (lstCoor.SelectedItem != null)
            {
                if (MessageBox.Show("是否删除该项", "询问", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    OperationResult result = null;
                    if (GetListboxSelected() != null)
                    {
                        _coordinator = GetListboxSelected();
                        result = CoordinatorApp.Delete(_coordinator);

                        Message message = new Message();
                        message.Time = DateTime.Now;
                        message.Messages = "删除协调器:" + _coordinator.Name + "    串口号：" + _coordinator.COM + "    PanID:" +
                                           _coordinator.PanID;


                        if (result.ResultType != OperationResultType.Success)
                        {
                            MessageBox.Show(result.Message);
                        }
                        else
                        {
                            MessageApp.Insert(message);
                            SetListCoor();
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择要删除的协调器！");
                    }
                }
            }
        }


        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>     
        private void BtnOpenPortSensor_OnClick(object sender, RoutedEventArgs e)
        {
            if (lstCoor.SelectedItem != null)
            {
                if (lblPort.Content.ToString() == "打开串口")
                {

                    if (lstCoor != null)
                    {

                        _coordinator = GetListboxSelected();
                        kvCoordinator.OpenSerialPort(_coordinator.COM);
                        lblPort.Content = "关闭串口";

                        OperationResult result;
                        Message message = new Message();
                        message.Time = DateTime.Now;
                        message.Messages = "打开串口：" + _coordinator.COM;
                        result = MessageApp.Insert(message);
                        if (result.ResultType != OperationResultType.Success)
                        {
                            MessageBox.Show(result.Message);
                            return;
                        }


                    }

                }
                else
                {
                    if (lblPing.Content.ToString() != "取消")
                    {
                        kvCoordinator.CloseSerialPort();
                        lblPort.Content = "打开串口";

                        OperationResult result;
                        Message message = new Message();
                        message.Time = DateTime.Now;
                        message.Messages = "关闭串口:" + _coordinator.COM;
                        result = MessageApp.Insert(message);
                        if (result.ResultType != OperationResultType.Success)
                        {
                            MessageBox.Show(result.Message);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("请先取消Ping");
                    }

                }
            }
            else
            {
                MessageBox.Show("请先选择协调器！");
            }

        }

        /// <summary>
        /// 获取传感器数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>      
        private void BtnPingSensor_OnClick(object sender, RoutedEventArgs e)
        {
            if (lblPort.Content.ToString() == "关闭串口")
            {
                if (lblPing.Content.ToString() == "Ping")
                {
                    threadPing = true;

                    Thread tStart = new Thread(new ThreadStart(tState));
                    tStart.IsBackground = true;
                    tStart.Start();

                    lblPing.Content = "取消";
                }
                else
                {

                    lblPing.Content = "Ping";
                    threadPing = false;

                    OperationResult result = null;
                    Message message = new Message();
                    message.Time = DateTime.Now;
                    message.Messages = "取消获取传感器数据";
                    result = MessageApp.Insert(message);
                    if (result.ResultType != OperationResultType.Success)
                    {
                        MessageBox.Show(result.Message);
                        return;
                    }


                }

            }
            else
            {
                MessageBox.Show("请先打开串口！");
            }
        }
        /// <summary>
        /// 新增传感器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void BtnInsertSensor_OnClick(object sender, RoutedEventArgs e)
        {
            if (lstCoor.SelectedItem != null)
            {
                _coordinator = GetListboxSelected();
                Sensor sensor = new Sensor();

                if (lstCoor != null)
                {
                    sensor.CoordinatorID = _coordinator.ID;

                    WindowSensorInfo windowSensorInfo = new WindowSensorInfo(sensor, true);
                    if (windowSensorInfo.ShowDialog() == true)
                    {
                        _coordinator = GetListboxSelected();
                        lvSensorList.ItemsSource = ShowSensorData.ShowSensorDatasList(_coordinator);
                        lblSensorCount.Content = "数量：" + lvSensorList.Items.Count;

                    }
                }

            }
            else
            {
                MessageBox.Show("请先选择协调器！");
            }
        }

        /// <summary>
        /// 编辑传感器信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void BtnUpdateSensor_OnClick(object sender, RoutedEventArgs e)
        {
            if (lstCoor.SelectedItem != null)
            {
                ShowSensorData showSensorData = GetListviewSelected();
                if (showSensorData != null)
                {
                    Sensor sensor;
                    sensor = SensorApp.GetList().Where(p => p.Name == showSensorData.Name).First();
                    WindowSensorInfo windowSensorInfo = new WindowSensorInfo(sensor, false);
                    if (windowSensorInfo.ShowDialog() == true)
                    {
                        _coordinator = GetListboxSelected();
                        lvSensorList.ItemsSource = ShowSensorData.ShowSensorDatasList(_coordinator);
                        lblSensorCount.Content = "数量：" + lvSensorList.Items.Count;
                    }
                }
                else
                {
                    MessageBox.Show("请先选择编辑的传感器");
                }
            }
        }

        /// <summary>
        /// 删除传感器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void BtnDeleteSebsor_OnClick(object sender, RoutedEventArgs e)
        {
            if (lstCoor.SelectedItem != null)
            {
                OperationResult result = null;
                ShowSensorData showSensorData = GetListviewSelected();
                if (showSensorData != null)
                {
                    if (MessageBox.Show("是否删除该项", "询问", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {


                        Sensor sensor;
                        sensor = SensorApp.GetList().Where(p => p.Name == showSensorData.Name).First();
                        result = SensorApp.Delete(sensor);

                        Message message = new Message();
                        message.Time = DateTime.Now;
                        message.Messages = "删除" + sensor.Coordinator.Name + "协调器下" + "传感器:" + sensor.Name + "    类型：" + sensor.Type + "    地址:" +
                                           sensor.Address;

                        if (result.ResultType != OperationResultType.Success)
                        {
                            MessageBox.Show(result.Message);
                        }
                        else
                        {
                            MessageApp.Insert(message);
                            _coordinator = GetListboxSelected();
                            lvSensorList.ItemsSource = ShowSensorData.ShowSensorDatasList(_coordinator);
                            lblSensorCount.Content = "数量：" + lvSensorList.Items.Count;
                        }


                    }
                }
                else
                {
                    MessageBox.Show("请选择要删除的传感器！");
                }

            }
        }

        #region 私有方法

        /// <summary>
        /// 获取Listview选中项
        /// </summary>
        /// <returns></returns>
        private ShowSensorData GetListviewSelected()
        {
            return lvSensorList.SelectedItem as ShowSensorData;
        }

        /// <summary>
        /// 选择协调器
        /// </summary>
        /// <returns></returns>
        private ES.Domain.Coordinator GetListboxSelected()
        {
            return (lstCoor.SelectedItem as Label).Tag as ES.Domain.Coordinator;

        }

        /// <summary>
        /// 设置协调器列表
        /// </summary>
        private void SetListCoor()
        {

            lstCoor.Items.Clear();
            foreach (ES.Domain.Coordinator coor in CoordinatorApp.GetList())
            {
                Label lbl = new Label();
                lbl.Content = coor.Name;
                lbl.Tag = coor;
                lstCoor.Items.Add(lbl);
            }
            this.lstCoor.SelectedIndex = 0;

            lblSensorCount.Content = "传感器数量：" + lvSensorList.Items.Count;

        }

        /// <summary>
        /// 添加接收的传感器数据导数据库
        /// </summary>
        private void InsertSensordata()
        {

            sensorList = KV_WSN.Coordinator.sensorList.OrderByDescending(p => p.Type).ThenBy(p => p.Addr).ToList();
            foreach (SensorBase sensor in sensorList)
            {
                switch (sensor.Type)
                {

                    case 0x30:
                        GetSensorData(sensor);
                        break;
                    case 0x33:
                        GetSensorData(sensor);
                        break;
                    case 0xC0:
                        GetSensorData(sensor);
                        break;
                    case 0x40:
                        GetSensorData(sensor);
                        break;
                    case 0x44:
                        GetSensorData(sensor);
                        break;
                    case 0x20:
                        GetSensorData(sensor);
                        break;
                    case 0x50:
                        GetSensorData(sensor);
                        break;

                }

                Thread.Sleep(10);
            }
            Thread.Sleep(10000);

        }

        /// <summary>
        /// 获取传感器数据
        /// </summary>
        /// <param name="sensor">传感器</param>
        private void GetSensorData(SensorBase sensor)
        {
            ShowSensorData showSensorData;
           
            _sensor = SensorApp.GetCoorList(_coordinator).Find(p => p.Type == sensor.Type && p.Address == sensor.Addr);
            OperationResult result;
            if (_sensor != null)
            {
                try
                {
                    _sensorData.SensorID = _sensor.ID;
                    _sensorData.Time = DateTime.Now;
                    _sensorData.Data = sensor.GetData();
                    result = SensorDataApp.Insert(_sensorData);


                    Message message = new Message();
                    message.Time = (DateTime)_sensorData.Time;
                    message.Messages = "获取" + _sensor.Name + "数据：" + _sensorData.Data + "   状态：" + "已连接";
                    MessageApp.Insert(message);


                    showSensorData = data.Where(p => p.Name == _sensor.Name).First();
                    showSensorData.Data = sensor.GetData();
                    showSensorData.State = "已连接";
                    showSensorData.Time = DateTime.Now.ToString();
                }
                catch (Exception)
                {

                }

            }
            else
            {
                MessageBox.Show("传感器信息不符，请检查设备");
            }
        }

        /// <summary>
        /// 控制线程
        /// </summary>
        private void tState()
        {
            while (threadPing)
            {
                kvCoordinator.Ping();

                Thread tAdd = new Thread(new ThreadStart(InsertSensordata));
                tAdd.IsBackground = true;
                tAdd.Start();



                Thread.Sleep(5000);
            }

        }

        #endregion



    }
}
