using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace doanoopfinal
{

    public class User : Person
    {
        public string matKhau;
        public string TaiKhoan { get; private set; }
        public List<string> HoatDongVuaThamGia { get; private set; }

        // Constructor
        public User(string taiKhoan, string matKhau)
        {
            TaiKhoan = taiKhoan;
            this.matKhau = matKhau;
            // Khởi tạo danh sách hoạt động vừa tham gia
            HoatDongVuaThamGia = new List<string>();
        }

        // Override phương thức ThongTin để hiển thị thông tin tài khoản
        public override void ThongTin()
        {
            base.ThongTin();
            Console.WriteLine($"Tài khoản: {TaiKhoan}");
        }

        // Phương thức để thêm hoạt động
        public void ThemHoatDong(string hoatDong)
        {
            HoatDongVuaThamGia.Add(hoatDong);
        }
    }
}
