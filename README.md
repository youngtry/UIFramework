# UI Framework

A simple and flexible UI framework for Unity that provides a clean architecture for managing UI pages and popups.

## Features

- **Hierarchical Architecture**: BasePage base class with Page and Popup specialized classes
- **Type-Safe Management**: Use `ShowPage<T>()` and `ShowPopup<T>()` for type-safe page management
- **Manual Registration**: Pages and popups are manually assigned in the inspector, no dynamic discovery
- **Flexible Parameters**: Support for unlimited parameters of any type
- **Modal Support**: Popups support modal display with background masks
- **Animation Support**: Built-in animation system with customizable transitions

## Installation

### Via Git URL (Recommended)
1. Open Unity Package Manager
2. Click the "+" button and select "Add package from git URL"
3. Enter: `https://github.com/youngtry/UIBaseFramework.git`

### Via Local Package
1. Download or clone this repository
2. In Unity Package Manager, click "+" and select "Add package from disk"
3. Select the `package.json` file

### Samples
After installation, you can import the included samples:
1. Open Package Manager and find "UI Framework"
2. Expand the "Samples" section
3. Click "Import" next to the desired sample

## Dependencies

### Required Dependencies
- Unity 2020.3 or later
- Unity UI (com.unity.ugui)
- TextMeshPro (com.unity.textmeshpro)
- CommonTools (https://github.com/youngtry/CommonTools.git) - Required for SingletonMonoBehaviour and utilities

### Optional Dependencies
- DOTween (com.demigiant.dotween) - For enhanced animations (fallback animations provided if not available)

## Quick Start

### 1. Setup UIManager
1. Create an empty GameObject named "UIManager" in your scene
2. Add the UIManager component
3. Assign your UI Canvas in the inspector
4. Drag your Page objects to the "Pages" array
5. Drag your Popup objects to the "Popups" array

### 2. Create a Page
```csharp
using UIFramework;

public class MainPage : Page
{
    protected override void OnAfterShow(params object[] data)
    {
        // Page shown logic here
        if (data.Length > 0 && data[0] is string title)
        {
            // Handle title parameter
        }
    }
}
```

### 3. Create a Popup
```csharp
using UIFramework;

public class ConfirmPopup : Popup
{
    protected override void OnAfterShow(params object[] data)
    {
        // Popup shown logic here
    }
}
```

### 4. Show Pages and Popups
```csharp
// Show a page (hides other pages)
UIManager.Instance.ShowPage<MainPage>();

// Show a popup (overlays on current page)
UIManager.Instance.ShowPopup<ConfirmPopup>("Title", "Message");

// Show a message dialog (quick method)
MessagePopup.ShowMessage("Hello World!");
MessagePopup.ShowConfirm("Delete Item", "Are you sure?",
    () => Debug.Log("Confirmed"),
    () => Debug.Log("Cancelled"));

// Show collection animations
var goldAnim = GetComponent<GoldCollectionAnimation>();
goldAnim.ShowMoneyEffect(worldPosition, 100);

// Customize UI resources
UIResourceManager.Instance.SetCustomCollectionIcon(CollectionType.Gold, myCustomGoldSprite);
UIResourceManager.Instance.PlaySound(SoundType.Collect);

// Hide pages/popups
UIManager.Instance.HidePage<MainPage>();
UIManager.Instance.HidePopup<ConfirmPopup>();
```

## Core Classes

### BasePage
Base class for all UI elements providing:
- `Show(params object[] data)` - Display the UI element
- `Hide()` - Hide the UI element  
- `Refresh(params object[] data)` - Refresh with new data
- `Close()` - Close the UI element

### Page
Inherits from BasePage for full-screen pages:
- Only one page can be visible at a time
- Showing a new page automatically hides the current page
- Perfect for main screens, settings, inventory, etc.

### Popup
Inherits from BasePage for overlay popups:
- Multiple popups can be displayed simultaneously
- Supports modal display with background masks
- Can be closed by clicking background (configurable)
- Perfect for dialogs, confirmations, tooltips, etc.

### MessagePopup
A ready-to-use popup component for common dialog scenarios:
- Configurable title, message, and button text
- Support for confirm/cancel actions
- Quick static methods for common use cases
- Customizable button visibility and callbacks

### Collection Animations
Rich animation components for reward and collection effects:
- **GoldCollectionAnimation**: Animated gold collection with customizable icons
- **DiamondCollectionAnimation**: Diamond collection effects
- **MoneyCollectionAnimation**: Money collection animations
- **AddTextAnimation**: Animated text for showing rewards
- **Tip**: Tooltip and notification component
- **Notice**: Scrolling notification banner

### UIResourceManager
Centralized resource management for customizable UI elements:
- Customizable icons for all collection types
- Configurable UI backgrounds and button sprites
- Audio clip management for UI sounds
- Easy runtime customization of visual elements

### UIManager
Singleton manager providing:
- `ShowPage<T>()` / `ShowPopup<T>()` - Display pages/popups
- `HidePage<T>()` / `HidePopup<T>()` - Hide specific pages/popups
- `HideAllPages()` / `HideAllPopups()` - Hide all pages/popups
- `GetPage<T>()` / `GetPopup<T>()` - Get page/popup references

## Requirements

- Unity 2022.3 or later
- UI Toolkit (com.unity.ugui)
- TextMeshPro (com.unity.textmeshpro)

## License

MIT License - see LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
