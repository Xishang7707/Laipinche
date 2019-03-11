using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Laipinche.DAL
{
    public class ToolsDAL
    {
        /// <summary>
        /// 验证手机号码是否被注册
        /// </summary>
        /// <param name="tel">手机号码</param>
        /// <returns></returns>
        public static bool TelRegistered(string tel)
        {
            string cmd = @"select TOP 1 * from [User] where tel=@tel";
            return DBHelper.GetTable(cmd, "@tel", tel).Rows.Count > 0;
        }

        //加密密码访问字符串
        private const string PasswordPublickkey = "<RSAKeyValue><Modulus>yrGpzHts1Vj9/804w4VYMRDOqHS5+pqzO6h450BsqhRZ2gOBsrBBpxlgk+iE7pw27Qxm1dtak9cjW556BjOQrw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string PasswordPrivatekey = "<RSAKeyValue><Modulus>yrGpzHts1Vj9/804w4VYMRDOqHS5+pqzO6h450BsqhRZ2gOBsrBBpxlgk+iE7pw27Qxm1dtak9cjW556BjOQrw==</Modulus><Exponent>AQAB</Exponent><P>+T+gvh0Q7nr+Y02bEH7a7cCCEfXwclMTl6wy6mGnhfU=</P><Q>0C8176aeOf6zKziqkkv0fSdTQ0W0bNyWGzhqXzCO8ZM=</Q><DP>XU+TxvisuQs0p0qLbc5/+ZgjWcQAA1zUreiamyJ6C+0=</DP><DQ>UMPQB+46+kLenYj5W5JOAnPMMJANRCJ7tYm4cr9y5TM=</DQ><InverseQ>2XDw7WoXAokmFyHoJsK3LJD32T6ahpLtwrgdgZ2GU/4=</InverseQ><D>iRkyaITzUDspUmRVCKqgxE9B+N87DC6nHKsPXBtPZJm55UQk9Xwgs32NrnuFygEz3jMpTM2dGF1DvLqpM0r2QQ==</D></RSAKeyValue>";
        /// <summary>
        /// 加密客户端的访问字符串
        /// </summary>
        /// <param name="val">id</param>
        /// <returns>加密字符串</returns>
        public static string EncryptPasswordKey(string val)
        {
            var password_struct = new
            {
                id = val,
                t = GetNowTime(),
            };
            string pwd_str = JsonConvert.SerializeObject(password_struct);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(PasswordPublickkey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(pwd_str), false);
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// 解密客户端访问字符串
        /// </summary>
        /// <param name="val">访问字符串</param>
        /// <returns>JObject</returns>
        public static JObject DecryptPasswordKey(string val)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(PasswordPrivatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(val), false);
            JObject val_json = JObject.Parse(Encoding.UTF8.GetString(cipherbytes));
            return val_json;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetNowTime()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
    }
}
