# Complete Setup Guide - In Quarantine AR

## 📋 Table of Contents
- [Enhanced Features Setup](#enhanced-features-setup)
- [Script Configuration](#script-configuration)
- [Asset Requirements](#asset-requirements)
- [Performance Optimization](#performance-optimization)
- [Troubleshooting](#troubleshooting)
- [Advanced Customization](#advanced-customization)

## 🚀 Enhanced Features Setup

### SimpleRaycast.cs Configuration

The enhanced `SimpleRaycast.cs` script includes many new features that need proper setup:

#### Required Components
1. **ARRaycastManager** - Auto-detects if not assigned
2. **AR Camera** - Auto-detects Camera.main if not assigned
3. **AudioSource** - Auto-creates if not found

#### Inspector Settings

```csharp
[Header("AR Components")]
public ARRaycastManager raycastManager;  // Drag AR Session Origin here
public Camera arCamera;                  // Drag AR Camera here

[Header("Prefab Settings")]
public GameObject objectToPlacePrefab;           // Your Kirby prefab
public GameObject placementIndicatorPrefab;     // Optional: visual indicator

[Header("Audio Settings")]
public AudioSource audioSource;        // Auto-assigned or created
public AudioClip placementSound;       // Sound when placing Kirby
public AudioClip moveSound;           // Sound when moving Kirby

[Header("Visual Effects")]
public ParticleSystem placementEffect; // Effect when placing
public bool showPlacementIndicator = true;

[Header("Interaction Settings")]
public float doubleClickTime = 0.3f;   // Time window for double-click
```

### VirtualCompanion.cs Setup

The `VirtualCompanion.cs` script should be attached to your Kirby prefab:

#### Required Setup
1. **Attach to Kirby Prefab**: Add this script to your main Kirby GameObject
2. **Add Collider**: Ensure Kirby has a collider for touch detection
3. **Setup Animator**: Configure animation triggers (see below)

#### Inspector Configuration

```csharp
[Header("Companion Settings")]
public string companionName = "Kirby";
public float interactionRadius = 2f;
public LayerMask interactionLayer = -1;

[Header("Animation Settings")]
public Animator companionAnimator;      // Kirby's animator
public float idleAnimationInterval = 5f;
public float greetingDuration = 3f;

[Header("Audio Responses")]
public AudioSource audioSource;
public AudioClip[] greetingSounds;     // Array of greeting sounds
public AudioClip[] idleSounds;         // Array of idle sounds
public AudioClip[] happySounds;        // Array of happy sounds
public AudioClip[] interactionSounds;  // Array of interaction sounds

[Header("Visual Effects")]
public ParticleSystem happinessEffect; // Effect when very happy
public ParticleSystem magicSparkles;   // Continuous sparkles when happy
public Light companionLight;           // Light that changes color with mood
public Gradient moodColors;            // Gradient for mood colors

[Header("Quarantine Support Features")]
public bool enableReminders = true;
public float reminderInterval = 1800f; // 30 minutes
public string[] encouragingMessages;    // Array of supportive messages
```

## 🎨 Asset Requirements

### Animation Controller Setup

Create an Animator Controller for Kirby with these trigger parameters:

```
Triggers:
- Idle
- Happy
- Greet
- Bored
- VeryHappy

States:
- Idle (default)
- Happy_Animation
- Greeting_Animation
- Bored_Animation
- VeryHappy_Animation
```

### Audio Assets Organization

Create folders in your project:

```
Assets/
└── Audio/
    ├── Greetings/
    │   ├── hello.wav
    │   ├── hi_there.wav
    │   └── welcome.wav
    ├── Happy/
    │   ├── joy1.wav
    │   ├── joy2.wav
    │   └── laugh.wav
    ├── Interactions/
    │   ├── touch1.wav
    │   ├── touch2.wav
    │   └── pet.wav
    ├── Idle/
    │   ├── ambient1.wav
    │   └── ambient2.wav
    └── Placement/
        ├── place.wav
        └── move.wav
```

### Visual Effects Setup

#### Particle Systems
1. **Happiness Effect**: Burst of colorful particles
   - Shape: Cone or Circle
   - Emission: Burst (10-20 particles)
   - Duration: 1-2 seconds

2. **Magic Sparkles**: Continuous gentle sparkles
   - Shape: Box around Kirby
   - Emission: Rate over time (5-10/sec)
   - Looping: Yes

#### Lighting Setup
1. **Companion Light**: Point Light as child of Kirby
   - Range: 2-3 units
   - Intensity: 0.5-1.0 (varies with mood)
   - Color: Changes with mood gradient

### Mood Color Gradient

Set up a gradient for mood representation:
- **0.0 (Sad)**: Blue (#4A90E2)
- **0.3 (Neutral)**: White (#FFFFFF)
- **0.7 (Happy)**: Yellow (#F5A623)
- **1.0 (Very Happy)**: Pink (#FF69B4)

## ⚡ Performance Optimization

### Frame Rate Optimization
```csharp
// In Update(), consider limiting update frequency for non-critical systems
private float lastMoodUpdate = 0f;
private const float moodUpdateInterval = 0.1f; // Update mood 10 times per second

void Update()
{
    if (Time.time - lastMoodUpdate > moodUpdateInterval)
    {
        UpdateMood();
        lastMoodUpdate = Time.time;
    }
}
```

### Memory Management
- Use object pooling for particle effects if creating many instances
- Limit audio clip quality for mobile devices
- Optimize texture sizes for mobile platforms

### AR Performance Tips
1. **Plane Detection**: Disable plane detection after successful placement
2. **Camera Resolution**: Lower camera resolution on older devices
3. **Update Frequency**: Reduce Update() calls where possible

## 🔧 Troubleshooting

### Common Issues

#### 1. AR Camera Not Working
**Problem**: Camera shows black screen or no tracking
**Solutions**:
- Ensure AR Foundation packages are installed
- Check platform-specific XR settings
- Verify camera permissions on device
- Test in well-lit environment

#### 2. Touch Detection Not Working
**Problem**: Can't interact with Kirby
**Solutions**:
```csharp
// Ensure Kirby has a collider
if (GetComponent<Collider>() == null)
{
    gameObject.AddComponent<BoxCollider>();
}

// Check layer mask settings
public LayerMask interactionLayer = -1; // Should be -1 for all layers
```

#### 3. Audio Not Playing
**Problem**: No sound effects
**Solutions**:
- Check AudioSource is not muted
- Verify audio clips are assigned
- Check device volume settings
- Ensure AudioListener is in scene

#### 4. Animations Not Triggering
**Problem**: Kirby doesn't animate
**Solutions**:
```csharp
// Verify animator is assigned and has controller
if (companionAnimator == null)
{
    companionAnimator = GetComponent<Animator>();
}

// Check if parameters exist
if (companionAnimator.parameters.Length == 0)
{
    Debug.LogError("Animator Controller has no parameters!");
}
```

### Debug Console Messages

Enable debug logging to track issues:
```csharp
// Add to SimpleRaycast.cs
#if UNITY_EDITOR
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"Placed: {IsObjectPlaced()}");
        GUI.Label(new Rect(10, 30, 300, 20), $"Touch Supported: {Input.touchSupported}");
    }
#endif
```

## 🎛️ Advanced Customization

### Custom Encouraging Messages

Customize messages for different scenarios:
```csharp
private string[] morningMessages = {
    "Good morning! Ready to start a new day together?",
    "The sun is shining! Let's make today count!",
    "Rise and shine! I'm here to brighten your day!"
};

private string[] eveningMessages = {
    "What a day! Time to relax and unwind.",
    "The stars are coming out. You've done great today!",
    "Evening time - let's reflect on the good moments."
};
```

### Dynamic Mood System

Implement time-based mood changes:
```csharp
private void UpdateMoodBasedOnTime()
{
    int hour = System.DateTime.Now.Hour;
    
    if (hour >= 6 && hour < 12) // Morning
    {
        IncreaseMood(0.01f); // Slightly happier in morning
    }
    else if (hour >= 18 && hour < 22) // Evening
    {
        IncreaseMood(0.005f); // Calm evening mood
    }
    else if (hour >= 22 || hour < 6) // Night
    {
        currentMood = Mathf.Max(0.3f, currentMood - 0.01f); // Sleepy
    }
}
```

### Gesture Recognition

Add gesture-based interactions:
```csharp
private Vector2 swipeStartPos;
private bool isSwipeDetection = false;

private void DetectSwipeGestures()
{
    if (Input.touchCount == 1)
    {
        Touch touch = Input.GetTouch(0);
        
        if (touch.phase == TouchPhase.Began)
        {
            swipeStartPos = touch.position;
            isSwipeDetection = true;
        }
        else if (touch.phase == TouchPhase.Ended && isSwipeDetection)
        {
            Vector2 swipeDirection = touch.position - swipeStartPos;
            
            if (swipeDirection.magnitude > 100f) // Minimum swipe distance
            {
                HandleSwipeGesture(swipeDirection.normalized);
            }
            
            isSwipeDetection = false;
        }
    }
}
```

### Weather Integration

Make Kirby respond to weather (requires weather API):
```csharp
public void UpdateMoodBasedOnWeather(string weatherCondition)
{
    switch (weatherCondition.ToLower())
    {
        case "sunny":
            IncreaseMood(0.2f);
            ShowUIMessage("What a beautiful sunny day!");
            break;
        case "rainy":
            IncreaseMood(-0.1f);
            ShowUIMessage("A bit gloomy today, but I'm here with you!");
            break;
        case "cloudy":
            // Neutral effect
            ShowUIMessage("Cloudy skies, but our spirits are bright!");
            break;
    }
}
```

## 📱 Platform-Specific Setup

### iOS Specific
1. **Xcode Project Settings**:
   - Enable Camera usage description
   - Set minimum iOS version to 12.0
   - Include ARKit framework

2. **Info.plist additions**:
```xml
<key>NSCameraUsageDescription</key>
<string>This app uses the camera for AR features to place your virtual companion.</string>
```

### Android Specific
1. **Manifest permissions**:
```xml
<uses-permission android:name="android.permission.CAMERA" />
<uses-feature android:name="android.hardware.camera.ar" android:required="true"/>
```

2. **Gradle settings**:
   - Minimum API level: 24 (Android 7.0)
   - ARCore required

## 📊 Analytics & Metrics

Track user engagement (optional):
```csharp
public class CompanionAnalytics
{
    public static int totalInteractions = 0;
    public static float totalPlayTime = 0f;
    public static float averageMood = 0f;
    
    public static void LogInteraction()
    {
        totalInteractions++;
        // Send to your analytics service
    }
    
    public static void UpdatePlayTime()
    {
        totalPlayTime += Time.deltaTime;
    }
}
```

---

## 🎯 Final Notes

This enhanced version of "In Quarantine" provides a much more engaging and emotionally supportive experience. The improved code structure makes it easier to:

- Add new interactive features
- Customize the companion's behavior
- Optimize performance
- Debug issues
- Extend functionality

Remember that the core purpose is to provide companionship and emotional support during difficult times. The technical improvements serve this emotional goal by creating a more responsive, engaging, and reliable virtual companion.

For additional help or advanced customization needs, consider:
- Unity's AR Foundation documentation
- Community forums for AR development
- Mental health resources for quarantine support

**Happy developing, and may Kirby bring joy to those who need it most! 💖**