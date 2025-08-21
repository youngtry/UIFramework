# Utils 工具类文档

## 概述

`Utils` 是一个Unity通用工具类，位于 `CommonTools` 命名空间下，提供了多种实用功能，包括UI布局管理、日志系统、时间格式化、数字格式化、货币符号获取等功能。

## 类属性

### 静态属性
- `Country` (string): 当前国家代码，默认为 "US"
- `ShowLog` (bool): 是否显示日志，默认为 true

## 主要功能

### 1. UI布局管理

#### ResizeLayout(GameObject container)
强制重建指定容器的布局。

```csharp
Utils.ResizeLayout(myContainer);
```

#### DeleteAllChildren(Transform parent)
删除指定父对象的所有子节点（延迟删除）。

```csharp
Utils.DeleteAllChildren(parentTransform);
```

#### DeleteAllChildrenImmediate(Transform parent)
立即删除指定父对象的所有子节点。

```csharp
Utils.DeleteAllChildrenImmediate(parentTransform);
```

### 2. 日志系统

#### Log(string message, LogType type = LogType.Info, params object[] args)
增强的日志输出功能，支持复杂对象格式化和彩色输出。

**日志类型：**
- `LogType.Debug`: 调试信息
- `LogType.Info`: 一般信息（绿色）
- `LogType.Warning`: 警告信息（黄色）
- `LogType.Error`: 错误信息（红色）

```csharp
Utils.Log("简单消息");
Utils.Log("用户ID: {0}, 分数: {1}", LogType.Info, userId, score);
Utils.Log("错误发生", LogType.Error);
```

**特性：**
- 支持复杂对象自动格式化（列表、字典等）
- 在编辑器中显示彩色日志
- 可通过 `ShowLog` 属性控制是否输出

### 3. 时间处理

#### FormatTime(int totalSeconds)
将秒数转换为时:分:秒格式。

```csharp
string time1 = Utils.FormatTime(3661); // 返回 "01:01:01"
string time2 = Utils.FormatTime(125);  // 返回 "02:05"
```

#### GetUtcTimestamp()
获取当前UTC时间戳（秒）。

```csharp
long timestamp = Utils.GetUtcTimestamp();
```

#### GetUtcTimestampMilliseconds()
获取当前UTC时间戳（毫秒）。

```csharp
long timestampMs = Utils.GetUtcTimestampMilliseconds();
```

### 4. 数字格式化

#### FormatDouble(double number)
格式化double数字，智能处理小数位数和大数值。

```csharp
string result1 = Utils.FormatDouble(123.456);   // "123.46"
string result2 = Utils.FormatDouble(1000.0);    // "1K" (使用单位缩写)
string result3 = Utils.FormatDouble(123.00);    // "123"
```

#### Format(double value)
将大数值转换为带单位的简化格式。

```csharp
string result = Utils.Format(1500000); // "1.5M"
```

### 5. 序数词转换

#### ToOrdinal(int number)
将数字转换为英语序数词格式。

```csharp
string first = Utils.ToOrdinal(1);   // "1st"
string second = Utils.ToOrdinal(2);  // "2nd"
string third = Utils.ToOrdinal(3);   // "3rd"
string fourth = Utils.ToOrdinal(4);  // "4th"
string eleventh = Utils.ToOrdinal(11); // "11th"
```

### 6. Sprite资源管理

#### GetSpriteFromAtlas(string spriteName, string atlasName)
从Sprite Atlas中获取指定的Sprite。

```csharp
Sprite mySprite = Utils.GetSpriteFromAtlas("iconName", "atlasName");
// 如果atlasName为空，会遍历所有Atlas查找
Sprite sprite = Utils.GetSpriteFromAtlas("iconName", "");
```

### 7. 货币符号管理

#### GetCurrencySymbol()
根据当前国家代码获取对应的货币符号。

```csharp
Utils.Country = "US";
string symbol = Utils.GetCurrencySymbol(); // 返回 "$"

Utils.Country = "CN";
string symbol = Utils.GetCurrencySymbol(); // 返回 "¥"
```

#### GetAllCurrencySymbols()
获取所有支持的货币符号字典。

```csharp
var currencyMap = Utils.GetAllCurrencySymbols();
```

**支持的主要货币：**
- 美元 ($)、人民币 (¥)、日元 (¥)、韩元 (₩)
- 英镑 (£)、欧元 (€)、加元 (C$)、澳元 (A$)
- 以及100+种其他国际货币

## 使用示例

```csharp
using CommonTools;

public class Example : MonoBehaviour
{
    void Start()
    {
        // 设置国家和日志开关
        Utils.Country = "CN";
        Utils.ShowLog = true;
        
        // 日志输出
        Utils.Log("游戏开始", Utils.LogType.Info);
        
        // 时间格式化
        string gameTime = Utils.FormatTime(3725); // "01:02:05"
        
        // 数字格式化
        string score = Utils.FormatDouble(1234567.89); // "1.23M"
        
        // 货币符号
        string currency = Utils.GetCurrencySymbol(); // "¥"
        
        // 序数词
        string rank = Utils.ToOrdinal(1); // "1st"
        
        Utils.Log("游戏时间: {0}, 分数: {1}, 排名: {2}", 
                  Utils.LogType.Info, gameTime, score, rank);
    }
}
```

## 注意事项

1. **日志系统**：在发布版本中建议将 `ShowLog` 设置为 false 以提高性能
2. **子节点删除**：`DeleteAllChildren` 使用延迟删除，`DeleteAllChildrenImmediate` 立即删除，根据需要选择
3. **货币符号**：支持ISO 3166-1 alpha-2国家代码和ISO 4217货币代码
4. **数字格式化**：大数值会自动使用科学计数法表示
5. **Sprite获取**：需要确保Sprite Atlas已正确配置并加载到Resources中

## 依赖项

- Unity Engine
- Unity UI
- System.Globalization
- Unity 2D Sprite Atlas (用于Sprite功能)
