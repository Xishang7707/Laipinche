using System.Data;

namespace Laipinche.Model
{
    public class Car
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string Us_id { get; set; }
        /// <summary>
        /// 车名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 车辆类型
        /// 1.轿车
        /// 2.MPV
        /// 3.SUV
        /// 4.跑车
        /// 5.客车
        /// 6.其他
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 最大乘客数
        /// </summary>
        public string Capacity { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string Time { get; set; }
    }
}
