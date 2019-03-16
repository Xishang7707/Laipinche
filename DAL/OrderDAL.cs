using Newtonsoft.Json.Linq;
using System.Data;

namespace Laipinche.DAL
{
    public class OrderDAL
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="type">拼车类型</param>
        /// <param name="count">数据条数</param>
        /// <param name="cur">获取的页</param>
        /// <returns>json格式</returns>
        public static JObject GetHotPagesInfo(string type, int count, int page)
        {
            //string cmd = @"select Top(@count) * from [Order] where type like @type1 and not id in (select Top(@nocnt) id from [Order] where [type] like @type2)";
            //int nocnt = (page - 1) * count;
            //DataTable dt = DBHelper.GetTable(cmd, "@count", count, "@type1", o, "@nocnt", nocnt, "@type2", o);
            DataTable dt = Pagination(count, page, "type=" + type);
            JArray list = new JArray();
            foreach (DataRow item in dt.Rows)
            {
                JObject data_json = new JObject();
                data_json.Add("id", item["id"]?.ToString());
                data_json.Add("us_id", item["us_id"]?.ToString());
                data_json.Add("from", item["from"]?.ToString());
                data_json.Add("way", item["way"]?.ToString());
                data_json.Add("to", item["to"]?.ToString());
                data_json.Add("starttime", item["starttime"]?.ToString());
                data_json.Add("isret", item["isret"]?.ToString());
                //data_json.Add("rettime", item["rettime"]?.ToString());
                //data_json.Add("remarks", item["remarks"]?.ToString());
                data_json.Add("paytype", item["paytype"]?.ToString());
                data_json.Add("price", item["price"]?.ToString());
                data_json.Add("type", item["type"]?.ToString());
                data_json.Add("or_type", item["or_type"]?.ToString());
                //data_json.Add("state", item["state"]?.ToString());
                //data_json.Add("time", item["time"]?.ToString());

                JObject car = CarDAL.GetInfo(item["us_id"]?.ToString());
                if (car != null)
                    data_json.Add("cartype", car["type"]);
                else
                    data_json.Add("cartype", "无车");

                list.Add(data_json);
            }

            //string cmd2 = @"select Count(*) as cnt from [Order] where type like @type";

            //DataTable dt2 = DBHelper.GetTable(cmd2, "@type", o);
            //string pagesDataCount = dt2.Rows[0]["cnt"].ToString();
            int pagecount = GetPageCount("type=" + type, count);
            JObject ret_json = new JObject();
            ret_json.Add("curpage", page);
            ret_json.Add("pagecount", pagecount);
            ret_json.Add("data", list);

            return ret_json;
        }
        /// <summary>
        /// 获取拼车订单信息
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public static JObject GetInfo(string id)
        {
            string cmd = "select * from [Order] where id=@id";
            DataTable dt = DBHelper.GetTable(cmd, "@id", id);
            if (dt.Rows.Count == 0)
                return null;
            JObject ret_json = new JObject();
            DataRow dr = dt.Rows[0];
            JArray jsa = DtToJarray(dt);
            ret_json = (JObject)jsa[0];
            return ret_json;
        }
        /// <summary>
        /// 获取已加入拼车的人数
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public static JObject GetOrderUserCount(string id)
        {
            string cmd = @"select COUNT(*) as cnt from [OrderDetail] where or_id=@or_id";
            DataTable dt = DBHelper.GetTable(cmd, "@or_id", id);
            JObject ret_json = new JObject();
            ret_json.Add("orderusercount", dt.Rows[0]["cnt"].ToString());
            return ret_json;
        }
        /// <summary>
        /// 获取申请/邀请中的人数
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="state">
        /// 0.正在进行
        /// 4.申请中
        /// </param>
        /// <returns></returns>
        public static int GetOrderApplyingCount(string id, string state)
        {
            string cmd = @"select COUNT(*) as cnt from [OrderDetail] where or_id=@or_id and state=@state";
            DataTable dt = DBHelper.GetTable(cmd, "@or_id", id, "@state", state);
            return int.Parse(dt.Rows[0]["cnt"].ToString());
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="from">起点</param>
        /// <param name="to">终点</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        /// select * from [Order] where type like '0%' order by id offset 12 ROWS fetch next 12 ROWS only
        public static JObject Search(string us_id, string from, string to, string time, string type, int count, int page)
        {
            string cmd = "";
            int flag = 0;
            if (us_id != null && us_id != "")
            {
                cmd += " us_id=" + us_id;
                flag++;
            }
            if ((from != null && from != ""))
            {
                if (flag > 0)
                    cmd += " and [from] like '%" + from + "%'";
                else cmd += " [from] like '%" + from + "%'";
                flag++;
            }
            if ((to != null && to != ""))
            {
                if (flag > 0)
                    cmd += " and [to] like '%" + to + "%'";
                else cmd += " [to] like '%" + to + "%'";
                flag++;
            }
            if (time != null && time != "")
            {
                if (flag > 0)
                    cmd += " and datediff(day, '" + time + "', starttime)=0";
                else cmd += " datediff(day, '" + time + "', starttime)=0";
                flag++;
            }
            if (type != null && type != "")
            {
                if (flag > 0)
                    cmd += " and [type]=" + type;
                else cmd += " [type]=" + type;
                flag++;
            }

            DataTable dt = Pagination(count, page, cmd);
            JArray data_list = DtToJarray(dt);


            //获取总页数
            //string cmd2 = @"[from] like '%" + from + "%' and [to] like '%" + to + "%' and type like '" + o + "' and datediff(day, '" + time + "', starttime)=0";
            int pagecount = GetPageCount(cmd, count);

            JObject ret_json = new JObject();

            ret_json.Add("pagecount", pagecount);
            ret_json.Add("data", data_list);
            return ret_json;
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="pages">页数</param>
        /// <param name="cmds">条件</param>
        /// <returns></returns>
        public static DataTable Pagination(int count, int pages, string condition)
        {
            string cmd = null;
            if (count > 0)
            {
                cmd = @"select * from [Order] where " + condition + " order by id desc offset (@counts) ROWS fetch next (@count) ROWS only";
                return DBHelper.GetTable(cmd, "@counts", count * (pages - 1), "@count", count);
            }
            else
            {
                cmd = @"select * from [Order] where " + condition;
                return DBHelper.GetTable(cmd);
            }
        }
        /// <summary>
        /// 根据条件获取总页数
        /// </summary>
        /// <param name="comdition">条件</param>
        /// <returns></returns>
        public static int GetPageCount(string comdition, int count)
        {
            if (count <= 0)
                return 0;
            string cmd = @"select COUNT(*) as cnt from [Order] where " + comdition;
            int sum = int.Parse(DBHelper.GetTable(cmd).Rows[0]["cnt"].ToString());
            int flag = 0;
            if (sum % count > 0)
                flag++;
            int pagecount = sum / count + flag;
            return pagecount;
        }
        /// <summary>
        /// 申请/邀请用户加入
        /// </summary>
        /// <param name="or_id">订单id</param>
        /// <param name="us_id">用户id</param>
        /// <param name="state">
        /// 4.申请
        /// 5.邀请
        /// </param>
        /// <returns>true/false</returns>
        public static bool Applyfor(string or_id, string us_id, int state)
        {
            string cmd = @"insert [OrderDetail](or_id, us_id, state) values(@or_id, @us_id, @state)";
            int result = DBHelper.Exec(cmd, "@or_id", or_id, "@us_id", us_id, "@state", state);

            return result > 0;
        }

        /// <summary>
        /// 获取指定拼车信息
        /// </summary>
        /// <param name="us_id">用户id</param>
        /// <param name="type">类型</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public static JArray GetCarpools(string us_id, string type, string state)
        {
            string cmd = @"select * from [Order] where us_id=@us_id";
            if (type != "-1")
                cmd += " and type=" + type;
            if (state != "-1")
                cmd += " and state=" + state;
            cmd += " order by state";
            DataTable dt = DBHelper.GetTable(cmd);
            JArray ret_jsa = DtToJarray(dt);
            return ret_jsa;
        }
        /// <summary>
        /// 将DataTable转换成JArray
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static JArray DtToJarray(DataTable dt)
        {
            JArray jsa = new JArray();
            foreach (DataRow item in dt.Rows)
            {
                JObject json = new JObject();
                json.Add("id", item["id"]?.ToString());
                json.Add("us_id", item["us_id"]?.ToString());
                json.Add("dv_id", item["dv_id"]?.ToString());
                json.Add("from", item["from"]?.ToString());
                json.Add("way", item["way"]?.ToString());
                json.Add("to", item["to"]?.ToString());
                json.Add("starttime", item["starttime"]?.ToString());
                json.Add("isret", item["isret"]?.ToString());
                json.Add("rettime", item["rettime"]?.ToString());
                json.Add("remarks", item["remarks"]?.ToString());
                json.Add("paytype", item["paytype"]?.ToString());
                json.Add("price", item["price"]?.ToString());
                json.Add("type", item["type"]?.ToString());
                json.Add("or_type", item["or_type"]?.ToString());
                json.Add("state", item["state"]?.ToString());
                json.Add("time", item["time"]?.ToString());

                jsa.Add(json);
            }
            return jsa;
        }
        /// <summary>
        /// 获取进行中的拼车订单数量
        /// </summary>
        /// <param name="us_id">用户id</param>
        /// <returns></returns>
        public static int GetCarpoolingCount(string us_id, string type)
        {
            string cmd = @"select COUNT(*) as cnt from [Order] where type=@type state=0 and us_id=@us_id";
            int or_cnt = int.Parse(DBHelper.GetTable(cmd, "@type", type, "@us_id", us_id).Rows[0]["cnt"].ToString());
            string cmd2 = @"select COUNT(*) as cnt from [OrderDetail] where state=0 and us_id=@us_id";
            int ord_cnt = int.Parse(DBHelper.GetTable(cmd2, "@us_id", us_id).Rows[0]["cnt"].ToString());
            return or_cnt + ord_cnt;
        }
        /// <summary>
        /// 获取总支出
        /// </summary>
        /// <param name="us_id">用户id</param>
        /// <returns></returns>
        public static float GetDisburse(string us_id)
        {
            string cmd = @"select ISNULL(SUM(price),0) as sprice from [OrderDetail] where us_id=@us_id and state=2";
            float sumprice = float.Parse(DBHelper.GetTable(cmd, "@us_id", us_id).Rows[0]["sprice"].ToString());
            return sumprice;
        }
        /// <summary>
        /// 获取总收入
        /// </summary>
        /// <param name="us_id">用户id</param>
        /// <returns></returns>
        public static float GetIncome(string us_id)
        {
            string cmd = @"select ISNULL(SUM(price),0) as sprice from [Order] where dv_id=@us_id and state=2";
            float sumprice = float.Parse(DBHelper.GetTable(cmd, "@us_id", us_id).Rows[0]["sprice"].ToString());
            return sumprice;
        }

        /// <summary>
        /// 获取指定拼车数量
        /// </summary>
        /// <param name="us_id">用户id</param>
        /// <param name="type">类型</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public static int GetCarpoolCount(string us_id, string type, string state)
        {
            string cmd = @"select COUNT(*) as cnt from [Order] where us_id=@us_id";
            if (type != null && type != "-1")
                cmd += " and type=" + type;
            if (state != null && state != "-1")
                cmd += " and state=" + state;

            return int.Parse(DBHelper.GetTable(cmd, "@us_id", us_id).Rows[0]["cnt"].ToString());
        }
        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CloseOrder(string id, string us_id)
        {
            //关闭主订单
            string cmd = @"update [Order] set state=3 where id=@id and us_id=@us_id";
            int result = DBHelper.Exec(cmd, "@id", id, "@us_id", us_id);
            if (result <= 0)
                return false;
            //被关闭从订单
            string cmd2 = @"update [OrderDetail] set state=5 where state=0 and or_id=@or_id";
            result = DBHelper.Exec(cmd2, "@or_id", id);

            return true;
        }
        /// <summary>
        /// 取消订单申请
        /// </summary>
        /// <param name="id"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public static bool CloseApplyOrder(string id, string us_id)
        {
            //被关闭从订单
            string cmd2 = @"update [OrderDetail] set state=5 where state=4 and or_id=@or_id and us_id=@us_id";
            int result = DBHelper.Exec(cmd2, "@or_id", id, "@us_id", us_id);

            return result > 0;
        }
        /// <summary>
        /// 获取用户正在申请的拼车订单
        /// </summary>
        /// <param name="us_id">用户id</param>
        /// <param name="type">
        /// 0.长途
        /// 1.上下班
        /// </param>
        /// <returns></returns>
        public static JArray GetApplying(string us_id, string type)
        {
            string cmd = @"select 
                            ods.id as odsid, ods.us_id as odsusid,  ods.price as odsprice, * 
                            from [Order] od, [OrderDetail] ods 
                            where od.type=@type and od.id=ods.or_id and ods.state=4 and ods.us_id=@us_id";
            JArray jsa_data = JArray.FromObject(DBHelper.GetTable(cmd, "@type", type, "@us_id", us_id));//DtToJarray(DBHelper.GetTable(cmd, "@us_id", us_id));
            return jsa_data;
        }
    }
}
