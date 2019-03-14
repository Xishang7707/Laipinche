using Newtonsoft.Json.Linq;
using System.Data;

namespace Laipinche.DAL
{
    public class CarDAL
    {
        /// <summary>
        /// 获取用户车辆信息
        /// </summary>
        /// <param name="us_id">用户id</param>
        /// <returns></returns>
        public static JObject GetInfo(string us_id)
        {
            string cmd = @"select * from [Car] where us_id=@us_id";
            DataTable dt = DBHelper.GetTable(cmd, "@us_id", us_id);
            if (dt.Rows.Count == 0)
                return null;
            DataRow dr = dt.Rows[0];
            JObject ret_json = new JObject();
            ret_json.Add("id", dr["id"]?.ToString()); ;
            ret_json.Add("us_id", dr["us_id"]?.ToString());
            ret_json.Add("name", dr["name"]?.ToString());
            ret_json.Add("idcard", dr["idcard"]?.ToString());
            ret_json.Add("type", dr["type"]?.ToString());
            ret_json.Add("capacity", dr["capacity"]?.ToString());
            ret_json.Add("time", dr["time"]?.ToString());
            return ret_json;
        }
    }
}
