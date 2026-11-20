using System;

namespace SensorSystem
{
    public sealed class Alarm
    {
        public bool IsActive { get; private set; }

        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            Console.WriteLine("  [ALARM] Đã kích hoạt còi báo cháy!");
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
            Console.WriteLine("  [ALARM] Đã tắt còi báo cháy.");
        }
    }

    public sealed class DoorController
    {
        public bool IsUnlocked { get; private set; }

        public void UnlockEmergencyDoors()
        {
            if (IsUnlocked) return;
            IsUnlocked = true;
            Console.WriteLine("  [DOOR] Đã mở khóa toàn bộ cửa thoát hiểm.");
        }

        public void LockDoors()
        {
            if (!IsUnlocked) return;
            IsUnlocked = false;
            Console.WriteLine("  [DOOR] Đã khóa lại cửa phòng máy.");
        }
    }

    public sealed class VentilationController
    {
        public bool IsRunning { get; private set; } = true;

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            Console.WriteLine("  [VENTILATION] Đã ngắt hệ thống thông gió.");
        }

        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            Console.WriteLine("  [VENTILATION] Đã khởi động lại hệ thống thông gió.");
        }
    }

    public sealed class EmergencyMediator : IEmergencyMediator
    {
        private readonly Alarm alarm;
        private readonly DoorController doorController;
        private readonly VentilationController ventilationController;

        public EmergencyMediator(
            Alarm alarm,
            DoorController doorController,
            VentilationController ventilationController)
        {
            this.alarm = alarm ?? throw new ArgumentNullException(nameof(alarm));
            this.doorController = doorController ?? throw new ArgumentNullException(nameof(doorController));
            this.ventilationController = ventilationController ?? throw new ArgumentNullException(nameof(ventilationController));
        }

        public void Notify(object sender, string ev)
        {
            Console.WriteLine($"[MEDIATOR] Nhận sự kiện '{ev}' từ {sender.GetType().Name}:");

            switch (ev)
            {
                case "FIRE_DETECTED":
                    alarm.Activate();
                    doorController.UnlockEmergencyDoors();
                    ventilationController.Stop();
                    break;

                case "ALL_CLEAR":
                    alarm.Deactivate();
                    doorController.LockDoors();
                    ventilationController.Start();
                    break;

                default:
                    Console.WriteLine($"  [MEDIATOR] Bỏ qua sự kiện không xác định: {ev}");
                    break;
            }
        }
    }
}