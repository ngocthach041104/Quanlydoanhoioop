using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace doanoopfinal
{
    internal class DoanVienManager : IDataIO
    {
        private List<DoanVien> doanViens;

        public List<DoanVien> DoanViens
        {
            get { return doanViens; }
            set { doanViens = value; }
        }

        public DoanVienManager()
        {
            doanViens = new List<DoanVien>();
        }

        public void ThemDoanVien(DoanVien dv)
        {
            doanViens.Add(dv);
        }

        public void TaoTaiKhoanDoanVien(string taiKhoan, string matKhau)
        {
            // Tạo một đối tượng đoàn viên mới và thêm vào danh sách
            DoanVien newDoanVien = new DoanVien();
            newDoanVien.TaiKhoan = taiKhoan;
            newDoanVien.MatKhau = matKhau;
            doanViens.Add(newDoanVien);
        }

        public void LuuDanhSach(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<DoanVien>));

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, doanViens);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi khi lưu danh sách đoàn viên: {ex.Message}");
            }
        }

        public void DocDanhSach(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<DoanVien>));

                using (StreamReader reader = new StreamReader(filePath))
                {
                    doanViens = (List<DoanVien>)serializer.Deserialize(reader);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Không tìm thấy tệp {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi khi đọc danh sách đoàn viên: {ex.Message}");
            }
        }

        public List<DoanVien> TimKiem(string ten)
        {
            return doanViens.FindAll(dv => dv.HoTen.Contains(ten));
        }
    }
}
