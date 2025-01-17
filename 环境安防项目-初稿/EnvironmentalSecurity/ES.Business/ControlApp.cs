﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ES.Business
{
    /// <summary>
    /// 智能控制类
    /// </summary>
    public class ControlApp
    {
        ///初始默认值
        /// <summary>
        /// 最大空气温度
        /// </summary>
        double maxAirTemp = 35;
        /// <summary>
        /// 最小空气温度
        /// </summary>
        double minAirTemp = 15;
        /// <summary>
        /// 最大空气湿度
        /// </summary>
        double maxAirHumi = 50;
        /// <summary>
        /// 最小空气湿度
        /// </summary>
        double minAirHumi = 20;
        /// <summary>
        /// 最大PM2.5
        /// </summary>
        double maxPM25 = 44;
        /// <summary>
        /// 最小PM2.5
        /// </summary>
        double minPM25 = 17;
        /// <summary>
        /// 最大二氧化碳浓度
        /// </summary>
        double maxC02Thickness = 700;
        /// <summary>
        /// 最小二氧化碳浓度
        /// </summary>
        double minC02Thickness = 365;
        /// <summary>
        /// 最大光照度
        /// </summary>
        double maxIlluminance = 2500;
        /// <summary>
        /// 最小光照度
        /// </summary>
        double minIlluminance = 200;
        


        /// <summary>
        /// 空气温度
        /// </summary>
        string airTemp;
        /// <summary>
        /// 空气湿度
        /// </summary>
        string airHumi;
        /// <summary>
        /// 光照度
        /// </summary>
        string illuminance;
        /// <summary>
        /// 二氧化碳浓度
        /// </summary>
        string c02Thickness;
        /// <summary>
        /// PM25
        /// </summary>
        string pM25;
        /// <summary>
        /// 烟雾
        /// </summary>
        string smoke;
        ///红外
        ///
        string redinfrared; 

        /// <summary>
        /// 最大空气温度
        /// </summary>
        public double MaxAirTemp
        {
            get { return maxAirTemp; }
            set
            {
                maxAirTemp = value;
            }
        }

        /// <summary>
        /// 最小空气温度
        /// </summary>
        public double MinAirTemp
        {
            get
            {
                return minAirTemp;
            }
            set
            {

                minAirTemp = value;


            }
        }

        /// <summary>
        /// 最大空气湿度
        /// </summary>
        public double MaxAirHumi
        {
            get { return maxAirHumi; }
            set
            {
                maxAirHumi = value;
            }
        }

        /// <summary>
        /// 最小空气湿度
        /// </summary>
        public double MinAirHumi
        {
            get { return minAirHumi; }
            set
            {

                minAirHumi = value;


            }
        }

        /// <summary>
        /// 最大PM2.5
        /// </summary>
        public double MaxPM25
        {
            get { return maxPM25; }
            set
            {
                maxPM25 = value;

            }
        }

        /// <summary>
        /// 最小PM2.5
        /// </summary>
        public double MinPM25
        {
            get { return minPM25; }
            set
            {


                minPM25 = value;


            }
        }

        /// <summary>
        /// 最大二氧化碳浓度
        /// </summary>
        public double MaxC02Thickness
        {
            get { return maxC02Thickness; }
            set
            {
                maxC02Thickness = value;

            }
        }

        /// <summary>
        /// 最小二氧化碳浓度
        /// </summary>
        public double MinC02Thickness
        {
            get { return minC02Thickness; }
            set
            {

                minC02Thickness = value;


            }
        }

        /// <summary>
        /// 最大光照度
        /// </summary>
        public double MaxIlluminance
        {
            get { return maxIlluminance; }
            set
            {
                maxIlluminance = value;
            }
        }

        /// <summary>
        /// 最小光照度
        /// </summary>
        public double MinIlluminance
        {
            get { return minIlluminance; }
            set
            {

                minIlluminance = value;


            }
        }




        /// <summary>
        /// 空气温度
        /// </summary>
        public string AirTemp
        {
            get { return airTemp; }
            set { airTemp = value; }
        }

        /// <summary>
        /// 空气湿度
        /// </summary>
        public string AirHumi
        {
            get { return airHumi; }
            set { airHumi = value; }
        }

        /// <summary>
        /// 光照度
        /// </summary>
        public string Illuminance
        {
            get { return illuminance; }
            set { illuminance = value; }
        }

        /// <summary>
        /// 二氧化碳浓度
        /// </summary>
        public string C02Thickness
        {
            get { return c02Thickness; }
            set { c02Thickness = value; }
        }

        /// <summary>
        /// PM25
        /// </summary>
        public string PM25
        {
            get { return pM25; }
            set { pM25 = value; }
        }

        /// <summary>
        /// 烟雾
        /// </summary>
        public string Smoke
        {
            get { return smoke; }
            set { smoke = value; }
        }

        ///红外
        ///
        public string radInfrared
        {
            get { return redinfrared; }
            set { redinfrared = value; }
        }
    }
}
