# Basic UI Framework Examples

This sample demonstrates the basic usage of the UI Framework with simple page and popup examples.

## What's Included

### Scripts
- **ExampleMainPage.cs** - Main page with navigation buttons
- **ExampleSettingsPage.cs** - Settings page with volume slider and sound toggle
- **ExampleInventoryPage.cs** - Inventory page with item management
- **ExampleConfirmPopup.cs** - Confirmation popup with customizable title and message

## How to Use

1. **Import the Sample**
   - Open Package Manager
   - Find "UI Framework" package
   - Expand "Samples" section
   - Click "Import" next to "Basic UI Examples"

2. **Setup Scene**
   - Create a Canvas in your scene
   - Create an empty GameObject named "UIManager"
   - Add UIManager component to it
   - Assign the Canvas to the "UI Canvas" field

3. **Create Page GameObjects**
   - Create empty GameObjects for each page under the Canvas
   - Add the corresponding page scripts (ExampleMainPage, ExampleSettingsPage, etc.)
   - Design your UI layout for each page
   - Drag the page GameObjects to the UIManager's "Pages" array

4. **Create Popup GameObjects**
   - Create empty GameObjects for popups under the Canvas
   - Add the popup scripts (ExampleConfirmPopup)
   - Design your popup UI layout
   - Drag the popup GameObjects to the UIManager's "Popups" array

5. **Test the Framework**
   - Run the scene
   - Use the navigation buttons to switch between pages
   - Test popup functionality

## Key Features Demonstrated

- **Page Navigation**: Switch between different full-screen pages
- **Popup Display**: Show modal popups over current pages
- **Parameter Passing**: Pass data when showing pages/popups
- **UI Event Binding**: Handle button clicks and UI interactions
- **Settings Persistence**: Save and load user preferences
- **Dynamic Content**: Add/remove items in inventory

## Code Examples

### Showing a Page
```csharp
UIManager.Instance.ShowPage<ExampleMainPage>();
```

### Showing a Popup with Parameters
```csharp
UIManager.Instance.ShowPopup<ExampleConfirmPopup>("Title", "Message", onConfirm, onCancel);
```

### Handling Page Data
```csharp
protected override void OnAfterShow(params object[] data)
{
    if (data.Length > 0 && data[0] is string title)
    {
        titleText.text = title;
    }
}
```

This sample provides a solid foundation for understanding how to use the UI Framework in your own projects.
