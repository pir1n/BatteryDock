using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace BatteryDock.Pages;

internal sealed partial class BatteryDockPage : ListPage
{
    private readonly BatteryDockItem _batteryItem;

    public BatteryDockPage(BatteryDockItem batteryItem)
    {
        Id = "com.example.batterydock.page";
        Title = "Battery Dock";
        Name = "Open";
        Icon = new IconInfo("\uE83F");

        _batteryItem = batteryItem;
    }

    public override IListItem[] GetItems()
    {
        return [_batteryItem];
    }
}