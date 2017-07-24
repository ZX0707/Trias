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
        private const string domain = @"http://api.map.baidu.com";

        public static string GetLocationsByName(string city, string place)
        {
            const string uri = "/place/v2/search";

            var dict = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(place))
            {
                place = city;
            }
            dict.Add("query", place);//检索关键字，多个关键字之间用$分隔
            //dict.Add("tag","");//标签项，选项值http://lbsyun.baidu.com/index.php?title=lbscloud/poitags
            dict.Add("output", "json");//输出格式，json或者xml
            dict.Add("scope", "1");//结果的详细程度，1或者空返回基本信息，2返回详细信息
            //dict.Add("filter","");//scope为2的时候的有效http://lbsyun.baidu.com/index.php?title=webapi/guide/webservice-placeapi
            //dict.Add("oord_type", "3");//坐标类型，1（wgs84ll即GPS经纬度），2（gcj02ll即国测局经纬度坐标），3（bd09ll即百度经纬度坐标），4（bd09mc即百度米制坐标） 
            //dict.Add("ret_coordtype", "gcj02ll ");//添加后POI返回国测局经纬度坐标 
            dict.Add("page_size", "10");//查询多少条数据，分页用
            dict.Add("page_num", "0");//页码，从0开始数，分页用
            dict.Add("ak", ak);
            dict.Add("timestamp", DateTime.Now.Ticks.ToString());//设置sn后该值必填

            dict.Add("region", city);//城市，如果为全国或者省份，则返回指定区域的POI及数量（POI兴趣点）
            dict.Add("city_limit", "false");//是否只返回region内的POI

            dict.Add("sn", AKSNCaculater.CaculateAKSN(ak, sk, uri, dict));//计算sn的值

            var url = domain + uri + "?" + AKSNCaculater.HttpBuildQuery(dict);//拼接URL

            var stream = WebRequest.Create(url).GetResponse().GetResponseStream();//获取返回的流对象
            var str = new StreamReader(stream).ReadToEnd();//将流对象转换成字符串

            return str;
        }
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