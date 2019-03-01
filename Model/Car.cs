namespace DAL.Laipinche
{
    public class Car
    {
        int     id;             //id
        int     us_id;          //用户id
        string  name;           //车名
        string  cardId;         //车牌号
        int     type_id;        //车辆类型
        int     capacity;       //最大乘客数
        string  time;           //注册时间数量

        public int Id { get => id; set => id = value; }
        public int Us_id { get => us_id; set => us_id = value; }
        public string Name { get => name; set => name = value; }
        public string CardId { get => cardId; set => cardId = value; }
        public int Type_id { get => type_id; set => type_id = value; }
        public int Capacity { get => capacity; set => capacity = value; }
        public string Time { get => time; set => time = value; }
    }
}
