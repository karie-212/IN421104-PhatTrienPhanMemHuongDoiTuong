# Phân tích vi phạm nguyên tắc SOLID trong `ClubManager` và Giải pháp khắc phục

> Số dòng tham chiếu theo file `ClubManager.cs` hiện tại (trước khi refactor).

## 1. Single Responsibility Principle (SRP) — Nguyên tắc Trách nhiệm đơn lẻ
* **Vị trí vi phạm:** Lớp `ClubManager` (dòng 28–117) và các phương thức:
  * `CreateClub` (dòng 35–64): validate tên (dòng 38–41), tạo và lưu CLB (dòng 44–45), ghi log (dòng 48), gửi thông báo (dòng 52–63).
  * `AddMemberToClub` (dòng 67–84): kiểm tra nghiệp vụ và lưu thành viên (dòng 69–76), ghi log (dòng 79), gửi email chào mừng và Slack (dòng 82–83).
  * `CreateEvent` (dòng 88–99): tạo sự kiện (dòng 93), ghi log (dòng 94), tự gửi email cho từng thành viên (dòng 96–97).
  * `PrintClubReport` (dòng 103–113): định dạng và in báo cáo (dòng 105–108), ghi log ngay trong vòng lặp báo cáo (dòng 111).
* **Mô tả lỗi:** Class này đang ôm đồm quá nhiều việc: vừa quản lý dữ liệu CLB/Thành viên/Sự kiện, vừa ghi file log, vừa định dạng báo cáo, vừa gửi thông báo qua email/Slack.
* **Hậu quả:** Giống như một người vừa làm bếp vừa làm bưu tá. Khi muốn thay đổi cách ghi log hay sửa báo cáo, ta bắt buộc phải chỉnh sửa class này, rất dễ làm hỏng các tính năng quản lý CLB đang chạy ổn định.
* **Giải pháp khắc phục:** Tách `ClubManager` thành các Service nhỏ đúng chuyên môn:
  * `ClubService`: Chỉ xử lý nghiệp vụ liên quan đến CLB và Thành viên.
  * `EventService`: Chỉ quản lý tạo và xử lý sự kiện.
  * `ReportService`: Chỉ chịu trách nhiệm tạo báo cáo.
  * `IAppLogger` & `INotificationService`: Đảm nhận việc ghi log và gửi thông báo.

---

## 2. Open/Closed Principle (OCP) — Nguyên tắc Mở/Đóng
* **Vị trí vi phạm:**
  * `CreateClub` (dòng 52–63): chuỗi `if/else` kiểm tra đuôi email (`@gmail.com`, `@yahoo.com`, còn lại) để chọn cách gửi thông báo.
  * `AddMemberToClub` (dòng 82–83): kênh email và Slack được viết cứng ngay trong method.
  * `CreateEvent` (dòng 96–97): cách báo tin sự kiện mới (email cho từng thành viên) được viết cứng trong vòng lặp `foreach`.
  * `PrintClubReport` (dòng 105–108): chỉ có một định dạng in ra Console, không có điểm mở rộng cho định dạng khác.
* **Mô tả lỗi:** Khi muốn thêm kênh thông báo mới (như Outlook) hoặc thêm định dạng xuất báo cáo (như CSV/Excel), ta phải chui trực tiếp vào trong phương thức cũ để viết thêm code.
* **Hậu quả:** Phá vỡ tính khép kín của ứng dụng. Mỗi lần thêm tính năng mới là một lần nguy cơ phát sinh lỗi ngoài ý muốn ở những tính năng cũ.
* **Giải pháp khắc phục:** 
  * Áp dụng **Factory Pattern** cho `INotificationService` để dễ dàng mở rộng kênh thông báo mới mà không sửa code cũ.
  * Áp dụng **Strategy Pattern** (`IReportStrategy`) cho báo cáo, tách riêng `ConsoleReportStrategy` và `CsvReportStrategy`.
  * Áp dụng **Observer Pattern** (`IEventObserver`) cho `CreateEvent`: thêm cách báo tin mới chỉ cần đăng ký thêm observer, không sửa `EventService`.

---

## 3. Dependency Inversion Principle (DIP) — Nguyên tắc Đảo ngược Phụ thuộc
* **Vị trí vi phạm:**
  * Trường `_logFilePath = "club-log.txt"` viết cứng đường dẫn file (dòng 32).
  * `File.AppendAllText(_logFilePath, ...)` gọi trực tiếp trong `CreateClub` (dòng 48), `AddMemberToClub` (dòng 79), `CreateEvent` (dòng 94), `PrintClubReport` (dòng 111).
  * `Console.WriteLine(...)` gọi trực tiếp trong `CreateClub` (dòng 54, 58, 62), `AddMemberToClub` (dòng 82, 83), `CreateEvent` (dòng 97), `PrintClubReport` (dòng 108).
