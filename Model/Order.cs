namespace Laipinche.Model
{
    public class Order
    {
        public enum OrderState
        {

        };

        public enum PaymentMethod
        {
            /// <summary>
            /// 免费
            /// </summary>
            Free,
            /// <summary>
            /// 面议
            /// </summary>
            Negotiable,
            /// <summary>
            /// 一口价
            /// </summary>
            Price
        };

        string id;                 //id
        string us_id;              //司机id
        string from;               //起点
        string way;                //途径
        string to;                 //目的地
        string startTime;          //出发时间
        bool isRet;              //是否返程
        string retTime;            //返程时间
        string remarks;            //备注
        PaymentMethod payType;            //支付方式
        double price;              //总价格
        //double            rating;             //评价==>由详细订单统计而来
        OrderState state;              //订单状态
        string time;               //订单创建时间



        public string Id { get => id; set => id = value; }
        public string Us_id { get => us_id; set => us_id = value; }
        public string From { get => from; set => from = value; }
        public string Way { get => way; set => way = value; }
        public string To { get => to; set => to = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public bool IsRet { get => isRet; set => isRet = value; }
        public string RetTime { get => retTime; set => retTime = value; }
        public string Remarks { get => remarks; set => remarks = value; }
        public PaymentMethod PayType { get => payType; set => payType = value; }
        public double Price { get => price; set => price = value; }
        public OrderState State { get => state; set => state = value; }
        public string Time { get => time; set => time = value; }

        public OrderDetail OrderDetail { get; set; }
    }
}
