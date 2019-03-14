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
            ret_json.Add("id", dr["id"]?.ToString());
            ret_json.Add("us_id", dr["us_id"]?.ToString());
            ret_json.Add("from", dr["from"]?.ToString());
            ret_json.Add("way", dr["way"]?.ToString());
            ret_json.Add("to", dr["to"]?.ToString());
            ret_json.Add("starttime", dr["starttime"]?.ToString());
            ret_json.Add("isRet", dr["isRet"]?.ToString());
            ret_json.Add("rettime", dr["rettime"]?.ToString());
            ret_json.Add("remarks", dr["remarks"]?.ToString());
            ret_json.Add("paytype", dr["paytype"]?.ToString());
            ret_json.Add("price", dr["price"]?.ToString());
            ret_json.Add("type", dr["type"]?.ToString());
            ret_json.Add("or_type", dr["or_type"]?.ToString());
            ret_json.Add("state", dr["state"]?.ToString());
            ret_json.Add("time", dr["time"]?.ToString());

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
        public static JObject Search(string from, string to, string time, string type, int count, int page)
        {
            string cmd = @"[from] like '%" + from + "%' and [to] like '%" + to + "%' and type=" + type + " and datediff(day, '" + time + "', starttime)=0";
            DataTable dt = Pagination(count, page, cmd);
            JArray data_list = new JArray();

            foreach (DataRow item in dt.Rows)
            {
                JObject json = new JObject();
                json.Add("id", item["id"]?.ToString());
                json.Add("us_id", item["us_id"]?.ToString());
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

                data_list.Add(json);
            }
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
            string cmd = @"select * from [Order] where " + condition + " order by id offset (@counts) ROWS fetch next (@count) ROWS only";
            return DBHelper.GetTable(cmd, "@counts", count * (pages - 1), "@count", count);
        }
        /// <summary>
        /// 根据条件获取总页数
        /// </summary>
        /// <param name="comdition">条件</param>
        /// <returns></returns>
        public static int GetPageCount(string comdition, int count)
        {
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
    }
}
