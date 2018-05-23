using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KV_WSN
{
    public enum EnumType
    {
        未知设备,
        数码管 = 0x10,
        继电器 = 0x20,
        温湿度传感器 = 0x30,
        烟雾传感器 = 0x40,
        红外传感器 = 0x50,
        震动传感器 = 0x60,
        红外对射光栅 = 0x70,
        智能插座 = 0x80,
        ZigBee信号转发器 = 0x90,
        门磁报警器 = 0xA0,
        电动窗帘 = 0xB0,
        光照强度 = 0xC0,
        空气温湿度 = 0x31,
        土壤温湿度 = 0x32,
        二氧化碳浓度 = 0x33,
        光强 = 0x34,
        激光测距传感器 = 0x41,
        转速传感器 = 0x42,
        大气压力传感器 = 0x43,
        PM25 = 0x44,
        角度传感器 = 0x45,
        位移传感器 = 0x46,
        扭矩传感器 = 0x47,
        拉力压力传感器 = 0x48,
        超声波液位传感器 = 0x49,
        热偶传感器 = 0x4a

    }
}
