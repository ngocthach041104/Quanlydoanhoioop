    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    namespace doanoopfinal
    {

        internal class HoatDongManager : IDataIO
        {
            private List<HoatDong> hoatDongs;
            public HoatDongManager()
            {
                hoatDongs = new List<HoatDong>();
            }

            public void ThemHoatDong(HoatDong hoatDong)
            {
                hoatDongs.Add(hoatDong);
            }

            public void LuuDanhSach(string filePath)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<HoatDong>));

                    // Kiểm tra xem tệp đã tồn tại chưa
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"Không tìm thấy tệp {filePath}");
                        return;
                    }

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        serializer.Serialize(writer, hoatDongs);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Đã xảy ra lỗi khi lưu danh sách hoạt động: {ex.Message}");
                }
            }

            public void DocDanhSach(string filePath)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<HoatDong>));

                    // Kiểm tra xem tệp đã tồn tại chưa
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"Không tìm thấy tệp {filePath}");
                        return;
                    }

                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        hoatDongs = (List<HoatDong>)serializer.Deserialize(reader);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Đã xảy ra lỗi khi đọc danh sách hoạt động: {ex.Message}");
                }
            }
        }
    }
