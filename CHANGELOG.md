# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.2.1] - 2024-01-20

### Added
- CommonTools dependency properly declared in package.json

### Fixed
- Added missing CommonTools dependency for SingletonMonoBehaviour
- Updated documentation to reflect all dependencies

## [1.2.0] - 2024-01-20

### Added
- UIResourceManager for customizable UI resources and icons
- Collection animation components (Gold, Diamond, Money) in Runtime
- Customizable icons and sprites for all UI elements
- DOTween integration with conditional compilation support
- Tip component for tooltips and notifications
- Notice component for scrolling notifications
- AddTextAnimation for reward text effects
- CommonTools dependency for SingletonMonoBehaviour and utilities

### Updated
- Moved animation components from Samples to Runtime (they are features, not examples)
- Added conditional compilation for DOTween dependency
- Enhanced collection animations with customizable icons
- Improved resource management system
- Added CommonTools as a required dependency

### Fixed
- Added fallback animations for when DOTween is not available
- Better separation between core features and optional dependencies
- Proper dependency management for external packages

## [1.1.0] - 2024-01-20

### Added
- MessagePopup component for common dialog scenarios
- Enhanced animation examples with collection animations
- Notice component for scrolling notifications (in samples)
- Additional animation scripts for rewards and collections
- More comprehensive sample content

### Updated
- Improved BasePage, Page, and Popup implementations
- Enhanced UIManager with better popup support
- Updated samples with more realistic examples

### Fixed
- Removed external dependencies from core runtime
- Moved animation-dependent scripts to samples

## [1.0.0] - 2024-01-20

### Added
- Initial release of UI Framework
- BasePage base class for all UI elements
- Page class for full-screen pages with mutual exclusion
- Popup class for overlay popups with modal support
- UIManager singleton for centralized page management
- Manual page registration system (no dynamic discovery)
- Type-safe page management with generics
- Support for unlimited parameters of any type
- Built-in animation system with customizable transitions
- Background mask support for modal popups
- Editor validation tools for setup verification
- Comprehensive documentation and examples

### Features
- **Hierarchical Architecture**: Clean separation between pages and popups
- **Manual Registration**: Pages must be manually assigned in inspector
- **Type Safety**: Generic methods ensure compile-time type checking
- **Flexible Parameters**: Pass any number of parameters of any type
- **Animation Support**: Smooth transitions with customizable duration
- **Modal Popups**: Background masks with click-to-close functionality
- **Editor Tools**: Validation and setup assistance in inspector

### Technical Details
- Minimum Unity version: 2022.3
- Dependencies: UI Toolkit, TextMeshPro
- No external dependencies required
- Full source code included
- MIT License
