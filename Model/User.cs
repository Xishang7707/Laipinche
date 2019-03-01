using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Laipinche
{
    public class User
    {
        int     id;             //id
        string  userName;       //用户名
        string  pwd;            //密码
        string  salt;           //盐
        string  tel;            //联系电话
        string  name;           //姓名
        string  cardId;         //身份证号
        string  gender;         //性别
        string  attention;      //关注
        int     type;           //乘客or司机
        string  time;           //注册时间

        public int Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Pwd { get => pwd; set => pwd = value; }
        public string Salt { get => salt; set => salt = value; }
        public string Tel { get => tel; set => tel = value; }
        public string Name { get => name; set => name = value; }
        public string CardId { get => cardId; set => cardId = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Attention { get => attention; set => attention = value; }
        public int Type { get => type; set => type = value; }
        public string Time { get => time; set => time = value; }
    }
}
