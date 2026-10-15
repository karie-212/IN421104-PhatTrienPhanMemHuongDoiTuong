# Sơ đồ lớp sơ bộ — EduHub

Vẽ bằng Mermaid (hiển thị trực tiếp trên GitHub) hoặc xuất ảnh từ draw.io rồi đặt cạnh file này.
Ghi rõ thuộc tính chính, khoá ngoại (FK) và bội số quan hệ (1, *, 0..1).

```mermaid
classDiagram
    class Club {
        +Guid Id
        +string Name
    }
    class Event {
        +Guid Id
        +Guid ClubId
    }
    Club "1" --> "*" Event : tổ chức

    %% TODO: bổ sung Member, Registration, CheckIn, đủ thuộc tính, phương thức và các quan hệ còn lại
```
