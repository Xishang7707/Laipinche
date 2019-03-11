using Newtonsoft.Json.Linq;
using System.Data;

namespace Laipinche.Model
{
    public class User
    {

        public User(JObject json)
        {
            this.Id = json["id"]?.ToString();
            this.UserName = json["username"]?.ToString();
            this.pwd = json["password"]?.ToString();
            this.Tel = json["tel"]?.ToString();
            this.Name = json["name"]?.ToString();
            this.IdCard = json["idcard"]?.ToString();
            this.attention = json["attention"]?.ToString();
            this.Type = json["type"]?.ToString();
            this.time = json["time"]?.ToString();
        }
        public User(DataTable dt)
        {
            Id = dt.Rows[0]["id"]?.ToString();
            UserName = dt.Rows[0]["username"]?.ToString();
            Pwd = dt.Rows[0]["pwd"]?.ToString();
            Tel = dt.Rows[0]["tel"]?.ToString();
            Name = dt.Rows[0]["name"]?.ToString();
            IdCard = dt.Rows[0]["idcard"]?.ToString();
            Attention = dt.Rows[0]["attention"]?.ToString();
            Type = dt.Rows[0]["type"]?.ToString();
            time = dt.Rows[0]["time"]?.ToString();
        }

        string id;             //id
        string userName;       //用户名
        string pwd;            //密码
        string tel;            //联系电话
        string name;           //姓名
        string idcard;         //身份证号
        //string              gender;         //性别
        string attention;      //关注
        string type;           //乘客or司机
        string time;           //注册时间

        public enum UserType
        {
            /// <summary>
            /// 乘客
            /// </summary>
            Customer = 0,
            /// <summary>
            /// 司机
            /// </summary>
            Driver = 1
        };

        /// <summary>
        /// id
        /// </summary>
        public string Id { get => id; set => id = value; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get => userName; set => userName = value; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get => pwd; set => pwd = value; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Tel { get => tel; set => tel = value; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCard { get => idcard; set => idcard = value; }
        /// <summary>
        /// 关注的用户id
        /// 分割','
        /// </summary>
        public string Attention { get => attention; set => attention = value; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public string Type { get => type; set => type = value; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string Time { get => time; set => time = value; }

        public Car Car { get; set; }
        public Order Order { get; set; }
    }
}
