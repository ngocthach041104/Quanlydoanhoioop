using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace doanoopfinal
{
    public class DoanVien : Person
    {
        public string CanCuocCongDan { get; set; }
        public string Khoa { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }

        // Constructor
        public DoanVien()
        {
            // Khởi tạo các danh sách hoạt động
            HoatDongHocThuat = new List<string>();
            HoatDongVanNghe = new List<string>();
            HoatDongTheThao = new List<string>();
        }

        // Thêm các danh sách hoạt động
        public List<string> HoatDongHocThuat { get; set; }
        public List<string> HoatDongVanNghe { get; set; }
        public List<string> HoatDongTheThao { get; set; }

        // Thêm hoạt động với loại cụ thể
        public void ThemHoatDong(string loaiHoatDong, string hoatDong)
        {
            switch (loaiHoatDong)
            {
                case "Học thuật":
                    HoatDongHocThuat.Add(hoatDong);
                    break;
                case "Văn nghệ":
                    HoatDongVanNghe.Add(hoatDong);
                    break;
                case "Thể thao":
                    HoatDongTheThao.Add(hoatDong);
                    break;
                default:
                    Console.WriteLine("Loại hoạt động không hợp lệ.");
                    break;
            }
        }

        // Số lượng hoạt động đã tham gia
        public int SoLuongHoatDongDaThamGia
        {
            get
            {
                return HoatDongVanNghe.Count + HoatDongHocThuat.Count +
                       HoatDongTheThao.Count;
            }
        }

        // Override phương thức ThongTin để hiển thị thông tin hoạt động
        public override void ThongTin()
        {
            base.ThongTin();
            //Console.WriteLine($"Số căn cước: {CanCuocCongDan}, Khoa: {Khoa}");
            Console.WriteLine($"Các hoạt động học thuật: ");
            foreach (string hoatDong in HoatDongHocThuat)
            {
                Console.Write($"\t{hoatDong} \t");

            }
            Console.WriteLine();
            Console.WriteLine("Các hoạt động văn nghệ:");
            foreach (string hoatDong in HoatDongVanNghe)
            {
                Console.Write($"\t{hoatDong} \t");

            }
            Console.WriteLine();
            Console.WriteLine("Các hoạt động thể thao:");
            foreach (string hoatDong in HoatDongTheThao)
            {
                Console.Write($"\t {hoatDong} \t");

            }
            Console.WriteLine();
        }
    }
}