* **Mô tả lỗi:** Class cấp cao (`ClubManager`) đang dính chặt trực tiếp vào các công cụ chi tiết cấp thấp (`System.IO.File`, `Console`) mà không thông qua "ổ cắm trung gian" (Interface/Abstraction).
* **Hậu quả:** Code bị hàn chết vào đĩa cứng và màn hình console, không thể thay đổi cơ chế ghi log (vd: chuyển sang lưu Database) và cực kỳ khó viết Unit Test (không thể mock việc ghi file).
* **Giải pháp khắc phục:** 
  * Định nghĩa các Interface `IAppLogger` và `IClubRepository`.
  * Tiêm (Inject) các phụ thuộc này vào Constructor của `ClubService`, `EventService`, `ReportService` thay vì tự gọi trực tiếp các lớp hạ tầng (`File`, `Console`).

---

## 4. Interface Segregation Principle (ISP) — Nguyên tắc Phân tách Giao diện
* **Vị trí vi phạm:**
  * `_clubs` (dòng 30) và `_members` (dòng 31) là `List<T>` giữ thẳng trong class, không có repository interface nào đứng giữa.
  * Mọi thao tác dữ liệu được viết trực tiếp lên `List`: thêm (`_clubs.Add` dòng 45, `_members.Add` dòng 76), tìm (`_clubs.FirstOrDefault` dòng 69, 90), đếm (`_members.Count` dòng 72, 107), lọc (`_members.Where` dòng 96).
  * `GetAllClubs` (dòng 116) trả về chính `List` nội bộ, bên ngoài có thể `Add`/`Remove`/`Clear` tùy ý.
  * Sự kiện tạo ở `CreateEvent` (dòng 93) không được lưu vào đâu cả, vì không có kho chứa sự kiện.
* **Mô tả lỗi:** Việc lưu trữ CLB, thành viên và sự kiện không được tách thành các hợp đồng (Interface) riêng. Ai muốn dùng dữ liệu CLB thì chỉ có cách đi qua `ClubManager`, tức là kéo theo luôn cả phần thành viên, sự kiện, log, thông báo và báo cáo.
* **Hậu quả:** Một client chỉ cần đọc danh sách CLB (vd: báo cáo) cũng bị buộc phụ thuộc vào toàn bộ class lớn. Không thể thay cách lưu (vd: chuyển sang Database) hay thay bằng bản giả khi viết Unit Test cho riêng từng phần.
* **Giải pháp khắc phục:** Tách thành các repository interface nhỏ, mỗi interface chỉ lo một loại dữ liệu:
  * `IClubRepository`: thêm, tìm theo Id, lấy danh sách CLB.
  * `IMemberRepository`: thêm thành viên, lấy/đếm thành viên theo CLB.
  * `IEventRepository`: lưu và lấy sự kiện theo CLB.
  * Mỗi Service chỉ nhận đúng interface nó cần qua Constructor (vd: `ReportService` chỉ cần `IClubRepository` và `IMemberRepository`, không cần `IEventRepository`).

---

## 5. Tổng hợp: Vi phạm → Class/interface mới → Pattern áp dụng

| Vi phạm | Class/interface mới | Pattern áp dụng |
|---|---|---|
| SRP — `CreateClub`, `AddMemberToClub` trộn validate, lưu, log, thông báo (dòng 35–84) | `ClubService` | — |
| SRP + OCP — `CreateEvent` tự gửi email cho thành viên (dòng 96–97) | `EventService`, `IEventObserver`, `EmailNotifier`, `LogNotifier` | Observer |
| SRP + OCP — `PrintClubReport` chỉ in Console, trộn với log (dòng 103–113) | `ReportService`, `IReportStrategy`, `ConsoleReportStrategy`, `CsvReportStrategy` | Strategy |
| OCP — chuỗi `if/else` chọn kênh thông báo (dòng 52–63) | `INotificationService`, `INotificationServiceFactory` | Factory |
| DIP — `File.AppendAllText` và `_logFilePath` viết cứng (dòng 32, 48, 79, 94, 111) | `IAppLogger` | — |
| DIP — ghi log lẫn trong thao tác dữ liệu | `LoggingClubRepository` (bọc `IClubRepository`) | Decorator |
| ISP — `_clubs`, `_members` là `List` giữ thẳng trong class (dòng 30–31, 116) | `IClubRepository`, `IMemberRepository`, `IEventRepository` | — |
