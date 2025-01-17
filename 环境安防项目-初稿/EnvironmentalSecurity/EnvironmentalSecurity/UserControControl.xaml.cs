﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using KV_WSN.Sensor;
using System.Windows.Threading;
using System.Threading;
using EnvironmentalSecurity.TypeConvertTool;

namespace EnvironmentalSecurity
{
    /// <summary>
    /// UserControLintelligentControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserControLintelligentControl : UserControl
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
        private KV_WSN.Coordinator kvCoordinator = new KV_WSN.Coordinator();

        /// <summary>
        /// 传感器数据集合
        /// </summary>
        private List<SensorBase> sensorList = new List<SensorBase>();

        /// <summary>
        /// 控制线程
        /// </summary>
        private bool threadPing=true;

        public UserControLintelligentControl()
        {
            InitializeComponent();

            SetListRelay(); //设置继电器列表
        }    

      


        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDefault_OnClick(object sender, RoutedEventArgs e)
        {
            ReadScheme();  //获取默认值

            
            OperationResult result = null;
            Message message = new Message();
            message.Time = DateTime.Now;
            message.Messages = "获取智能控制默认值";
            result = MessageApp.Insert(message);
            if (result.ResultType != OperationResultType.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }
        }

        /// <summary>
        /// 保存阈值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
           
            SaveScheme(); //保存阈值

