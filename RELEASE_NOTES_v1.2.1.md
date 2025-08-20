# UI Framework v1.2.1 Release Notes

## 🎉 Release Information

- **Version**: v1.2.1
- **Release Date**: August 20, 2025
- **Git Tag**: v1.2.1
- **Repository**: https://github.com/youngtry/UIFramework.git

## 📦 Installation

### Via Unity Package Manager (Recommended)

```
https://github.com/youngtry/UIFramework.git#v1.2.1
```

### Via Git URL (Latest)

```
https://github.com/youngtry/UIFramework.git
```

## 🆕 What's New in v1.2.1

### Added
- **CommonTools Dependency**: Properly declared in package.json for automatic installation
- **Complete Meta Files**: All files now have proper Unity .meta files for seamless integration
- **Enhanced Documentation**: Added INSTALLATION.md and USAGE_GUIDE.md for better user experience

### Fixed
- **Dependency Management**: Added missing CommonTools dependency for SingletonMonoBehaviour
- **Unity Compatibility**: All meta files included to prevent GUID conflicts
- **Documentation Updates**: Updated all documentation to reflect current dependencies

## 🔧 Key Features

### Core UI Management
- **BasePage**: Foundation class for all UI elements
- **Page**: Full-screen pages with mutual exclusion
- **Popup**: Overlay popups with modal support
- **UIManager**: Centralized singleton management system

### Collection Animations
- **GoldCollectionAnimation**: Animated gold collection effects
- **DiamondCollectionAnimation**: Diamond collection animations
- **MoneyCollectionAnimation**: Money collection effects
- **AddTextAnimation**: Reward text animations
- **Tip**: Tooltip and notification system
- **Notice**: Scrolling notification banner

### Resource Management
- **UIResourceManager**: Centralized resource and icon management
- **Customizable Icons**: Runtime customization of all UI elements
- **Audio Integration**: Built-in sound effect management
- **Editor Tools**: Visual configuration tools

### Message System
- **MessagePopup**: Ready-to-use dialog component
- **Quick Methods**: `MessagePopup.ShowMessage()`, `MessagePopup.ShowConfirm()`
- **Custom Dialogs**: Fully customizable popup configurations

## 📋 Dependencies

### Required
- Unity 2022.3 or later
- Unity UI (com.unity.ugui) 1.0.0
- TextMeshPro (com.unity.textmeshpro) 3.0.0
- CommonTools (https://github.com/youngtry/CommonTools.git)

### Optional
- DOTween (com.demigiant.dotween) 1.2.0 - Enhanced animations

## 🚀 Quick Start

1. **Install via Package Manager**:
   - Open Package Manager
   - Add package from git URL: `https://github.com/youngtry/UIFramework.git#v1.2.1`

2. **Import Samples**:
   - Basic UI Examples: Shows fundamental usage
   - Animation Examples: Demonstrates collection animations

3. **Setup UIManager**:
   - Create GameObject with UIManager component
   - Assign UI Canvas
   - Register your pages and popups

## 📚 Documentation

- **README.md**: Overview and basic usage
- **INSTALLATION.md**: Detailed installation guide
- **USAGE_GUIDE.md**: Comprehensive usage instructions
- **CHANGELOG.md**: Complete version history
- **PUBLISHING.md**: Package publishing guidelines

## 🔗 Links

- **Repository**: https://github.com/youngtry/UIFramework
- **Issues**: https://github.com/youngtry/UIFramework/issues
- **Releases**: https://github.com/youngtry/UIFramework/releases
- **CommonTools**: https://github.com/youngtry/CommonTools

## 🎯 Next Steps

1. Explore the sample projects
2. Read the usage guide for detailed instructions
3. Create your first UI page following the quick start guide
4. Customize resources using UIResourceManager

## 💡 Support

For questions, issues, or feature requests:
1. Check the documentation first
2. Search existing issues on GitHub
3. Create a new issue with detailed information
4. Include Unity version and error messages

---

**Happy UI Development!** 🎨✨
