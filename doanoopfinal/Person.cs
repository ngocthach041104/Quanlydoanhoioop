using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace doanoopfinal
{

    public abstract class Person
    {
        public string HoTen { get; set; }

        public virtual void ThongTin()
        {
            Console.WriteLine($"Họ tên: {HoTen}");
        }
    }
}
