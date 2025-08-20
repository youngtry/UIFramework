# Animation UI Framework Examples

This sample demonstrates advanced UI animations and effects using the UI Framework with DOTween integration.

## What's Included

### Scripts
- **AddTextAnimation.cs** - Animated text effects for rewards and notifications
- **Tip.cs** - Tooltip system with fade animations
- **DiamondCollectionAnimation.cs** - Diamond collection animation effects
- **GoldCollectionAnimation.cs** - Gold collection animation effects  
- **MoneyCollectionAnimation.cs** - Money collection animation effects

### Prefabs
- **AddTextAnimation.prefab** - Animated text prefab
- **Tip.prefab** - Tooltip prefab
- **DiamondCollectionAnimation.prefab** - Diamond animation prefab
- **GoldCollectionAnimation.prefab** - Gold animation prefab
- **MoneyCollectionAnimation.prefab** - Money animation prefab
- **diamond.prefab** - Diamond item prefab
- **gold.prefab** - Gold item prefab
- **money.prefab** - Money item prefab

### Sprites
- Various UI sprites for buttons and icons
- Resource icons (diamond, gold, money)
- Background and gradient textures

## Dependencies

This sample requires DOTween for animations. Install it via:
1. Window → DOTween Utility Panel
2. Setup DOTween
3. Or install via Package Manager if available

## How to Use

1. **Import the Sample**
   - Open Package Manager
   - Find "UI Framework" package
   - Expand "Samples" section
   - Click "Import" next to "Animation Examples"

2. **Setup DOTween**
   - Install DOTween from the Asset Store or Package Manager
   - Setup DOTween in your project

3. **Use Animation Components**
   - Add animation scripts to your UI elements
   - Configure animation parameters in the inspector
   - Call animation methods from your page/popup scripts

## Key Features Demonstrated

- **Text Animations**: Animated text effects for rewards and notifications
- **Collection Animations**: Smooth animations for collecting resources
- **Tooltip System**: Fade-in/fade-out tooltips with positioning
- **DOTween Integration**: Professional animation library integration
- **Reusable Components**: Modular animation components

## Code Examples

### Showing Animated Text
```csharp
// Instantiate and show animated text
GameObject textAnim = Instantiate(addTextAnimationPrefab);
AddTextAnimation anim = textAnim.GetComponent<AddTextAnimation>();
anim.ShowText("+100 Gold", Color.yellow);
```

### Collection Animation
```csharp
// Play collection animation
GoldCollectionAnimation goldAnim = GetComponent<GoldCollectionAnimation>();
goldAnim.PlayCollectionAnimation(startPosition, endPosition);
```

### Tooltip Usage
```csharp
// Show tooltip
Tip.ShowTip("This is a tooltip message", targetTransform);

// Hide tooltip
Tip.HideTip();
```

## Animation Types

### AddTextAnimation
- Configurable text content and color
- Scale and fade animations
- Auto-destroy after animation
- Customizable duration and easing

### Collection Animations
- Move from source to target position
- Scale and rotation effects during movement
- Particle effects on collection
- Sound effect integration

### Tip System
- Fade in/out animations
- Smart positioning to stay on screen
- Customizable appearance
- Auto-hide timer support

This sample showcases how to create polished UI animations that enhance user experience and provide visual feedback for game actions.
