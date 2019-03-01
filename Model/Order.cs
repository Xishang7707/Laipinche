namespace DAL.Laipinche
{
    public class Order
    {
        int     id;                 //id
        int     us_id;              //司机id
        string  from;               //起点
        string  way;                //途径
        string  to;                 //目的地
        string  startTime;          //出发时间
        int     isRet;              //是否返程
        string  retTime;            //返程时间
        string  remarks;            //备注
        int     payType;            //支付方式
        double  price;              //总价格
        //double  rating;             //评价==>由详细订单统计而来
        int     state_id;           //订单状态
        string  time;               //订单创建时间

        public int Id { get => id; set => id = value; }
        public int Us_id { get => us_id; set => us_id = value; }
        public string From { get => from; set => from = value; }
        public string Way { get => way; set => way = value; }
        public string To { get => to; set => to = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public int IsRet { get => isRet; set => isRet = value; }
        public string RetTime { get => retTime; set => retTime = value; }
        public string Remarks { get => remarks; set => remarks = value; }
        public int PayType { get => payType; set => payType = value; }
        public double Price { get => price; set => price = value; }
        public int State_id { get => state_id; set => state_id = value; }
        public string Time { get => time; set => time = value; }
    }
}
