using Laipinche.Model;
using Newtonsoft.Json;
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
        public static string GetUserInfo(string id)
        {
            string cmd = @"select * from [User] where id=@id";
            DataTable dt = DBHelper.GetTable(cmd, "@id", id);
            if (dt == null || dt.Rows.Count == 0)
                return null;
            User us = new User(dt);
            var us_info = new
            {
                username = us.UserName,
                //tel = us.Tel,
                //name = us.Name,
                attention = us.Attention,
                type = us.Type
            };
            return JsonConvert.SerializeObject(us_info);
        }
    }
}