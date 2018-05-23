using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EnvironmentalSecurity.TypeConvertTool
{
    public class Converts
    {
        #region 公共方法

        public static string StrToHex(string mStr) //返回处理后的十六进制字符串
        {
            return BitConverter.ToString(
                ASCIIEncoding.Default.GetBytes(mStr)).Replace("-", " ");
        } /* StrToHex */
        public static string HexToStr(string mHex) // 返回十六进制代表的字符串
        {
            mHex = mHex.Replace(" ", "");
            if (mHex.Length <= 0) return "";
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return ASCIIEncoding.Default.GetString(vBytes);
        } /* HexToStr */

        /// <summary>
        /// 字节数据转字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] bytes)
        {
            StringBuilder result = new StringBuilder();
            foreach (byte b in bytes)
            {
                result.AppendFormat("{0:X2}", b);
            }
            return result.ToString();
        }

        /// <summary>
        /// 单个字节转字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToString(byte bytes)
        {
            return String.Format("{0:X2}", bytes);
        }

        /// <summary>
        /// 字节数据转字符串(带格式)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToStringFormat(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes)
            {
                result = result + string.Format("{0:X2}", b) + "-";
            }
            return result.Substring(0, result.Length - 1);
        }

        /// <summary>
        /// 2位字符串转字节
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte StringToByte(string str)
        {
            try
            {
                str = System.Convert.ToInt32(str, 16).ToString();
            }
            catch (Exception err)
            {
                throw err;
            }

            byte result = 0;
            if (byte.TryParse(str, out result) == true)
            {
                return result;
            }
            else
            {
                throw new Exception("StringToByte error");
            }
        }

        /// <summary>
        /// 字符串转字节数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StringToBytes(string str)
        {
            byte[] result = new byte[str.Length / 2];
            for (int i = 0; i < str.Length; i = i + 2)
            {
                result[i / 2] = StringToByte(str.Substring(i, 2));
            }
            return result;
        }
        #endregion
    }
}
