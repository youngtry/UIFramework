# Publishing Guide for UI Framework UPM Package

This document explains how to publish and distribute the UI Framework as a Unity Package Manager (UPM) package.

## Package Structure

The package follows the standard UPM structure:

```
UIBaseFramework/
├── package.json                 # Package manifest
├── README.md                   # Main documentation
├── CHANGELOG.md               # Version history
├── LICENSE                    # MIT License
├── .gitignore                # Git ignore rules
├── Runtime/                   # Runtime scripts
│   ├── UIFramework.asmdef    # Assembly definition
│   ├── BasePage.cs           # Base UI class
│   ├── Page.cs               # Full-screen page class
│   ├── Popup.cs              # Popup overlay class
│   └── UIManager.cs          # Manager singleton
├── Editor/                    # Editor-only scripts
│   ├── UIFramework.Editor.asmdef  # Editor assembly definition
│   └── UIManagerEditor.cs    # Custom inspector
└── Samples/                  # Sample content
    ├── BasicExamples/        # Basic usage examples
    └── AnimationExamples/    # Animation examples
```

## Publishing Methods

### Method 1: Git Repository (Recommended)

1. **Create Git Repository**
   ```bash
   git init
   git add .
   git commit -m "Initial commit - UI Framework v1.0.0"
   ```

2. **Push to GitHub**
   ```bash
   git remote add origin https://github.com/yourusername/UIBaseFramework.git
   git branch -M main
   git push -u origin main
   ```

3. **Create Release Tag**
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

4. **Installation via Git URL**
   Users can install via Package Manager:
   - Add package from git URL: `https://github.com/yourusername/UIBaseFramework.git`
   - Or specific version: `https://github.com/yourusername/UIBaseFramework.git#v1.0.0`

### Method 2: Local Package

1. **Zip the Package**
   - Create a zip file containing all package files
   - Exclude Unity project files (Assets/, Library/, etc.)

2. **Distribution**
   - Share the zip file
   - Users extract and use "Add package from disk"

### Method 3: Unity Asset Store

1. **Prepare Asset Store Package**
   - Follow Unity Asset Store guidelines
   - Include documentation and examples
   - Test thoroughly

2. **Submit to Asset Store**
   - Create Asset Store publisher account
   - Upload package following their process

### Method 4: Custom Package Registry

1. **Setup NPM-compatible Registry**
   - Use services like Verdaccio or npm
   - Configure package.json for registry

2. **Publish to Registry**
   ```bash
   npm publish
   ```

## Version Management

### Semantic Versioning
Follow semantic versioning (semver):
- **MAJOR**: Breaking changes
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes (backward compatible)

### Updating Versions
1. Update `version` in `package.json`
2. Update `CHANGELOG.md`
3. Create git tag
4. Push changes and tags

### Example Version Updates
```json
{
  "version": "1.0.0"  // Initial release
  "version": "1.0.1"  // Bug fix
  "version": "1.1.0"  // New feature
  "version": "2.0.0"  // Breaking change
}
```

## Testing Before Release

### Local Testing
1. **Test in Clean Project**
   - Create new Unity project
   - Install package via local path
   - Test all functionality

2. **Test Samples**
   - Import all samples
   - Verify they work correctly
   - Check for missing dependencies

### Git Testing
1. **Test Git Installation**
   - Push to repository
   - Install in clean project via git URL
   - Verify everything works

## Documentation Requirements

### Essential Files
- ✅ `package.json` - Package manifest
- ✅ `README.md` - Usage documentation
- ✅ `CHANGELOG.md` - Version history
- ✅ `LICENSE` - License file

### Optional Files
- ✅ `PUBLISHING.md` - This file
- ✅ `.gitignore` - Git ignore rules
- ✅ Sample READMEs

## Best Practices

### Package Quality
1. **Assembly Definitions**
   - Use proper assembly definitions
   - Separate Runtime and Editor assemblies
   - Minimize dependencies

2. **Namespace Organization**
   - Use consistent namespace structure
   - Avoid conflicts with other packages

3. **Documentation**
   - Comprehensive README
   - Code comments
   - Sample projects

### Git Repository
1. **Clean History**
   - Meaningful commit messages
   - Proper branching strategy
   - Tagged releases

2. **File Organization**
   - Exclude Unity project files
   - Include only package files
   - Proper .gitignore

## Troubleshooting

### Common Issues
1. **Missing Meta Files**
   - Ensure all files have .meta files
   - Use Unity to generate them

2. **Assembly Definition Issues**
   - Check assembly references
   - Verify platform settings

3. **Sample Import Issues**
   - Check Samples~ folder structure
   - Verify sample manifests

### Support
- Create GitHub Issues for bug reports
- Provide clear reproduction steps
- Maintain active support

## Release Checklist

Before releasing a new version:

- [ ] Update version in package.json
- [ ] Update CHANGELOG.md
- [ ] Test in clean Unity project
- [ ] Test all samples
- [ ] Update documentation if needed
- [ ] Create git tag
- [ ] Push to repository
- [ ] Create GitHub release
- [ ] Test installation via git URL

This ensures a smooth release process and good user experience.
