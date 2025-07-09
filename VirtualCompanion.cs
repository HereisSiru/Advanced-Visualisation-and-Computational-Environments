using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Virtual Companion behavior system for enhanced interaction and emotional support
/// Perfect for quarantine companionship with Kirby
/// </summary>
public class VirtualCompanion : MonoBehaviour
{
    [Header("Companion Settings")]
    [SerializeField] private string companionName = "Kirby";
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask interactionLayer = -1;
    
    [Header("Animation Settings")]
    [SerializeField] private Animator companionAnimator;
    [SerializeField] private float idleAnimationInterval = 5f;
    [SerializeField] private float greetingDuration = 3f;
    
    [Header("Audio Responses")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] greetingSounds;
    [SerializeField] private AudioClip[] idleSounds;
    [SerializeField] private AudioClip[] happySounds;
    [SerializeField] private AudioClip[] interactionSounds;
    
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem happinessEffect;
    [SerializeField] private ParticleSystem magicSparkles;
    [SerializeField] private Light companionLight;
    [SerializeField] private Gradient moodColors;
    
    [Header("Interaction Settings")]
    [SerializeField] private float touchCooldown = 1f;
    [SerializeField] private int touchesForHappiness = 3;
    [SerializeField] private float moodDecayRate = 0.1f;
    
    [Header("Quarantine Support Features")]
    [SerializeField] private bool enableReminders = true;
    [SerializeField] private float reminderInterval = 1800f; // 30 minutes
    [SerializeField] private string[] encouragingMessages = {
        "You're doing great! This quarantine will be over soon!",
        "Remember to stay hydrated and take breaks!",
        "I'm here with you - you're not alone!",
        "Let's make the best of this time together!",
        "Every day brings us closer to freedom!"
    };
    
    // Private variables
    private float currentMood = 0.5f; // 0 = sad, 1 = very happy
    private float lastInteractionTime;
    private float lastIdleAnimation;
    private float lastReminder;
    private int consecutiveTouches = 0;
    private bool isGreeting = false;
    private bool isInitialized = false;
    
    // Animation hashes for performance
    private int idleHash;
    private int happyHash;
    private int greetHash;
    private int boredHash;
    
    void Start()
    {
        InitializeCompanion();
    }
    
    void Update()
    {
        if (!isInitialized) return;
        
        UpdateMood();
        CheckForIdleAnimations();
        CheckForReminders();
        UpdateVisualEffects();
        HandleUserInput();
    }
    
    /// <summary>
    /// Initialize all companion systems and components
    /// </summary>
    private void InitializeCompanion()
    {
        // Auto-assign components if needed
        if (companionAnimator == null)
            companionAnimator = GetComponent<Animator>();
            
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        if (companionLight == null)
            companionLight = GetComponentInChildren<Light>();
        
        // Initialize animation hashes
        if (companionAnimator != null)
        {
            idleHash = Animator.StringToHash("Idle");
            happyHash = Animator.StringToHash("Happy");
            greetHash = Animator.StringToHash("Greet");
            boredHash = Animator.StringToHash("Bored");
        }
        
        // Start with a greeting
        StartCoroutine(GreetUser());
        
        // Initialize timestamps
        lastInteractionTime = Time.time;
        lastIdleAnimation = Time.time;
        lastReminder = Time.time;
        
        isInitialized = true;
        
        Debug.Log($"{companionName} is ready to keep you company during quarantine!");
    }
    
