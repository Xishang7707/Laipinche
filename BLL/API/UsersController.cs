using Laipinche.DAL;
using Laipinche.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using System.Web.Http;
namespace Laipinche.BLL
{
    public class UsersController : APIController
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public string Register(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                User us = new User(data);
                //验证请求时间
                string c_t = data["t"]?.ToString();
                if (!Vertify_time(c_t, Config.Config.Timeout))
                    return SendData(403);

                //验证手机验证码是否正确
                string c_telcode = data["telcode"]?.ToString();
                string telcode_struct = Session["telcode"].ToString();
                if (c_telcode == null || telcode_struct == null)
                    return SendData(400);
                JObject telcode_json = JObject.Parse(telcode_struct);
                string telcode = telcode_json["code"]?.ToString();
                string tel = telcode_json["tel"]?.ToString();
                string telcode_t = telcode_json["t"]?.ToString();
                if (telcode_t == null || !Vertify_time(telcode_t, 1000 * 60 * 10))
                    return SendData(10005);
                if (tel == null || us.Tel != tel ||
                    telcode == null || c_telcode != telcode)
                    return SendData(10004);

                //验证必要信息是否存在
                if (
                us.UserName == null ||
                us.Pwd == null ||
                us.Tel == null ||
                us.Name == null ||
                us.IdCard == null)
                    return SendData(400);

                //判断用户名是否存在
                if (UserDAL.Exist("username", us.UserName))
                    return SendData(10001);

                // 验证密码
                string c_password = us.Pwd;
                string[] reg_pwd = {
                    @"^[\s]*$",   //包含空白字符
                    @"^.{6,18}$",//6-18个字符
                    @"[\da-zA-Z]+",@"^[^\u4e00-\u9fa5]+$"   //有数字和字母,不包含中文
                };

                //验证姓名和身份证
                string reg_name = @"^[\u4e00-\u9fa5]{2,4}$";
                if (!Regex.IsMatch(us.Name, reg_name))
                    return SendData(10008);
                string reg_idcard = @"[1-9]\d{5}((19)|(20))\d{2}(0[1-9]|1[0-2])((0[1-9])|[1-2]\d|30|31)\d{3}[\dXx]";
                if (!Regex.IsMatch(us.IdCard, reg_idcard))
                    return SendData(100009);

                if (
                    Regex.IsMatch(c_password, reg_pwd[0]) ||
                    !Regex.IsMatch(c_password, reg_pwd[1]) ||
                    !Regex.IsMatch(c_password, reg_pwd[2]) ||
                    !Regex.IsMatch(c_password, reg_pwd[3])
                    )
                    return SendData(10002);
                //加密密码
                us.Pwd = Tools.SHA256Encrypt(us.Pwd);
                bool result = UserDAL.Register(us);
                if (!result)
                    return SendData(20000);
                return SendData(200);
            }
            catch (Exception e)
            {
                return SendData(400, data: e.ToString());
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public string Login(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                User us = new User(data);
                //验证请求是否过期
                if (!Vertify_time(data["t"]?.ToString(), Config.Config.Timeout))
                    return SendData(403);
                //加密密码
                us.Pwd = Tools.SHA256Encrypt(us.Pwd);
                string login_result = UserDAL.Login(us);
                if (login_result == null)
                    return SendData(10010);
                Session["ssid"] = login_result;
                //HttpCookie sid_acs_cookie = new HttpCookie("ssid", login_result);
                //sid_acs_cookie.Expires = DateTime.Now.AddMonths(1);
                //Response.SetCookie(sid_acs_cookie);

                return SendData(200, data: login_result);
            }
            catch (Exception)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns>用户基本信息</returns>
        [HttpPost]
        public string GetInfo(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                //验证请求是否过期
                if (!Vertify_time(data["t"]?.ToString(), Config.Config.Timeout))
                    return SendData(403);
                string ssid = data["ssid"]?.ToString();
                if (!VerifyAuthorization(ssid))
                    return SendData(10011);
                JObject acs_json = ToolsDAL.DecryptPasswordKey(ssid);
                string us_id = acs_json["id"]?.ToString();
                if (us_id == null)
                    return SendData(10011);
                string ret_data = UserDAL.GetUserInfo(us_id);
                if (ret_data == null)
                    return SendData(10011);
                return SendData(StatusCode: 200, data: ret_data);
            }
            catch (Exception e)
            {
                return SendData(400, e.ToString());
            }
        }
    }
}