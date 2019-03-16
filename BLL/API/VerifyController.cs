using Laipinche.DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
namespace Laipinche.BLL
{
    public class VerifyController : APIController
    {
        /// <summary>
        /// 检查字段值是否存在
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns>
        /// 0.不存在
        /// 1.存在
        /// </returns>
        [HttpPost]
        public JObject Exists(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                string c_t = data["t"]?.ToString();
                if (!Vertify_time(c_t, Config.Config.Timeout))
                    return SendData(403);
                string type = data["type"]?.ToString();
                string key = null;
                string val = null;
                if (type == null)
                    return SendData(400);
                switch (type)
                {
                    case "0":
                        key = "username";
                        break;
                    case "1":
                        key = "idcard";
                        break;
                    case "2":
                        key = "tel";
                        break;
                }
                if (key == null)
                    return SendData(400);

                val = data[key]?.ToString();
                if (val == null)
                    return SendData(400);

                if (int.Parse(type) < 10)
                    if (UserDAL.Exist(key, val))
                        return SendData(1);
                return SendData(0);
            }
            catch (Exception)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 验证是否登录
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        public JObject Verifylog(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                //验证请求时间
                string c_t = data["t"]?.ToString();
                if (!Vertify_time(c_t, Config.Config.Timeout))
                    return SendData(403);
                string ssid = data["LPCSSID"]?.ToString();
                //是否登录
                if (!VerifyAuthorization(ssid))
                    return SendData(10011);
                return SendData(200);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
    }
}