﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ES.Business
{
    /// <summary>
    /// 传感器枚举类型类
    /// </summary>
    public enum EnumSensorType : byte
    {
        继电器 = 0x20,
        温湿度传感器 = 0x30,
        烟雾传感器=0x40,
        PM25 = 0x44,   
        二氧化碳浓度 = 0x33,
        光照强度 = 0xC0,
        红外传感器  =0x50,
    }
}
