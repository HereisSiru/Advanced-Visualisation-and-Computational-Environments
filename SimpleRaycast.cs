using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Enhanced AR object placement system with touch support, visual feedback, and audio cues
/// for the "In Quarantine" virtual companion experience
/// </summary>
public class SimpleRaycast : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private Camera arCamera;
    
    [Header("Prefab Settings")]
    [SerializeField] private GameObject objectToPlacePrefab;
    [SerializeField] private GameObject placementIndicatorPrefab; // Visual indicator for placement
    
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip placementSound;
    [SerializeField] private AudioClip moveSound;
    
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem placementEffect;
    [SerializeField] private bool showPlacementIndicator = true;
    
    [Header("Interaction Settings")]
    [SerializeField] private float doubleClickTime = 0.3f;
    [SerializeField] private LayerMask interactionLayer = -1;
    
    // Private variables
    private GameObject placedObject;
    private GameObject placementIndicator;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private bool isObjectPlaced = false;
    private float lastClickTime = 0f;
    private Vector2 lastTouchPosition;
    
    // Touch handling for mobile devices
    private bool isTouchSupported;
    
    void Start()
    {
        InitializeComponents();
        isTouchSupported = Input.touchSupported;
    }
    
    void Update()
    {
        HandleInput();
        UpdatePlacementIndicator();
    }
    
    /// <summary>
    /// Initialize and validate all required components
    /// </summary>
    private void InitializeComponents()
    {
        // Auto-assign components if not set
        if (raycastManager == null)
            raycastManager = FindObjectOfType<ARRaycastManager>();
            
        if (arCamera == null)
            arCamera = Camera.main ?? FindObjectOfType<Camera>();
            
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        
        // Create placement indicator if prefab is provided
        if (placementIndicatorPrefab != null && showPlacementIndicator)
        {
            placementIndicator = Instantiate(placementIndicatorPrefab);
            placementIndicator.SetActive(false);
        }
        
        // Validate required components
        if (raycastManager == null)
            Debug.LogError("ARRaycastManager not found! Please add one to the scene.");
            
        if (arCamera == null)
            Debug.LogError("AR Camera not found! Please assign one in the inspector.");
            
        if (objectToPlacePrefab == null)
            Debug.LogWarning("Object prefab not assigned! Please assign a prefab to place.");
    }
    
    /// <summary>
    /// Handle both mouse and touch input for object placement
    /// </summary>
    private void HandleInput()
    {
        Vector2 screenPosition = Vector2.zero;
        bool inputDetected = false;
        
        // Handle touch input (mobile)
        if (isTouchSupported && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            screenPosition = touch.position;
            
            if (touch.phase == TouchPhase.Began)
            {
                inputDetected = true;
                lastTouchPosition = screenPosition;
            }
        }
        // Handle mouse input (editor/desktop)
        else if (Input.GetMouseButtonDown(0))
        {
            inputDetected = true;
            screenPosition = Input.mousePosition;
        }
        
        if (inputDetected)
        {
            HandleRaycastPlacement(screenPosition);
            HandleDoubleClick();
        }
    }
    
    /// <summary>
    /// Perform raycast and handle object placement or movement
    /// </summary>
    private void HandleRaycastPlacement(Vector2 screenPosition)
    {
        if (raycastManager == null || arCamera == null || objectToPlacePrefab == null)
            return;
        
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        
        if (raycastManager.Raycast(ray, raycastHits, TrackableType.Planes))
        {
            Pose hitPose = raycastHits[0].pose;
            
            if (!isObjectPlaced)
            {
                PlaceNewObject(hitPose);
            }
            else
            {
                MoveExistingObject(hitPose);
            }
        }
    }
    
    /// <summary>
    /// Place a new object at the specified pose
    /// </summary>
    private void PlaceNewObject(Pose pose)
    {
        if (placedObject != null)
            DestroyImmediate(placedObject);
            
        placedObject = Instantiate(objectToPlacePrefab, pose.position, pose.rotation);
        isObjectPlaced = true;
        
        // Hide placement indicator
        if (placementIndicator != null)
            placementIndicator.SetActive(false);
        
        // Play audio feedback
        PlayAudioClip(placementSound);
        
        // Play visual effect
        PlayPlacementEffect(pose.position);
        
        Debug.Log($"Virtual companion placed at position: {pose.position}");
    }
    
    /// <summary>
    /// Move existing object to new position
    /// </summary>
    private void MoveExistingObject(Pose pose)
    {
        if (placedObject == null) return;
        
        // Smooth movement option
        StartCoroutine(SmoothMoveTo(placedObject.transform, pose.position, pose.rotation, 0.3f));
        
        // Play move sound
        PlayAudioClip(moveSound);
        
        Debug.Log($"Virtual companion moved to position: {pose.position}");
    }
    
    /// <summary>
    /// Smoothly move object to target position
    /// </summary>
    private IEnumerator SmoothMoveTo(Transform target, Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPosition = target.position;
        Quaternion startRotation = target.rotation;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            
            target.position = Vector3.Lerp(startPosition, targetPosition, progress);
            target.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);
            
            yield return null;
        }
        
        target.position = targetPosition;
        target.rotation = targetRotation;
    }
    
    /// <summary>
    /// Update placement indicator position and visibility
    /// </summary>
    private void UpdatePlacementIndicator()
    {
        if (placementIndicator == null || isObjectPlaced || !showPlacementIndicator)
            return;
        
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = arCamera.ScreenPointToRay(screenCenter);
        
        if (raycastManager.Raycast(ray, raycastHits, TrackableType.Planes))
        {
            Pose hitPose = raycastHits[0].pose;
            placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            placementIndicator.SetActive(true);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }
    
    /// <summary>
    /// Handle double-click to reset or interact with placed object
    /// </summary>
    private void HandleDoubleClick()
    {
        float currentTime = Time.time;
        
        if (currentTime - lastClickTime < doubleClickTime)
        {
            // Double click detected
            OnDoubleClick();
        }
        
        lastClickTime = currentTime;
    }
    
    /// <summary>
    /// Handle double-click actions (reset placement, special interactions)
    /// </summary>
    private void OnDoubleClick()
    {
        if (isObjectPlaced && placedObject != null)
        {
            // Reset placement - allow user to place object again
            DestroyImmediate(placedObject);
            placedObject = null;
            isObjectPlaced = false;
            
            // Show placement indicator again
            if (placementIndicator != null && showPlacementIndicator)
                placementIndicator.SetActive(true);
            
            Debug.Log("Virtual companion placement reset. Tap to place again!");
        }
    }
    
    /// <summary>
    /// Play audio clip with null checking
    /// </summary>
    private void PlayAudioClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    /// <summary>
    /// Play visual placement effect
    /// </summary>
    private void PlayPlacementEffect(Vector3 position)
    {
        if (placementEffect != null)
        {
            placementEffect.transform.position = position;
            placementEffect.Play();
        }
    }
    
    /// <summary>
    /// Public method to manually reset placement
    /// </summary>
    public void ResetPlacement()
    {
        if (placedObject != null)
        {
            DestroyImmediate(placedObject);
            placedObject = null;
            isObjectPlaced = false;
        }
        
        if (placementIndicator != null && showPlacementIndicator)
            placementIndicator.SetActive(true);
    }
    
    /// <summary>
    /// Get the currently placed object (if any)
    /// </summary>
    public GameObject GetPlacedObject()
    {
        return placedObject;
    }
    
    /// <summary>
    /// Check if an object is currently placed
    /// </summary>
    public bool IsObjectPlaced()
    {
        return isObjectPlaced && placedObject != null;
    }
}

