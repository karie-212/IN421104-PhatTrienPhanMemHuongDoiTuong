# Phân tích vi phạm nguyên tắc SOLID trong `ClubManager` và Giải pháp khắc phục

## 1. Single Responsibility Principle (SRP) — Nguyên tắc Trách nhiệm đơn lẻ
* **Vị trí vi phạm:** Lớp `ClubManager` và các phương thức `CreateClub`, `AddMemberToClub`, `CreateEvent`, `PrintClubReport`.
* **Mô tả lỗi:** Class này đang ôm đồm quá nhiều việc: vừa quản lý dữ liệu CLB/Thành viên/Sự kiện, vừa ghi file log, vừa định dạng báo cáo, vừa gửi thông báo qua email/Slack.
* **Hậu quả:** Giống như một người vừa làm bếp vừa làm bưu tá. Khi muốn thay đổi cách ghi log hay sửa báo cáo, ta bắt buộc phải chỉnh sửa class này, rất dễ làm hỏng các tính năng quản lý CLB đang chạy ổn định.
* **Giải pháp khắc phục:** Tách `ClubManager` thành các Service nhỏ đúng chuyên môn:
  * `ClubService`: Chỉ xử lý nghiệp vụ liên quan đến CLB và Thành viên.
  * `EventService`: Chỉ quản lý tạo và xử lý sự kiện.
  * `ReportService`: Chỉ chịu trách nhiệm tạo báo cáo.
  * `IAppLogger` & `INotificationService`: Đảm nhận việc ghi log và gửi thông báo.

---

## 2. Open/Closed Principle (OCP) — Nguyên tắc Mở/Đóng
* **Vị trí vi phạm:** Phương thức `CreateClub` (chuỗi `if/else` kiểm tra loại email) và `PrintClubReport`.
* **Mô tả lỗi:** Khi muốn thêm kênh thông báo mới (như Outlook) hoặc thêm định dạng xuất báo cáo (như CSV/Excel), ta phải chui trực tiếp vào trong phương thức cũ để viết thêm code.
* **Hậu quả:** Phá vỡ tính khép kín của ứng dụng. Mỗi lần thêm tính năng mới là một lần nguy cơ phát sinh lỗi ngoài ý muốn ở những tính năng cũ.
* **Giải pháp khắc phục:** 
  * Áp dụng **Factory Pattern** cho `INotificationService` để dễ dàng mở rộng kênh thông báo mới mà không sửa code cũ.
  * Áp dụng **Strategy Pattern** (`IReportStrategy`) cho báo cáo, tách riêng `ConsoleReportStrategy` và `CsvReportStrategy`.

---

## 3. Dependency Inversion Principle (DIP) — Nguyên tắc Đảo ngược Phụ thuộc
* **Vị trí vi phạm:** Các câu lệnh `File.AppendAllText(_logFilePath, ...)` và `Console.WriteLine(...)` nằm rải rác trong class.
* **Mô tả lỗi:** Class cấp cao (`ClubManager`) đang dính chặt trực tiếp vào các công cụ chi tiết cấp thấp (`System.IO.File`, `Console`) mà không thông qua "ổ cắm trung gian" (Interface/Abstraction).
* **Hậu quả:** Code bị hàn chết vào đĩa cứng và màn hình console, không thể thay đổi cơ chế ghi log (vd: chuyển sang lưu Database) và cực kỳ khó viết Unit Test (không thể mock việc ghi file).
* **Giải pháp khắc phục:** 
  * Định nghĩa các Interface `IAppLogger` và `IClubRepository`.
  * Tiêm (Inject) các phụ thuộc này vào Constructor của `ClubService`, `EventService`, `ReportService` thay vì tự gọi trực tiếp các lớp hạ tầng (`File`, `Console`).

---

## 4. Interface Segregation Principle (ISP) — Nguyên tắc Phân tách Giao diện
* **Vị trí vi phạm:** Không có các Interface riêng biệt cho từng dịch vụ (Logging, Notification, Reporting, Repository).
* **Mô tả lỗi:** Gom tất cả mọi chức năng vào một class duy nhất thay vì chia nhỏ thành các hợp đồng (Interface) riêng biệt.
* **Hậu quả:** Người dùng chỉ muốn gọi chức năng quản lý CLB nhưng lại bị ép buộc phải kéo theo toàn bộ các phụ thuộc liên quan đến File System và I/O.
* **Giải pháp khắc phục:** Tách thành nhiều Interface nhỏ, độc lập: `IClubRepository`, `IAppLogger`, `INotificationService`, `IReportStrategy`, `IEventObserver`. Mỗi thành phần chỉ phụ thuộc đúng Interface mà nó cần sử dụng.