namespace Laipinche.Model
{
    public class Car
    {
        int                 id;             //id
        int                 us_id;          //用户id
        string              name;           //车名
        string              idcard;         //车牌号
        int                 type;           //车辆类型
        int                 capacity;       //最大乘客数
        string              time;           //注册时间数量

        /// <summary>
        /// id
        /// </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int Us_id { get => us_id; set => us_id = value; }
        /// <summary>
        /// 车名
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string IdCard { get => idcard; set => idcard = value; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public int Type_id { get => type; set => type = value; }
        /// <summary>
        /// 最大乘客数
        /// </summary>
        public int Capacity { get => capacity; set => capacity = value; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string Time { get => time; set => time = value; }
    }
}
