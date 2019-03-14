using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace Laipinche.BLL
{
    public abstract class APIController : ApiController
    {
        public APIController()
        {
            Context = HttpContext.Current;
            Session = HttpContext.Current.Session;
            Response = HttpContext.Current.Response;
        }
        /// <summary>
        /// 当前Http实例
        /// </summary>
        private HttpContext context = HttpContext.Current;
        /// <summary>
        /// Http Session
        /// </summary>
        private HttpSessionState session = HttpContext.Current.Session;
        private HttpResponse response = HttpContext.Current.Response;

        protected HttpContext Context { get => context; set => context = value; }
        protected HttpSessionState Session { get => session; set => session = value; }
        protected HttpResponse Response { get => response; set => response = value; }
        /// <summary>
        /// 返回值列表
        /// </summary>
        protected static Dictionary<int, string> ReturnCodeState = new Dictionary<int, string>
        {
            {0, "否"                                 },
            {1, "是"                                 },
            {200, "成功"                             },
            {400, "请求错误"                         },
            {403, "请求过期"                         },
            {408, "请求超时"                         },

            {10001, "用户名已被注册"                 },
            {10002, "密码格式错误"                   },
            {10004, "手机验证码错误"                 },
            {10005, "手机验证码过期"                 },
            {10006, "请60秒后再次发送短信"           },
            {10007, "短信发送失败"                   },
            {10008, "姓名格式错误"                   },
            {10009, "身份证格式错误"                 },
            {10010, "登录失败"                       },
            {10011, "未授权访问"                     },

            {11001, "不能将自己加入自己的拼车中"     },
            {11002, "已经加入到拼车中"               },
            {11003, "不是司机，不能邀请"             },

            {20000, "服务错误"                       }
        };

        /// <summary>
        /// 包装向客户端发送的json数据
        /// </summary>
        /// <param name="statusCode">状态码</param>
        /// <param name="status">状态信息</param>
        /// <param name="data">数据</param>
        [NonAction]
        //protected string SendData(int StatusCode = 200, string Status = "成功", string data = null)
        //{
        //    string status = ReturnCodeState[StatusCode] == null ? Status : ReturnCodeState[StatusCode];

        //    var ret_data = new
        //    {
        //        code = StatusCode,
        //        status = status,
        //        data = data
        //    };
        //    return JsonConvert.SerializeObject(ret_data);
        //}
        protected JObject SendData(int StatusCode = 200, string Status = "成功", JObject data = null)
        {
            string status = ReturnCodeState[StatusCode] == null ? Status : ReturnCodeState[StatusCode];

            JObject json = new JObject();
            json.Add("code", StatusCode);
            json.Add("status", status);
            json.Add("data", data);
            return json;
        }

        /// <summary>
        /// 验证时间戳
        /// </summary>
        /// <param name="in_t">时间戳</param>
        /// <param name="interval">有效时长</param>
        /// <returns></returns>
        [NonAction]
        protected bool Vertify_time(string in_t, long interval)
        {
            try
            {
                long t = long.Parse(in_t);
                long n_t = GetNowTime();
                if (n_t < t)
                    return false;
                else if (GetNowTime() - t > interval)
                    return false;
                else return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 比较时间戳
        /// </summary>
        /// <param name="in_t1">时间戳</param>
        /// <param name="in_t2">参照时间戳</param>
        /// <param name="interval">间隔</param>
        /// <returns></returns>
        protected bool Vertify_time(string in_t1, string in_t2, long interval)
        {
            try
            {
                long t1 = long.Parse(in_t1);
                long t2 = long.Parse(in_t2);

                if (t1 - t2 < interval)
                    return false;
                else return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected long GetNowTime()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
        /// <summary>
        /// 验证授权id是否正确
        /// </summary>
        /// <param name="in_acsid">授权id</param>
        /// <returns></returns>
        public bool VerifyAuthorization(string in_acsid)
        {
            string acsid = Session["LPCSSID"]?.ToString();
            if (acsid == null)
                return false;
            if (acsid == in_acsid)
                return true;
            return false;
        }
    }
}