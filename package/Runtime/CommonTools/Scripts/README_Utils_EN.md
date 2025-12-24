# Utils Class Documentation

## Overview

`Utils` is a Unity utility class located in the `CommonTools` namespace, providing various practical functions including UI layout management, logging system, time formatting, number formatting, currency symbol retrieval, and more.

## Class Properties

### Static Properties
- `Country` (string): Current country code, default is "US"
- `ShowLog` (bool): Whether to show logs, default is true

## Main Features

### 1. UI Layout Management

#### ResizeLayout(GameObject container)
Force rebuild the layout of the specified container.

```csharp
Utils.ResizeLayout(myContainer);
```

#### DeleteAllChildren(Transform parent)
Delete all child nodes of the specified parent object (delayed deletion).

```csharp
Utils.DeleteAllChildren(parentTransform);
```

#### DeleteAllChildrenImmediate(Transform parent)
Immediately delete all child nodes of the specified parent object.

```csharp
Utils.DeleteAllChildrenImmediate(parentTransform);
```

### 2. Logging System

#### Log(string message, LogType type = LogType.Info, params object[] args)
Enhanced logging function with complex object formatting and colored output support.

**Log Types:**
- `LogType.Debug`: Debug information
- `LogType.Info`: General information (green)
- `LogType.Warning`: Warning information (yellow)
- `LogType.Error`: Error information (red)

```csharp
Utils.Log("Simple message");
Utils.Log("User ID: {0}, Score: {1}", LogType.Info, userId, score);
Utils.Log("Error occurred", LogType.Error);
```

**Features:**
- Supports automatic formatting of complex objects (lists, dictionaries, etc.)
- Displays colored logs in the editor
- Can be controlled via `ShowLog` property

### 3. Time Handling

#### FormatTime(int totalSeconds)
Convert seconds to HH:mm:ss or mm:ss format.

```csharp
string time1 = Utils.FormatTime(3661); // Returns "01:01:01"
string time2 = Utils.FormatTime(125);  // Returns "02:05"
```

#### GetUtcTimestamp()
Get current UTC timestamp in seconds.

```csharp
long timestamp = Utils.GetUtcTimestamp();
```

#### GetUtcTimestampMilliseconds()
Get current UTC timestamp in milliseconds.

```csharp
long timestampMs = Utils.GetUtcTimestampMilliseconds();
```

### 4. Number Formatting

#### FormatDouble(double number)
Format double numbers with intelligent decimal handling and large number abbreviations.

```csharp
string result1 = Utils.FormatDouble(123.456);   // "123.46"
string result2 = Utils.FormatDouble(1000.0);    // "1K" (uses unit abbreviation)
string result3 = Utils.FormatDouble(123.00);    // "123"
```

#### Format(double value)
Convert large numbers to simplified format with units.

```csharp
string result = Utils.Format(1500000); // "1.5M"
```

### 5. Ordinal Number Conversion

#### ToOrdinal(int number)
Convert numbers to English ordinal format.

```csharp
string first = Utils.ToOrdinal(1);   // "1st"
string second = Utils.ToOrdinal(2);  // "2nd"
string third = Utils.ToOrdinal(3);   // "3rd"
string fourth = Utils.ToOrdinal(4);  // "4th"
string eleventh = Utils.ToOrdinal(11); // "11th"
```

### 6. Sprite Resource Management

#### GetSpriteFromAtlas(string spriteName, string atlasName)
Get specified Sprite from Sprite Atlas.

```csharp
Sprite mySprite = Utils.GetSpriteFromAtlas("iconName", "atlasName");
// If atlasName is empty, will search through all atlases
Sprite sprite = Utils.GetSpriteFromAtlas("iconName", "");
```

### 7. Currency Symbol Management

#### GetCurrencySymbol()
Get corresponding currency symbol based on current country code.

```csharp
Utils.Country = "US";
string symbol = Utils.GetCurrencySymbol(); // Returns "$"

Utils.Country = "CN";
string symbol = Utils.GetCurrencySymbol(); // Returns "¥"
```

#### GetAllCurrencySymbols()
Get dictionary of all supported currency symbols.

```csharp
var currencyMap = Utils.GetAllCurrencySymbols();
```

**Supported Major Currencies:**
- USD ($), CNY (¥), JPY (¥), KRW (₩)
- GBP (£), EUR (€), CAD (C$), AUD (A$)
- Plus 100+ other international currencies

## Usage Example

```csharp
using CommonTools;

public class Example : MonoBehaviour
{
    void Start()
    {
        // Set country and log switch
        Utils.Country = "CN";
        Utils.ShowLog = true;
        
        // Log output
        Utils.Log("Game started", Utils.LogType.Info);
        
        // Time formatting
        string gameTime = Utils.FormatTime(3725); // "01:02:05"
        
        // Number formatting
        string score = Utils.FormatDouble(1234567.89); // "1.23M"
        
        // Currency symbol
        string currency = Utils.GetCurrencySymbol(); // "¥"
        
        // Ordinal number
        string rank = Utils.ToOrdinal(1); // "1st"
        
        Utils.Log("Game time: {0}, Score: {1}, Rank: {2}", 
                  Utils.LogType.Info, gameTime, score, rank);
    }
}
```

## Important Notes

1. **Logging System**: Recommend setting `ShowLog` to false in release builds for better performance
2. **Child Deletion**: `DeleteAllChildren` uses delayed deletion, `DeleteAllChildrenImmediate` deletes immediately - choose based on needs
3. **Currency Symbols**: Supports ISO 3166-1 alpha-2 country codes and ISO 4217 currency codes
4. **Number Formatting**: Large numbers will automatically use scientific notation
5. **Sprite Retrieval**: Ensure Sprite Atlas is properly configured and loaded in Resources

## Dependencies

- Unity Engine
- Unity UI
- System.Globalization
- Unity 2D Sprite Atlas (for Sprite functionality)
