# In Quarantine - AR Virtual Companion

[![Unity Version](https://img.shields.io/badge/Unity-2021.3%2B-blue)](https://unity3d.com/)
[![AR Foundation](https://img.shields.io/badge/AR%20Foundation-4.2%2B-green)](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.2/manual/index.html)
[![Platform](https://img.shields.io/badge/Platform-iOS%20%7C%20Android-lightgrey)](https://unity3d.com/)

**An AR virtual companion app featuring Kirby (星のカービィ) designed to provide emotional support and companionship during quarantine periods.**

<div align="center">

| Demo Video | Development Process |
|------------|-------------------|
| [![Game Demo](https://img.youtube.com/vi/XpMvlw15VoQ/0.jpg)](https://youtu.be/XpMvlw15VoQ) | [![Iterative Design](https://img.youtube.com/vi/Cz1tZ5khXWk/0.jpg)](https://youtu.be/Cz1tZ5khXWk) |

</div>

## 🌟 Overview

"In Quarantine" is an AR application that brings a virtual companion into your real environment during challenging isolation periods. Featuring the beloved character Kirby, this app provides emotional support, interactive entertainment, and encouraging reminders to help make quarantine time more bearable.

### 🎯 Purpose

This project was created during the COVID-19 pandemic when the developer experienced quarantine isolation in Shanghai. The app aims to:
- Provide companionship during isolation periods
- Offer emotional support through interactive features
- Create positive distractions during challenging times
- Demonstrate practical applications of AR technology for mental health

## ✨ Features

### 🎮 Core AR Features
- **Plane Detection**: Automatic detection of horizontal and vertical surfaces
- **Touch/Tap Placement**: Place Kirby in your environment with a simple tap
- **Smooth Movement**: Relocate your companion with fluid animations
- **Placement Indicator**: Visual feedback showing where Kirby will appear
- **Double-tap Reset**: Easily reset placement to try new locations

### 🤖 Interactive Companion System
- **Mood System**: Kirby's emotional state changes based on interaction
- **Touch Interactions**: Tap Kirby to boost happiness and trigger animations
- **Audio Feedback**: Varied sound responses for different interactions
- **Visual Effects**: Particle effects and lighting that reflect Kirby's mood
- **Encouraging Messages**: Periodic reminders and motivational quotes

### 💫 Enhanced Experience
- **Mobile & Desktop Support**: Works with both touch and mouse input
- **Visual Feedback**: Dynamic lighting and particle effects
- **Audio System**: Comprehensive sound design for immersive experience
- **Performance Optimized**: Efficient code structure for smooth performance

## 🔧 Technical Requirements

### Minimum Requirements
- **Unity**: 2021.3 LTS or later
- **AR Foundation**: 4.2.0 or later
- **iOS**: iOS 12+ with A12 chip (iPhone XS/XR or later)
- **Android**: Android 7.0+ with ARCore support

### Recommended Setup
- Modern smartphone with good camera quality
- Well-lit environment for better plane detection
- Stable surface for initial placement

## 📱 Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/in-quarantine-ar.git
cd in-quarantine-ar
```

### 2. Unity Setup
1. Open Unity Hub
2. Click "Open" and select the project folder
3. Ensure you have Unity 2021.3 LTS installed
4. Wait for Unity to import all assets

### 3. Install Required Packages
The project uses these Unity packages:
- AR Foundation (4.2.0+)
- ARCore XR Plugin (Android)
- ARKit XR Plugin (iOS)
- XR Plugin Management

**Auto-installation**: These should install automatically when opening the project.

**Manual installation** (if needed):
1. Window → Package Manager
2. Select "Unity Registry"
3. Search for and install each package listed above

### 4. Platform Configuration

#### For iOS:
1. File → Build Settings → iOS
2. Player Settings → XR Plug-in Management → ARKit
3. Set up iOS build settings (Team ID, Bundle Identifier, etc.)

#### For Android:
1. File → Build Settings → Android
2. Player Settings → XR Plug-in Management → ARCore
3. Ensure "ARCore Supported" is checked in XR Settings

### 5. Scene Setup
1. Open the `InQuarantine.unity` scene
2. Ensure AR Camera and AR Session are present
3. Add the enhanced scripts to appropriate GameObjects:
   - `SimpleRaycast.cs` → AR Camera or Scene Manager
   - `VirtualCompanion.cs` → Kirby prefab

## 🎮 How to Use

### Basic Usage
1. **Launch the app** on your AR-capable device
2. **Point your camera** at a flat surface (table, floor, etc.)
3. **Wait for plane detection** - you'll see a placement indicator
4. **Tap the screen** where you want Kirby to appear
5. **Interact with Kirby** by tapping on the character

### Advanced Interactions
- **Single Tap**: Place or move Kirby
- **Tap Kirby**: Increase happiness and trigger animations
- **Double Tap**: Reset placement (when Kirby is placed)
- **Multiple Taps**: Build up happiness for special effects

### Mood System
- Kirby's mood affects visual effects and behaviors
- Regular interaction keeps Kirby happy
- Neglecting Kirby will slowly decrease mood
- Happy Kirby shows sparkles and bright colors

## 📁 Project Structure

```
In-Quarantine/
├── Assets/
│   ├── Scripts/
│   │   ├── SimpleRaycast.cs          # Enhanced AR placement system
│   │   └── VirtualCompanion.cs       # Interactive companion behaviors
│   ├── Scenes/
│   │   └── InQuarantine.unity        # Main AR scene
│   ├── Prefabs/
│   │   └── Kirby/                    # Kirby character assets
│   └── Audio/                        # Sound effects and music
├── Unity-iPhone.xcodeproj.zip        # iOS build project
└── README.md                         # This file
```

## 🛠️ Development

### Key Scripts

#### `SimpleRaycast.cs`
Enhanced AR placement system with:
- Cross-platform input handling
- Visual placement indicators
- Smooth object movement
- Audio feedback integration
- Robust error handling

#### `VirtualCompanion.cs`
Comprehensive companion behavior system:
- Mood and emotion simulation
- Interactive touch responses
- Audio-visual feedback
- Encouraging message system
- Animation state management

### Customization Options

#### Companion Settings
```csharp
[SerializeField] private string companionName = "Kirby";
[SerializeField] private float interactionRadius = 2f;
[SerializeField] private float idleAnimationInterval = 5f;
```

#### Quarantine Support Features
```csharp
[SerializeField] private bool enableReminders = true;
[SerializeField] private float reminderInterval = 1800f; // 30 minutes
[SerializeField] private string[] encouragingMessages = { /* ... */ };
```

## 🎨 Assets & Resources

### Audio Requirements
- Greeting sounds for initial interaction
- Happiness/joy sounds for positive interactions
- Idle sounds for background ambiance
- Interaction feedback sounds

### Visual Effects
- Particle systems for happiness indicators
- Dynamic lighting for mood representation
- Placement indicator prefab
- Animation controller for Kirby

### 3D Model Setup
The Kirby model should include:
- Animator component with trigger parameters:
  - `Idle`, `Happy`, `Greet`, `Bored`, `VeryHappy`
- Collider for touch detection
- Audio source for companion sounds

## 🤝 Contributing

### How to Contribute
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Follow Unity coding conventions
- Add XML documentation for public methods
- Test on both iOS and Android devices
- Ensure backward compatibility

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Nintendo** - For creating the beloved Kirby character
- **Unity Technologies** - For AR Foundation framework
- **The Quarantine Experience** - For inspiring this meaningful project
- **Mental Health Awareness** - For highlighting the importance of companionship during isolation

## 📞 Support & Contact

- **Issues**: Please report bugs via GitHub Issues
- **Questions**: Start a GitHub Discussion
- **Updates**: Watch this repository for latest improvements

---

<div align="center">

**Made with ❤️ during challenging times to bring joy and companionship to those in isolation**

*"I'm here with you - you're not alone!"* - Kirby

</div>

