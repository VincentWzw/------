using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace EnvironmentalSecurity
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
            ContentControl.Content=new UserControlinterface();
            
        }

        /// <summary>
        /// 数据信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDataInfo_OnClick(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new UserControlSensorList();
           
        }

        /// <summary>
        /// 智能控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnControl_OnClick(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new UserControLintelligentControl();
        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHistroy_OnClick(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new UserControlHistoryMessage();
        }

        /// <summary>
        /// 主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMain_OnClick(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new UserControlinterface();
        }
    }
}
