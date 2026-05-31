# BatteryDock

BatteryDock is a Windows PowerToys Command Palette extension that displays the current battery percentage as a Dock item.

It is designed for users who want a small, always-visible battery indicator inside the PowerToys Command Palette Dock.

## Features

* Display current battery percentage.
* Show charging, plugged-in, battery mode, and battery saver status.
* Support PowerToys Command Palette Dock band.
* Auto-refresh battery status.
* Manual refresh command from Command Palette.
* Lightweight native Windows battery status reading.

## Preview

Example Dock display:

```text
🔋 56%
```

## Requirements

To build and run this extension, you need:

* Windows 11
* PowerToys with Command Palette support
* Visual Studio Community 2026
* .NET 9 SDK
* Windows SDK 10.0.26100 or newer
* MSIX packaging support in Visual Studio

Recommended Visual Studio workloads:

* `.NET desktop development`
* `Windows application development`
* `Windows App SDK`
* `MSIX Packaging Tools`

## Project Structure

```text
BatteryDock/
├── README.md
├── BatteryDock.sln
├── Directory.Build.props
├── Directory.Packages.props
├── nuget.config
└── BatteryDock/
    ├── app.manifest
    ├── BatteryApi.cs
    ├── BatteryDock.cs
    ├── BatteryDock.csproj
    ├── BatteryDockCommandsProvider.cs
    ├── BatteryDockItem.cs
    ├── Package.appxmanifest
    ├── Program.cs
    ├── RefreshBatteryCommand.cs
    ├── Pages/
    │   └── BatteryDockPage.cs
    └── Properties/
        ├── launchSettings.json
        └── PublishProfiles/
            ├── win-arm64.pubxml
            └── win-x64.pubxml
```

## Clone

```powershell
git clone https://github.com/YOUR_USERNAME/BatteryDock.git
cd BatteryDock
```

Replace `YOUR_USERNAME` with your GitHub username.

## Restore Dependencies

Restore NuGet packages:

```powershell
dotnet restore BatteryDock.sln
```

Or use Visual Studio:

```text
Build > Restore NuGet Packages
```

## Build

Open the solution in Visual Studio:

```text
BatteryDock.sln
```

Select:

```text
Release | x64
```

Then build:

```text
Build > Build Solution
```

You can also build from terminal:

```powershell
dotnet build BatteryDock.sln -c Release -p:Platform=x64
```

For ARM64:

```powershell
dotnet build BatteryDock.sln -c Release -p:Platform=ARM64
```

## Deploy Locally

This extension is packaged as an MSIX app extension. Building is not enough. You must deploy it so PowerToys Command Palette can detect it.

In Visual Studio:

1. Select the package profile:

```text
BatteryDock (Package)
```

2. Select configuration:

```text
Release | x64
```

3. Deploy the project:

```text
Build > Deploy BatteryDock
```

Do not use only `Build`. Use `Deploy`, because deployment registers the MSIX package and the COM server required by Command Palette.

## Reload Command Palette Extensions

After deploying:

1. Open Command Palette:

```text
Win + Alt + Space
```

2. Search for:

```text
Reload
```

3. Run:

```text
Reload Command Palette extensions
```

BatteryDock should now appear in Command Palette.

## Enable the Dock

To show BatteryDock on the Command Palette Dock:

1. Open PowerToys.
2. Go to Command Palette settings.
3. Enable Dock.
4. Open Dock settings or edit mode.
5. Enable or pin the `Battery` band from BatteryDock.

You can also search for BatteryDock in Command Palette and pin it to Dock if the pin option is available.

## Recommended Dock Layout

For a compact battery indicator:

1. Open Dock edit mode.
2. Right-click the BatteryDock item.
3. Enable title display.
4. Disable subtitle display.

The Dock should show something similar to:

```text
🔋 56%
```

## Debugging

Use Debug configuration while developing:

```text
Debug | x64
```

Then deploy again:

```text
Build > Deploy BatteryDock
```

After every deployment, reload Command Palette extensions:

```text
Win + Alt + Space
Reload Command Palette extensions
```

## Common Issues

### Extension does not appear in Command Palette

Make sure you deployed the project, not only built it.

Fix:

```text
Build > Deploy BatteryDock
```

Then reload Command Palette extensions.

### Dock item does not appear

Check that `GetDockBands()` is implemented in:

```text
BatteryDock/BatteryDockCommandsProvider.cs
```

Also make sure the Command Palette Dock is enabled in PowerToys settings.

### Build succeeds but deploy does not update the extension

Try cleaning and rebuilding:

```text
Build > Clean Solution
Build > Rebuild Solution
Build > Deploy BatteryDock
```

Then reload Command Palette extensions.

### BatteryDockPage namespace error

If `BatteryDockPage.cs` uses:

```csharp
namespace BatteryDock.Pages;
```

then `BatteryDockCommandsProvider.cs` must include:

```csharp
using BatteryDock.Pages;
```

Alternatively, use the same namespace for all custom classes:

```csharp
namespace BatteryDock;
```

### Battery percentage shows as unknown

This can happen on:

* Desktop PCs without a battery.
* Virtual machines.
* Devices where Windows does not expose battery status.

On laptops, BatteryDock reads the battery status from Windows using the system power status API.

## Source Control

Do not commit generated build output.

Recommended ignored folders and files:

```text
.vs/
bin/
obj/
AppPackages/
BundleArtifacts/
PackageArtifacts/
*.appx
*.msix
*.msixbundle
*.pfx
*.cer
```

Keep these files in the repository:

```text
BatteryDock.sln
Directory.Build.props
Directory.Packages.props
nuget.config
BatteryDock/BatteryDock.csproj
BatteryDock/Package.appxmanifest
BatteryDock/app.manifest
BatteryDock/Properties/launchSettings.json
BatteryDock/Properties/PublishProfiles/*.pubxml
BatteryDock/**/*.cs
BatteryDock/Assets/**
```

## Publishing Notes

Before publishing publicly, update the package identity in:

```text
BatteryDock/Package.appxmanifest
```

Important fields:

```xml
<Identity
  Name="BatteryDock"
  Publisher="CN=Your Publisher Name"
  Version="0.0.1.0" />
```

For local development, Visual Studio can deploy the package for testing. For public release, use your own publisher identity and signing certificate.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Credits

Built for PowerToys Command Palette Dock.
