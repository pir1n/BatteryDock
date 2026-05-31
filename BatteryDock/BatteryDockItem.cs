using System;
using System.Threading;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace BatteryDock;

internal sealed partial class BatteryDockItem : ListItem, IDisposable
{
    private readonly Timer _timer;

    public BatteryDockItem() : base(new NoOpCommand())
    {
        Command = new RefreshBatteryCommand(Update);
        Title = "Pin ?";
        Subtitle = "Đang đọc trạng thái pin...";
        Icon = new IconInfo("\uE83F"); // Battery10

        Update();

        // Cập nhật sau 5 giây, rồi mỗi 15 giây.
        _timer = new Timer(
            _ => Update(),
            null,
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(15));
    }

    private void Update()
    {
        var battery = BatteryApi.Read();

        Title = battery.Percent is int p ? $"{p}%" : "Pin ?";
        Subtitle = BuildSubtitle(battery);
        Icon = new IconInfo(BuildBatteryGlyph(battery));
    }

    private static string BuildSubtitle(BatterySnapshot battery)
    {
        if (battery.NoBattery)
        {
            return "Không có pin hệ thống";
        }

        if (battery.Percent is null)
        {
            return "Không đọc được phần trăm pin";
        }

        var state = battery.Charging
            ? "Đang sạc"
            : battery.AcConnected
                ? "Đang cắm sạc"
                : "Đang dùng pin";

        if (battery.BatterySaver)
        {
            state += " • Tiết kiệm pin";
        }

        if (!battery.AcConnected && !battery.Charging && battery.Remaining is not null)
        {
            state += $" • còn {FormatRemaining(battery.Remaining.Value)}";
        }

        return state;
    }

    private static string FormatRemaining(TimeSpan time)
    {
        var hours = (int)time.TotalHours;
        var minutes = time.Minutes;

        return hours > 0
            ? $"{hours}h {minutes}m"
            : $"{minutes}m";
    }

    private static string BuildBatteryGlyph(BatterySnapshot battery)
    {
        if (battery.Percent is not int percent)
        {
            return battery.Charging ? "\uEB1A" : "\uE850";
        }

        var level = Math.Clamp(
            (int)Math.Round(percent / 10.0, MidpointRounding.AwayFromZero),
            0,
            10);

        return battery.Charging
            ? ChargingGlyph(level)
            : NormalGlyph(level);
    }

    private static string NormalGlyph(int level) => level switch
    {
        0 => "\uE850",
        1 => "\uE851",
        2 => "\uE852",
        3 => "\uE853",
        4 => "\uE854",
        5 => "\uE855",
        6 => "\uE856",
        7 => "\uE857",
        8 => "\uE858",
        9 => "\uE859",
        _ => "\uE83F",
    };

    private static string ChargingGlyph(int level) => level switch
    {
        0 => "\uE85A",
        1 => "\uE85B",
        2 => "\uE85C",
        3 => "\uE85D",
        4 => "\uE85E",
        5 => "\uE85F",
        6 => "\uE860",
        7 => "\uE861",
        8 => "\uE862",
        9 => "\uE83E",
        _ => "\uEA93",
    };

    public void Dispose()
    {
        _timer.Dispose();
    }
}