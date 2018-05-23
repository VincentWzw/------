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
using ES.Business;
using ES.Domain;
using ES.Utility;

namespace EnvironmentalSecurity
{
    /// <summary>
    /// WindowCoorInfo.xaml 的交互逻辑
    /// </summary>
    public partial class WindowCoorInfo : Window
    {
        private bool _isInsert = true;
        private Coordinator _coordinator;
        Message message=new Message();
        public WindowCoorInfo()
        {
            InitializeComponent();
        }
        public WindowCoorInfo(Coordinator coordinator,bool isInsert)
        {
            InitializeComponent();
            this.cboComCoor.ItemsSource = SerialPortOperation.GetPorts();

            _isInsert = isInsert;
            _coordinator = coordinator;
            tbName.Text = _coordinator.Name;
            cboComCoor.Text = _coordinator.COM;
            tbPanId.Text = _coordinator.PanID;
        }

        

       
             
        /// <summary>
        /// 确认新增/编辑协调器
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
                    result = CoordinatorApp.Insert(_coordinator);

                    message.Time = DateTime.Now;
                    message.Messages = "新增协调器:" + _coordinator.Name + "    串口号：" + _coordinator.COM + "    PanID:" +
                                       _coordinator.PanID;

                }
                else
                {
                    result = CoordinatorApp.Update(_coordinator);


                    message.Time = DateTime.Now;
                    message.Messages = "更改协调器:" + _coordinator.Name + "    串口号：" + _coordinator.COM + "    PanID:" +
                                       _coordinator.PanID;

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
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancle_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region 私有方法

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
            if (string.IsNullOrWhiteSpace(cboComCoor.Text))
            {
                MessageBox.Show("串口不能为空");
                cboComCoor.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(tbPanId.Text))
            {
                MessageBox.Show("PanId不能为空");
                tbPanId.Focus();
                return false;
            }
            if (!Regex.IsMatch(tbPanId.Text, @"^[A-Fa-f0-9]+$"))
            {
                MessageBox.Show("请输入正确PanId");
                tbPanId.Focus();
                return false;
            }

            this._coordinator.Name = tbName.Text.Trim();
            this._coordinator.COM = cboComCoor.Text.Trim();
            this._coordinator.PanID = tbPanId.Text.ToUpper();

            return true;
        }

        #endregion
    }
}
