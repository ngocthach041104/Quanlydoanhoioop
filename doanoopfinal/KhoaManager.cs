using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace doanoopfinal
{
    internal class KhoaManager : IDataIO
    {
        private List<Khoa> khoas;

        public KhoaManager()
        {
            khoas = new List<Khoa>();
        }

        public void ThemKhoa(Khoa khoa)
        {
            khoas.Add(khoa);
        }

        public void LuuDanhSach(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Khoa>));

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, khoas);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi khi lưu danh sách khoa: {ex.Message}");
            }
        }

        public void DocDanhSach(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Khoa>));

                using (StreamReader reader = new StreamReader(filePath))
                {
                    khoas = (List<Khoa>)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi khi đọc danh sách khoa: {ex.Message}");
            }
        }
    }
}
    