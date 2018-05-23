using System;
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
using System.Windows.Shapes;
using EnvironmentalSecurity.TypeConvertTool;
using ES.Business;
using ES.Domain;

using ES.Utility;

namespace EnvironmentalSecurity
{
    /// <summary>
    /// WindowSensorInfo.xaml 的交互逻辑
    /// </summary>
    public partial class WindowSensorInfo : Window
    {
        /// <summary>
        /// 是否新增
        /// </summary>
        private bool _isInsert = true;

        /// <summary>
        /// 操作信息
        /// </summary>
        Message message = new Message();

        /// <summary>
        /// 传感器
        /// </summary>
        private Sensor _sensor;
        public WindowSensorInfo()
        {
            InitializeComponent();
        }
        public WindowSensorInfo(Sensor sensor ,bool isInsert)
        {
            InitializeComponent();


            cboType.ItemsSource = SensorTypeApp.GetList();
            _isInsert = isInsert;
            _sensor = sensor;
            tbName.Text = _sensor.Name;
            GetType();   //获取传感器类型

            if (isInsert!=true)
            {
                tbAddress.Text = Converts.ByteToString((byte)_sensor.Address);
            }
            else
            {
                tbAddress.Text = _sensor.Address.ToString();
            }
            
        }


        /// <summary>
        /// 新增或编辑传感器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            if (GetControl())
            {
                OperationResult result;
                if (_isInsert)
                {
                    result = SensorApp.Insert(_sensor);

                    message.Time = DateTime.Now;
                    message.Messages = "新增传感器:" + _sensor.Name + "    类型：" + _sensor.Type + "    地址:" +
                                       _sensor.Address;

                }
                else
                {
                    result = SensorApp.Update(_sensor);


                    message.Time = DateTime.Now;
                    message.Messages = "更改传感器:" + _sensor.Name + "    类型：" + _sensor.Type + "    地址:" +
                                       _sensor.Address;

                }
                if (result.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
                else
                {
                    MessageApp.Insert(message);
                }
                DialogResult = true;
            }
        }

        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancle_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region 私有方法

        /// <summary>
        /// 获取传感器类型
        /// </summary>
        /// <returns></returns>
        private bool GetType()
        {
            if (cboType.Text != null)
            {
                switch (_sensor.Type)
                {
                    case 0x20:
                        this.cboType.Text = "继电器";
                        break;
                    case 0x30:
                        this.cboType.Text = "温湿度传感器";
                        break;
                    case 0x44:
                        this.cboType.Text = "PM25";
                        break;
                    case 0x40:
                        this.cboType.Text = "烟雾传感器";
                        break;
                    case 0x31:
                        this.cboType.Text = "空气温湿度";
                        break;
                    case 0x33:
                        this.cboType.Text = "二氧化碳浓度";
                        break;
                    case 0xC0:
                        this.cboType.Text = "光照强度";
                        break;
                    case 0x50:
                        this.cboType.Text = "红外热感";
                        break;
                }

            }
            return true;
        }

        /// <summary>
        /// 判断所填信息是否为空
        /// </summary>
        /// <returns></returns>
        public bool GetControl()
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("名称不能为空");
                tbName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cboType.Text))
            {
                MessageBox.Show("类型不能为空");
                tbName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(tbAddress.Text))
            {
                MessageBox.Show("地址不能为空");
                tbName.Focus();
                return false;
            }
            if (!Regex.IsMatch(tbAddress.Text, @"^[A-Fa-f0-9]+$"))
            {
                MessageBox.Show("请输入正确的地址");
                tbAddress.Focus();
                return false;
            }
            if (cboType.Text != null)
            {
                switch (cboType.Text)
                {
                    case "继电器":
                        this._sensor.Type = 0x20;
                        break;
                    case "温湿度传感器":
                        this._sensor.Type = 0x30;
                        break;
                    case "二氧化碳浓度":
                        this._sensor.Type = 0x33;
                        break;
                    case "光照强度":
                        this._sensor.Type = 0xC0;
                        break;
                    case "烟雾传感器":
                        this._sensor.Type = 0x40;
                        break;
                    case "PM25":
                        this._sensor.Type = 0x44;
                        break;
                    case "红外传感器":
                        this._sensor.Type = 0x50;
                        break;
                }
            }

            this._sensor.Name = tbName.Text.Trim();
            this._sensor.Address = Converts.StringToByte(tbAddress.Text.ToUpper());


            return true;
        }

        #endregion


       
    }
}
