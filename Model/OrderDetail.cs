using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laipinche.Model
{
    public class OrderDetail
    {
        int                 id;             //id
        int                 or_id;          //对应订单id
        int                 us_id;          //乘客id
        double              price;          //乘客价格
        Order.OrderState    state;       //订单状态id
        int                 rating;         //评价
        string              time;           //创建时间

        public int Id { get => id; set => id = value; }
        public int Or_id { get => or_id; set => or_id = value; }
        public int Us_id { get => us_id; set => us_id = value; }
        public double Price { get => price; set => price = value; }
        public Order.OrderState State { get => state; set => state = value; }
        public int Rating { get => rating; set => rating = value; }
        public string Time { get => time; set => time = value; }
    }
}
