# 🚀 UI Framework 安装指南

## ✅ 解决方案：Git凭据问题

如果你遇到以下错误：
```
Unable to add package [https://github.com/youngtry/UIBaseFramework.git]:
Error when executing git command. fatal: could not read Username for 'https://github.com': terminal prompts disabled
```

这是因为仓库名称或Git配置问题。请按照以下步骤解决：

## 📦 正确的安装方法

### 方法1：使用正确的仓库URL（推荐）

在Unity Package Manager中使用以下URL：

```
https://github.com/youngtry/UIFramework.git
```

**注意**：仓库名是 `UIFramework`，不是 `UIBaseFramework`

### 方法2：安装特定版本

如果你想安装特定版本，使用：

```
https://github.com/youngtry/UIFramework.git#v1.2.2
```

### 方法3：如果仍有Git凭据问题

1. **检查Git配置**：
   ```bash
   git config --global user.name "Your Name"
   git config --global user.email "your.email@example.com"
   ```

2. **清除Git凭据缓存**：
   ```bash
   git config --global --unset credential.helper
   ```

3. **重启Unity**并重试安装

### 方法4：本地安装（备用方案）

如果Git URL方式仍有问题：

1. 下载仓库ZIP文件：https://github.com/youngtry/UIFramework/archive/main.zip
2. 解压到本地目录
3. 在Unity Package Manager中选择"Add package from disk"
4. 选择解压后的`package.json`文件

## 🔧 安装步骤详解

### 步骤1：打开Package Manager
- Unity菜单：`Window > Package Manager`

### 步骤2：添加包
- 点击左上角的"+"按钮
- 选择"Add package from git URL"

### 步骤3：输入URL
```
https://github.com/youngtry/UIFramework.git
```

### 步骤4：等待安装
- Unity会自动下载并安装包及其依赖项
- 包括CommonTools依赖会自动安装

### 步骤5：验证安装
- 在Package Manager中查看"UI Framework"包
- 检查Console是否有错误信息
- 确认可以创建UIManager组件

## 📋 依赖项

安装时会自动安装以下依赖：

### 必需依赖
- Unity UI (com.unity.ugui) 1.0.0
- TextMeshPro (com.unity.textmeshpro) 3.0.0  
- CommonTools (https://github.com/youngtry/CommonTools.git)

### 可选依赖
- DOTween (com.demigiant.dotween) 1.2.0

## 🎯 安装后设置

### 1. 导入示例（可选）
- 在Package Manager中找到"UI Framework"
- 展开"Samples"部分
- 点击"Import"导入示例项目

### 2. 创建UIManager
- 在场景中创建空GameObject
- 添加UIManager组件
- 分配UI Canvas
- 注册你的页面和弹窗

### 3. 创建第一个页面
```csharp
using UIFramework;

public class MyFirstPage : Page
{
    protected override void OnAfterShow(params object[] data)
    {
        Debug.Log("My first page is shown!");
    }
}
```

## ❗ 常见问题

### Q: 仍然提示Git凭据错误
**A**: 确保使用正确的仓库URL：`https://github.com/youngtry/UIFramework.git`

### Q: CommonTools安装失败
**A**: 手动安装CommonTools：
```
https://github.com/youngtry/CommonTools.git
```

### Q: 找不到UIManager组件
**A**: 检查包是否正确安装，重启Unity编辑器

### Q: 编译错误
**A**: 确保Unity版本为2022.3或更高

## 🔗 相关链接

- **仓库地址**: https://github.com/youngtry/UIFramework
- **问题反馈**: https://github.com/youngtry/UIFramework/issues
- **使用文档**: 查看包内的USAGE_GUIDE.md
- **更新日志**: 查看包内的CHANGELOG.md

## 💡 获取帮助

如果安装过程中遇到问题：

1. 检查Unity版本（需要2022.3+）
2. 确认网络连接正常
3. 查看Unity Console的错误信息
4. 在GitHub仓库创建Issue并提供详细信息

---

**安装成功后，开始享受UI Framework带来的便利吧！** 🎉
