# Ch04 — EF Core, PostgreSQL và Full API: EduHub Database

Bài tập Chương 4, môn IN421104 - Phát triển phần mềm hướng đối tượng.

## Cấu trúc

```
EduHub.Domain          Entities (Member, Club, Event, Registration) + interface repository + IUnitOfWork
EduHub.Application     Handler/Service, DTO + mapping, PagedResult/PageRequest, AppException, IEventReportQuery
EduHub.Infrastructure  EduHubDbContext (Fluent API + SeedAsync), EF repository, EfUnitOfWork,
                       DapperEventReportQuery (báo cáo SQL thuần), Migrations/Init
EduHub.API             Controllers, Requests (validation), GlobalExceptionHandler (ProblemDetails), Serilog, Dockerfile
EduHub.Tests           xUnit + Moq + SQLite in-memory (26 test)
```

## Chạy

```bash
docker compose up -d                       # PostgreSQL 16 (DB_PORT=5433 nếu cổng 5432 đã bị chiếm)
dotnet build
dotnet test                                # 26/26 pass, dùng SQLite in-memory, không cần PostgreSQL thật
dotnet run --project EduHub.API --urls http://localhost:5000     # Swagger: http://localhost:5000/swagger
```

API tự `MigrateAsync()` và `SeedAsync()` khi khởi động (mặc định bật ở Development).

## Endpoint

| Method | Route | Kết quả |
|---|---|---|
| GET | `/api/clubs?search=&page=&pageSize=` | 200 `{items,totalCount,page,pageSize,totalPages}` |
| GET, PUT, DELETE | `/api/clubs/{id}` | 200 / 404, 204 |
| POST | `/api/clubs` | 201 / 400 |
| GET | `/api/events?clubId=&search=&page=&pageSize=` | 200 (phân trang) |
| GET | `/api/events/{id}` | 200 / 404 |
| POST | `/api/events` | 201 / 400 |
| PUT | `/api/events/{id}` | 200 / 400 / 404 |
| DELETE | `/api/events/{id}` | 204 / 404 |
| POST | `/api/events/{eventId}/registrations` | 201 / 400 / 404 / 409 |
| GET | `/api/events/{eventId}/registrations` | 200 / 404 |
| DELETE | `/api/events/{eventId}/registrations/{registrationId}` | 204 / 404 |
| GET | `/api/reports/events?minRegistered=` | 200 (Dapper: báo cáo thống kê) |

Mọi lỗi 4xx/5xx trả `application/problem+json` (RFC 9457) có `traceId`.

## Ghi chú thiết kế

### 1. Vì sao dự án này có `IEventRepository` nhưng không tạo `IMemberRepository` hay generic `IRepository<T>`?

- **Không tạo `IMemberRepository`**: `Member` trong phạm vi dự án này không có nhu cầu CRUD riêng, chỉ cần kiểm tra xem sinh viên có tồn tại khi đăng ký sự kiện hay không. Việc đặt `MemberExistsAsync` ngay trong `IRegistrationRepository` giúp thiết kế gọn nhẹ, tập trung đúng vào nghiệp vụ đăng ký.
- **Không dùng Generic `IRepository<T>`**: `DbSet<T>` của EF Core vốn dĩ đã là một Generic Repository. Việc viết thêm một lớp vỏ generic không đem lại giá trị nghiệp vụ, làm che giấu các tính năng mạnh mẽ của EF và vi phạm nguyên tắc YAGNI (You Aren't Gonna Need It). Mỗi Aggregate có các câu truy vấn đặc thù (phân trang, lọc, kiểm tra trạng thái) nên cần interface riêng biệt.

### 2. `IUnitOfWork` có cần thiết không khi `DbContext` đã là Unit of Work?

- **Lý do GIỮ**: Giúp tầng Application hoàn toàn không bị phụ thuộc vào EF Core (tuân thủ Dependency Inversion Principle). Nhờ đó dễ dàng mock `IUnitOfWork.SaveChangesAsync()` bằng Moq khi viết unit test cho các handler ở Application layer.
- **Lý do BỎ**: `DbContext` bản chất đã triển khai toàn vẹn Unit of Work pattern (Change Tracker + Transaction khi gọi `SaveChangesAsync`). Bọc thêm `IUnitOfWork` chỉ tạo ra một lớp trung gian thừa (boilerplate) mà không có thêm logic bổ trợ nào.
