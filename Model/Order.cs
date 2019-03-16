namespace Laipinche.Model
{
    public class Order
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 司机id
        /// </summary>
        public string Us_id { get; set; }
        /// <summary>
        /// 司机Id
        /// </summary>
        public string Dv_id { get; set; }
        /// <summary>
        /// 起点
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 途径
        /// </summary>
        public string Way { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 出发时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 是否返程
        /// 0.否
        /// 1.是
        /// </summary>
        public string IsRet { get; set; }
        /// <summary>
        /// 返程时间
        /// </summary>
        public string RetTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 支付方式
        /// 0.免费
        /// 1.面议
        /// 2.一口价
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 总价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 订单类型
        /// 0.长途
        /// 1.上下班
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 发布者类型
        /// 1.乘客
        /// 2.司机
        /// </summary>
        public string Or_type { get; set; }
        /// <summary>
        /// 订单状态
        /// 0.正在进行
        /// 1.已经开始
        /// 2.完成
        /// 3.关闭
        /// 4.申请中
        /// 5.被关闭
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string Time { get; set; }

        public OrderDetail OrderDetail { get; set; }
    }
}
