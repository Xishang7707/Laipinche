using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Laipinche
{
    public class CarType
    {
        int     id;         //id
        string  name;       //类型：轿车/MPV/SUV/跑车/客车/其他

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
    }
}
