using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // For hover functionality
public class MainMenuUIAnim : MonoBehaviour
{
    public GameObject title; // Reference to the title (for floating)
    public List<Button> buttons; // List of Button components to apply the hover and click effects to
    public float floatHeight = 10f; // Height the title will float up/down
    public float floatSpeed = 2f; // Speed of the float animation
    public float hoverScale = 1.1f; // Scale factor for hover effect on button
    public float clickScale = 0.9f; // Scale factor for click effect on button
    public float animationSpeed = 0.2f; // Speed of the hover/click animation on button
    public float initialOpacity = 0.5f; // Initial opacity (dark before hover)

    public Image blackPanel;
    public AudioManager audioManager;
    
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, CanvasGroup> buttonCanvasGroups = new Dictionary<GameObject, CanvasGroup>();

    private void Awake()
    {
        
        LeanTween.init(800);
        
        
    }
    public void FadeOutPanel(float duration = 1.5f)
    {
        blackPanel.gameObject.SetActive(true);
        LeanTween.alpha(blackPanel.rectTransform, 0f, duration).setEase(LeanTweenType.easeOutQuad);
        
    }
    public void FadeOutPanelFalse()
    {
        blackPanel.gameObject.SetActive(false);
    }
    public void FadeInPanel(float duration = 1.5f)
    {
        blackPanel.gameObject.SetActive(true);
        LeanTween.alpha(blackPanel.rectTransform, 1f, duration).setEase(LeanTweenType.easeInQuad);
    }
    public IEnumerator StartMenuCoroutine()
    {
        FadeOutPanel();
        yield return new WaitForSeconds(1.5f);
        FadeOutPanelFalse();

    }

    private void FixedUpdate()
    {
        //StartTitleFloating();
    }

    void Start()
    {
        // Start floating effect for title
        blackPanel.gameObject.SetActive(true);
        StartCoroutine(StartMenuCoroutine());
        //StartTitleFloating();
        
        

        // Initialize button effects for each button in the list
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                GameObject buttonObject = button.gameObject;

                // Store the original scale of the button
                originalScales[buttonObject] = buttonObject.transform.localScale;

                // Add CanvasGroup if not already present
                CanvasGroup canvasGroup = buttonObject.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = buttonObject.AddComponent<CanvasGroup>();
                }
                buttonCanvasGroups[buttonObject] = canvasGroup;

                // Set initial opacity
                canvasGroup.alpha = initialOpacity;

                // Add button click listener
                button.onClick.AddListener(() => OnButtonClick(buttonObject));

                // Add event triggers for hover effects
                EventTrigger trigger = buttonObject.AddComponent<EventTrigger>();
                AddEventTrigger(trigger, EventTriggerType.PointerEnter, () => OnPointerEnter(buttonObject));
                AddEventTrigger(trigger, EventTriggerType.PointerExit, () => OnPointerExit(buttonObject));
            }
        }
    }

    // Helper function to add event triggers
    void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }

    // Start floating animation for the title
    void StartTitleFloating()
    {
        LeanTween.moveY(title, title.transform.position.y + floatHeight, floatSpeed)
            .setEase(LeanTweenType.easeInOutSine) // Smooth up and down
            .setLoopPingPong(); // This makes it float up and down continuously
    }

    // Hover effect for the button (scale it up and set opacity when hovered)
    public void OnPointerEnter(GameObject button)
    {
        LeanTween.scale(button, originalScales[button] * hoverScale, animationSpeed).setEase(LeanTweenType.easeOutQuad);

        // Increase opacity on hover
        buttonCanvasGroups[button].alpha = 1f; // Fully visible when hovered
        audioManager.Play("ButtonHover");
        //SoundManager.PlaySound(SoundType.HOVER);
    }

    // Revert scale back to original and opacity when pointer exits the button
    public void OnPointerExit(GameObject button)
    {
        LeanTween.scale(button, originalScales[button], animationSpeed).setEase(LeanTweenType.easeInQuad);

        // Revert opacity when not hovered
        buttonCanvasGroups[button].alpha = initialOpacity;
    }

    // Click effect for the button (scale it down when clicked, then back to normal)
    public void OnButtonClick(GameObject button)
    {
        LeanTween.scale(button, originalScales[button] * clickScale, animationSpeed).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
        {
            // Return to original scale after button press
            LeanTween.scale(button, originalScales[button], animationSpeed).setEase(LeanTweenType.easeOutQuad);
        });

        // Keep opacity at full when clicked
        buttonCanvasGroups[button].alpha = 1f;
    }
}

