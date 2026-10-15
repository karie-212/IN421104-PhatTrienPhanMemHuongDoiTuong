# User Stories — EduHub

Ba vai trò: Sinh viên, Ban CLB, Admin.

## US-01 · Sinh viên: đăng ký tham gia sự kiện

Là sinh viên, tôi muốn đăng ký tham gia một sự kiện của CLB, để giữ chỗ trước khi sự kiện diễn ra.

- Tiêu chí 1: Given sự kiện còn chỗ, When tôi đăng ký, Then hệ thống tạo đăng ký ở trạng thái chờ xác nhận.
- Tiêu chí 2: Given sự kiện đã đủ sức chứa, When tôi đăng ký, Then hệ thống từ chối và báo sự kiện đã đầy.
- Tiêu chí 3: Given tôi đã đăng ký sự kiện này, When tôi đăng ký lần nữa, Then hệ thống từ chối đăng ký trùng.

## US-02 · Sinh viên: xem thông tin sự kiện

Là sinh viên, tôi muốn xem thời gian, địa điểm và số chỗ còn lại của sự kiện, để quyết định có tham gia hay không.

- Tiêu chí 1: Given sự kiện tồn tại, When tôi xem chi tiết, Then hệ thống hiển thị tên, thời gian, địa điểm và số đăng ký trên sức chứa.
- Tiêu chí 2: Given có người vừa đăng ký, When tôi xem lại, Then số đăng ký được cập nhật đúng.

## US-03 · Ban CLB: tạo sự kiện

Là thành viên Ban CLB, tôi muốn tạo sự kiện với thời gian và sức chứa cụ thể, để sinh viên có thể đăng ký tham gia.

- Tiêu chí 1: Given tôi nhập đủ tên, thời gian và sức chứa lớn hơn 0, When tôi tạo sự kiện, Then sự kiện được lưu và gắn với CLB của tôi.
- Tiêu chí 2: Given sức chứa nhỏ hơn hoặc bằng 0 hoặc thiếu tên, When tôi tạo sự kiện, Then hệ thống từ chối và báo lỗi rõ ràng.

## US-04 · Ban CLB: xác nhận đăng ký

Là thành viên Ban CLB, tôi muốn xác nhận các đăng ký của sinh viên, để biết chính xác ai sẽ tham dự.

- Tiêu chí 1: Given đăng ký đang chờ xác nhận, When tôi xác nhận, Then trạng thái đăng ký chuyển sang đã xác nhận.
- Tiêu chí 2: Given đăng ký đã được xác nhận, When tôi xác nhận lần nữa, Then hệ thống từ chối thao tác trùng.

## US-05 · Ban CLB: check-in và xem báo cáo tham dự

Là thành viên Ban CLB, tôi muốn check-in sinh viên tại sự kiện và xem báo cáo tham dự, để đánh giá mức độ tham gia.

- Tiêu chí 1: Given sinh viên có đăng ký đã xác nhận, When tôi check-in, Then hệ thống ghi nhận thời điểm check-in.
- Tiêu chí 2: Given sinh viên chưa có đăng ký đã xác nhận, When tôi check-in, Then hệ thống từ chối.
- Tiêu chí 3: Given sự kiện đã kết thúc, When tôi xem báo cáo, Then hệ thống hiển thị số đăng ký, số check-in và tỷ lệ tham dự.

## US-06 · Admin: quản lý CLB và thành viên

Là Admin, tôi muốn tạo CLB và thêm thành viên vào CLB, để toàn bộ hoạt động của CLB được quản lý tập trung.

- Tiêu chí 1: Given tôi nhập tên CLB hợp lệ, When tôi tạo CLB, Then CLB được lưu và có thể thêm thành viên.
- Tiêu chí 2: Given thành viên đã thuộc CLB, When tôi thêm lại thành viên đó, Then hệ thống từ chối thêm trùng.