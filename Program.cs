//% =====================================================================
//%  Mã sinh viên: 202418836
//%  Họ tên      : Nguyễn Vũ Quang Anh
//% =====================================================================
using System;
using System.Text;

namespace SensorSystem
{
    internal sealed class FaultyObserver : IAlertObserver
    {
        public void Update(AlertEvent alertEvent)
        {
            throw new InvalidOperationException("Lỗi giả lập từ observer bị sự cố!");
        }
    }

    internal sealed class OneTimeObserver : IAlertObserver
    {
        private readonly SensorMonitor monitor;

        public OneTimeObserver(SensorMonitor monitor)
        {
            this.monitor = monitor;
        }

        public void Update(AlertEvent alertEvent)
        {
            Console.WriteLine($"[ONE-TIME] Nhận cảnh báo 1 lần ({alertEvent.Severity}) rồi tự hủy đăng ký.");
            monitor.Detach(this);
        }
    }

    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // 1. Test Adapter (Mục C.4)
            Console.WriteLine("=== 1. KIỂM THỬ ADAPTER ===");
            var legacy = new LegacySmokeDetector(735);
            ISensor sensor = new SmokeSensorAdapter(legacy);
            Console.WriteLine($"ReadValue(): {sensor.ReadValue()}"); // 73.5
            Console.WriteLine($"GetUnit(): {sensor.GetUnit()}");     // % smoke

            // 2. Khởi tạo Mediator & Observer
            var alarm = new Alarm();
            var door = new DoorController();
            var ventilation = new VentilationController();
            IEmergencyMediator mediator = new EmergencyMediator(alarm, door, ventilation);

            var monitor = new SensorMonitor();
            var dashboard = new DashboardObserver();
            var email = new EmailAlertObserver();
            var auditLog = new AuditLogObserver();
            var bridge = new EmergencyBridgeObserver(mediator);
            var oneTime = new OneTimeObserver(monitor);

            monitor.Attach(dashboard);
            monitor.Attach(dashboard); // Thử đăng ký trùng -> bị bỏ qua
            monitor.Attach(email);
            monitor.Attach(auditLog);
            monitor.Attach(oneTime);
            monitor.Attach(bridge);

            // 3. Mức bình thường (< 50%) -> Không phát cảnh báo
            Console.WriteLine("\n=== 2. MỨC BÌNH THƯỜNG (350 -> 35.0%) ===");
            legacy.SetSmokeLevel(350);
            monitor.CheckSensor("ServerRoom-Smoke-01", sensor);
            Console.WriteLine("(Không có cảnh báo nào được phát)");

            // 4. Mức WARNING (50% .. < 70%) -> Phát Observer, chưa kích hoạt Mediator
            Console.WriteLine("\n=== 3. MỨC WARNING (580 -> 58.0%) ===");
            legacy.SetSmokeLevel(580);
            monitor.CheckSensor("ServerRoom-Smoke-01", sensor);

            // 5. Mức CRITICAL (>= 70%) -> Kích hoạt cả Observer và Mediator (FIRE_DETECTED)
            Console.WriteLine("\n=== 4. MỨC CRITICAL (735 -> 73.5%) ===");
            legacy.SetSmokeLevel(735);
            monitor.CheckSensor("ServerRoom-Smoke-01", sensor);

            // 6. Hủy đăng ký Email & gắn Observer lỗi để kiểm thử cô lập lỗi
            Console.WriteLine("\n=== 5. HỦY ĐĂNG KÝ EMAIL & CÔ LẬP OBSERVER LỖI ===");
            monitor.Detach(email);
            monitor.Attach(new FaultyObserver());
            legacy.SetSmokeLevel(610);
            monitor.CheckSensor("ServerRoom-Smoke-01", sensor);

            // 7. Tổng kết AuditLog & khôi phục thiết bị qua Mediator
            Console.WriteLine("\n=== 6. TỔNG KẾT & KHÔI PHỤC TRẠNG THÁI AN TOÀN ===");
            Console.WriteLine($"Tổng số bản ghi trong AuditLog: {auditLog.Logs.Count}");
            mediator.Notify(monitor, "ALL_CLEAR");
        }
    }
}