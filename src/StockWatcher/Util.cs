﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPinyin;

namespace StockWatcher
{
    class Util
    {
        private static object lockObj = new object();
        public static void Alert(string message, string title = "提示")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void Info(string message, string title = "提示")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Error(string message, string title = "错误")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool Confirm(string message, string title = "询问")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK;
        }

        public static string Input(string message, string title = "请输入")
        { 
            return Microsoft.VisualBasic.Interaction.InputBox(message, title);
        }

        public static void Log(string description)
        {
            lock (lockObj)
            {
                var msg = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}] [INF] {description}\r\n";
                File.AppendAllText(StockConfig.LOG_PATH, msg);
            }
        }

        public static void Log(string description, Exception exception)
        {
            lock (lockObj)
            {
                var msg = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}] [ERR] {description}\r\n--------------------------------->\r\n{exception}\r\n<---------------------------------\r\n";
                File.AppendAllText(StockConfig.LOG_PATH, msg);
            }
        }

        /// <summary>
        /// 股票代码不同：沪市主板是60开头，B股是900开头;深市主板是000开头，中小板是002开头、创业板是300开头、B股是200开头。
        /// 
        /// 修正失败，返回 null
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ReturnFixStockCode(string code)
        {
            code = code.Trim();
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            var codeStringBuilder = new StringBuilder();
            foreach (var item in code)
            {
                if (item >= '0' && item <= '9')
                {
                    codeStringBuilder.Append(item);
                }
            }
            code = codeStringBuilder.ToString();
            if (code.Length != 6)
            {
                return null;
            }
            if (code == StockConfig.CODE_SH_STOCK_NUMBER)
            {
                code = StockConfig.CODE_SH_STOCK;
            }
            else if (code == StockConfig.CODE_SZ_STOCK_NUMBER)
            {
                code = StockConfig.CODE_SZ_STOCK;
            }
            else if (code.StartsWith("60") || code.StartsWith("900"))
            {
                code = $"sh{code}";
            }
            else if (code.StartsWith("000") || code.StartsWith("002") || code.StartsWith("300") || code.StartsWith("200"))
            {
                code = $"sz{code}";
            }
            return code;
        }
        
        /// <summary>
        /// 获取中文拼音的首字母
        /// </summary>
        /// <param name="str">中文字符串</param>
        /// <returns></returns>
        public static string GetPinyinInitials(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
    
            StringBuilder result = new StringBuilder();
            foreach (char c in str)
            {
                if (c >= 0x4E00 && c <= 0x9FA5) // 判断是否是汉字
                {
                    string pinyin = Pinyin.GetPinyin(c.ToString());
                    if (!string.IsNullOrEmpty(pinyin))
                    {
                        result.Append(pinyin[0]);
                    }
                }
                else
                {
                    result.Append(c); // 非汉字字符直接保留
                }
            }
            return result.ToString().ToUpper(); // 转换为大写
        }
    }
}
