using System;
using System.Runtime.InteropServices;

namespace BatteryDock;

internal readonly record struct BatterySnapshot(
    int? Percent,
    bool AcConnected,
    bool Charging,
    bool NoBattery,
    bool BatterySaver,
    TimeSpan? Remaining);

internal static class BatteryApi
{
    [StructLayout(LayoutKind.Sequential)]
    private struct SYSTEM_POWER_STATUS
    {
        public byte ACLineStatus;
        public byte BatteryFlag;
        public byte BatteryLifePercent;
        public byte SystemStatusFlag;
        public int BatteryLifeTime;
        public int BatteryFullLifeTime;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS status);

    public static BatterySnapshot Read()
    {
        if (!GetSystemPowerStatus(out var s))
        {
            return new BatterySnapshot(null, false, false, false, false, null);
        }

        var noBattery = (s.BatteryFlag & 128) == 128;
        var charging = (s.BatteryFlag & 8) == 8;
        var acConnected = s.ACLineStatus == 1;
        var batterySaver = s.SystemStatusFlag == 1;

        int? percent = s.BatteryLifePercent <= 100 ? s.BatteryLifePercent : null;
        TimeSpan? remaining = s.BatteryLifeTime >= 0
            ? TimeSpan.FromSeconds(s.BatteryLifeTime)
            : null;

        return new BatterySnapshot(
            percent,
            acConnected,
            charging,
            noBattery,
            batterySaver,
            remaining);
    }
}