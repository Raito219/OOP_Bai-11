# BÀI TẬP TUẦN 11: CAMPUS SAFETY HUB (ADAPTER, OBSERVER & MEDIATOR)
- **Ngôn ngữ:** C# (.NET)

---

## 1. Mục tiêu kiến thức áp dụng
Hệ thống mô phỏng trung tâm giám sát an toàn phòng máy chạy dạng Console, kết hợp 3 mẫu thiết kế hướng đối tượng:
* **Adapter (`SmokeSensorAdapter`):** Tích hợp cảm biến khói cũ `LegacySmokeDetector` (thang đo `0..1000`, `sealed`) vào giao diện chuẩn `ISensor` (`0..100` `% smoke`) bằng quan hệ bao bọc (Composition) mà không sửa mã nguồn cũ.
* **Observer (`SensorMonitor`, `IAlertObserver`, `AlertEvent`):** Phát dữ liệu cảnh báo bất biến (`sealed record`) tới nhiều bên nhận đăng ký động (`DashboardObserver`, `EmailAlertObserver`, `AuditLogObserver`, `EmergencyBridgeObserver`), duyệt trên bản sao `ToArray()` và cô lập lỗi từng observer bằng `try-catch`.
* **Mediator (`IEmergencyMediator`, `EmergencyMediator`):** Điều phối đồng bộ `Alarm`, `DoorController` và `VentilationController` khi nhận sự kiện `FIRE_DETECTED` (hoặc khôi phục qua `ALL_CLEAR`) mà các thiết bị không phụ thuộc trực tiếp vào nhau.

---

## 2. Cấu trúc mã nguồn (10 tệp)
* `ISensor.cs`, `LegacySmokeDetector.cs`, `SmokeSensorAdapter.cs`: Nhóm cảm biến và mẫu **Adapter**.
* `AlertEvent.cs`, `IAlertObserver.cs`, `SensorMonitor.cs`, `AlertObservers.cs`: Nhóm giám sát, phân loại ngưỡng (`<50%`, `WARNING`, `CRITICAL`) và mẫu **Observer**.
* `IEmergencyMediator.cs`, `EmergencySystem.cs`: Nhóm thiết bị ứng phó khẩn cấp và mẫu **Mediator**.
* `Program.cs`: Kịch bản kiểm thử toàn diện cả 3 mẫu thiết kế và các ràng buộc ngoại lệ.

---

## 3. Hướng dẫn chạy
Mở Terminal tại thư mục gốc của dự án và chạy:
```bash
dotnet run
