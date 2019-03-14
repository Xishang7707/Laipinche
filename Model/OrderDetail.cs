namespace Laipinche.Model
{
    public class OrderDetail
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 对应主订单Id
        /// </summary>
        public int Or_id { get; set; }
        /// <summary>
        /// 乘客
        /// </summary>
        public int Us_id { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 订单状态
        /// 0.正在进行
        /// 1.已经开始
        /// 2.完成
        /// 3.关闭
        /// 4.申请中
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 评价
        /// </summary>
        public int Rating { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public string Time { get; set; }
    }
}
