using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
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
using System.Windows.Threading;
using ES.Business;
using ES.Domain;
using ES.Utility;
using KV_WSN.Sensor;
using Coordinator = KV_WSN.Coordinator;

namespace EnvironmentalSecurity
{
    /// <summary>
    /// UserControlinterface.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlinterface : UserControl
    {

        /// <summary>
        /// 获取默认值
        /// </summary>
        ControlApp controlApp = new ControlApp();

        /// <summary>
        /// 设置阈值
        /// </summary>
        ControlApp _controlApp = new ControlApp();

        /// <summary>
        /// 协调器接口
        /// </summary>
        KV_WSN.Coordinator kvCoordinator = new Coordinator();

        /// <summary>
        /// 协调器数据集合（协调器）
        /// </summary>
        private List<SensorBase> sensorList = new List<SensorBase>();

        /// <summary>
        /// 传感器
        /// </summary>
        Sensor sensor = new Sensor();

        /// <summary>
        /// 控制线程
        /// </summary>
        private bool threadPing = true;

        /// <summary>
        /// 显示的传感器集合
        /// </summary>
        ObservableCollection<ShowInterface> showInterfaces;

        public UserControlinterface()
        {
            InitializeComponent();



            cboRelay.ItemsSource = SensorApp.GetRelayList();
            cboRelay.SelectedIndex = 0;

            cboCoor.ItemsSource = CoordinatorApp.GetList().OrderByDescending(p => p.ID);
            cboCoor.SelectedIndex = 0;


        }

        /// <summary>
        /// 对应协调器的传感器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboCoor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showInterfaces = ShowInterface.ShowInterfacesList(cboCoor.SelectedItem as ES.Domain.Coordinator);
            lvSensorList.ItemsSource = showInterfaces;

        }

