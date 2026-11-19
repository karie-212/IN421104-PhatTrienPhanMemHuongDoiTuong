# Ch03 — Clean Architecture và REST API EduHub

Bài tập chương 3 môn Phát triển phần mềm hướng đối tượng (IN421104).
Cấu trúc theo Clean Architecture 4 lớp kết hợp REST API và Unit Test.

## Cấu trúc dự án

- **EduHub.Domain**: Chứa các Entity (`Club`, `Member`) và Repository Interface (`IClubRepository`, `IMemberRepository`).
- **EduHub.Application**: Chứa các Use Case theo mô hình CQRS (Command/Query và Handlers), kiểm tra hợp lệ nghiệp vụ.
- **EduHub.Infrastructure**: Cài đặt Repository lưu trữ dữ liệu trong bộ nhớ (`InMemoryClubRepository`, `InMemoryMemberRepository`).
- **EduHub.API**: ASP.NET Core Web API, cấu hình Dependency Injection, Swagger UI và Controllers.
- **EduHub.Tests**: Unit Test cho tầng Application sử dụng xUnit và Moq.

## Chạy dự án

```bash
# Build solution
dotnet build EduHub.sln

# Chạy unit tests
dotnet test EduHub.sln

# Chạy Web API
dotnet run --project EduHub.API
```

Sau khi chạy API, truy cập Swagger UI tại: `http://localhost:5000/swagger`
