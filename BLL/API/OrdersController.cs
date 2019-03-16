using Laipinche.DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace Laipinche.BLL.API
{
    public class OrdersController : APIController
    {
        /// <summary>
        /// 获取热门拼车信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JObject GetHotPagesInfo(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                //验证请求时间
                string c_t = data["t"]?.ToString();
                if (!Vertify_time(c_t, Config.Config.Timeout))
                    return SendData(403);
                string type = data["type"]?.ToString();
                string count = data["count"]?.ToString();
                string page = data["page"]?.ToString();
                if (type == null || count == null || page == null)
                    return SendData(400);
                JObject data_json = OrderDAL.GetHotPagesInfo(type, int.Parse(count), int.Parse(page));
                JObject ret_json = new JObject();
                JArray data_jsa = new JArray();
                foreach (JObject item in data_json["data"])
                {
                    JObject json = new JObject();
                    json.Add("id", item["id"]?.ToString());
                    //json.Add("us_id", item["us_id"]?.ToString());
                    json.Add("from", item["from"]?.ToString());
                    //json.Add("way", item["way"]?.ToString());
                    json.Add("to", item["to"]?.ToString());
                    json.Add("starttime", item["starttime"]?.ToString());
                    //json.Add("isret", item["isret"]?.ToString());
                    //json.Add("rettime", item["rettime"]?.ToString());
                    //json.Add("remarks", item["remarks"]?.ToString());
                    json.Add("paytype", item["paytype"]?.ToString());
                    json.Add("price", item["price"]?.ToString());
                    json.Add("type", item["type"]?.ToString());
                    //json.Add("state", item["state"]?.ToString());
                    //json.Add("time", item["time"]?.ToString());

                    data_jsa.Add(json);
                }
                ret_json.Add("curpage", data_json["curpage"]);
                ret_json.Add("pagecount", data_json["pagecount"]);
                ret_json.Add("data", data_jsa);

                return SendData(200, data: data_json);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 根据拼车订单id查询信息
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public JObject GetOrderInfo(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                //验证请求时间
                string c_t = data["t"]?.ToString();
                if (!Vertify_time(c_t, Config.Config.Timeout))
                    return SendData(403);
                string or_id = data["id"]?.ToString();
                if (or_id == null)
                    return SendData(400);
                JObject or_json = OrderDAL.GetInfo(or_id);
                JObject car_json = CarDAL.GetInfo(or_json["us_id"]?.ToString());
                //JObject or_us_cnt = OrderDAL.GetOrderUserCount(or_id);
                JObject us_json = UserDAL.GetInfo(or_json["us_id"]?.ToString());
                JObject usIntegrity = UserDAL.GetUserIntegrity(or_json["us_id"]?.ToString());

                int applyed = OrderDAL.GetOrderApplyingCount(or_id, "0");
                int applying = OrderDAL.GetOrderApplyingCount(or_id, "4");

                JObject ret_json = new JObject();


                ret_json.Add("or_id", or_json["id"]?.ToString());
                ret_json.Add("from", or_json["from"]?.ToString());
                ret_json.Add("way", or_json["way"]?.ToString());
                ret_json.Add("starttime", or_json["starttime"]?.ToString());
                ret_json.Add("to", or_json["to"]?.ToString());
                ret_json.Add("remarks", or_json["remarks"]?.ToString());
                ret_json.Add("isret", or_json["isret"]?.ToString());
                ret_json.Add("rettime", or_json["rettime"]?.ToString());
                ret_json.Add("paytype", or_json["paytype"]?.ToString());
                ret_json.Add("price", or_json["price"]?.ToString());
                ret_json.Add("state", or_json["state"]?.ToString());
                ret_json.Add("type", or_json["type"]?.ToString());
                ret_json.Add("or_type", or_json["or_type"]?.ToString());
                ret_json.Add("time", or_json["time"]?.ToString());

                ret_json.Add("cartype", car_json["type"]?.ToString());
                ret_json.Add("capacity", car_json["capacity"]?.ToString());

                ret_json.Add("applyed", applyed);
                ret_json.Add("applying", applying);

                ret_json.Add("name", us_json["name"]?.ToString());
                ret_json.Add("tel", us_json["tel"]?.ToString());
                ret_json.Add("integrity", usIntegrity["integrity"]?.ToString());


                return SendData(200, data: ret_json);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 搜索信息
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        public JObject Search(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                //验证请求时间
                string c_t = data["t"]?.ToString();
                if (!Vertify_time(c_t, Config.Config.Timeout))
                    return SendData(403);
                string from = data["from"]?.ToString();
                string to = data["to"]?.ToString();
                string time = data["time"]?.ToString();
                string count = data["count"]?.ToString();
                string page = data["page"]?.ToString();
                string type = data["type"]?.ToString();
                if (from == null || to == null || count == null || page == null || type == null)
                    return SendData(400);
                JObject search_json = OrderDAL.Search(null, from, to, time, type, int.Parse(count), int.Parse(page));
                JArray list_data = (JArray)search_json["data"];
                JArray result_jsa = new JArray();
                foreach (JObject item in list_data)
                {
                    int applying = OrderDAL.GetOrderApplyingCount(item["id"].ToString(), "4");
                    int applyed = OrderDAL.GetOrderApplyingCount(item["id"].ToString(), "0");
                    JObject car_json = CarDAL.GetInfo(item["us_id"]?.ToString());

                    JObject json = new JObject();
                    json.Add("id", item["id"]);
                    json.Add("from", item["from"]?.ToString());
                    json.Add("way", item["way"]?.ToString());
                    json.Add("to", item["to"]?.ToString());
                    json.Add("starttime", item["starttime"]?.ToString());
                    json.Add("paytype", item["paytype"]?.ToString());
                    json.Add("price", item["price"]?.ToString());
                    json.Add("type", item["type"]?.ToString());
                    json.Add("or_type", item["or_type"]?.ToString());
                    json.Add("isret", bool.Parse(item["isret"]?.ToString()));
                    json.Add("rettime", item["rettime"]?.ToString());
                    json.Add("state", item["state"]?.ToString());
                    json.Add("time", item["time"]?.ToString());
                    json.Add("applying", applying);
                    json.Add("applyed", applyed);

                    json.Add("capacity", car_json["capacity"]);

                    result_jsa.Add(json);
                }


                JObject ret_json = new JObject();
                ret_json.Add("curpage", page);
                ret_json.Add("count", count);
                ret_json.Add("pagecount", search_json["pagecount"]);
                ret_json.Add("data", result_jsa);

                return SendData(200, "成功", ret_json);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 申请/邀请加入
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public JObject Applyfor(dynamic in_data)
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
                //解密us_id
                JObject ssid_json = ToolsDAL.DecryptPasswordKey(ssid);
                if (ssid_json == null)
                    return SendData(400);

                string us_id = ssid_json["id"].ToString();
                string or_id = data["or_id"]?.ToString();

                JObject us_json = UserDAL.GetInfo(us_id);
                JObject or_json = OrderDAL.GetInfo(or_id);
                //是否已经加入到拼车中
                if (OrderDetailDAL.IsExist(or_id, us_id))
                    return SendData(11002);
                //订单所有者与申请人相同
                if (us_id == or_json["us_id"].ToString())
                    return SendData(11001);

                string or_type = or_json["or_type"].ToString();
                string us_type = us_json["type"].ToString();
                //乘客订单
                if (or_type == "0")
                {
                    if (us_type == "1")
                    {
                        if (OrderDAL.Applyfor(or_id, us_id, 5))
                            return SendData(200);
                        else return SendData(20000);
                    }
                    else return SendData(11003);
                }
                else//司机订单
                {
                    if (OrderDAL.Applyfor(or_id, us_id, 4))
                        return SendData(200);
                    else return SendData(20000);
                }
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 上下班拼车列表
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public JObject GetWorkList(dynamic in_data)
        {
            try
            {
                JObject data = JObject.Parse(Tools.RSADecrypt(in_data));
                //验证请求时间
                string c_t = data["t"]?.ToString();
                if (!Vertify_time(c_t, Config.Config.Timeout))
                    return SendData(403);
                string from = data["from"]?.ToString();
                string to = data["to"]?.ToString();

                string count = data["count"]?.ToString();
                string page = data["page"]?.ToString();
                string type = data["type"]?.ToString();
                if (count == null || page == null || type == null)
                    return SendData(400);
                JObject search_json = OrderDAL.Search(null, from, to, null, type, int.Parse(count), int.Parse(page));
                JArray list_data = (JArray)search_json["data"];
                JArray result_jsa = new JArray();
                foreach (JObject item in list_data)
                {
                    int applying = OrderDAL.GetOrderApplyingCount(item["id"].ToString(), "4");
                    int applyed = OrderDAL.GetOrderApplyingCount(item["id"].ToString(), "0");

                    JObject car_json = CarDAL.GetInfo(item["us_id"]?.ToString());

                    JObject json = new JObject();
                    json.Add("id", item["id"]);
                    json.Add("from", item["from"]?.ToString());
                    json.Add("way", item["way"]?.ToString());
                    json.Add("to", item["to"]?.ToString());
                    json.Add("starttime", item["starttime"]?.ToString());
                    json.Add("paytype", item["paytype"]?.ToString());
                    json.Add("price", item["price"]?.ToString());
                    json.Add("type", item["type"]?.ToString());
                    json.Add("or_type", item["or_type"]?.ToString());
                    json.Add("isret", bool.Parse(item["isret"]?.ToString()));
                    json.Add("state", item["state"]?.ToString());
                    json.Add("time", item["time"]?.ToString());
                    json.Add("applying", applying);
                    json.Add("applyed", applyed);

                    json.Add("capacity", car_json["capacity"]);

                    result_jsa.Add(json);
                }

                JObject ret_json = new JObject();
                ret_json.Add("curpage", page);
                ret_json.Add("count", count);
                ret_json.Add("pagecount", search_json["pagecount"]);
                ret_json.Add("data", result_jsa);

                return SendData(200, "成功", ret_json);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public JObject GetStatistical(dynamic in_data)
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
                //解密us_id
                JObject ssid_json = ToolsDAL.DecryptPasswordKey(ssid);
                if (ssid_json == null)
                    return SendData(400);
                int carpooling = OrderDAL.GetCarpoolCount(ssid_json["id"].ToString(), data["type"]?.ToString(), "0");
                int carpoolcount = OrderDAL.GetCarpoolCount(ssid_json["id"].ToString(), data["type"]?.ToString(), "-1");
                int carpooled = OrderDAL.GetCarpoolCount(ssid_json["id"].ToString(), data["type"]?.ToString(), "2");

                float disburse = OrderDAL.GetDisburse(ssid_json["id"].ToString());
                float income = OrderDAL.GetIncome(ssid_json["id"].ToString());

                JObject ret_json = new JObject();
                ret_json.Add("carpooling", carpooling);
                ret_json.Add("carpoolcount", carpoolcount);
                ret_json.Add("carpooled", carpooled);
                ret_json.Add("disburse", disburse);
                ret_json.Add("income", income);



                return SendData(200, "成功", ret_json);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 获取用户发布的拼车订单
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public JObject GetCarpools(dynamic in_data)
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
                //解密us_id
                JObject ssid_json = ToolsDAL.DecryptPasswordKey(ssid);
                if (ssid_json == null)
                    return SendData(400);

                JArray s_json = (JArray)OrderDAL.Search(ssid_json["id"]?.ToString(), null, null, null, data["type"].ToString(), -1, -1)["data"];
                JArray or_datas = new JArray();
                foreach (JObject item in s_json)
                {
                    int applying = OrderDAL.GetOrderApplyingCount(item["id"]?.ToString(), "4");
                    JObject json = new JObject();
                    json.Add("id", item["id"]);
                    json.Add("from", item["from"]);
                    json.Add("to", item["to"]);
                    json.Add("paytype", item["paytype"]);
                    json.Add("price", item["price"]);
                    json.Add("state", item["state"]);
                    json.Add("applying", applying);

                    or_datas.Add(json);
                }

                return SendData(200, data: or_datas);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 关闭拼车订单
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public JObject CloseOrder(dynamic in_data)
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
                //解密us_id
                JObject ssid_json = ToolsDAL.DecryptPasswordKey(ssid);
                if (ssid_json == null)
                    return SendData(400);

                if (!OrderDAL.CloseOrder(data["id"].ToString(), ssid_json["id"].ToString()))
                    return SendData(20000);
                return SendData(200);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 取消申请
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public JObject CloseApplyOrder(dynamic in_data)
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
                //解密us_id
                JObject ssid_json = ToolsDAL.DecryptPasswordKey(ssid);
                if (ssid_json == null)
                    return SendData(400);

                if (!OrderDAL.CloseApplyOrder(data["id"].ToString(), ssid_json["id"].ToString()))
                    return SendData(20000);
                return SendData(200);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
        /// <summary>
        /// 获取正在申请的拼车订单
        /// </summary>
        /// <param name="in_data"></param>
        /// <returns></returns>
        [HttpPost]
        public JObject GetApplying(dynamic in_data)
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
                //解密us_id
                JObject ssid_json = ToolsDAL.DecryptPasswordKey(ssid);
                if (ssid_json == null)
                    return SendData(400);

                JArray jsa_data = OrderDAL.GetApplying(ssid_json["id"].ToString(), data["type"].ToString());
                JArray ret_jsa = new JArray();
                foreach (JObject item in jsa_data)
                {
                    JObject json = new JObject();
                    json.Add("id", item["id"]);
                    json.Add("from", item["from"]);
                    json.Add("to", item["to"]);
                    json.Add("price", item["odsprice"]);
                    json.Add("starttime", item["starttime"].ToString());
                    json.Add("paytype", item["paytype"]);
                    json.Add("state", item["state"]);
                    json.Add("odsid", item["odsid"]);

                    ret_jsa.Add(json);
                }

                return SendData(200, data: ret_jsa);
            }
            catch (Exception e)
            {
                return SendData(400);
            }
        }
    }
}