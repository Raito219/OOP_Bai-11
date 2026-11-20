using System;
using System.Collections.Generic;

namespace SensorSystem
{
    public sealed class DashboardObserver : IAlertObserver
    {
        public void Update(AlertEvent alertEvent)
        {
            Console.WriteLine(
                $"[DASHBOARD] {alertEvent.SensorName}: {alertEvent.Value:F1}{alertEvent.Unit} [{alertEvent.Severity}]");
        }
    }

    public sealed class EmailAlertObserver : IAlertObserver
    {
        private readonly string recipient;

        public EmailAlertObserver(string recipient = "safety-admin@campus.edu.vn")
        {
            this.recipient = string.IsNullOrWhiteSpace(recipient)
                ? throw new ArgumentException("Địa chỉ email không hợp lệ.", nameof(recipient))
                : recipient;
        }

        public void Update(AlertEvent alertEvent)
        {
            Console.WriteLine(
                $"[EMAIL -> {recipient}] {alertEvent.Severity} tại {alertEvent.SensorName}: " +
                $"{alertEvent.Value:F1}{alertEvent.Unit} lúc {alertEvent.OccurredAt:HH:mm:ss}");
        }
    }

    public sealed class AuditLogObserver : IAlertObserver
    {
        private readonly List<string> logs = new();

        public IReadOnlyList<string> Logs => logs.AsReadOnly();

        public void Update(AlertEvent alertEvent)
        {
            string entry =
                $"{alertEvent.OccurredAt:yyyy-MM-dd HH:mm:ss} | {alertEvent.Severity} | " +
                $"{alertEvent.SensorName} | {alertEvent.Value:F1}{alertEvent.Unit}";
            logs.Add(entry);
            Console.WriteLine($"[AUDIT LOG] {entry}");
        }
    }

    public sealed class EmergencyBridgeObserver : IAlertObserver
    {
        private readonly IEmergencyMediator mediator;

        public EmergencyBridgeObserver(IEmergencyMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void Update(AlertEvent alertEvent)
        {
            if (alertEvent.Severity == "CRITICAL")
            {
                mediator.Notify(this, "FIRE_DETECTED");
            }
        }
    }
}