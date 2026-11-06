# Mangareading

Ứng dụng web đọc và quản lý truyện tranh xây dựng bằng ASP.NET Core MVC trên .NET 8. Dự án có giao diện đọc truyện, quản trị nội dung, API cho các thao tác chính và tích hợp MangaDex/Imgur.

## Chức năng

- Quản lý manga, chương, trang truyện, thể loại, tác giả, nhóm và nguồn truyện.
- Đọc truyện, đánh dấu yêu thích, lưu lịch sử đọc và theo dõi tiến độ.
- Tài khoản, đăng nhập cookie, hồ sơ người dùng, bình luận và phản hồi.
- Quản trị người dùng, nội dung, thống kê, log và đồng bộ dữ liệu.
- Đồng bộ/tải dữ liệu từ MangaDex; tải ảnh thông qua dịch vụ Imgur.
- Background service cho thống kê và dọn cache; log file/cấu hình AWS khi chạy production.

## Cấu trúc

- `Mangareading/Controllers/`: MVC controllers và API controllers.
- `Mangareading/Models/`, `Repositories/`, `Services/`: dữ liệu, truy cập dữ liệu và nghiệp vụ.
- `Mangareading/Views/` và `wwwroot/`: Razor Views cùng CSS, JavaScript, ảnh tĩnh.
- `Mangareading/SQL/`: script khởi tạo và cập nhật database.
- `Mangareading/appsettings.example.json`: mẫu cấu hình an toàn.

## Yêu cầu

- .NET SDK 8.
- SQL Server hoặc LocalDB.
- Thông tin cấu hình hợp lệ cho database; Imgur/AWS chỉ cần khi dùng các tích hợp tương ứng.

## Chạy cục bộ

1. Tạo `Mangareading/appsettings.json` từ `Mangareading/appsettings.example.json`.
2. Điền connection string và các khóa dịch vụ vào file cục bộ hoặc biến môi trường. Không commit file cấu hình thực tế.
3. Khôi phục và chạy ứng dụng:

   ```powershell
   dotnet restore Mangareading.sln
   dotnet run --project Mangareading/Mangareading.csproj
   ```

4. Theo cấu hình mặc định, môi trường phát triển dùng HTTPS ở cổng `5001`; production dùng HTTP cổng `5000` và có thể dùng biến môi trường `RDS_CONNECTION_STRING`.

