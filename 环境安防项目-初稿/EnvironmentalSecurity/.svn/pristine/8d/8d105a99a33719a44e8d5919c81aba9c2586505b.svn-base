using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace KV_WSN.Sensor
{

    /// <summary>
    /// 传感器基类
    /// </summary>
    public class SensorBase
    {
        #region 字段

        private byte type;

        private byte addr;

        private byte[] data;

        #endregion

        #region 属性

        /// <summary>
        /// 传感器数据
        /// </summary>
        public string DataStr
        {
            get { return Converts.BytesToString(this.Data); }
        }

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public byte Type
        {
            get { return type; }
            set { type = value; }
        }

        public byte Addr
        {
            get { return addr; }
            set { addr = value; }
        }

        #endregion

        #region 构造函数
        public SensorBase()
        { }

        public SensorBase(byte type, byte addr, byte[] data)
        {
            this.Type = type;
            this.Addr = addr;
            this.Data = data;
        }

        #endregion

        #region 公有方法

        public virtual string GetData()
        {
            return "";
        }

        public  string GetType()
        {
            return ((EnumType)this.Type).ToString();
        }

        #endregion

    }
}
