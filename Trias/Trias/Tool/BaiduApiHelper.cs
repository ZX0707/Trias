using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Trias.Tool
{
    public class BaiduApiHelper
    {
        private const string ak = @"Qj2TZfMjyZZXynCMWYFUVlgoShqhA2pE";
        private const string sk = @"XG9kZZqkBDPxUFiYi9l1QTdGhcdTsG1j";

        public static object GetLocationByName(string name)
        {
            const string domain = "http://api.map.baidu.com";
            const string uri = "/geocoder/v2/";
            var dict = new Dictionary<string, string> { { "ak", ak }, { "address", name }, { "output", "json" } };
            var sn = AKSNCaculater.CaculateAKSN(ak, sk, uri, dict);
            dict.Add("sn", sn);
            var result = domain + uri + "?" + AKSNCaculater.HttpBuildQuery(dict);
            var stream = WebRequest.Create(result).GetResponse().GetResponseStream();
            var str = new StreamReader(stream).ReadToEnd();
            var json = JObject.Parse(str);
            if (json.Property("status").Value.Value<string>() == "0")
            {
                return new
                {
                    status = "success",
                    msg = json.Property("result").Value.Value<JObject>().Property("location").Value.Value<JObject>().ToString()
                };
            }
            return new
            {
                status = "error",
                msg = json.Property("msg").Value.Value<string>()
            };
        }
    }

    public class AKSNCaculater
    {
        public static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    ret += a.ToString("x2");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        public static string UrlEncode(string str)
        {
            str = System.Web.HttpUtility.UrlEncode(str);
            byte[] buf = Encoding.ASCII.GetBytes(str);//等同于Encoding.ASCII.GetBytes(str)
            for (int i = 0; i < buf.Length; i++)
                if (buf[i] == '%')
                {
                    if (buf[i + 1] >= 'a') buf[i + 1] -= 32;
                    if (buf[i + 2] >= 'a') buf[i + 2] -= 32;
                    i += 2;
                }
            return Encoding.ASCII.GetString(buf);//同上，等同于Encoding.ASCII.GetString(buf)
        }

        public static string HttpBuildQuery(IDictionary<string, string> querystring_arrays)
        {

            StringBuilder sb = new StringBuilder();
            foreach (var item in querystring_arrays)
            {
                sb.Append(UrlEncode(item.Key));
                sb.Append("=");
                sb.Append(UrlEncode(item.Value));
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static string CaculateAKSN(string ak, string sk, string url, IDictionary<string, string> querystring_arrays)
        {
            var queryString = HttpBuildQuery(querystring_arrays);

            var str = UrlEncode(url + "?" + queryString + sk);

            return MD5(str);
        }
    }
}