        /// <summary>
        /// 警报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlarm_OnClick(object sender, RoutedEventArgs e)
        {
            Sensor relay = new Sensor();
            relay = cboRelay.SelectedItem as ES.Domain.Sensor;
            if (relay != null)
            {
                if (lblAlarm.Content.ToString() == "警报")
                {
                    lblAlarm.Content = "取消";

                    kvCoordinator.OpenSerialPort(relay.Coordinator.COM);
                    byte[] Senddata = new byte[] { 0x00, 0x00, 0x01, 0x00, 0xFF };
                    kvCoordinator.SendData(relay.Type, (byte)relay.Address, Senddata);
                }
                else
                {
                    lblAlarm.Content = "警报";

                    byte[] Senddata = new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF };
                    kvCoordinator.SendData(relay.Type, (byte)relay.Address, Senddata);

                    Thread.Sleep(100);

                    kvCoordinator.CloseSerialPort();
                }
            }
            else
            {
                MessageBox.Show("请选择正确的继电器");
            }


        }

        /// <summary>
        /// 检测传感器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>      
        private void BtnDetection_OnClick(object sender, RoutedEventArgs e)
        {
            ES.Domain.Coordinator coor = cboCoor.SelectedItem as ES.Domain.Coordinator;

            Sensor relay = new Sensor();
            relay = cboRelay.SelectedItem as ES.Domain.Sensor;
            if (lblDetection.Content.ToString() == "检测")
            {
                threadPing = true;


                kvCoordinator.OpenSerialPort(coor.COM);

                Thread tStart = new Thread(new ThreadStart(tState));
                tStart.IsBackground = true;
                tStart.Start();

                OperationResult result = null;

                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "开始检测";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
                else
                {
                    lblDetection.Content = "取消";
                }

            }
            else
            {
                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "取消检测";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
                else
                {
                    byte[] Senddata = new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF };
                    kvCoordinator.SendData(relay.Type, (byte)relay.Address, Senddata);

                    Thread.Sleep(100);

                    threadPing = false;
                    kvCoordinator.CloseSerialPort();

                    lblDetection.Content = "检测";
                }
            }

        }


        /// <summary>
        /// 传感器布防/撤防
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeployment_OnClick(object sender, RoutedEventArgs e)
        {
            if (lblDeployment.Content.ToString() == "撤防")
            {
                IsChecked();
                lblDeployment.Content = "布防";
            }
            else
            {
                IsChecked();
                lblDeployment.Content = "撤防";
            }
        }


        #region 私有方法


        /// <summary>
        /// 控制线程
        /// </summary>
        private void tState()
        {
            while (threadPing)
            {
                kvCoordinator.Ping();

                Thread tAdd = new Thread(new ThreadStart(GetData));
                tAdd.IsBackground = true;
                tAdd.Start();



                Thread.Sleep(5000);
            }

        }

        /// <summary>
        /// Checked是否选中
        /// </summary>
        private void IsChecked()
        {
            ///co2
            if (chbCO2.IsChecked != true)
            {
                chbCO2.IsChecked = true;

                OperationResult result = null;

                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "二氧化碳传感器已布防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            else
            {
                chbCO2.IsChecked = false;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "二氧化碳传感器已撤防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

            }
            ///光强
            if (chbH.IsChecked != true)
            {
                chbH.IsChecked = true;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "光照强度传感器已布防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

            }
            else
            {
                chbH.IsChecked = false;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "光照强度传感器已撤防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            ///pm2.5
            if (chbPM25.IsChecked != true)
            {
                chbPM25.IsChecked = true;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "PM2.5传感器已布防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

            }
            else
            {
                chbPM25.IsChecked = false;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "PM2.5传感器已撤防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

            }
            //烟雾
            if (chbSmkoe.IsChecked != true)
            {
                chbSmkoe.IsChecked = true;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "烟雾传感器已布防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

            }
            else
            {
                chbSmkoe.IsChecked = false;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "烟雾传感器已撤防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            ///红外
            if(chbradInfrared.IsChecked!=true)
            {
                chbradInfrared.IsChecked = true;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "红外传感器已布防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

            }
            else
            {
                chbradInfrared.IsChecked = false;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "红外传感器已撤防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            ///温湿度
            if (chbT.IsChecked != true)
            {
                chbT.IsChecked = true;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "温湿度传感器已布防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

            }
            else
            {
                chbT.IsChecked = false;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "温湿度传感器已撤防";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// 获取传感器数据
        /// </summary>
        private void GetData()
        {

            this.Dispatcher.BeginInvoke
            (
                DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   sensorList = KV_WSN.Coordinator.sensorList.OrderByDescending(p => p.Type).ThenBy(p => p.Addr)
                       .ToList();
                   Thread.Sleep(100);
                   foreach (SensorBase sensorBase in sensorList)
                   {

                       switch (sensorBase.Type)
                       {
                               ///温湿度
                           case 0x30:
                               if (chbT.IsChecked == true)
                               {
                                   _controlApp.AirTemp = sensorBase.Data[1].ToString();
                                   _controlApp.AirHumi = sensorBase.Data[3].ToString();

                                   if (Convert.ToDouble(_controlApp.AirTemp) < controlApp.MinAirTemp ||
                                       Convert.ToDouble(_controlApp.AirTemp) > controlApp.MaxAirTemp)
                                   {
                                       ShowData(sensorBase, true);
                                       //SetRelay(sensorBase);
                                   }
                                   else if (Convert.ToDouble(_controlApp.AirHumi) < controlApp.MinAirHumi ||
                                            Convert.ToDouble(_controlApp.AirHumi) > controlApp.MaxAirHumi)
                                   {
                                       ShowData(sensorBase, true);
                                   }
                                   else
                                   {
                                       ShowData(sensorBase, false);
                                   }


                               }

                               break;
                               ///二氧化碳
                           case 0x33:
                               if (chbCO2.IsChecked == true)
                               {
                                   _controlApp.C02Thickness =
                                       (sensorBase.Data[0] * 256 + sensorBase.Data[1]).ToString();
                                   if (Convert.ToDouble(_controlApp.C02Thickness) < controlApp.MinC02Thickness ||
                                       Convert.ToDouble(_controlApp.C02Thickness) > controlApp.MaxIlluminance)
                                   {
                                       ShowData(sensorBase, true);

                                   }
                                   else
                                   {
                                       ShowData(sensorBase, false);
                                   }

                               }

                               break;
                               ///光照
                           case 0xC0:
                               if (chbH.IsChecked == true)
                               {
                                   _controlApp.Illuminance =
                                       (sensorBase.Data[0] * 256 + sensorBase.Data[1]).ToString();
                                   if (Convert.ToDouble(_controlApp.Illuminance) < controlApp.MinIlluminance ||
                                       Convert.ToDouble(_controlApp.Illuminance) > controlApp.MaxIlluminance)
                                   {
                                       ShowData(sensorBase, true);

                                   }
                                   else
                                   {
                                       ShowData(sensorBase, false);
                                   }

                               }

                               break;
                               ///烟雾
                           case 0x40:
                               if (chbSmkoe.IsChecked == true)
                               {

                                   if (sensorBase.Data[0] == 0x01)
                                   {
                                       _controlApp.Smoke = "1";
                                       ShowData(sensorBase, true);

                                   }
                                   else
                                   {
                                       _controlApp.Smoke = "0";
                                       ShowData(sensorBase, false);
                                   }


                               }

                               break;
                               ///红外
                           case 0x50:
                               if (chbradInfrared.IsChecked == true)
                               {

                                   if (sensorBase.Data[0] == 0x01)
                                   {
                                       _controlApp.radInfrared = "1";//有人
                                       ShowData(sensorBase, true);

                                   }
                                   else
                                   {
                                       _controlApp.radInfrared = "255";//无人
                                       ShowData(sensorBase, false);
                                   }


                               }

                               break;
                               ///pm2.5
                           case 0x44:
                               if (chbPM25.IsChecked == true)
                               {


                                   _controlApp.PM25 = (sensorBase.Data[0] * 256 + sensorBase.Data[1]).ToString();
                                   if (Convert.ToDouble(_controlApp.PM25) < controlApp.MinPM25 ||
                                       Convert.ToDouble(_controlApp.PM25) > controlApp.MaxPM25)
                                   {
                                       ShowData(sensorBase, true);

                                   }
                                   else
                                   {
                                       ShowData(sensorBase, false);
                                   }

                               }

                               break;


                       }
                   }
               }
            );
        }

        /// <summary>
        /// 控制警报
        /// </summary>
        private void SetRelay(SensorBase sensorBase)
        {
            Sensor relay = new Sensor();
            relay = cboRelay.SelectedItem as ES.Domain.Sensor;

            if (relay != null)
            {
                byte[] Senddata;
                Senddata = new byte[] { 0x00, 0x00, 0x01, 0x00, 0xFF };
                kvCoordinator.SendData(relay.Type, (byte)relay.Address, Senddata);
            }
            else
            {
                MessageBox.Show("请检查继电器连接是否正确");
            }

        }

        /// <summary>
        /// 显示的数据
        /// </summary>
        private void ShowData(SensorBase sensorBase, bool isStrike)
        {

            ShowInterface showInterface;

            if (isStrike == true)
            {
                sensor = SensorApp.GetCoorList(cboCoor.SelectedItem as ES.Domain.Coordinator)
                    .Find(p => p.Type == sensorBase.Type && p.Address == sensorBase.Addr);      //查找对应的传感器
                if (sensor != null)
                {
                    showInterface = showInterfaces.First(p => p.Name == sensor.Name);
                    showInterface.Time = DateTime.Now.ToLongTimeString();
                    showInterface.State = "已连接";
                    showInterface.Strike = "已触发";
                    showInterfaces.ToBindingList();

                    OperationResult result = null;
                    Message message = new Message();
                    message.Time = DateTime.Now;
                    message.Messages = "检测：" + showInterface.Name + "  " + showInterface.State + "  " +
                                       showInterface.Strike + "  已开启警报";
                    result = MessageApp.Insert(message);
                    if (result.ResultType != OperationResultType.Success)
                    {
                        MessageBox.Show(result.Message);
                        return;
                    }
                    else
                    {

                        SetRelay(sensorBase);   //设置警报的继电器
                    }
                }
                else
                {
                    MessageBox.Show("请检查连接的设备是否正确");
                }

            }
            else
            {
                sensor = SensorApp.GetCoorList(cboCoor.SelectedItem as ES.Domain.Coordinator)
                    .Find(p => p.Type == sensorBase.Type && p.Address == sensorBase.Addr);       //查找对应的传感器


                if (sensor != null)
                {
                    showInterface = showInterfaces.First(p => p.Name == sensor.Name);
                    showInterface.Time = DateTime.Now.ToLongTimeString();
                    showInterface.State = "已连接";
                    showInterface.Strike = "未触发";
                    showInterfaces.ToBindingList();

                    OperationResult result = null;
                    Message message = new Message();
                    message.Time = DateTime.Now;
                    message.Messages = "检测：" + showInterface.Name + "  " + showInterface.State + "  " +
                                       showInterface.Strike;
                    result = MessageApp.Insert(message);
                    if (result.ResultType != OperationResultType.Success)
                    {
                        MessageBox.Show(result.Message);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("请检查设备连接是否正确");
                }

            }


        }




        #endregion


    }


}
