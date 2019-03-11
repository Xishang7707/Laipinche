using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Laipinche.BLL
{
    public class Tools
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="tel">手机号码</param>
        /// <param name="tel_code">验证码</param>
        /// <returns></returns>
        public static bool SendTelCode(string tel, string tel_code)
        {
            try
            {
                Random rand = new Random((int)GetNowTime());
                string app_code = "eab9ae3d33ef8003";
                string tel_req_url = "http://api.jisuapi.com/sms/send";
                string querys = "mobile=" + tel + "&content=验证码：" + tel_code + "，如非本人操作，请忽略本短信【拼车网】&appkey=" + app_code;

                HttpWebRequest httpRequest = null;
                HttpWebResponse httpResponse = null;

                tel_req_url += "?" + querys;

                httpRequest = (HttpWebRequest)WebRequest.Create(tel_req_url);

                httpRequest.Method = "GET";
                //httpRequest.Headers.Add("Authorization", "APPCODE " + app_code);

                try
                {
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    httpResponse = (HttpWebResponse)ex.Response;
                }

                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));

                string ret_str = reader.ReadToEnd();
                JObject ret_json = JObject.Parse(ret_str);

                string code = ret_json["status"]?.ToString();

                if (code == null || code != "0")
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetNowTime()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="val">加密的数据</param>
        /// <returns></returns>
        public static string RSAEncrypt(string val)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(Config.Config.Pubkey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(val), false);
            return Convert.ToBase64String(cipherbytes);
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="val">解密的数据</param>
        /// <returns></returns>
        public static string RSADecrypt(dynamic in_val)
        {
            JObject val_json = JObject.Parse(in_val.ToString());
            string data = val_json["data"]?.ToString();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(Config.Config.Prvkey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(data), false);
            return Encoding.UTF8.GetString(cipherbytes);
        }
        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="val">加密的数据</param>
        /// <returns></returns>
        public static string SHA256Encrypt(string val)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] src = Encoding.Default.GetBytes(val);
            byte[] crypt = sha256.ComputeHash(src);
            return Convert.ToBase64String(crypt);
        }

    }
}