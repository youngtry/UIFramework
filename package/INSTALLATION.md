# UI Framework Installation Guide

## Prerequisites

- Unity 2022.3 or later
- Git (for package installation)

## Installation Methods

### Method 1: Via Git URL (Recommended)

1. Open Unity Package Manager (`Window > Package Manager`)
2. Click the "+" button in the top-left corner
3. Select "Add package from git URL"
4. Enter: `https://github.com/youngtry/UIBaseFramework.git`
5. Click "Add"

Unity will automatically resolve and install all dependencies including CommonTools.

### Method 2: Via Local Package

1. Clone or download this repository
2. Open Unity Package Manager (`Window > Package Manager`)
3. Click the "+" button and select "Add package from disk"
4. Navigate to the downloaded folder and select `package.json`
5. Click "Open"

## Dependencies

The UI Framework will automatically install the following dependencies:

### Required Dependencies
- **Unity UI (com.unity.ugui)** - Unity's built-in UI system
- **TextMeshPro (com.unity.textmeshpro)** - Advanced text rendering
- **DOTween (com.demigiant.dotween)** - Enhanced animations for UI components
- **CommonTools** - Provides SingletonMonoBehaviour and utility functions
  - Repository: https://github.com/youngtry/CommonTools.git
  - Automatically installed via Git URL

## Post-Installation Setup

### 1. Import Samples (Optional)

After installation, you can import the included samples:

1. Open Package Manager and find "UI Framework"
2. Expand the "Samples" section
3. Click "Import" next to the desired sample:
   - **Basic UI Examples**: Shows basic page and popup usage
   - **Animation Examples**: Demonstrates collection animations and effects

### 2. Create UIResourceManager (Optional)

For customizable UI resources:

1. Right-click in Project window
2. Select `Create > UI Framework > UI Resource Manager`
3. Configure your custom icons and resources
4. Place in a `Resources` folder for automatic loading

### 3. Setup UIManager in Scene

1. Create an empty GameObject named "UIManager"
2. Add the UIManager component
3. Assign your UI Canvas
4. Register your pages and popups in the inspector

## Troubleshooting

### CommonTools Not Found

If you encounter errors about CommonTools:

1. Ensure you have internet connection during installation
2. Try removing and re-adding the package
3. Manually add CommonTools: `https://github.com/youngtry/CommonTools.git`

### DOTween Compilation Errors

If you see DOTween-related errors:

1. DOTween is now a required dependency and should be automatically installed
2. If errors persist, try removing and re-adding the package to ensure DOTween is properly installed

### Package Manager Issues

If Package Manager fails to resolve dependencies:

1. Clear Package Manager cache:
   - Close Unity
   - Delete `Library/PackageCache` folder
   - Reopen Unity and try again

2. Manual dependency installation:
   - Install CommonTools first: `https://github.com/youngtry/CommonTools.git`
   - Then install UI Framework

### Git URL Issues

If Git URL installation fails:

1. Ensure Git is installed on your system
2. Check your internet connection
3. Try using SSH URL if HTTPS fails: `git@github.com:youngtry/UIBaseFramework.git`
4. Use local package installation as alternative

## Verification

After successful installation, you should see:

1. "UI Framework" in Package Manager
2. No compilation errors in Console
3. UIManager component available in Component menu
4. Sample folders available for import

## Next Steps

1. Read the [Usage Guide](USAGE_GUIDE.md) for detailed usage instructions
2. Import and explore the sample projects
3. Create your first UI page following the Quick Start guide in README.md

## Support

If you encounter issues:

1. Check the [Troubleshooting](#troubleshooting) section above
2. Review the Console for specific error messages
3. Ensure all dependencies are properly installed
4. Check Unity version compatibility (2022.3+)

For additional support, please refer to the project repository issues section.
