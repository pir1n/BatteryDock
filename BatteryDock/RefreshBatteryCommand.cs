using System;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace BatteryDock;

internal sealed partial class RefreshBatteryCommand : InvokableCommand
{
    private readonly Action _refresh;

    public RefreshBatteryCommand(Action refresh)
    {
        Id = "com.example.batterydock.refresh";
        Name = "Refresh battery";
        Icon = new IconInfo("\uE895"); // Sync icon
        _refresh = refresh;
    }

    public override CommandResult Invoke()
    {
        _refresh();
        return CommandResult.KeepOpen();
    }
}