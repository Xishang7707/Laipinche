using Laipinche.Model;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Laipinche.DAL
{
    public class UserDAL
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="us"></param>
        /// <returns></returns>
        public static bool Register(User us)
        {
            string cmd = @"insert [User](username, pwd, tel, [name], idcard) values(@username, @pwd, @tel, @name, @idcard)";
            return DBHelper.Exec(cmd, "@username", us.UserName, "@pwd", us.Pwd, "@tel", us.Tel, "@name", us.Name, "@idcard", us.IdCard) > 0;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="us">用户信息</param>
        /// <returns>
        /// 成功：登录字符串
        /// 失败：null
        /// </returns>
        public static string Login(User us)
        {
            string cmd = @"select * from [User] where username=@username and pwd=@pwd";
            DataTable dt = DBHelper.GetTable(cmd, "@username", us.UserName, "@pwd", us.Pwd);
            if (dt.Rows.Count <= 0)
                return null;
            string id = dt.Rows[0]["id"].ToString();

            return ToolsDAL.EncryptPasswordKey(id); ;
        }
        /// <summary>
        /// 验证字段值是否存在
        /// </summary>
        /// <param name="key">字段</param>
        /// <param name="val">值</param>
        /// <returns></returns>
        public static bool Exist(string key, string val)
        {
            string cmd = @"select TOP 1 * from [User] where " + key + "=@" + key;
            return DBHelper.GetTable(cmd, "@" + key, val).Rows.Count > 0;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户基本信息</returns>
        public static JObject GetInfo(string id)
        {
            string cmd = @"select * from [User] where id=@id";
            DataTable dt = DBHelper.GetTable(cmd, "@id", id);
            if (dt == null || dt.Rows.Count == 0)
                return null;

            JObject ret_json = new JObject();
            ret_json.Add("id", dt.Rows[0]["id"]?.ToString());
            ret_json.Add("username", dt.Rows[0]["username"]?.ToString());
            ret_json.Add("pwd", dt.Rows[0]["pwd"]?.ToString());
            ret_json.Add("tel", dt.Rows[0]["tel"]?.ToString());
            ret_json.Add("name", dt.Rows[0]["name"]?.ToString());
            ret_json.Add("idcard", dt.Rows[0]["idcard"]?.ToString());
            ret_json.Add("attention", dt.Rows[0]["attention"]?.ToString());
            ret_json.Add("type", dt.Rows[0]["type"]?.ToString());
            ret_json.Add("time", dt.Rows[0]["time"]?.ToString());

            return ret_json;
        }
        /// <summary>
        /// 获取用户诚信度
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public static JObject GetUserIntegrity(string id)
        {
            string cmd = @"select COUNT(*) as cnt, SUM(rating) as sum from [OrderDetail] where us_id=@us_id";
            DataTable dt = DBHelper.GetTable(cmd, "@us_id", id);
            if (dt.Rows.Count == 0)
                return null;
            DataRow dr = dt.Rows[0];
            int count = int.Parse(dr["cnt"].ToString());
            int sum = int.Parse(dr["sum"].ToString() == "" ? "0" : dr["sum"].ToString());

            JObject ret_json = new JObject();
            if (count == 0)
                ret_json.Add("integrity", "0");
            else
                ret_json.Add("integrity", ((float)sum / count) * 0.1);

            return ret_json;
        }
    }
}