using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace doanoopfinal
{
    internal class Admin : Person
    {
        public string matKhau { get; set; }
        public string TaiKhoan { get; private set; }

        public Admin(string taiKhoan, string matKhau)
        {
            TaiKhoan = taiKhoan;
            this.matKhau = matKhau;
        }

        // Phương thức này sẽ không được sử dụng bên ngoài lớp
        private bool KiemTraMatKhau(string matKhau)
        {
            // Thực hiện kiểm tra độ dài hoặc các yêu cầu phức tạp khác
            return matKhau.Length >= 6; // Ví dụ: kiểm tra độ dài tối thiểu
        }

        public override void ThongTin()
        {
            base.ThongTin();
            Console.WriteLine($"Tài khoản: {TaiKhoan}");
        }
        // Phương thức này sẽ cho phép admin thay đổi mật khẩu
        public void DoiMatKhau(string matKhauMoi)
        {
            if (KiemTraMatKhau(matKhauMoi))
            {
                matKhau = matKhauMoi;
                Console.WriteLine("Đổi mật khẩu thành công.");
            }
            else
            {
                Console.WriteLine("Mật khẩu mới không hợp lệ.");
            }
        }
    }
}
