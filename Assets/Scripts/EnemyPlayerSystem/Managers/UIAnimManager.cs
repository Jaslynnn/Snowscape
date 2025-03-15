using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimManager : MonoBehaviour
{
    [System.Serializable]
    public class UIElement
    {
        public string key;
        public GameObject uiElement;
        public KeyCode? glowKey;
        public Image glowImage;
    }

    public List<UIElement> uiElementsList;

    private Dictionary<string, (GameObject uiElement, Vector3 originalScale, KeyCode? glowKey, Image glowImage, Vector3 originalParentScale)> uiElements = new Dictionary<string, (GameObject, Vector3, KeyCode?, Image, Vector3)>();

    private float enlargedScale = 1.2f;
    private float animationDuration = 0.3f;
    private float popScaleFactor = 1.3f;
    private float popDuration = 0.2f;

    private void Start()
    {
        foreach (var element in uiElementsList)
        {
            Transform parentObj = element.glowImage ? element.glowImage.transform.parent : null;
            Vector3 parentScale = parentObj ? parentObj.localScale : Vector3.one;

            uiElements[element.key] = (element.uiElement, element.uiElement.transform.localScale, element.glowKey, element.glowImage, parentScale);

            if (element.glowImage)
            {
                SetGlowOpacity(element.glowImage, 0f);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar Pressed");
            ShowGlow(KeyCode.Space);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Spacebar Released");
            HideGlow(KeyCode.Space);
        }
    }

    public void EnlargeUIElement(string key)
    {
        foreach (var elementKey in uiElements.Keys)
        {
            if (elementKey != key)
            {
                LeanTween.scale(uiElements[elementKey].uiElement, uiElements[elementKey].originalScale, animationDuration).setEase(LeanTweenType.easeOutBack);
                if (uiElements[elementKey].glowKey.HasValue)
                {
                    HideGlow(uiElements[elementKey].glowKey.Value);
                }
            }
        }

        if (uiElements.ContainsKey(key))
        {
            var element = uiElements[key];
            if (element.glowKey.HasValue)
            {
                ShowGlow(element.glowKey.Value);
            }

            LeanTween.scale(element.uiElement, element.originalScale * enlargedScale, animationDuration).setEase(LeanTweenType.easeOutBack);
        }
    }

    public void PopUIElement(string key)
    {
        if (uiElements.ContainsKey(key))
        {
            LeanTween.scale(uiElements[key].uiElement, uiElements[key].originalScale * popScaleFactor, popDuration)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(() => ShrinkBackUIElement(key));
        }
    }

    private void ShrinkBackUIElement(string key)
    {
        if (uiElements.ContainsKey(key))
        {
            LeanTween.scale(uiElements[key].uiElement, uiElements[key].originalScale, popDuration).setEase(LeanTweenType.easeInQuad);
        }
    }

    public void ShowGlow(KeyCode key)
    {
        foreach (var element in uiElements.Values)
        {
            if (element.glowKey == key && element.glowImage)
            {
                LeanTween.value(gameObject, alpha => SetGlowOpacity(element.glowImage, alpha), element.glowImage.color.a, 1f, 0.3f);
                LeanTween.scale(element.glowImage.transform.parent.gameObject, element.originalParentScale * 1.1f, 0.1f).setEaseOutQuad();
            }
        }
    }

    public void HideGlow(KeyCode key)
    {
        foreach (var element in uiElements.Values)
        {
            if (element.glowKey == key && element.glowImage)
            {
                LeanTween.value(gameObject, alpha => SetGlowOpacity(element.glowImage, alpha), element.glowImage.color.a, 0f, 0.3f);
                LeanTween.scale(element.glowImage.transform.parent.gameObject, element.originalParentScale, 0.1f).setEaseOutQuad();
            }
        }
    }

    private void SetGlowOpacity(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
