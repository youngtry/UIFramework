# UI Framework Usage Guide

## Quick Start

### 1. Basic Setup

1. Install the UI Framework package via Package Manager
2. Create a UIManager in your scene: `GameObject > UI > UI Manager`
3. Register your pages and popups with the UIManager

### 2. Creating Pages and Popups

```csharp
// Create a page class
public class MainMenuPage : Page
{
    protected override void OnPageShow(object data = null)
    {
        // Page shown logic
    }
    
    protected override void OnPageHide()
    {
        // Page hidden logic
    }
}

// Create a popup class
public class ConfirmPopup : Popup
{
    protected override void OnPopupShow(object data = null)
    {
        // Popup shown logic
    }
}
```

### 3. Using the UI Manager

```csharp
// Show pages
UIManager.Instance.ShowPage<MainMenuPage>();
UIManager.Instance.ShowPage<InventoryPage>("inventory_data");

// Show popups
UIManager.Instance.ShowPopup<ConfirmPopup>();
UIManager.Instance.ShowPopup<MessagePopup>("Hello World!");

// Hide UI
UIManager.Instance.HidePage<MainMenuPage>();
UIManager.Instance.HidePopup<ConfirmPopup>();
```

## Collection Animations

### Gold Collection Animation

```csharp
// Get the component
var goldAnimation = GetComponent<GoldCollectionAnimation>();

// Show gold collection effect
goldAnimation.ShowMoneyEffect(worldPosition, 100);

// Customize the gold icon
goldAnimation.SetCustomIcon(myCustomGoldSprite);
```

### Diamond Collection Animation

```csharp
var diamondAnimation = GetComponent<DiamondCollectionAnimation>();
diamondAnimation.ShowDiamondEffect(worldPosition, 50, true);
```

### Money Collection Animation

```csharp
var moneyAnimation = GetComponent<MoneyCollectionAnimation>();
moneyAnimation.ShowMoneyEffect(worldPosition, 1000);
```

## Resource Customization

### Setting Up UIResourceManager

1. Create a UIResourceManager asset: `Assets > Create > UI Framework > UI Resource Manager`
2. Configure your custom icons and resources
3. Place the asset in a `Resources` folder or set it as the current instance

### Customizing Icons at Runtime

```csharp
// Set custom collection icons
UIResourceManager.Instance.SetCustomCollectionIcon(CollectionType.Gold, myGoldSprite);
UIResourceManager.Instance.SetCustomCollectionIcon(CollectionType.Diamond, myDiamondSprite);

// Set custom UI icons
UIResourceManager.Instance.SetCustomIcon(IconType.Confirm, myConfirmIcon);
UIResourceManager.Instance.SetCustomIcon(IconType.Warning, myWarningIcon);

// Play sounds
UIResourceManager.Instance.PlaySound(SoundType.Collect);
UIResourceManager.Instance.PlaySound(SoundType.Success);
```

### Getting Resources

```csharp
// Get icons
Sprite goldIcon = UIResourceManager.Instance.GetGoldIcon();
Sprite confirmIcon = UIResourceManager.Instance.GetIcon(IconType.Confirm);

// Get prefabs
GameObject goldPrefab = UIResourceManager.Instance.GetCollectionPrefab(CollectionType.Gold);
```

## Message Popups

### Quick Methods

```csharp
// Simple message
MessagePopup.ShowMessage("Operation completed!");

// Confirmation dialog
MessagePopup.ShowConfirm("Delete Item", "Are you sure?", 
    () => Debug.Log("Confirmed"), 
    () => Debug.Log("Cancelled"));

// Titled confirmation
MessagePopup.ShowConfirm("Save Game", "Do you want to save your progress?",
    () => SaveGame(),
    () => Debug.Log("Not saved"));
```

### Custom Message Popup

```csharp
var data = new MessagePopupData("Custom message")
{
    title = "Custom Title",
    confirmButtonText = "OK",
    cancelButtonText = "Cancel",
    showCancelButton = true,
    onConfirm = () => Debug.Log("Custom confirm"),
    onCancel = () => Debug.Log("Custom cancel"),
    onClose = () => Debug.Log("Dialog closed")
};

MessagePopup.ShowCustom(data);
```

## Notifications and Tips

### Notice Component

```csharp
var notice = GetComponent<Notice>();

// Set notice text and start scrolling
notice.SetNoticeText("This is a scrolling notice message!");
notice.StartScrolling();

// Set callback for click events
notice.SetNoticeCallback(() => Debug.Log("Notice clicked"));

// Stop scrolling
notice.StopScrolling();
```

### Tip Component

```csharp
var tip = GetComponent<Tip>();

// Show tip with fade effect
tip.ShowTip("This is a helpful tip!");

// Hide tip
tip.HideTip();
```

## Dependencies

### CommonTools (Required)

The framework requires CommonTools for core functionality:
- SingletonMonoBehaviour pattern for UIManager
- Utility functions and extensions

CommonTools is automatically installed as a dependency when you install the UI Framework.

### DOTween Integration

The framework uses DOTween for enhanced animations. DOTween is automatically installed as a required dependency.

#### DOTween Features

The framework leverages DOTween for:
- Smooth UI animations and transitions
- Collection animations (gold, diamond effects)
- Page and popup transitions
- Tip animations with fade and movement effects

## Best Practices

### 1. Resource Management

- Always use UIResourceManager for customizable resources
- Place your UIResourceManager asset in a Resources folder for automatic loading
- Use the editor tools to easily configure resources

### 2. Performance

- Pool animation objects when possible
- Use object pooling for frequently shown popups
- Limit the number of simultaneous collection animations

### 3. Customization

- Create custom themes by extending UIResourceManager
- Use the provided editor tools for easy configuration
- Test your UI with different resource configurations

### 4. Error Handling

- Always check if UIManager.Instance is available before use
- Handle cases where resources might be missing
- Provide fallback UI elements for critical functionality

## Troubleshooting

### Common Issues

1. **UIManager not found**: Make sure UIManager is in the scene and properly initialized
2. **Resources not loading**: Check that UIResourceManager is in a Resources folder
3. **Animations not working**: Verify DOTween installation or check fallback animations
4. **Icons not showing**: Ensure sprites are properly assigned in UIResourceManager

### Debug Tips

- Use the UIManager editor to verify page/popup registration
- Check the Console for UIFramework debug messages
- Use the UIResourceManager editor tools to test resource loading
