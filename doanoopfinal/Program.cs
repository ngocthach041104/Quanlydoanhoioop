using doanoopfinal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml.Serialization;

class Program
{
    //static DoanVienManager doanVienManager = new DoanVienManager();
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        DashBoard();
    }
    static void DashBoard()
    {
        List<DoanVien> danhSachDoanVien = DocDanhSachDoanVien("doanvien.xml");
        List<User> users = new List<User>
                    {
                        new User("user1", "123"),
                        new User("user2", "456")
                    };
        DocDanhSachTuDoanVien(users, "doanvien.xml");
        List<Admin> admins = new List<Admin>
                    {
                        new Admin("admin", "admin123")
                    };

        int opt;
        do
        {
            Console.Clear();
            Console.WriteLine("----- ĐĂNG NHẬP -----");
            Console.WriteLine("1.Admin\n2.User");
            Console.Write("Đăng nhập với tư cách: ");
        } while (!int.TryParse(Console.ReadLine(), out opt) || (opt != 1 && opt != 2));
        Console.Clear();
        Console.Write("Tên đăng nhập: ");
        string username = Console.ReadLine();
        if (string.IsNullOrEmpty(username))
        {
            Console.WriteLine("Tài Khoản không được để trống");
            Console.ReadLine();
            DashBoard();
        }
        Console.Write("Mật khẩu: ");
        string password = Console.ReadLine();
        if (string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Mật khẩu không được để trống");
            Console.ReadLine();
            DashBoard();
        }
        if (DangNhap(users, admins, danhSachDoanVien, username, password, opt == 1))
        {
            if (opt == 1)
            {
                Console.Clear();
                MenuAdmin(danhSachDoanVien);
            }
            else
            {
                User currentUser = TimUserTrongDanhSach(users, username, password);
                if (currentUser != null)
                {
                    Console.Clear();
                    MenuUser(currentUser);
                }
                else
                {
                    Console.WriteLine("Không tìm thấy người dùng.");
                }
            }
        }
        else
        {
            Console.WriteLine("Đăng nhập không thành công. Vui lòng kiểm tra lại tên đăng nhập và mật khẩu.");
            Console.ReadLine();
            DashBoard();
        }
    }
    static void DocDanhSachTuDoanVien(List<User> users, string filePath)
    {
        try
        {
            // Đọc dữ liệu từ tệp XML
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            // Lấy danh sách các phần tử "DoanVien" từ tệp XML
            XmlNodeList doanVienList = doc.GetElementsByTagName("DoanVien");

            // Duyệt qua từng phần tử "DoanVien" và lấy thông tin tài khoản và mật khẩu
            foreach (XmlNode node in doanVienList)
            {
                string taiKhoan = node.SelectSingleNode("TaiKhoan").InnerText;
                string matKhau = node.SelectSingleNode("MatKhau").InnerText;

                // Thêm thông tin vào danh sách users
                users.Add(new User(taiKhoan, matKhau));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while reading user data from XML: {ex.Message}");
            // Optionally, handle the exception or log it.
        }
    }
    static List<DoanVien> DocDanhSachDoanVien(string filePath)
    {
        List<DoanVien> danhSachDoanVien = new List<DoanVien>();
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DoanVien>));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                danhSachDoanVien = (List<DoanVien>)serializer.Deserialize(fileStream);
            }
            Console.WriteLine("Đọc danh sách đoàn viên từ file XML thành công.");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Không tìm thấy tệp {filePath}. Sẽ tạo tệp mới.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Đã xảy ra lỗi khi đọc danh sách đoàn viên từ tệp: {ex.Message}");
        }
        return danhSachDoanVien;
    }
    static User TimUserTrongDanhSach(List<User> users, string username, string password)
    {
        foreach (User user in users)
        {
            if (user.TaiKhoan == username && user.matKhau == password)
            {
                return user;
            }
        }
        return null;
    }
    static bool DangNhap(List<User> users, List<Admin> admins, List<DoanVien> danhSachDoanVien, string username, string password, bool isAdmin)
    {
        bool isValidUser = false;

        if (isAdmin)
        {
            // Kiểm tra danh sách quản trị viên
            foreach (Admin admin in admins)
            {
                if (admin.TaiKhoan == username && admin.matKhau == password)
                {
                    isValidUser = true;
                    break;
                }
            }
        }
        else
        {
            // Kiểm tra danh sách người dùng (bao gồm cả đoàn viên)
            foreach (User user in users)
            {
                if (user.TaiKhoan == username && user.matKhau == password)
                {
                    isValidUser = true;
                    break;
                }
            }
        }

        // Nếu thông tin đăng nhập chính xác, trả về true
        return isValidUser;
    }
    static bool MenuAdmin(List<DoanVien> danhSachDoanVien)
    {
        Console.Clear();
        Console.WriteLine("----- MENU ADMIN -----");
        Console.WriteLine("1. Thêm tài khoản đoàn viên mới");
        Console.WriteLine("2. Xem danh sách đoàn viên");
        Console.WriteLine("3. Cập nhật dữ liệu đoàn viên");
        Console.WriteLine("4. Xóa dữ liệu đoàn viên");
        Console.WriteLine("5. Quản lý hoạt động");
        Console.WriteLine("6. Thống kê");
        Console.WriteLine("7. Đăng xuất");
        Console.Write("Nhập lựa chọn của bạn: ");
        int luaChon = int.Parse(Console.ReadLine());

        switch (luaChon)
        {
            case 1:
                ThemTaiKhoanDoanVien(danhSachDoanVien);
                return true;
            case 2:
                XemDanhSachDoanVien();
                return true;
            case 3:
                CapNhatDuLieuDoanVien();
                return true;
            case 4:
                XoaDuLieuDoanVien(danhSachDoanVien);
                return true;
            case 5:
                QuanLyHoatDong(danhSachDoanVien);
                return true;
            case 6:
                ThongKe();
                return true;
            case 7:
                Console.WriteLine("Đăng Xuất");
                DashBoard();
                return true;
            default:
                Console.WriteLine("Lựa chọn không hợp lệ.");
                return true;
        }
    }
    static bool MenuUser(User user)
    {
        Console.WriteLine("----- MENU USER -----");
        Console.WriteLine("1. Cập nhật thông tin cá nhân");
        Console.WriteLine("2. Xem thông tin cá nhân");
        Console.WriteLine("3. Đăng xuất");
        Console.Write("Nhập lựa chọn của bạn: ");
        int luaChon = int.Parse(Console.ReadLine());

        switch (luaChon)
        {
            case 1:
                CapNhatThongTinCaNhan(user);
                return true;
            case 2:
                XemThongTinCaNhan(user);
                return true;
            case 3:
                Console.WriteLine("Đăng xuất");
                DashBoard();
                return true;
            default:
                Console.WriteLine("Lựa chọn không hợp lệ.");
                return true;
        }
    }
    static void ThemTaiKhoanDoanVien(List<DoanVien> danhSachDoanVien)
    {
        Console.Clear();
        string doanVienFilePath = "doanvien.xml";
        Console.WriteLine("Chức năng Đăng ký tài khoản mới");
        Console.Write("Nhập tài khoản: ");
        string taiKhoan = Console.ReadLine();
        if (string.IsNullOrEmpty(taiKhoan))
        {
            Console.WriteLine("Tài khoản không được để trống");
            Console.ReadLine();
            ThemTaiKhoanDoanVien(danhSachDoanVien);
        }
        foreach (DoanVien dv in danhSachDoanVien)
        {
            if (dv.TaiKhoan == taiKhoan)
            {
                Console.WriteLine("Tài khoản đã tồn tại. Vui lòng chọn tài khoản khác.");
                Console.ReadLine();
                ThemTaiKhoanDoanVien(danhSachDoanVien);
            }
        }
        Console.Write("Nhập mật khẩu: ");
        string matKhau = Console.ReadLine();
        if (string.IsNullOrEmpty(matKhau))
        {
            Console.WriteLine("Mật khẩu không được để trống");
            Console.ReadLine();
            ThemTaiKhoanDoanVien(danhSachDoanVien);
        }
        // Kiểm tra xem tài khoản đã tồn tại trong danh sách hay không
        Console.Write("Nhập tên đoàn viên: ");
        string ten = Console.ReadLine();
        if (string.IsNullOrEmpty(ten) || ContainsDigit(ten) || ContainsPunctuation(ten))
        {
            Console.WriteLine("Tên đoàn viên không được để trống và không được chứa số.");
            Console.ReadLine();
            ThemTaiKhoanDoanVien(danhSachDoanVien);
        }

        Console.Write("Nhập số căn cước công dân: ");
        string canCuoc = Console.ReadLine();
        if (string.IsNullOrEmpty(canCuoc) || ContainsLetter(canCuoc) || ContainsPunctuation(canCuoc))
        {
            Console.WriteLine("Số căn cước công dân không được để trống và không được chứa ký tự chữ.");
            Console.ReadLine();
            ThemTaiKhoanDoanVien(danhSachDoanVien);
        }

        Console.Write("Nhập khoa: ");
        string khoa = Console.ReadLine();
        if (string.IsNullOrEmpty(khoa) || ContainsPunctuation(khoa))
        {
            Console.WriteLine("Khoa không được để trống và không được chứa ký tự đặc biệt.");
            Console.ReadLine();
            ThemTaiKhoanDoanVien(danhSachDoanVien);
        }

        // Tạo một đối tượng đoàn viên mới và thêm vào danh sách
        DoanVien newDoanVien = new DoanVien()
        {
            HoTen = ten,
            CanCuocCongDan = canCuoc,
            Khoa = khoa,
            TaiKhoan = taiKhoan,
            MatKhau = matKhau
        };

        // Thêm đoàn viên mới vào danh sách
        danhSachDoanVien.Add(newDoanVien);

        // Lưu danh sách xuống file XML
        LuuDanhSachDoanVien(danhSachDoanVien, doanVienFilePath);

        Console.WriteLine("Đăng ký tài khoản mới thành công!");
        MenuAdmin(danhSachDoanVien);
    }
    static bool ContainsDigit(string input)
    {
        foreach (char c in input)
        {
            if (char.IsDigit(c))
                return true;
        }
        return false;
    }
    static bool ContainsLetter(string input)
    {
        foreach (char c in input)
        {
            if (char.IsLetter(c))
                return true;
        }
        return false;
    }
    static bool ContainsPunctuation(string input)
    {
        foreach (char c in input)
        {
            if (char.IsPunctuation(c))
                return true;
        }
        return false;
    }
    static void LuuDanhSachDoanVien(List<DoanVien> danhSachDoanVien, string filePath)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DoanVien>));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, danhSachDoanVien);
            }
            Console.WriteLine("Lưu danh sách đoàn viên xuống file XML thành công.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Đã xảy ra lỗi khi lưu danh sách đoàn viên: {ex.Message}");
            MenuAdmin(danhSachDoanVien);
        }
    }
    static void XemDanhSachDoanVien()
    {
        Console.Clear();
        string doanVienFilePath = "doanvien.xml";
        List<DoanVien> danhSachDoanVien = DocDanhSachDoanVien(doanVienFilePath);

        Console.WriteLine("----- DANH SÁCH ĐOÀN VIÊN -----");
        foreach (DoanVien doanVien in danhSachDoanVien)
        {
            Console.WriteLine($"Họ và tên: {doanVien.HoTen}");
            Console.WriteLine($"Số CCCD: {doanVien.CanCuocCongDan}");
            Console.WriteLine($"Khoa: {doanVien.Khoa}");
            Console.WriteLine($"Tài Khoản: {doanVien.TaiKhoan}");
            doanVien.ThongTin();
            Console.WriteLine("----------------------------------");
        }
        Console.ReadLine();
        MenuAdmin(danhSachDoanVien);
    }
    static void CapNhatDuLieuDoanVien()
    {
        Console.WriteLine("Chức năng Cập nhật dữ liệu đoàn viên");
        string doanVienFilePath = "doanvien.xml";
        try
        {
            // Đọc danh sách đoàn viên từ tệp XML
            List<DoanVien> danhSachDoanVien = DocDanhSachDoanVien(doanVienFilePath);
            if (danhSachDoanVien != null)
            {
                // Yêu cầu người dùng nhập số căn cước công dân để tìm đoàn viên cần cập nhật
                Console.Write("Nhập số căn cước công dân của đoàn viên cần cập nhật: ");
                string soCCCD = Console.ReadLine();

                // Tìm kiếm đoàn viên trong danh sách
                DoanVien dvCanCapNhat = danhSachDoanVien.Find(dv => dv.CanCuocCongDan == soCCCD);
                if (dvCanCapNhat != null)
                {
                    // Hiển thị thông tin đoàn viên cần cập nhật và yêu cầu nhập thông tin mới
                    Console.WriteLine("Thông tin đoàn viên cần cập nhật:");
                    dvCanCapNhat.ThongTin();
                    Console.WriteLine("Nhập thông tin mới:");

                    // Yêu cầu người dùng chọn thông tin cần cập nhật
                    Console.WriteLine("Chọn thông tin cần cập nhật:");
                    Console.WriteLine("1. Họ tên");
                    Console.WriteLine("2. Số căn cước công dân (CCCD)");
                    Console.WriteLine("3. Khoa");
                    Console.Write("Lựa chọn của bạn: ");
                    int luaChon = int.Parse(Console.ReadLine());

                    // Thực hiện cập nhật theo lựa chọn của người dùng
                    switch (luaChon)
                    {
                        case 1:
                            Console.Write("Nhập họ tên mới: ");
                            string hoTenMoi = Console.ReadLine();
                            dvCanCapNhat.HoTen = hoTenMoi;
                            break;
                        case 2:
                            Console.Write("Nhập số căn cước công dân mới: ");
                            string canCuocMoi = Console.ReadLine();
                            dvCanCapNhat.CanCuocCongDan = canCuocMoi;
                            break;
                        case 3:
                            Console.Write("Nhập khoa mới: ");
                            string khoaMoi = Console.ReadLine();
                            dvCanCapNhat.Khoa = khoaMoi;
                            break;
                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ.");
                            return;
                    }

                    // Lưu danh sách đã cập nhật vào tệp XML
                    LuuDanhSachDoanVien(danhSachDoanVien, doanVienFilePath);

                    Console.WriteLine("Cập nhật thông tin đoàn viên thành công!");
                    MenuAdmin(danhSachDoanVien);
                }
                else
                {
                    Console.WriteLine("Không tìm thấy đoàn viên cần cập nhật.");
                    MenuAdmin(danhSachDoanVien);
                }
            }
            else
            {
                Console.WriteLine("Không thể đọc danh sách đoàn viên từ tệp XML.");
                MenuAdmin(danhSachDoanVien);
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Lựa chọn không hợp lệ.");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Không tìm thấy tệp {ex.FileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
        }
    }
    static void XoaDuLieuDoanVien(List<DoanVien> danhSachDoanVien)
    {
        Console.WriteLine("Nhập số CCCD của đoàn viên bạn muốn xóa: ");
        string cccdCanXoa = Console.ReadLine();

        bool found = false;
        for (int i = 0; i < danhSachDoanVien.Count; i++)
        {
            if (danhSachDoanVien[i].CanCuocCongDan == cccdCanXoa)
            {
                found = true;

                Console.WriteLine($"Bạn có chắc chắn muốn xóa đoàn viên có số CCCD {cccdCanXoa}? (Y/N)");
                string confirmation = Console.ReadLine();

                if (confirmation.ToUpper() == "Y")
                {
                    // Xóa từ danh sách
                    danhSachDoanVien.RemoveAt(i);
                    Console.WriteLine("Đã xóa thành công!");

                    // Cập nhật lại tệp XML
                    CapNhatDuLieuDoanVienTrongXML(danhSachDoanVien);
                    MenuAdmin(danhSachDoanVien);
                }
                else
                {
                    Console.WriteLine("Hủy bỏ việc xóa.");
                    MenuAdmin(danhSachDoanVien);
                }
                break;
            }
        }

        if (!found)
        {
            Console.WriteLine($"Không tìm thấy đoàn viên có số CCCD {cccdCanXoa} trong danh sách.");
            MenuAdmin(danhSachDoanVien);
        }
    }
    static void CapNhatDuLieuDoanVienTrongXML(List<DoanVien> danhSachDoanVien)
    {
        // Đường dẫn tới tệp XML
        string doanVienFilePath = "doanvien.xml";

        // Tạo một đối tượng XmlDocument để làm việc với tệp XML
        XmlDocument doc = new XmlDocument();
        doc.Load(doanVienFilePath);

        // Lấy danh sách các nút "DoanVien" từ tệp XML
        XmlNodeList doanVienNodes = doc.SelectNodes("//DoanVien");

        // Xóa tất cả các nút "DoanVien" hiện có trong tệp XML
        foreach (XmlNode node in doanVienNodes)
        {
            node.ParentNode.RemoveChild(node);
        }

        // Thêm lại các nút "DoanVien" mới dựa trên danh sách đoàn viên sau khi xóa
        foreach (DoanVien doanVien in danhSachDoanVien)
        {
            XmlElement doanVienElement = doc.CreateElement("DoanVien");

            XmlElement hoTenElement = doc.CreateElement("HoTen");
            hoTenElement.InnerText = doanVien.HoTen;
            doanVienElement.AppendChild(hoTenElement);

            XmlElement cccdElement = doc.CreateElement("CanCuocCongDan");
            cccdElement.InnerText = doanVien.CanCuocCongDan;
            doanVienElement.AppendChild(cccdElement);

            XmlElement khoaElement = doc.CreateElement("Khoa");
            khoaElement.InnerText = doanVien.Khoa;
            doanVienElement.AppendChild(khoaElement);

            XmlElement taiKhoanElement = doc.CreateElement("TaiKhoan");
            taiKhoanElement.InnerText = doanVien.TaiKhoan;
            doanVienElement.AppendChild(taiKhoanElement);

            // Thêm nút "DoanVien" mới vào tệp XML
            doc.DocumentElement.AppendChild(doanVienElement);
        }

        // Lưu lại tệp XML sau khi đã cập nhật
        doc.Save(doanVienFilePath);
    }
    static List<HoatDong> DocDanhSachHoatDong(string filePath)
    {
        List<HoatDong> danhSachHoatDong = null;
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<HoatDong>));
            using (StreamReader reader = new StreamReader(filePath))
            {
                danhSachHoatDong = (List<HoatDong>)serializer.Deserialize(reader);
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Không tìm thấy tệp {filePath}: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Đã xảy ra lỗi khi đọc danh sách hoạt động: {ex.Message}");
        }
        return danhSachHoatDong;
    }
    static void LuuDanhSachHoatDong(List<HoatDong> danhSachHoatDong, string filePath)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<HoatDong>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, danhSachHoatDong);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Đã xảy ra lỗi khi lưu danh sách hoạt động: {ex.Message}");
        }
    }
    static void QuanLyHoatDong(List<DoanVien> danhSachDoanVien)
    {
        Console.Clear();
        Console.WriteLine("Quản Lý Hoạt Động");
        Console.WriteLine("Danh sách đoàn viên:");
        foreach (var doanVien in danhSachDoanVien)
        {
            Console.WriteLine($"Tên: {doanVien.HoTen}, Số căn cước công dân: {doanVien.CanCuocCongDan}");
        }

        Console.Write("Nhập số căn cước công dân của đoàn viên: ");
        string canCuoc = Console.ReadLine();

        // Tìm đoàn viên trong danh sách
        DoanVien selectedDoanVien = null;
        foreach (var doanVien in danhSachDoanVien)
        {
            if (doanVien.CanCuocCongDan == canCuoc)
            {
                selectedDoanVien = doanVien;
                break;
            }
        }

        if (selectedDoanVien != null)
        {
            Console.WriteLine($"Đã chọn đoàn viên: {selectedDoanVien.HoTen}");
            ThemHoatDong(selectedDoanVien, danhSachDoanVien);
        }
        else
        {
            Console.WriteLine("Không tìm thấy đoàn viên có số căn cước công dân này.");
            Console.ReadLine();
            MenuAdmin(danhSachDoanVien);
        }
    }
    static void ThemHoatDong(DoanVien doanVien, List<DoanVien> danhSachDoanVien)
    {
        string doanVienFilePath = "doanvien.xml";
        Console.WriteLine("Chức năng Thêm hoạt động");
        // Hiển thị menu thêm hoạt động
        Console.WriteLine("1. Học thuật");
        Console.WriteLine("2. Văn Nghệ");
        Console.WriteLine("3. Thể thao");
        Console.Write("Nhập lựa chọn của bạn: ");
        int luaChon = int.Parse(Console.ReadLine());

        switch (luaChon)
        {
            case 1:
                Console.Write("Nhập tên hoạt động học thuật: ");
                string hoatDongHocThuat = Console.ReadLine();
                doanVien.ThemHoatDong("Học thuật", hoatDongHocThuat);
                break;
            case 2:
                Console.Write("Nhập tên hoạt động văn nghệ: ");
                string hoatDongNgheThuat = Console.ReadLine();
                doanVien.ThemHoatDong("Văn nghệ", hoatDongNgheThuat);
                break;
            case 3:
                Console.Write("Nhập tên hoạt động thể thao: ");
                string hoatDongTheThao = Console.ReadLine();
                doanVien.ThemHoatDong("Thể thao", hoatDongTheThao);
                break;
            default:
                Console.WriteLine("Lựa chọn không hợp lệ.");
                break;
        }
        LuuDanhSachDoanVien(danhSachDoanVien, doanVienFilePath);
        Console.ReadLine();
        QuanLyHoatDong(danhSachDoanVien);
    }

    static void ThongKe()
    {
        string doanVienFilePath = "doanvien.xml";
        List<DoanVien> danhSachDoanVien = DocDanhSachDoanVien(doanVienFilePath);

        foreach (DoanVien doanVien in danhSachDoanVien)
        {
            Console.WriteLine("Tên: " + doanVien.HoTen);
            Console.WriteLine("SL hoạt động tham gia: " + doanVien.SoLuongHoatDongDaThamGia);
            if (doanVien.SoLuongHoatDongDaThamGia >= 5)
            {
                Console.WriteLine("Xếp loại giỏi");
            } else if (doanVien.SoLuongHoatDongDaThamGia < 5 && doanVien.SoLuongHoatDongDaThamGia >=3)
            {
                Console.WriteLine("Xếp loại Khá");
            }
            else if (doanVien.SoLuongHoatDongDaThamGia < 3 && doanVien.SoLuongHoatDongDaThamGia > 0)
            {
                Console.WriteLine("Xếp loại trung bình");
            }
            else
            {
                Console.WriteLine(" X ");
            }
        }
        Console.ReadLine();
        MenuAdmin(danhSachDoanVien);

    }
    static void CapNhatThongTinCaNhan(User user)
    {
        Console.WriteLine("Chức năng Cập nhật thông tin cá nhân");

        // Hiển thị menu lựa chọn
        Console.WriteLine("1. Thay đổi mật khẩu");
        Console.WriteLine("2. Cập nhật hoạt động vừa tham gia");
        Console.Write("Nhập lựa chọn của bạn: ");
        int luaChon = int.Parse(Console.ReadLine());

        switch (luaChon)
        {
            case 1:
                ThayDoiMatKhau(user);
                break;
            case 2:
                CapNhatHoatDongVuaThamGia(user);
                break;
            default:
                Console.WriteLine("Lựa chọn không hợp lệ.");
                break;
        }
    }
    static void ThayDoiMatKhau(User user)
    {
        Console.Write("Nhập mật khẩu mới: ");
        string newPassword = Console.ReadLine();

        // Cập nhật mật khẩu mới cho người dùng
        user.matKhau = newPassword;

        // Hiển thị thông báo cập nhật thành công
        Console.WriteLine("Mật khẩu đã được cập nhật thành công!");

        // Yêu cầu người dùng đăng nhập lại bằng mật khẩu mới
        Console.WriteLine("Vui lòng đăng nhập lại bằng mật khẩu mới.");
        Main(null); // Gọi lại phương thức Main để đăng nhập lại
    }
    static void CapNhatHoatDongVuaThamGia(User user)
    {
        Console.WriteLine("Nhập tên hoạt động vừa tham gia: ");
        string hoatDong = Console.ReadLine();

        // Thêm hoạt động vừa tham gia vào danh sách hoạt động của người dùng
        user.HoatDongVuaThamGia.Add(hoatDong);

        // Hiển thị thông báo cập nhật thành công
        Console.WriteLine("Hoạt động vừa tham gia đã được cập nhật thành công!");
    }
    static void XemThongTinCaNhan(User currentUser)
    {
        string doanVienFilePath = "doanvien.xml";
        List<DoanVien> danhSachDoanVien = DocDanhSachDoanVien(doanVienFilePath); // Đọc danh sách đoàn viên từ tệp XML

        if (danhSachDoanVien != null)
        {
            // Kiểm tra xem thông tin của đoàn viên có trùng khớp với thông tin người dùng hiện tại không
            foreach (DoanVien dv in danhSachDoanVien)
            {
                if (dv.MatKhau == currentUser.matKhau && dv.TaiKhoan == currentUser.TaiKhoan)
                {
                    Console.WriteLine($"Tên: {dv.HoTen}");
                    Console.WriteLine($"Số căn cước công dân: {dv.CanCuocCongDan}");
                    Console.WriteLine($"Khoa: {dv.Khoa}");
                    Console.WriteLine($"Tài khoản: {dv.TaiKhoan}");
                    dv.ThongTin();
                    Console.WriteLine("----------------------");
                    MenuUser(currentUser);
                }
            }

            // Nếu không tìm thấy thông tin người dùng trong danh sách đoàn viên
            Console.WriteLine("Tài khoản hoặc mật khẩu không đúng.");
        }
        else
        {
            Console.WriteLine("Không thể đọc danh sách đoàn viên sau cập nhật.");
        }

        // Quay lại menu người dùng sau khi xem thông tin cá nhân
        Console.WriteLine("\nNhấn phím bất kỳ để quay lại menu.");
        MenuUser(currentUser);
    }
}

