using Newtonsoft.Json.Linq;
using System.Data;

namespace Laipinche.DAL
{
    public class OrderDetailDAL
    {
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public static JObject GetInfo(string id)
        {
            string cmd = @"select * from [OrderDetais] where id=@id";
            DataTable dt = DBHelper.GetTable(cmd, "@id", id);
            if (dt.Rows.Count == 0)
                return null;

            DataRow dr = dt.Rows[0];

            JObject ret_json = new JObject();
            ret_json.Add("id", dr["id"]?.ToString());
            ret_json.Add("or_id", dr["or_id"]?.ToString());
            ret_json.Add("us_id", dr["us_id"]?.ToString());
            ret_json.Add("price", dr["price"]?.ToString());
            ret_json.Add("state", dr["state"]?.ToString());
            ret_json.Add("rating", dr["rating"]?.ToString());
            ret_json.Add("time", dr["time"]?.ToString());

            return ret_json;
        }
        /// <summary>
        /// 判断用户是否已经加入拼车
        /// </summary>
        /// <param name="or_id">订单id</param>
        /// <param name="us_id">用户id</param>
        /// <returns></returns>
        public static bool IsExist(string or_id, string us_id)
        {
            string cmd = @"select id from [OrderDetail] where or_id=@or_id and us_id=@us_id";
            DataTable dt = DBHelper.GetTable(cmd, "@or_id", or_id, "@us_id", us_id);
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
    }
}
