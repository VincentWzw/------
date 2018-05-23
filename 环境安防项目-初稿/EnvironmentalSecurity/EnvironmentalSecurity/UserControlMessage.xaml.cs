using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace EnvironmentalSecurity
{
    /// <summary>
    /// UserControlMessage.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlMessage : UserControl
    {
        public UserControlMessage()
        {
            InitializeComponent();

            lvMessage.ItemsSource = MessageApp.GetList().OrderByDescending(p=>p.Time);
            lblNumber.Content = "数量：" + lvMessage.Items.Count;
        }

        /// <summary>
        /// 查询历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefer_OnClick(object sender, RoutedEventArgs e)
        {
            if (startDatePicker.Text != "" && endDatePicker.Text != "")
            {

                DateTime dateStart = Convert.ToDateTime(startDatePicker.Text);
                DateTime dateEnd = Convert.ToDateTime(endDatePicker.Text);

                if (dateStart<dateEnd)
                {
                    lvMessage.ItemsSource = MessageApp.GetMessageList(dateStart, dateEnd).OrderByDescending(p => p.Time);
                    lblNumber.Content = "数量：" + lvMessage.Items.Count;
                }
                else
                {
                    MessageBox.Show("请输入正确的时间段");
                }

            }
            else
            {
                MessageBox.Show("请选择日期！");
            }
        }

        /// <summary>
        /// 删除历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            Message message = GetListviewSelected();
            if (message != null)
            {
                if (MessageBox.Show("是否删除该项", "询问", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    OperationResult result = null;
                    result = MessageApp.Delete(message);                
                 

                    if (result.ResultType != OperationResultType.Success)
                    {
                        MessageBox.Show(result.Message);
                    }
                    else
                    {
                        if (startDatePicker.Text != "" && endDatePicker.Text != "")
                        {
                            DateTime dateStart = Convert.ToDateTime(startDatePicker.Text);
                            DateTime dateEnd = Convert.ToDateTime(endDatePicker.Text);
                            lvMessage.ItemsSource = MessageApp.GetMessageList(dateStart, dateEnd).OrderByDescending(p => p.Time);
                            lblNumber.Content = "数量：" + lvMessage.Items.Count;
                        }
                        else
                        {
                            lvMessage.ItemsSource = MessageApp.GetList().OrderByDescending(p => p.Time);
                            lblNumber.Content = "数量：" + lvMessage.Items.Count;
                        }

                      
                    }
                }
            }
        }

        #region 私有方法

        /// <summary>
        /// 获取Listview选中项
        /// </summary>
        /// <returns></returns>
        private Message GetListviewSelected()
        {
            return lvMessage.SelectedItem as Message;
        }

        #endregion


      
    }
}
