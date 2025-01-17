﻿using System;
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
    /// UserControlRefer.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlRefer : UserControl
    {
        public UserControlRefer()
        {
            InitializeComponent();
            
            InitTrees(); //设置TreeView
          
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
                    if (GetTreeViewSelected() != null)
                    {
                        if (GetTreeViewSelected() is Sensor)
                        {
                            Sensor sensor = GetTreeViewSelectedSensor();


                            lstRefer.ItemsSource = ShowRefer.ShowSensorDataTimeList(sensor, dateStart, dateEnd);

                            lblNumber.Content = "数量：" + lstRefer.Items.Count;
                        }
                        else
                        {
                            Coordinator coordinator = GetTreeViewSelectedCoor();

                            lstRefer.ItemsSource = ShowRefer.ShowCoorTimeList(coordinator, dateStart, dateEnd);

                            lblNumber.Content = "数量：" + lstRefer.Items.Count;
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择要查询的信息！");
                    }
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
            if (lstRefer.SelectedItems != null)
            {
                if (GetListviewSelected() != null)
                {
                    if (MessageBox.Show("是否删除该项", "询问", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        OperationResult result = null;
                        ShowRefer showRefer = GetListviewSelected();
                        
                       
                        if (showRefer != null)
                        {
                            if (startDatePicker.Text != "" && endDatePicker.Text != "")
                            {
                                DateTime dateStart = Convert.ToDateTime(startDatePicker.Text);
                                DateTime dateEnd = Convert.ToDateTime(endDatePicker.Text);
                                SensorData sensorData = SensorDataApp.GetList().Where(p => p.Time == showRefer.Time).First();
                                result = SensorDataApp.Delete(sensorData);
                                if (result.ResultType != OperationResultType.Success)
                                {
                                    MessageBox.Show(result.Message);
                                }
                                else
                                {
                                    if (GetTreeViewSelected() is Coordinator)
                                    {
                                        Coordinator coordinator = GetTreeViewSelectedCoor();

                                        lstRefer.ItemsSource = ShowRefer.ShowCoorTimeList(coordinator, dateStart, dateEnd);
                                        lblNumber.Content = "数量：" + lstRefer.Items.Count;
                                    }
                                    else
                                    {
                                        Sensor sensor = GetTreeViewSelectedSensor();
                                        lstRefer.ItemsSource = ShowRefer.ShowSensorDataTimeList(sensor, dateStart, dateEnd);
                                        lblNumber.Content = "数量：" + lstRefer.Items.Count;
                                    }


                                }
                            }
                            else
                            {
                                SensorData sensorData = SensorDataApp.GetList().Where(p => p.Time == showRefer.Time).First();
                                result = SensorDataApp.Delete(sensorData);
                                if (result.ResultType != OperationResultType.Success)
                                {
                                    MessageBox.Show(result.Message);
                                }
                                else
                                {
                                    if (GetTreeViewSelected() is Coordinator)
                                    {
                                        Coordinator coordinator = GetTreeViewSelectedCoor();

                                        lstRefer.ItemsSource = ShowRefer.ShowReferCoorList(coordinator);
                                        lblNumber.Content = "数量：" + lstRefer.Items.Count;
                                    }
                                    else
                                    {
                                        Sensor sensor = GetTreeViewSelectedSensor();
                                        lstRefer.ItemsSource = ShowRefer.ShowReferSensorList(sensor);
                                        lblNumber.Content = "数量：" + lstRefer.Items.Count;
                                    }


                                }
                            }
                           
                           
                        }
                        else
                        {
                            MessageBox.Show("请选择删除的传感器数据信息！");
                        }


                    }
                }
                else
                {
                    MessageBox.Show("请选择删除的传感器数据信息！");
                }
            }
        }

        #region 私有方法

        /// <summary>
        /// 获取Listview选中项
        /// </summary>
        /// <returns></returns>
        private ShowRefer GetListviewSelected()
        {
            return lstRefer.SelectedItem as ShowRefer;
        }

        /// <summary>
        /// TreeView显示内容
        /// </summary>
        private void InitTrees()
        {
            tvCoor.Items.Clear();
            List<ES.Domain.Coordinator> coor = CoordinatorApp.GetList();
            foreach (ES.Domain.Coordinator gtParent in coor)
            {
                TreeViewItem tvCoorParent = new TreeViewItem()
                {
                    Header = gtParent.Name,
                    Tag = gtParent
                };
                tvCoor.Items.Add(tvCoorParent); //设置父节点
                List<Sensor> childType = SensorApp.GetCoorList(gtParent);
                foreach (Sensor gtChild in childType)
                {
                    TreeViewItem tvCoorChild = new TreeViewItem()
                    {
                        Header = gtChild.Name,
                        Tag = gtChild
                    };
                    tvCoorParent.Items.Add(tvCoorChild);  //设置子节点
                }
            }
        }

        /// <summary>
        /// 获取TreeView选择的协调器
        /// </summary>
        /// <returns></returns>
        private ES.Domain.Coordinator GetTreeViewSelectedCoor()
        {
            return ((TreeViewItem)tvCoor.SelectedItem).Tag as ES.Domain.Coordinator;
        }

        /// <summary>
        /// 获取TreeView选择的传感器
        /// </summary>
        /// <returns></returns>
        private Sensor GetTreeViewSelectedSensor()
        {
            return ((TreeViewItem)tvCoor.SelectedItem).Tag as ES.Domain.Sensor;
        }

        /// <summary>
        /// 获取TreeView选择的对象
        /// </summary>
        /// <returns>TreeView选择的对象</returns>
        private object GetTreeViewSelected()
        {

            return ((TreeViewItem)tvCoor.SelectedItem).Tag;

        }

        /// <summary>
        /// TreeView改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvCoor_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvCoor.SelectedItem != null)
            {

                if (GetTreeViewSelected() is Sensor)
                {

                    Sensor sensor = GetTreeViewSelectedSensor();
                    lstRefer.ItemsSource = ShowRefer.ShowReferSensorList(sensor);
                    lblNumber.Content = "数量：" + lstRefer.Items.Count;


                }
                if (GetTreeViewSelected() is Coordinator)
                {
                    Coordinator coordinator = GetTreeViewSelectedCoor();

                    lstRefer.ItemsSource = ShowRefer.ShowReferCoorList(coordinator);
                    lblNumber.Content = "数量：" + lstRefer.Items.Count;
                }
            }
        }

        #endregion


      
    }
}
