using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Trias.Unit
{
    public class MD5Helper
    {
        /// <summary>
        /// 获取字符串的MD5
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <returns></returns>
        public static string GetMd5(string str)
        {
            var md5 = new MD5CryptoServiceProvider();
            var array = Encoding.UTF8.GetBytes(str);
            array = md5.ComputeHash(array);
            var resullt = new StringBuilder();
            foreach (var bate in array)
            {
                resullt.Append(bate.ToString("x2").ToLower());
            }
            return resullt.ToString();
        }

        /// <summary>
        /// 获取文件的MD5
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static string GetStreamMd5(Stream stream)
        {
            var strResult = "";
            var strHashData = "";
            var md5 = new MD5CryptoServiceProvider();
            var arrbytHashValue = md5.ComputeHash(stream); //计算指定Stream 对象的哈希值
            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            strHashData = BitConverter.ToString(arrbytHashValue);
            //替换-
            strHashData = strHashData.Replace("-", "");
            strResult = strHashData;
            return strResult;
        }
    }
}