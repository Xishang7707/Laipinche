using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Laipinche.BLL.API
{
    public class CommunicateController : APIController
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        [HttpPost]
        public string SendTelCode(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                //验证请求时间
                string c_t = data["t"]?.ToString();
                if (!Vertify_time(c_t, Config.Config.Timeout))
                {
                    return SendData(403);
                }

                string tel = data["tel"]?.ToString();
                string reg_tel = @"1((3\d)|(4[5-9])|(5[0-35-9])|(66)|(7[0-8])|(8\d)|(9[8-9]))\d{8}";
                if (tel == null || !Regex.IsMatch(tel, reg_tel))
                {
                    return SendData(400);

                }

                //验证手机60秒间隔
                string telcode_struct = Session["telcode"]?.ToString();
                JObject telcode_json = null;
                if (telcode_struct != null)
                {
                    telcode_json = JObject.Parse(telcode_struct);

                    if (!Vertify_time(c_t, telcode_json["t"].ToString(), 60000))
                    {
                        return SendData(10006);

                    }
                }

                string telcode = new Random((int)GetNowTime()).Next(10000, 99999) + "";
                if (Tools.SendTelCode(tel, telcode))
                {
                    //向Session记录请求
                    var telcode_log = new
                    {
                        tel = tel,
                        code = telcode,
                        t = GetNowTime()
                    };
                    Session["telcode"] = JsonConvert.SerializeObject(telcode_log);
                    return SendData(200);
                }
                else
                    return SendData(10007);
            }
            catch (Exception)
            {
                return SendData(400);

            }
        }

    }
}