            OperationResult result = null;
            Message message = new Message();
            message.Time = DateTime.Now;
            message.Messages = "设置智能控制阈值";
            result = MessageApp.Insert(message);
            if (result.ResultType != OperationResultType.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }
        }

        /// <summary>
        /// 智能控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnControl_OnClick(object sender, RoutedEventArgs e)
        {
            if (lblPort.Content.ToString() != "打开串口")
            {
                if (lblControl.Content.ToString() == "智能控制")
                {
                  
                    Thread tStart = new Thread(new ThreadStart(tState));     //开启智能控制线程
                    tStart.IsBackground = true;
                    tStart.Start();

                    lblControl.Content = "取消";

                    OperationResult result = null;
                    Message message = new Message();
                    message.Time = DateTime.Now;
                    message.Messages = "开始智能控制";
                    result = MessageApp.Insert(message);
                    if (result.ResultType != OperationResultType.Success)
                    {
                        MessageBox.Show(result.Message);
                        return;
                    }
                }
                else
                {
                    Sensor relay = new Sensor();
                    relay = GetListboxSelected();


                    lblControl.Content = "智能控制";

                    byte[] Senddata = new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF };    //关闭继电器
                    kvCoordinator.SendData(relay.Type, (byte)relay.Address, Senddata);

                    threadPing = false;  //取消Ping线程


                    OperationResult result = null;
                    Message message = new Message();
                    message.Time = DateTime.Now;
                    message.Messages = "取消智能控制";
                    result = MessageApp.Insert(message);
                    if (result.ResultType != OperationResultType.Success)
                    {
                        MessageBox.Show(result.Message);
                        return;
                    }
                    kvCoordinator.CloseSerialPort();
                }
            }
            else
            {
                MessageBox.Show("请先打开串口");
            }
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenPortSensor_OnClick(object sender, RoutedEventArgs e)
        {

            if (lblPort.Content.ToString() == "打开串口")
            {
                Sensor relay = new Sensor();
                relay = GetListboxSelected();

                kvCoordinator.OpenSerialPort(relay.Coordinator.COM);

                lblPort.Content = "关闭串口";
            }
            else
            {

                if (lblControl.Content.ToString()=="智能控制")
                {
                    kvCoordinator.CloseSerialPort();

                    lblPort.Content = "打开串口";
                }
                else
                {
                    MessageBox.Show("请先取消智能控制");
                }
                
            }
        }
      
     

        #region 私有方法

        /// <summary>
        /// 设置继电器器列表
        /// </summary>

        private void SetListRelay()
        {
            lstRelay.Items.Clear();

            foreach (ES.Domain.Sensor relay in SensorApp.GetRelayList())
            {

                Label lbl = new Label();
                lbl.Content = relay.Name + "  " + "地址：" + Converts.ByteToString((byte)relay.Address);
                lbl.Tag = relay;
                lstRelay.Items.Add(lbl);
            }
            this.lstRelay.SelectedIndex = 0;

        }

        /// <summary>
        /// 控制线程
        /// </summary>
        private void tState()
        {
            while (threadPing)
            {

                kvCoordinator.Ping();
                Thread tAdd = new Thread(new ThreadStart(GetSensorData));
                tAdd.IsBackground = true;
                tAdd.Start();

                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// 选择继电器
        /// </summary>
        /// <returns></returns>
        private ES.Domain.Sensor GetListboxSelected()
        {

            return (lstRelay.SelectedItem as Label).Tag as ES.Domain.Sensor;

        }

        /// <summary>
        /// 环境参数
        /// </summary>
        private void GetSensorData()
        {
            this.Dispatcher.BeginInvoke
            (
                DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    if (_controlApp != null)
                    {
                        _controlApp = controlApp;
                    }

                    sensorList = KV_WSN.Coordinator.sensorList.OrderByDescending(p => p.Type).ToList();  //传感器数据集合

                    foreach (SensorBase sensor in sensorList)
                    {
                        Thread.Sleep(100);

                        //判断连接传感器的类型
                        switch (sensor.Type)
                        {
                            case 0xC0://光照
                                lblIlluminance.Content = sensor.GetData();

                                _controlApp.Illuminance = (sensor.Data[0] * 256 + sensor.Data[1]).ToString();
                                break;
                            case 0x30://温湿度
                                lblTemperature.Content = sensor.GetData().Split(' ')[0];
                                lblHumidity.Content = sensor.GetData().Split(' ')[2];

                                _controlApp.AirTemp = sensor.Data[1].ToString();
                                _controlApp.AirHumi = sensor.Data[3].ToString();
                                break;
                            case 0x33://co2
                                lblCO2.Content = sensor.GetData();

                                _controlApp.C02Thickness = (sensor.Data[0] * 256 + sensor.Data[1]).ToString();
                                break;
                            case 0x44://pm2.5
                                lblPM25.Content = sensor.GetData();

                                _controlApp.PM25 = (sensor.Data[0] * 256 + sensor.Data[1]).ToString();
                                break;
                            case 0x40://烟雾
                                lblSmoke.Content = sensor.GetData();
                             
                                if (sensor.Data[0]==0x01)
                                {
                                    _controlApp.Smoke = "1";
                                }
                                else
                                {
                                    _controlApp.Smoke = "0";
                                }
                               
                                break;
                            case 0x50://红外
                                lblradInfrared.Content = sensor.GetData();
                              

                                if (sensor.Data[0] == 0x01)
                                {
                                    _controlApp.radInfrared = "1";
                                }
                                else
                                {
                                    _controlApp.radInfrared = "255";
                                }

                                break;

                            case 0x34://？？？
                                lblIlluminance.Content = sensor.GetData();
                             
                                break;
                            case 0x20://？？？

                                //继电器控制
                                switch (sensor.Addr)
                                {
                                    case 0x3F:
                                        SetRelay(sensor, true,_controlApp); 
                                        break;
                                    case 0x41:
                                        SetRelay(sensor, false,_controlApp);
                                        break;
                                   
                                }
                              

                                break;

                            default:
                                break;
                        }

                    }

                }

            );
        }

        /// <summary>
        /// 设置继电器的智能控制
        /// </summary>
        private void SetRelay(SensorBase sensor,bool isRelayNum,ControlApp control)
        {

             _controlApp=control;
            byte bIlluminance = 0x00;
            byte bAirTemp = 0x00;
            byte bAirHumi = 0x00;
            byte bCO2 = 0x00;
            byte bPM25 = 0x00;
            byte bSmoke = 0x00;
            byte bRedinfrared = 0x00;//红外
           
            if (Convert.ToDouble(_controlApp.AirTemp) < controlApp.MinAirTemp)
            {
                bAirTemp = 0x01;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测温度小于阈值，已自动打开温控系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            else if (Convert.ToDouble(_controlApp.AirTemp) > controlApp.MaxAirTemp)
            {
                bAirTemp = 0x00;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测温度大于阈值，已自动打开温控系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            if (Convert.ToDouble(_controlApp.AirHumi) < controlApp.MinAirHumi)
            {
                bAirHumi = 0x01;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测湿度小于阈值，已自动打开湿控系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            else if (Convert.ToDouble(_controlApp.AirHumi) > controlApp.MaxAirHumi)
            {
                bAirHumi = 0x00;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测湿度大于阈值，已自动打开湿控系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

            }
            if (Convert.ToDouble(_controlApp.PM25) < controlApp.MinPM25)
            {
                bPM25 = 0x01;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测PM25小于阈值，已自动打开净化系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            else if (Convert.ToDouble(_controlApp.PM25) > controlApp.MaxPM25)
            {
                bPM25 = 0x00;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测PM25大于阈值，已自动打开净化系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            if (Convert.ToDouble(_controlApp.C02Thickness) < controlApp.MinC02Thickness)
            {
                bCO2 = 0x01;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测CO2小于阈值，已自动打开排风系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            else if (Convert.ToDouble(_controlApp.C02Thickness) > controlApp.MaxC02Thickness)
            {
                bCO2 = 0x00;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测CO2大于阈值，已自动打开湿控系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            if (Convert.ToDouble(_controlApp.Illuminance) < controlApp.MinIlluminance)
            {
                bIlluminance = 0x01;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测光照强度小于阈值，已自动打开遮阳系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            else if (Convert.ToDouble(_controlApp.Illuminance) > controlApp.MaxIlluminance)
            {
                bIlluminance = 0x00;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测光照强度大于阈值，已自动打开遮阳系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
            }
            if (Convert.ToDouble(_controlApp.Smoke)!=0)
            {
                bSmoke = 0x01;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测有烟，已自动打开排风系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }         
         
            }
            if (Convert.ToDouble(_controlApp.radInfrared) != 0)
            {
                 bRedinfrared= 0x01;

                OperationResult result = null;
                Message message = new Message();
                message.Time = DateTime.Now;
                message.Messages = "智能控制：检测有人，已自动打开监测系统！";
                result = MessageApp.Insert(message);
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

            }

            if (isRelayNum == true)
            {
                Thread.Sleep(10);
                byte[] Senddata = sensor.Data;
                Senddata = new byte[] { bAirTemp, bAirHumi, bCO2, bIlluminance, Senddata[4] };
                kvCoordinator.SendData(sensor.Type, sensor.Addr, Senddata);
            }
            else
            {
                Thread.Sleep(10);
                byte[] Senddata = sensor.Data;
                Senddata = new byte[] { bPM25, bSmoke,bRedinfrared, Senddata[2], Senddata[3], Senddata[4] };
                kvCoordinator.SendData(sensor.Type, sensor.Addr, Senddata);
            }
           

        }

        /// <summary>
        /// 获取默认阈值
        /// </summary>
        private void ReadScheme()
        {
            txtHumidityMin.Text = controlApp.MinAirHumi.ToString();
            txtHumidityMax.Text = controlApp.MaxAirHumi.ToString();
            txtTemperatureMin.Text = controlApp.MinAirTemp.ToString();
            txtTemperatureMax.Text = controlApp.MaxAirTemp.ToString();
            txtCO2Min.Text = controlApp.MinC02Thickness.ToString();
            txtCO2Max.Text = controlApp.MaxC02Thickness.ToString();
            txtPM25Min.Text = controlApp.MinPM25.ToString();
            txtPM25Max.Text = controlApp.MaxPM25.ToString();
            txtIlluminanceMin.Text = controlApp.MinIlluminance.ToString();
            txtIlluminanceMax.Text = controlApp.MaxIlluminance.ToString();
        }

       

        /// <summary>
        /// 保存阈值
        /// </summary>
        private void SaveScheme()
        {
           
                if (txtTemperatureMin.Text != "")
                {
                    if (Regex.IsMatch(txtTemperatureMin.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MinAirTemp = Convert.ToDouble(txtTemperatureMin.Text);
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                   
                }
                else
                {
                    txtTemperatureMin.Text = _controlApp.MinAirTemp.ToString();
                }
                if (txtTemperatureMax.Text != "")
                {
                    if (Regex.IsMatch(txtTemperatureMax.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MaxAirTemp = Convert.ToDouble(txtTemperatureMax.Text);
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                  
                }
                else
                {
                    txtTemperatureMax.Text = _controlApp.MaxAirTemp.ToString();
                }

                if (txtHumidityMin.Text != "")
                {
                    if (Regex.IsMatch(txtHumidityMin.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MinAirHumi = Convert.ToDouble(txtHumidityMin.Text);
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                  
                }
                else
                {
                    txtHumidityMin.Text = _controlApp.MinAirHumi.ToString();
                }

                if (txtHumidityMax.Text != "")
                {
                    if (Regex.IsMatch(txtHumidityMax.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MaxAirHumi = Convert.ToDouble(txtHumidityMax.Text);
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                  
                }
                else
                {
                    txtHumidityMax.Text = _controlApp.MaxAirHumi.ToString();
                }

                if (txtCO2Min.Text != "")
                {
                    if (Regex.IsMatch(txtCO2Min.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MinC02Thickness = Convert.ToDouble(txtCO2Min.Text);
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                    
                }
                else
                {
                    txtCO2Min.Text = _controlApp.MinC02Thickness.ToString();
                }
                if (txtCO2Max.Text != "")
                {
                    if (Regex.IsMatch(txtCO2Max.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MaxC02Thickness = Convert.ToDouble(txtCO2Max.Text); ;
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                   
                }
                else
                {
                    txtCO2Max.Text = _controlApp.MaxC02Thickness.ToString();
                }
                if (txtPM25Min.Text != "")
                {
                    if (Regex.IsMatch(txtPM25Min.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MinPM25 = Convert.ToDouble(txtPM25Min.Text); ;
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                  
                }
                else
                {
                    txtPM25Min.Text = _controlApp.MinPM25.ToString();
                }

                if (txtPM25Max.Text != "")
                {
                    if (Regex.IsMatch(txtPM25Max.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MaxPM25 = Convert.ToDouble(txtPM25Max.Text);
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                   
                }
                else
                {
                    txtPM25Max.Text = _controlApp.MaxPM25.ToString();
                }
                if (txtIlluminanceMin.Text != "")
                {
                    if (Regex.IsMatch(txtIlluminanceMin.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MinIlluminance = Convert.ToDouble(txtIlluminanceMin.Text);
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                   
                }
                else
                {
                    txtIlluminanceMin.Text = _controlApp.MinIlluminance.ToString();
                }
                if (txtIlluminanceMax.Text != "")
                {
                    if (Regex.IsMatch(txtIlluminanceMax.Text, @"^[0-9]+$"))
                    {
                        _controlApp.MaxIlluminance = Convert.ToDouble(txtIlluminanceMax.Text);
                    }
                    else
                    {
                        MessageBox.Show("请输入数字");
                    }
                  
                }
                else
                {
                    txtIlluminanceMax.Text = _controlApp.MaxIlluminance.ToString();
                }
              
        }   

        #endregion


        
    }
}
