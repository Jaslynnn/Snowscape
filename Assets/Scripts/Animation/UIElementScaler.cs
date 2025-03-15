using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementScaler : MonoBehaviour
{
    [System.Serializable]
    public class KeyValuePair
    {
        public string key;        // add buttons
        public GameObject uiElement; // ui to scale
        public KeyCode glowKey;  // keycode for glow
    }

    public List<KeyValuePair> uiElementsList;
    private Dictionary<string, GameObject> uiElements = new Dictionary<string, GameObject>();
    private Dictionary<string, Vector3> originalScales = new Dictionary<string, Vector3>(); // To store the original scale

    private float enlargedScale = 1.2f; // scale factor when enlarging the object
    private float animationDuration = 0.3f; 
    private float popScaleFactor = 1.3f;  
    private float popDuration = 0.2f;   

    private TutorialUIAnim tutorialUIAnim;

    private void Start()
    {
        tutorialUIAnim = FindObjectOfType<TutorialUIAnim>();

        foreach (var pair in uiElementsList)
        {
            uiElements[pair.key] = pair.uiElement;
            originalScales[pair.key] = pair.uiElement.transform.localScale; // Store the original scale
        }
    }

    public void EnlargeUIElement(string key)
    {
        // Ensure key exists in UI elements before proceeding
        if (!uiElements.ContainsKey(key) || !originalScales.ContainsKey(key))
        {
            Debug.LogWarning($"UI element '{key}' not found, skipping enlargement.");
            return;
        }

        foreach (var elementKey in uiElements.Keys)
        {
            if (elementKey != key && uiElements.ContainsKey(elementKey) && originalScales.ContainsKey(elementKey))
            {
                // Shrink the element
                LeanTween.scale(uiElements[elementKey], originalScales[elementKey], animationDuration)
                    .setEase(LeanTweenType.easeOutBack);

                // Find glow key safely
                var glowKey = uiElementsList.Find(item => item.key == elementKey)?.glowKey;
                if (glowKey.HasValue)
                {
                    tutorialUIAnim.HideGlow(glowKey.Value);
                }

                // Avoid redundant scaling
                if (glowKey != null)
                {
                    LeanTween.scale(uiElements[elementKey], originalScales[elementKey], animationDuration)
                        .setEase(LeanTweenType.easeOutBack);
                }
            }
        }

        // Enlarge the selected element
        var selectedGlowKey = uiElementsList.Find(item => item.key == key)?.glowKey;
        if (selectedGlowKey.HasValue)
        {
            tutorialUIAnim.ShowGlow(selectedGlowKey.Value);
        }

        LeanTween.scale(uiElements[key], originalScales[key] * enlargedScale, animationDuration)
            .setEase(LeanTweenType.easeOutBack);
    }

    // New function to "pop" an element (e.g., profile or icon)
    public void PopUIElement(string key)
    {
        if (uiElements.ContainsKey(key))
        {
            // Enlarge the UI element (pop effect)
            LeanTween.scale(uiElements[key], originalScales[key] * popScaleFactor, popDuration)
                .setEase(LeanTweenType.easeOutQuad)  // Pop to larger size
                .setOnComplete(() => ShrinkBackUIElement(key)); // Shrink back after completion
        }
    }

    // Function to shrink the UI element back to its original scale after popping
    private void ShrinkBackUIElement(string key)
    {
        if (uiElements.ContainsKey(key))
        {
            LeanTween.scale(uiElements[key], originalScales[key], popDuration)
                .setEase(LeanTweenType.easeInQuad);  // Shrink back to original size
        }
    }
}