    /// <summary>
    /// Handle user input for interaction
    /// </summary>
    private void HandleUserInput()
    {
        // Check for touch/click on companion
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 inputPosition = Input.mousePosition;
            if (Input.touchCount > 0)
                inputPosition = Input.GetTouch(0).position;
            
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactionLayer))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    OnCompanionTouched();
                }
            }
        }
    }
    
    /// <summary>
    /// Handle companion being touched/clicked
    /// </summary>
    private void OnCompanionTouched()
    {
        if (Time.time - lastInteractionTime < touchCooldown)
            return;
        
        lastInteractionTime = Time.time;
        consecutiveTouches++;
        
        // Increase mood
        IncreaseMood(0.2f);
        
        // Play interaction sound
        PlayRandomSound(interactionSounds);
        
        // Trigger happy animation
        TriggerAnimation("Happy");
        
        // Check for happiness threshold
        if (consecutiveTouches >= touchesForHappiness)
        {
            TriggerHappinessResponse();
            consecutiveTouches = 0;
        }
        
        Debug.Log($"{companionName} enjoyed the interaction! Current mood: {currentMood:F2}");
    }
    
    /// <summary>
    /// Trigger special happiness response
    /// </summary>
    private void TriggerHappinessResponse()
    {
        // Play happiness effect
        if (happinessEffect != null)
            happinessEffect.Play();
        
        // Play happy sound
        PlayRandomSound(happySounds);
        
        // Trigger special animation
        TriggerAnimation("VeryHappy");
        
        // Add extra mood boost
        IncreaseMood(0.3f);
        
        Debug.Log($"{companionName} is very happy! You're both doing great!");
    }
    
    /// <summary>
    /// Update companion's mood over time
    /// </summary>
    private void UpdateMood()
    {
        // Decay mood slowly over time (loneliness factor)
        if (Time.time - lastInteractionTime > 60f) // No interaction for 1 minute
        {
            currentMood = Mathf.Max(0f, currentMood - moodDecayRate * Time.deltaTime);
        }
        
        // Clamp mood between 0 and 1
        currentMood = Mathf.Clamp01(currentMood);
    }
    
    /// <summary>
    /// Increase companion's mood
    /// </summary>
    private void IncreaseMood(float amount)
    {
        currentMood = Mathf.Clamp01(currentMood + amount);
    }
    
    /// <summary>
    /// Check and play idle animations
    /// </summary>
    private void CheckForIdleAnimations()
    {
        if (Time.time - lastIdleAnimation > idleAnimationInterval)
        {
            PlayIdleAnimation();
            lastIdleAnimation = Time.time;
        }
    }
    
    /// <summary>
    /// Play appropriate idle animation based on mood
    /// </summary>
    private void PlayIdleAnimation()
    {
        if (currentMood > 0.7f)
        {
            TriggerAnimation("Happy");
            PlayRandomSound(happySounds);
        }
        else if (currentMood < 0.3f)
        {
            TriggerAnimation("Bored");
            PlayRandomSound(idleSounds);
        }
        else
        {
            TriggerAnimation("Idle");
        }
    }
    
    /// <summary>
    /// Check for reminder times and show encouraging messages
    /// </summary>
    private void CheckForReminders()
    {
        if (!enableReminders) return;
        
        if (Time.time - lastReminder > reminderInterval)
        {
            ShowEncouragingMessage();
            lastReminder = Time.time;
        }
    }
    
    /// <summary>
    /// Show encouraging message for quarantine support
    /// </summary>
    private void ShowEncouragingMessage()
    {
        if (encouragingMessages.Length == 0) return;
        
        string message = encouragingMessages[Random.Range(0, encouragingMessages.Length)];
        
        // Play attention sound
        PlayRandomSound(greetingSounds);
        
        // Trigger greeting animation
        TriggerAnimation("Greet");
        
        // Show UI message (would need UI system)
        Debug.Log($"{companionName} says: {message}");
        
        // Could trigger UI popup here in a real implementation
        ShowUIMessage(message);
    }
    
    /// <summary>
    /// Update visual effects based on mood and state
    /// </summary>
    private void UpdateVisualEffects()
    {
        // Update companion light color based on mood
        if (companionLight != null)
        {
            Color moodColor = moodColors.Evaluate(currentMood);
            companionLight.color = moodColor;
            companionLight.intensity = 0.5f + (currentMood * 0.5f);
        }
        
        // Play sparkles when very happy
        if (magicSparkles != null && currentMood > 0.8f && !magicSparkles.isPlaying)
        {
            magicSparkles.Play();
        }
        else if (magicSparkles != null && currentMood <= 0.8f && magicSparkles.isPlaying)
        {
            magicSparkles.Stop();
        }
    }
    
    /// <summary>
    /// Trigger animation safely
    /// </summary>
    private void TriggerAnimation(string animationName)
    {
        if (companionAnimator == null) return;
        
        companionAnimator.SetTrigger(animationName);
    }
    
    /// <summary>
    /// Play random sound from array
    /// </summary>
    private void PlayRandomSound(AudioClip[] sounds)
    {
        if (audioSource == null || sounds == null || sounds.Length == 0) return;
        
        AudioClip randomClip = sounds[Random.Range(0, sounds.Length)];
        if (randomClip != null)
        {
            audioSource.PlayOneShot(randomClip);
        }
    }
    
    /// <summary>
    /// Initial greeting coroutine
    /// </summary>
    private IEnumerator GreetUser()
    {
        isGreeting = true;
        
        yield return new WaitForSeconds(1f); // Wait a moment after placement
        
        // Play greeting animation and sound
        TriggerAnimation("Greet");
        PlayRandomSound(greetingSounds);
        
        // Welcome message
        Debug.Log($"Hello! I'm {companionName}, your virtual companion during quarantine!");
        ShowUIMessage($"Hello! I'm {companionName}, here to keep you company! ♡");
        
        yield return new WaitForSeconds(greetingDuration);
        
        isGreeting = false;
    }
    
    /// <summary>
    /// Show UI message (placeholder for actual UI implementation)
    /// </summary>
    private void ShowUIMessage(string message)
    {
        // This would integrate with your UI system
        // For now, we'll just log it
        Debug.Log($"[UI Message] {message}");
        
        // In a real implementation, you might:
        // - Show a speech bubble
        // - Display text in a UI panel
        // - Show a notification
    }
    
    // Public methods for external interaction
    
    /// <summary>
    /// Manually trigger interaction (can be called by other scripts)
    /// </summary>
    public void InteractWithCompanion()
    {
        OnCompanionTouched();
    }
    
    /// <summary>
    /// Get current mood value
    /// </summary>
    public float GetMood()
    {
        return currentMood;
    }
    
    /// <summary>
    /// Set companion mood directly
    /// </summary>
    public void SetMood(float mood)
    {
        currentMood = Mathf.Clamp01(mood);
    }
    
    /// <summary>
    /// Manually trigger encouraging message
    /// </summary>
    public void ShowEncouragement()
    {
        ShowEncouragingMessage();
    }
    
    /// <summary>
    /// Check if companion is currently greeting
    /// </summary>
    public bool IsGreeting()
    {
        return isGreeting;
    }
}