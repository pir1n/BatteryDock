using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using BatteryDock.Pages;

namespace BatteryDock;

public partial class BatteryDockCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _topLevelCommands;
    private readonly ICommandItem[] _dockBands;

    public BatteryDockCommandsProvider()
    {
        DisplayName = "Battery Dock";
        Id = "com.example.batterydock";

        var batteryItem = new BatteryDockItem();
        var page = new BatteryDockPage(batteryItem);

        _topLevelCommands =
        [
            new CommandItem(page)
            {
                Title = "Battery Dock",
                Subtitle = "Hiển thị phần trăm pin"
            }
        ];

        _dockBands =
        [
            new WrappedDockItem(
                [batteryItem],
                "com.example.batterydock.band",
                "Battery")
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _topLevelCommands;
    }

    public override ICommandItem[]? GetDockBands()
    {
        return _dockBands;
    }
}